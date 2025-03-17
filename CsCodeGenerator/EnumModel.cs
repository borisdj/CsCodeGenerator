using CsCodeGenerator.Enums;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class EnumModel : BaseElement
    {
        public EnumModel(string name = null)
        {
            base.CustomDataType = Util.Enum;
            base.Name = name;
        }

        public override int IndentSize { get; set; } = (int)IndentType.Single * CsGenerator.DefaultTabSize;

        public new BuiltInDataType? BuiltInDataType { get; }

        public new string CustomDataType => base.CustomDataType;

        public new string Name => base.Name;

        public List<EnumValue> EnumValues { get; set; } = new List<EnumValue>();


        protected override void BuildStringInternal()
        {
            AppendIntent();
            Builder.Append("{");


            Builder.Append(EnumValues.Count > 0 ? Util.NewLine : "");
            AppendJoin("," + Util.NewLine, EnumValues);

            AppendIntent();
            Builder.Append("}");
        }
    }

    public class EnumValue
    {
        public EnumValue(string name = null, int? value = null)
        {
            Name = name;
            Value = value;
        }

        public virtual int IndentSize { get; set; } = (int)IndentType.Double * CsGenerator.DefaultTabSize;
        public string Indent => new string(' ', IndentSize);

        public string Name { get; set; }

        public int? Value { get; set; }
        public string ValuFormated => Value != null ? " = " + Value : "";

        public override string ToString()
        {
            string result = Indent + Name + ValuFormated;
            return result;
        }
    }
}
