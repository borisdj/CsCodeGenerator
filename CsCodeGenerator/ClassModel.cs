using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsCodeGenerator
{
    public class ClassModel : BaseElement
    {
        public ClassModel(string name = null)
        {
            base.CustomDataType = Util.Class;
            base.Name = name;
            Constructors.Add(new Constructor(Name) { IsVisible = false, BracesInNewLine = false });
        }

        public override int IndentSize { get; set; } = (int)IndentType.Single * CsGenerator.DefaultTabSize;

        public bool HasPropertiesSpacing { get; set; } = true;

        public new BuiltInDataType? BuiltInDataType { get; }

        public new string CustomDataType => Util.Class;

        public new string Name => base.Name;

        public string BaseClass { get; set; }

        public List<string> Interfaces { get; set; } = new List<string>();

        public virtual List<Field> Fields { get; set; } = new List<Field>();

        public virtual List<Constructor> Constructors { get; set; } = new List<Constructor>();

        public virtual Constructor DefaultConstructor
        {
            get { return Constructors[0]; }
            set { Constructors[0] = value; }
        }

        public virtual List<Property> Properties { get; set; } = new List<Property>();

        public virtual List<Method> Methods { get; set; } = new List<Method>();

        public virtual List<ClassModel> NestedClasses { get; set; } = new List<ClassModel>();
        // Nested indent have to be set for each Nested element and subelement separately, or after generation manualy to select nested code and indent it with tab
        // Setting it automaticaly and propagating could be done if the parent sets the child's parent reference (to itself) when the child is added/assigned to a parent. Parent setter is internal.
        //   http://softwareengineering.stackexchange.com/questions/261453/what-is-the-best-way-to-initialize-a-childs-reference-to-its-parent

        public override string ToString()
        {
            string result = base.ToString();
            result += (BaseClass != null || Interfaces?.Count > 0) ? $" : " : "";
            result += BaseClass != null ? BaseClass : "";
            result += (BaseClass != null && Interfaces?.Count > 0) ? $", " : "";
            result += Interfaces?.Count > 0 ? string.Join(", ", Interfaces) : "";
            result += Util.NewLine + Indent + "{";

            result += String.Join("", Fields);

            var visibleConstructors = Constructors.Where(a => a.IsVisible);
            bool hasFieldsBeforeConstructor = visibleConstructors.Any() && Fields.Any();
            result += hasFieldsBeforeConstructor ? Util.NewLine : "";
            result += String.Join(Util.NewLine, visibleConstructors);
            bool hasMembersAfterConstructor = (visibleConstructors.Any() || Fields.Any()) && (Properties.Any() || Methods.Any());
            result += hasMembersAfterConstructor ? Util.NewLine : "";

            result += String.Join(HasPropertiesSpacing ? Util.NewLine : "", Properties);

            bool hasPropertiesAndMethods = Properties.Count > 0 && Methods.Count > 0;
            result += hasMembersAfterConstructor ? Util.NewLine : "";
            result += String.Join(Util.NewLine, Methods);
            
            result += NestedClasses.Count > 0 ? Util.NewLine : "";
            result += String.Join(Util.NewLine, NestedClasses);

            result += Util.NewLine + Indent + "}";
            return result;
        }
    }
}
