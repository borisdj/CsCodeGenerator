using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class ClassModel : BaseElement
    {
        public ClassModel(string name = null)
        {
            base.BuiltInDataType = null;
            base.CustomDataType = Util.Class;
            base.Name = name;
            Constructors.Add(new Constructor(Name) { IsVisible = false, BracesInNewLine = false });
        }

        public override int IndentSize { get; set; } = (int)IndentType.Single * CsGenerator.DefaultTabSize;

        public bool HasPropertiesSpacing { get; set; } = true;

        public new BuiltInDataType? BuiltInDataType => base.BuiltInDataType;

        public new string CustomDataType => base.CustomDataType;

        public new string Name => base.Name;

        public string BaseClass { get; set; }
        public string BaseClassFormated => BaseClass != null ? $" : {BaseClass}" : "";

        public virtual Dictionary<string, Field> Fields { get; set; } = new Dictionary<string, Field>();

        public virtual List<Constructor> Constructors { get; set; } = new List<Constructor>();

        public virtual Constructor DefaultConstructor
        {
            get { return Constructors[0]; }
            set { Constructors[0] = value; }
        }

        public virtual Dictionary<string, Property> Properties { get; set; } = new Dictionary<string, Property>();

        public virtual Dictionary<string, Method> Methods { get; set; } = new Dictionary<string, Method>();

        public virtual Dictionary<string, ClassModel> NestedClasses { get; set; } = new Dictionary<string, ClassModel>();
        // Nested indent have to set for each Nested element and subelement separately, or afte genration manualy to select nested code and indet it with tab
        // Setting it automaticaly and oropagating could be done if The parent sets the child's parent reference (to itself) when the child is added/assigned to a parent. Parent setter is internal.
        //   http://softwareengineering.stackexchange.com/questions/261453/what-is-the-best-way-to-initialize-a-childs-reference-to-its-parent

        public override string ToString()
        {
            string result = base.ToString() + BaseClassFormated;
            result += Util.NewLine + Indent + "{";

            result += String.Join("", Fields.Values);

            bool hasFieldsBeforeConstructor = Constructors.Count > 0 && DefaultConstructor.IsVisible && Fields.Count > 0;
            result += hasFieldsBeforeConstructor ? Util.NewLine : "";
            result += String.Join(Util.NewLine, Constructors);
            bool hasMembersAfterConstructor = (DefaultConstructor.IsVisible || Fields.Count > 0) && (Properties.Count > 0 || Methods.Count > 0);
            result += hasMembersAfterConstructor ? Util.NewLine : "";

            result += String.Join(HasPropertiesSpacing ? Util.NewLine : "", Properties.Values);

            bool hasPropertiesAndMethods = Properties.Count > 0 && Methods.Count > 0;
            result += hasMembersAfterConstructor ? Util.NewLine : "";
            result += String.Join(Util.NewLine, Methods.Values);
            
            result += NestedClasses.Count > 0 ? Util.NewLine : "";
            result += String.Join(Util.NewLine, NestedClasses.Values);

            result += Util.NewLine + Indent + "}";
            return result;
        }
    }
}
