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
            BuiltInDataType = builtInDataType;
            Name = name;
        }

        public BaseElement(string customDataType, string name)
        {
            CustomDataType = customDataType;
            Name = name;
        }

        public virtual int IndentSize { get; set; } = (int)IndentType.Double * CsGenerator.DefaultTabSize;

        public virtual int FluentIndentSize { get; set; }
        public string Indent => new string(' ', IndentSize);
        public string FluentIndent => new string(' ', FluentIndentSize);

        public virtual string Comment { get; set; }

        public virtual bool CommentHasSummaryTag { get; set; } = true;

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
            string result = "";
            if (Comment != null)
            {
                string commentSummary = CommentHasSummaryTag ? Util.NewLine + Indent + "/// <summary>" : "";
                result += commentSummary;
                result += Util.NewLine + Indent + "// " + Comment ;
                result += commentSummary;
            }
            result += HasAttributes ? Attributes.ToStringList(Indent) : "";
            result += Util.NewLine + Signature;
            return result;
        }
    }
}
