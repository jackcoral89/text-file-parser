namespace Parser;

public class Program
{
	private static bool loadingComplete = false;

	public static void Main()
	{
		Console.Write("Enter search keyword: ");
		string searchString = Console.ReadLine() ?? "";

		string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\data"));

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

		var matchingLines = ReadTextFiles(projectDir, searchString);

		loadingComplete = true;
		loaderThread.Join();

		Console.WriteLine($"\rResults for \"{searchString}\":\n");

		foreach (var line in matchingLines)
		{
			Console.WriteLine(line);
		}

		Console.ReadKey();
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