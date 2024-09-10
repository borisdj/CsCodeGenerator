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

        public override string Body
        {
            get
            {
                string result = "";
                if (IsAutoImplemented && SetterBody == null)
                {
                    if (IsGetOnly)
                    {
                        if(GetterBody != null)
                            result += " => " + GetterBody + ";";
                        else
                            result += " { get; }";
                    }
                    else
                    {
                        result += " { get; set; }";
                    }
                }
                else
                {
                    result += Util.NewLine + Indent + "{";
                    string curentIndent = Util.NewLine + Indent + CsGenerator.IndentSingle;

                    result += curentIndent + "get { return " + GetterBody + "; }";
                    if (!IsGetOnly && SetterBody != null)
                    {
                        result += curentIndent + "set { " + SetterBody + "; }";
                    }
                    result += Util.NewLine + Indent + "}";
                }
                return result;
            } 
        }
    }
}
