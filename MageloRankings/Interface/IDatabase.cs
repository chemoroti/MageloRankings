using MageloRankings.Models;

namespace MageloRankings.Interface
{
    public interface IDatabase
    {
        public void PopulateDatabase();
        public List<Character> Search(QueryParams queryParams);
    }
}
