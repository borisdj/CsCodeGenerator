using CsCodeGenerator.Enums;

namespace CsCodeGenerator
{
    public class Property : Field
    {
        public Property() { }

        public Property(BuiltInDataType builtInDataType, string name) : base(builtInDataType, name) { }

        public Property(string customDataType, string name) : base(customDataType, name) { }

        public override bool HasAttributes => true;

        public override AccessModifier AccessModifier { get; set; } = AccessModifier.Public;

        public bool IsGetOnly { get; set; } = false;

        public bool IsAutoImplemented { get; set; } = true;
        
        public string GetterBody { get; set; }

        public string SetterBody { get; set; }

        protected override string Ending => DefaultValue != null ? ";" : "";

        protected override void BuildBody()
        {
            base.BuildBody();

            if (IsAutoImplemented && SetterBody == null)
            {
                if (IsGetOnly)
                {
                    if (GetterBody != null)
                       Builder.Append(" => ").Append(GetterBody).Append(";");
                    else
                        Builder.Append(" { get; }");
                }
                else
                {
                    Builder.Append(" { get; set; }");
                }
            }
            else
            {
                AppendIntent();
                Builder.Append("{");

                AppendIntent();
                Builder.Append(CsGenerator.IndentSingle);
                Builder.Append("get { return ").Append(GetterBody).Append("; }");
                if (!IsGetOnly && SetterBody != null)
                {
                    AppendIntent();
                    Builder.Append(CsGenerator.IndentSingle);
                    Builder.Append("set { ").Append(SetterBody).Append("; }");
                }

                AppendIntent();
                Builder.Append("}");
            }
        }
    }
}
