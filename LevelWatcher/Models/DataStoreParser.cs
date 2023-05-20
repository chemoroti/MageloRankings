using MageloRankings.Models;
using MageloRankings.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelWatcher.Models
{
    public class DataStoreParser
    {
        private string? _csvPath;
        private string _dataExportPath;
        private DateTime _exportDate;

        private InMemoryDatabaseService _databaseService;
        private CsvService _csvService;
        public DataStoreParser(string? csvPath, string dataExportPath, string? dateString) 
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
        public void IngestData()
        {
            LoadExistingCsvData();
            LoadNewData();
            CombineData();
            ExportData();
        }

        private void LoadExistingCsvData()
        {
            if (_exportDate == DateTime.Now)
            {
                throw new ArgumentException("Data for today already exists in this csv file. Ending program.");
            }

            if (_csvPath != null)
            {
                Console.WriteLine("Ingesting existing character data");
                StreamContent stream = new StreamContent(File.OpenRead(_csvPath));
                _csvService.PopulateData(stream);
                Console.WriteLine("Finished ingesting existing character data");
            }
        }

        private async void LoadNewData()
        {
            try
            {
                if (_dataExportPath == null)
                    throw new Exception("Must define valid path to new character data");

                Console.WriteLine("Ingesting new character data");
                FileStream stream = File.OpenRead(_dataExportPath);
                await _databaseService.PopulateDatabase(new StreamContent(stream));
                Console.WriteLine("Finished ingesting new character data");
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
            List<Character> allCharacters = _databaseService.Search(new QueryParams());
            Dictionary<string, CharacterSimple> trackedCharacters = _csvService.GetCharacters();
            _csvService.AddHeaderDate(_exportDate);

            foreach(Character character in allCharacters)
            {
                LevelDate levelDate = new LevelDate()
                {
                    Level = character.level,
                    Date = _exportDate
                };
                if (trackedCharacters.ContainsKey(character.name))
                {
                    Console.WriteLine($"Updating Character: {character.name}");
                    _csvService.UpdateCharacter(character.name, levelDate);
                }
                else
                {
                    Console.WriteLine($"Adding Character: {character.name}");
                    _csvService.AddCharacter(new CharacterSimple(character.name, character.guild_name, new List<LevelDate>() { levelDate }));
                }
            }

            Console.WriteLine("Finished combining data");
        }

        public void ExportData()
        {
            Console.WriteLine("Beginning data export");
            string data = _csvService.ExportData();
            string date = _exportDate.ToShortDateString().Replace('/', '_');
            string outputDir = _csvPath == null 
                ? _dataExportPath.Substring(0, _dataExportPath.LastIndexOf('\\'))
                : _csvPath.Substring(0, _csvPath.LastIndexOf('\\'));

            string outputPath = $"{outputDir}\\LevelWatcher_{date}.csv";
            File.WriteAllText(outputPath, data);

            Console.WriteLine($"Done. Data written to {outputPath}");
        }
    }
}
