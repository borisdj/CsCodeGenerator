using System;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class FileModel
    {
        public FileModel() { }
        public FileModel(string name)
        {
            Name = name;
        }

        public List<string> UsingDirectives { get; set; } = new List<string>();

        public string Namespace { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; } = Util.CsExtension;

        public string FullName => Name + "." + Extension;

        public Dictionary<string, EnumModel> Enums { get; set; } = new Dictionary<string, EnumModel>();

        public Dictionary<string, ClassModel> Classes { get; set; } = new Dictionary<string, ClassModel>();

        public void LoadUsingDirectives(List<string> usingDirectives)
        {
            foreach (var usingDirective in usingDirectives)
            {
                UsingDirectives.Add(usingDirective);
            }
        }

        public override string ToString()
        {
            string result = String.Join(Util.NewLine, UsingDirectives);
            result += Util.NewLineDouble + Namespace;
            result += Util.NewLine + "{";
            result += String.Join(Util.NewLine, Enums.Values);
            result += (Enums.Count > 0 && Classes.Count > 0) ? Util.NewLine : "";
            result += String.Join(Util.NewLine, Classes.Values);
            result += Util.NewLine + "}";
            result += Util.NewLine;
            return result;
        }
    }
}
