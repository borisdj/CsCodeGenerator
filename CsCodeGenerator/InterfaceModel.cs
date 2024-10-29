using CsCodeGenerator.Enums;
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

        public virtual List<Property> Properties { get; set; } = new List<Property>();

        public virtual List<Method> Methods { get; set; } = new List<Method>();

        protected override void BuildStringInternal()
        {
            AppendIntent();
            Builder.Append("{");

            AppendJoin("", Properties);
            bool hasPropertiesAndMethods = Properties.Count > 0 && Methods.Count > 0;
            Builder.Append(hasPropertiesAndMethods? Util.NewLine : "");
            AppendJoin(Util.NewLine, Methods);

            Builder.Append(Util.NewLine + Indent + "}");

            Builder.Replace(AccessModifier.Public.ToTextLower() + " ", "");
            Builder.Replace("\r\n        {\r\n        }", ";");
        }
    }
}
