using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class FileModel : Serialiazble
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

        public List<EnumModel> Enums { get; set; } = new List<EnumModel>();

        public List<ClassModel> Classes { get; set; } = new List<ClassModel>();

        public void LoadUsingDirectives(List<string> usingDirectives)
        {
            foreach (var usingDirective in usingDirectives)
            {
                UsingDirectives.Add(usingDirective);
            }
        }

        public override string ToString()
        {
            Builder.Clear();


            string usingText = UsingDirectives.Count > 0 ? Util.Using + " " : "";
            Builder.Append(usingText);

            AppendJoin(Util.NewLine + usingText, UsingDirectives);

            Builder.Append(Util.NewLineDouble).Append(Util.Namespace).Append(" ").Append(Namespace);
            Builder.Append(Util.NewLine).Append("{");


            AppendJoin(Util.NewLine, Enums);
            Builder.Append((Enums.Count > 0 && Classes.Count > 0) ? Util.NewLine : "");

            AppendJoin(Util.NewLine, Classes);


            Builder.Append(Util.NewLine + "}");
            Builder.Append(Util.NewLine);

            return Builder.ToString();
        }
    }
}
