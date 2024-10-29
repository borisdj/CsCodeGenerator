using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CsCodeGenerator
{
    public abstract class BaseElement : Serialiazble
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


        protected void AppendIntent()
        {
            Builder.Append(Util.NewLine).Append(' ', IndentSize);
        }

        private void BuildString()
        {
            Builder.Clear();

            BuildComment();
            BuildAttributes();
            BuildSignature();

            BuildStringInternal();
        }

        protected abstract void BuildStringInternal();

        private void BuildSignature()
        {
            Builder.Append(Util.NewLine).Append(Signature);
        }

        private void BuildAttributes()
        {
            if (Attributes.Any())
            {
                AppendIntent();
                AppendJoin(Util.NewLine + Indent, Attributes);
            }
        }

        private void BuildComment()
        {
            if (Comment == null)
            {
                return;
            }

            if (CommentHasSummaryTag)
            {
                AppendIntent();
                Builder.Append("/// ").Append(GetTag(CommentTag.Summary, true));
            }

            AppendIntent();
            Builder.Append("// ").Append(Comment);

            if (CommentHasSummaryTag)
            {
                AppendIntent();
                Builder.Append("/// ").Append(GetTag(CommentTag.Summary, false));
            }
        }


        private string GetTag(CommentTag tag, bool start)
        {
            string tagText = tag.ToString().ToLower();
            return start ? $"<{tagText}>" : $"</{tagText}>";
        }

        public sealed override string ToString()
        {
            BuildString();
            return Builder.ToString();
        }
    }
}
