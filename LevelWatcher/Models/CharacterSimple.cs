using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelWatcher.Models
{
    public class CharacterSimple
    {
        public string Name { get; set; }
        public string Guild { get; set; }
        public List<LevelDate> LevelDates { get; set; }

        public CharacterSimple(string row, List<string> headers) 
        {
            string[] data = row.Split(',');
            Name = data[0];
            Guild = data[data.Length-1];
            LevelDates = GetLevelDates(data, headers);
        }

        public CharacterSimple(string name, string guild, List<LevelDate> levelDates)
        {
            Name = name;
            Guild = guild;
            LevelDates = levelDates;
        }

        private List<LevelDate> GetLevelDates(string[] data, List<string> headers)
        {
            List<LevelDate> levelDates = new List<LevelDate>();
            string startsWith = "Level on ";

            for(int i = 0; i < data.Count(); i++)
            {
                if (headers[i].StartsWith(startsWith))
                {
                    levelDates.Add(new LevelDate()
                    {
                        Level = int.Parse(data[i]),
                        Date = DateTime.Parse(headers[i].Substring(startsWith.Length))
                    });
                }
            }

            return levelDates;
        }
    }

    public class LevelDate
    {
        public int Level { get; set; }
        public DateTime Date { get; set; }
    }
}
