using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelWatcher.Models
{
    public class CsvService
    {
        private List<string> _headers = new List<string>();
        private Dictionary<string, CharacterSimple> _characters = new Dictionary<string, CharacterSimple>();
        public CsvService() { }

        public Dictionary<string, CharacterSimple> GetCharacters()
        {
            return _characters;
        }
        public async void PopulateData(StreamContent stream)
        {
            string allData = await stream.ReadAsStringAsync();
            string[] rows = allData.Split('\n').Select(r => r.Trim()).ToArray();

            string[] headerData = rows[0].Split(',');
            foreach (string prop in headerData)
            {
                _headers.Add(prop);
            }

            foreach (string row in rows.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    CharacterSimple character = new CharacterSimple(row, _headers);
                    AddCharacter(character);
                }
            }
        }

        public void AddCharacter(CharacterSimple character)
        {
            if (!_characters.ContainsKey(character.Name))
            {
                _characters.Add(character.Name, character);
            }
        }

        public void UpdateCharacter(string name, LevelDate levelDate)
        {
            _characters[name].LevelDates.Add(levelDate);
        }

        public void AddHeaderDate(DateTime headerDate)
        {
            string dateString = headerDate.ToShortDateString();
            string newHeader = $"Level on {dateString}";
            if (!_headers.Contains(newHeader))
            {
                _headers.Add(newHeader);
            }
        }

        public string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            int offset = "Level on ".Length;
            List<DateTime> headerDates = _headers
                .Where(h => h.StartsWith("Level on "))
                .Select(h => DateTime.Parse(h.Substring(offset)))
                .OrderByDescending(h => h)
                .Take(7)
                .Reverse()
                .ToList();

            builder.AppendLine(BuildHeaderRowForExport(headerDates));

            foreach(var character in _characters)
            {
                builder.AppendLine(BuildCharacterRowForExport(character.Value, headerDates));
            }

            return builder.ToString();
        }

        private string BuildHeaderRowForExport(List<DateTime> headerDates)
        {
            string headerRow = "Name";
            
            foreach(DateTime date in headerDates)
            {
                headerRow += ($",Level on {date.ToShortDateString()}");
            }

            headerRow += ",1-Day Progress,2-Day Progress,3-Day Progress,7-Day Progress,Guild";

            return headerRow;
        }

        private string BuildCharacterRowForExport(CharacterSimple character, List<DateTime> headerDates)
        {
            string row = $"{character.Name}";
            foreach(DateTime date in headerDates)
            {
                LevelDate? levelDate = character.LevelDates.FirstOrDefault(ld => ld.Date == date);
                if (levelDate == null)
                {
                    // if we don't have level data for them on this day (maybe they are anonymous), we just assume they gained no levels that day.
                    // This should never be null because their character data would not exist in this csv unless a column is populated
                    levelDate = character.LevelDates.OrderByDescending(ld => ld.Date).First(ld => ld.Date <= date);
                }

                row += $",{levelDate.Level}";
            }

            LevelDate latestLevelDate = character.LevelDates.OrderByDescending(ld => ld.Date).First();

            //1-day diff
            LevelDate? diff1 = character.LevelDates.OrderByDescending(ld => ld.Date).FirstOrDefault(ld => ld.Date <= latestLevelDate.Date.AddDays(-1));
            int diff1Val = (diff1 == null) ? 0 : latestLevelDate.Level - diff1.Level;
            row += $",{diff1Val}";

            //2-day diff
            LevelDate? diff2 = character.LevelDates.OrderByDescending(ld => ld.Date).FirstOrDefault(ld => ld.Date <= latestLevelDate.Date.AddDays(-2));
            int diff2Val = (diff2 == null) ? diff1Val : latestLevelDate.Level - diff2.Level;
            row += $",{diff2Val}";

            //3-day diff
            LevelDate? diff3 = character.LevelDates.OrderByDescending(ld => ld.Date).FirstOrDefault(ld => ld.Date <= latestLevelDate.Date.AddDays(-3));
            int diff3Val = (diff3 == null) ? diff2Val : latestLevelDate.Level - diff3.Level;
            row += $",{diff3Val}";

            //7-day diff
            LevelDate? diff7 = character.LevelDates.OrderByDescending(ld => ld.Date).FirstOrDefault(ld => ld.Date <= latestLevelDate.Date.AddDays(-7));
            int diff7Val = (diff7 == null) ? diff3Val : latestLevelDate.Level - diff7.Level;
            row += $",{diff7Val}";

            row += $",{character.Guild}";

            return row;
        }
    }
}
