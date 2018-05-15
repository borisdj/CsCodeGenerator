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

        public List<FileModel> Files { get; set; } = new List<FileModel>();
        
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
                throw new InvalidOperationException(message);
            }

            if (!Directory.Exists(path + OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            foreach (var file in Files)
            {
                string filePath = $@"{path}\{OutputDirectory}\{file.FullName}";
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.Write(file);
                }
            }
        }
    }
}
