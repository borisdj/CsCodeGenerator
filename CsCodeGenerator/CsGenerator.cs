using System;
using System.Collections.Generic;
using System.IO;

namespace CsCodeGenerator
{
    public class CsGenerator
    {
        public static int DefaultTabSize = 4;

        public static string IndentSingle => new String(' ', CsGenerator.DefaultTabSize);

        public string OutputDirectory { get; set; } = "Output";
        public string DefaultPath { get; } = Directory.GetCurrentDirectory();

        public string Path { get; set; }

        public Dictionary<string, FileModel> Files { get; set; } = new Dictionary<string, FileModel>();
        
        public void CreateFiles()
        {
            string path = Path ?? DefaultPath;
            if (!Directory.Exists(path))
            {
                string message = "Path not valid: " + Path;
                Console.WriteLine(message);
                throw new InvalidOperationException(message);
            }
            if (OutputDirectory == null)
            {
                string message = "Generator.OutputDirectory not set!";
                Console.WriteLine(message);
                throw new InvalidOperationException(message);
            }

            if (!Directory.Exists(path + OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
                Console.WriteLine("Created folder: " + OutputDirectory);
            }

            Console.WriteLine("Created files: ");
            int i = 1;
            foreach (var file in Files.Values)
            {
                string filePath = $@"{path}\{OutputDirectory}\{file.FullName}";
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Write(file);
                    Console.WriteLine($"  {i}. {file.FullName}");
                    i++;
                }
            }

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }
    }
}
