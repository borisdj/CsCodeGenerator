using CsCodeGenerator.Enums;

namespace CsCodeGenerator
{
    public class Field : BaseElement
    {
        public Field() { }

        public Field(BuiltInDataType builtInDataType, string name) : base(builtInDataType, name) { }

        public Field(string customDataType, string name) : base(customDataType, name) { }

        public override AccessModifier AccessModifier { get; set; } = AccessModifier.Protected;


        public virtual string DefaultValue { get; set; }
        protected string DefaultValueFormated => DefaultValue != null ? " = " + DefaultValue : "";

        public override bool HasAttributes => false;

        protected virtual string Ending { get; } = ";";

        protected virtual void BuildBody()
        {

        }

        protected override void BuildStringInternal()
        {

            BuildBody();
            Builder.Append(DefaultValueFormated);
            Builder.Append(Ending);
        }
    }
}
