using CsCodeGenerator.Enums;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class Method : BaseElement
    {
        public Method() { }

        public Method(BuiltInDataType builtInDataType, string name) : base(builtInDataType, name) { }

        public Method(string customDataType, string name) : base(customDataType, name) { }

        public Method(AccessModifier accessModifier, KeyWord singleKeyWord, BuiltInDataType builtInDataType, string name) : base(builtInDataType, name)
        {
            AccessModifier = accessModifier;
            KeyWords.Add(singleKeyWord);
        }

        public virtual bool IsVisible { get; set; } = true;

        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public string BaseParameters { get; set; }
        public string BaseParametersFormated => BaseParameters != null ? $" : base({BaseParameters})" : "";

        public virtual bool BracesInNewLine { get; set; } = true;

        public List<string> BodyLines { get; set; } = new List<string>();

        public override string Signature => base.Signature + Parameters.ToStringList();

        protected override void BuildStringInternal()
        {

            if (!IsVisible)
            {
                return;
            }

            Builder.Append(BaseParametersFormated);

            string bracesPrefix = BracesInNewLine ? (Util.NewLine + Indent) : " ";
            string curentIndent = Util.NewLine + Indent + CsGenerator.IndentSingle;

            Builder.Append(bracesPrefix).Append("{");
            Builder.Append(BodyLines.Count == 0 ? "" : (BracesInNewLine ? curentIndent : " "));
            AppendJoin(curentIndent, BodyLines);
            Builder.Append(bracesPrefix).Append("}");
        }
    }
}
