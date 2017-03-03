using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class InterfaceModel : BaseElement
    {
        public InterfaceModel(string name = null)
        {
            base.CustomDataType = Util.Interface;
            base.Name = name;
        }

        public override int IndentSize { get; set; } = (int)IndentType.Single * CsGenerator.DefaultTabSize;

        public new BuiltInDataType? BuiltInDataType { get; }

        public new string CustomDataType => base.CustomDataType;

        public new string Name => base.Name;

        public virtual Dictionary<string, Property> Properties { get; set; } = new Dictionary<string, Property>();

        public virtual Dictionary<string, Method> Methods { get; set; } = new Dictionary<string, Method>();

        public override string ToString()
        {
            string result = base.ToString();
            result += Util.NewLine + Indent + "{";

            result += String.Join("", Properties.Values);
            bool hasPropertiesAndMethods = Properties.Count > 0 && Methods.Count > 0;
            result += hasPropertiesAndMethods ? Util.NewLine : "";
            result += String.Join(Util.NewLine, Methods.Values);

            result += Util.NewLine + Indent + "}";
            result = result.Replace(AccessModifier.Public.ToTextLower() + " ", "");
            result = result.Replace("\r\n        {\r\n        }", ";");
            return result;
        }
    }
}
