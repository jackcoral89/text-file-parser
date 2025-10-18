namespace Parser;

public class Program
{
    private static bool loadingComplete = false;

    public static void Main()
    {

        string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\data"));

        while (true)
        {
            if (loadingComplete)
            {
                Console.WriteLine("Search COMPLETED!");
            }

            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine() ?? "";

            if (keyword.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Program terminated");
                break;
            }

            var loaderThread = new Thread(() =>
            {
                string[] symbols = { "|", "/", "-", "\\" };
                int i = 0;
                while (!loadingComplete)
                {
                    Console.Write($"\rLoading {symbols[i++ % symbols.Length]}");
                    Thread.Sleep(150);
                }
            });

            loadingComplete = false;
            loaderThread.Start();

            var matchingLines = ReadTextFiles(projectDir, keyword);

            loadingComplete = true;
            loaderThread.Join();

            Console.WriteLine("Search COMPLETED!");
            Console.WriteLine($"\rResults for \"{keyword}\":\n");

            if (matchingLines.Count == 0)
            {
                Console.WriteLine($"No lines found containing: \"{keyword}\".\n");
            }
            else
            {
                foreach (var line in matchingLines)
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine();
            }

        }

    }

    private static List<string> ReadTextFiles(string directoryPath, string searchString)
    {
        var textFiles = Directory.EnumerateFiles(directoryPath, "*.txt");

        var allFilesRows = new List<string>();

        foreach (var filePath in textFiles)
        {
            var rows = File.ReadAllLines(filePath);
            allFilesRows.AddRange(rows);
        }

        var filteredLines = allFilesRows.Where(line => line.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();

        return filteredLines;
    }

}