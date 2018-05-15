using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCodeGenerator
{
    public abstract class BaseElement
    {
        public BaseElement() { }

        public BaseElement(BuiltInDataType builtInDataType, string name)
        {
            this.BuiltInDataType = builtInDataType;
            this.Name = name;
        }

        public BaseElement(string customDataType, string name)
        {
            this.CustomDataType = customDataType;
            this.Name = name;
        }

        public virtual int IndentSize { get; set; } = (int)IndentType.Double * CsGenerator.DefaultTabSize;
        public string Indent => new String(' ', IndentSize);

        public virtual string Comment { get; set; }

        public virtual bool HasAttributes => true;
        public virtual List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();

        public virtual AccessModifier AccessModifier { get; set; } = AccessModifier.Public;
        protected string AccessFormated => Indent + AccessModifier.ToTextLower() + " ";

        public List<KeyWord> KeyWords { get; set; } = new List<KeyWord>();
        public KeyWord SingleKeyWord { set { KeyWords.Add(value); } }
        protected string KeyWordsFormated => KeyWords?.Count > 0 ? String.Join(" ", KeyWords.Select(a => a.ToTextLower())) + " " : "";

        public virtual BuiltInDataType? BuiltInDataType { get; set; }
        public virtual string CustomDataType { get; set; }
        protected string DataTypeFormated => CustomDataType ?? BuiltInDataType?.ToTextLower();

        public virtual string Name { get; set; }

        public virtual string Signature => $"{AccessFormated}{KeyWordsFormated}{DataTypeFormated} {Name}";

        public void AddAttribute(AttributeModel attributeModel) => Attributes?.Add(attributeModel);

        public override string ToString()
        {
            string result = Comment != null ? (Util.NewLine + Indent + "// " + Comment) : "";
            result += HasAttributes ? Attributes.ToStringList(Indent) : "";
            result += Util.NewLine + Signature;
            return result;
        }
    }
}
