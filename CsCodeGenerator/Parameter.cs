using CsCodeGenerator.Enums;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class Parameter
    {
        public Parameter() { }

        public Parameter(string value)
        {
            Value = value;
        }

        public Parameter(BuiltInDataType builtInDataType, string name)
        {
            BuiltInDataType = builtInDataType;
            Name = name;
        }

        public Parameter(string customDataType, string name)
        {
            CustomDataType = customDataType;
            Name = name;
        }
        public KeyWord? KeyWord { get; set; }

        public BuiltInDataType? BuiltInDataType { get; set; }

        public string CustomDataType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string KeyWordFormated => KeyWord?.ToTextLower(" ");

        public string DataTypeFormated => CustomDataType == null ? BuiltInDataType?.ToTextLower(" ") : CustomDataType + " ";

        public string NameValueFormated => (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value)) ? Name + Value : Name + " = " + Value;

        public override string ToString() => KeyWordFormated + DataTypeFormated + NameValueFormated;
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
