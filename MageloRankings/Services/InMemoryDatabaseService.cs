using MageloRankings.Interface;
using MageloRankings.Models;
using System.Security.Cryptography.X509Certificates;

namespace MageloRankings.Services
{
    public class InMemoryDatabaseService : IDatabase
    {
        public static List<Character> Data = new List<Character>();
        public static Dictionary<string, List<Character>> Dict_Class = new Dictionary<string, List<Character>>();
        public InMemoryDatabaseService()
        {
            if (Data.Count == 0)
            {
                PopulateDatabase();
                BuildIndexes();
            }
        }

        public async void PopulateDatabase()
        {
            Data.Clear();
            string[] rows = await IngestService.GetFileData();
            foreach (string row in rows)
            {
                Data.Add(new Character(row));
            }
        }

        private void BuildIndexes()
        {
            foreach(Character character in Data)
            {
                string clss = character.clss;
                if (!Dict_Class.ContainsKey(clss))
                {
                    Dict_Class.Add(clss, new List<Character>());
                }
                Dict_Class[clss].Add(character);
            }
        }

        public List<Character> Search(QueryParams queryParams)
        {
            IQueryable<Character> query = Data.AsQueryable();
            Filter? classFilter = queryParams.Filters.FirstOrDefault(f => f.Field == FieldEnum.Clss);
            if (classFilter != null)
            {
                query = Dict_Class[classFilter.Value].AsQueryable();
            }

            Filter? hpFilter = queryParams.Filters.FirstOrDefault(f => f.Field == FieldEnum.HP);
            if (hpFilter != null)
            {
                if (hpFilter.Operator == OperatorEnum.LessThan)
                {
                    query = query.Where(q => q.hp_max_total < int.Parse(hpFilter.Value));
                }
                else if (hpFilter.Operator == OperatorEnum.GreaterThan)
                {
                    query = query.Where(q => q.hp_max_total > int.Parse(hpFilter.Value));
                }
                else if (hpFilter.Operator == OperatorEnum.Equal)
                {
                    query = query.Where(q => q.hp_max_total == int.Parse(hpFilter.Value));
                }
                else if (hpFilter.Operator == OperatorEnum.NotEqual)
                {
                    query = query.Where(q => q.hp_max_total != int.Parse(hpFilter.Value));
                }
            }

            return query.Take(queryParams.Take).ToList();
        }

        //private static Func<object, object, bool> GetFunc(Filter filter, object store, object input)
        //{
        //    if (filter.Operator == OperatorEnum.Equal)
        //    {
        //        return (left, right) => left == right;
        //    }
        //    if (filter.Operator == OperatorEnum.NotEqual)
        //    {

        //    }
        //}
    }
}
