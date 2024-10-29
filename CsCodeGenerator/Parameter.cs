using CsCodeGenerator.Enums;
using System.Collections.Generic;
using System.Text;

namespace CsCodeGenerator
{
    public class Parameter : Serialiazble
    {
        public Parameter() { }

        public Parameter(string value)
        {
            Value = value;
        }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Parameter(BuiltInDataType builtInDataType, string name, string value)
        {
            BuiltInDataType = builtInDataType;
            Name = name;
            Value = value;
        }

        public Parameter(string customDataType, string name, string value)
        {
            CustomDataType = customDataType;
            Name = name;
            Value = value;
        }

        public KeyWord? KeyWord { get; set; }

        public BuiltInDataType? BuiltInDataType { get; set; }

        public string CustomDataType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string KeyWordFormated => KeyWord?.ToTextLower(" ");

        public string DataTypeFormated => CustomDataType == null ? BuiltInDataType?.ToTextLower(" ") : CustomDataType + " ";

        public string NameValueFormated => (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value)) ? Name + Value : Name + " = " + Value;

        public override string ToString()
        {
            Builder.Clear();
            Builder.Append(KeyWordFormated).Append(DataTypeFormated).Append(NameValueFormated);
            return Builder.ToString();
        }

    }

    public static class ParameterExtensions
    {
        public static string ToStringList(this List<Parameter> parameters)
        {
            string parametersString = string.Join(", ", parameters);
            string result = $"({parametersString})";
            return result;
        }
    }
}
