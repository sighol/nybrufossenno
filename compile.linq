<Query Kind="Program" />

bool UseFilesystemWather = true;

void Main()
{
	CompileAll();
	
	SetupFilesystemWather();
}

void CompileAll()
{
	try
	{	        
		Console.WriteLine("Compiling...");
		var pages = Directory.GetFiles("pages", "*.html").ToList();
	
		foreach (var page in pages)
		{
			Compile(page);
		}
		Console.WriteLine("Done...\n");
	}
	catch (Exception ex)
	{
		Console.Error.WriteLine("Failed to compile " + ex);
	}
}

void SetupFilesystemWather()
{
	if (!UseFilesystemWather)
	{
		return;
	}
	
	var watcher = new FileSystemWatcher()
	{
		Path = Path.GetFullPath("pages"),
		Filter = "*.html",
		NotifyFilter = NotifyFilters.LastWrite,
	};

	watcher.Changed += (s, e) =>
	{
		Thread.Sleep(100);
		CompileAll();
	};
	watcher.EnableRaisingEvents = true;
	
	Console.WriteLine("Starting file system watcher");
}

void Compile(string page)
{
	var template = File.ReadAllText("template.html");
	var pageContent = File.ReadAllText(page);
	
	var totalContent = template.Replace("[[CONTENT]]", pageContent);
	
	var fileName = Path.GetFileName(page);
	File.WriteAllText(fileName, totalContent);

	Console.Out.WriteLine($"Done with {fileName}");
}

// Define other methods and classes here
