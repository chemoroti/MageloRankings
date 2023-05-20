using LevelWatcher.Models;
using System.Windows.Forms;
namespace LevelWatcher
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Select latest LevelWatcher.csv file (if any). If this is your first time running this then close the file selector dialog.");

            string? csvPath = PromptFilePath();
            if (csvPath == null)
            {
                Console.WriteLine("Since no LevelWatcher.csv was selected, a new one will be created");
            }

            Console.WriteLine("\nSelect latest TAKP_character.txt export");
            string? exportPath = PromptFilePath();
            if (exportPath == null) { return; }

            Console.WriteLine("\nEnter the date of this export in the format MM/DD/YYYY. Leave blank for today's date.");
            string? dateString = Console.ReadLine();

            DataStoreParser parser = new DataStoreParser(csvPath, exportPath, dateString);
            parser.IngestData();

            Console.ReadKey();
        }

        static private string? PromptFilePath()
        {
            OpenFileDialog fd = new OpenFileDialog();
            DialogResult result = fd.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!fd.CheckFileExists)
                {
                    Console.WriteLine($"File does not exist {fd.FileName}.");
                    return null;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                Console.WriteLine("No file specified. Starting fresh!");
                return null;
            }

            Console.WriteLine($"Got file: {fd.FileName}");
            return fd.FileName;
        }
    }
}