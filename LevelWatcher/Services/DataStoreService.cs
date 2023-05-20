using LevelWatcher.Models;
using MageloRankings.Models;
using MageloRankings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelWatcher.Services
{
    public class DataStoreService
    {
        private string? _csvPath;
        private string _dataExportPath;
        private DateTime _exportDate;

        private InMemoryDatabaseService _databaseService;
        private CsvService _csvService;
        public DataStoreService(string? csvPath, string dataExportPath, string? dateString)
        {
            _csvPath = csvPath;
            _dataExportPath = dataExportPath;
            if (string.IsNullOrEmpty(dateString))
            {
                _exportDate = DateTime.Now.Date;
            }
            else
            {
                if (!DateTime.TryParse(dateString, out _exportDate))
                {
                    throw new ArgumentException($"Invalid date: {dateString}. Please use the format MM/DD/YYYY");
                }
            }

            _databaseService = new InMemoryDatabaseService();
            _csvService = new CsvService();
        }
        public async Task DoTheThing()
        {
            try
            {
                await LoadExistingCsvData();
                await LoadNewData();
                CombineData();
                ExportData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private async Task LoadExistingCsvData()
        {
            if (_csvPath != null)
            {
                try
                {
                    Console.WriteLine("Ingesting existing character data");
                    using (StreamContent stream = new StreamContent(File.OpenRead(_csvPath)))
                    {
                        await _csvService.PopulateData(stream);
                    }
                    
                    Console.WriteLine("Finished ingesting existing character data");
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private async Task LoadNewData()
        {
            try
            {
                if (_dataExportPath == null)
                    throw new Exception("Must define valid path to new character data");

                Console.WriteLine("Ingesting new character data");
                using (FileStream stream = File.OpenRead(_dataExportPath))
                {
                    await _databaseService.PopulateDatabase(new StreamContent(stream));
                }
                Console.WriteLine("Finished ingesting new character data");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encountered an error parsing new character data", ex);
                throw;
            }
        }

        private void CombineData()
        {
            Console.WriteLine("Combining new and existing character data");
            List<Character> allCharacters = _databaseService.Search(new QueryParams() { Take = -1 });
            Dictionary<string, CharacterSimple> trackedCharacters = _csvService.GetCharacters();
            _csvService.AddHeaderDate(_exportDate);

            foreach (Character character in allCharacters)
            {
                LevelDate levelDate = new LevelDate()
                {
                    Level = character.level,
                    Date = _exportDate
                };
                if (trackedCharacters.ContainsKey(character.name))
                {
                    Console.WriteLine($"Updating Character: {character.name}");
                    _csvService.UpdateCharacter(character.name, character.guild_name, levelDate);
                }
                else
                {
                    Console.WriteLine($"Adding Character: {character.name}");
                    _csvService.AddCharacter(new CharacterSimple(character.name, character.guild_name, new List<LevelDate>() { levelDate }));
                }
            }

            Console.WriteLine("Finished combining data");
        }

        private void ExportData()
        {
            Console.WriteLine("Beginning data export");
            try
            {
                string data = _csvService.ExportData();
                string date = _exportDate.ToShortDateString().Replace('/', '_');
                string outputDir = _csvPath == null
                    ? _dataExportPath.Substring(0, _dataExportPath.LastIndexOf('\\'))
                    : _csvPath.Substring(0, _csvPath.LastIndexOf('\\'));

                string outputPath = $"{outputDir}\\LevelWatcher_{date}.csv";
                File.WriteAllText(outputPath, data);
                Console.WriteLine($"Done. Data written to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
