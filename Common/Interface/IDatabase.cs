using MageloRankings.Models;

namespace MageloRankings.Interface
{
    public interface IDatabase
    {
        public Task PopulateDatabase(StreamContent stream);
        public List<Character> Search(QueryParams queryParams);
    }
}
