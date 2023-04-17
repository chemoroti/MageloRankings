using System.Security.Cryptography.X509Certificates;

namespace MageloRankings.Services
{
    public class IngestService
    {
        public static string InputFilePath = @"D:\Users\Dayton\Downloads\TAKP_character.txt";
        public IngestService() { }

        public static async Task<string[]> GetFileData()
        {
            string[] characterData = await File.ReadAllLinesAsync(InputFilePath);
            return characterData;
        }
    }
}
