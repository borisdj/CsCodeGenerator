using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace CsCodeGenerator.Tests
{
    // CONSOLE: dotnet new -t xunittest  // create
    //          dotnet test              // run
    // VS: Test -> Windows -> Test Explorer
    public class WriteTests
    {
        private string GetText(List<string> lines)
        {
            string text = null;
            foreach (var line in lines)
                text += line + Util.NewLine;
            return text;
        }

        [Fact]
        public void ShouldWriteFields()
        {
            // Fields
            var fields = new List<Field>
            {
                new Field(BuiltInDataType.Int, "field1"),
                new Field(BuiltInDataType.Decimal, "field2") { AccessModifier = AccessModifier.Private, DefaultValue = "2.0m" },
                new Field(BuiltInDataType.String, "field3") { SingleKeyWord = KeyWord.Static, DefaultValue = @"""text""" },
                new Field(CommonDataType.DateTime.ToString(), "field4") { IndentSize = 0 },
                new Field("List<Guid>", "field5") { Comment = "Remark: List used for Ids.", IndentSize = (int)IndentType.Single * CsGenerator.DefaultTabSize },
            };
            string result = String.Join("", fields) + Util.NewLine;

            string text = GetFieldsText();

            Assert.Equal(result, text);
        }

        private string GetFieldsText()
        {
            var lines = new List<string>
            {
                "",
                "        protected int field1;",
                "        private decimal field2 = 2.0m;",
                "        protected static string field3 = \"text\";",
                "protected DateTime field4;",
                "    // Remark: List used for Ids.",
                "    protected List<Guid> field5;"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteProperties()
        {
            // Properties
            var properties = new List<Property>
            {
                new Property(BuiltInDataType.Int, "Property1"),
                new Property(BuiltInDataType.Decimal, "Property2") { AccessModifier = AccessModifier.Protected, DefaultValue = "2.0m" },
                new Property(BuiltInDataType.String, "Property3") { SingleKeyWord = KeyWord.Static, DefaultValue = @"""text""" },
                new Property(CommonDataType.DateTime.ToString(), "Property4"),
                new Property("List<Guid>", "Property5")
                {
                    Comment = "Remark: Manual implemented Property.",
                    IsAutoImplemented = false,
                    GetterBody = "field5",
                    SetterBody = "field5 = value"
                }
            };
            properties[0].AddAttribute(new AttributeModel("Key"));
            string result = String.Join("", properties) + Util.NewLine;

            string text = GetPropertiesText();

            Assert.Equal(result, text);
        }

        private string GetPropertiesText()
        {
            var lines = new List<string>
            {
                "",
                "        [Key]",
                "        public int Property1 { get; set; }",
                "        protected decimal Property2 { get; set; } = 2.0m;",
                "        public static string Property3 { get; set; } = \"text\";",
                "        public DateTime Property4 { get; set; }",
                "        // Remark: Manual implemented Property.",
                "        public List<Guid> Property5",
                "        {",
                "            get { return field5; }",
                "            set { field5 = value; }",
                "        }"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteMethods()
        {
            // Methods
            var methods = new List<Method>
            {
                new Method(BuiltInDataType.Void, "Method1")
                {
                    BodyLines = new List<string> { "throw new NotImplementedException();" },
                    BracesInNewLine = false
                },
                new Method(AccessModifier.Protected, KeyWord.Virtual, BuiltInDataType.Bool, "IsOdd")
                {
                    BodyLines = new List<string> { "return value % 2 == 1;" }
                }
            };

            methods.Single(a => a.Name == "IsOdd").Parameters.Add(new Parameter(BuiltInDataType.Int, "value"));
            methods.Single(a => a.Name == "IsOdd").AddAttribute(new AttributeModel("Authorize"));

            string result = String.Join(Util.NewLine, methods) + Util.NewLine;

            string text = GetMethodsText();

            Assert.Equal(result, text);
        }

        private string GetMethodsText()
        {
            var lines = new List<string>
            {
                "",
                "        public void Method1() { throw new NotImplementedException(); }",
                "",
                "        [Authorize]",
                "        protected virtual bool IsOdd(int value)",
                "        {",
                "            return value % 2 == 1;",
                "        }"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteNestedClass()
        {
            // Nested Class
            ClassModel nestedClass = new ClassModel("MyNestedClass");
            nestedClass.IndentSize += CsGenerator.DefaultTabSize; // increase indent to every nested element
            nestedClass.DefaultConstructor.IsVisible = true;
            nestedClass.DefaultConstructor.IndentSize += CsGenerator.DefaultTabSize; // increase indent to every nested element
            nestedClass.Comment = "Some Comment";

            ClassModel parentClass = new ClassModel("ParentClass");
            parentClass.BaseClass = "BaseClass";
            parentClass.Interfaces.Add("ParentInterface");
            parentClass.NestedClasses.Add(nestedClass);

            string result = parentClass.ToString() + Util.NewLine;

            string text = GetNestedClassText();
            
            Assert.Equal(result, text);
        }

        private string GetNestedClassText()
        {
            var lines = new List<string>
            {
                "",
                "    public class ParentClass : BaseClass, ParentInterface", 
                "    {",
                "",
                "        // Some Comment",
                "        public class MyNestedClass",
                "        {",
                "            public MyNestedClass() { }",
                "        }",
                "    }"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteEnum()
        {
            // Enums
            EnumModel myEnumModel = new EnumModel("MyEnumModel");
            myEnumModel.EnumValues.Add(new EnumValue("Val1", 1));
            myEnumModel.EnumValues.Add(new EnumValue("Val2", 2));

            string result = myEnumModel.ToString() + Util.NewLine;

            string text = GetEnumText();

            Assert.Equal(result, text);
        }

        private string GetEnumText()
        {
            var lines = new List<string>
            {
                "",
                "    public enum MyEnumModel",
                "    {",
                "        Val1 = 1,",
                "        Val2 = 2",
                "    }"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteInterface()
        {
            // Interfaces
            InterfaceModel myInterfaceModel = new InterfaceModel("MyInterfaceModel");
            myInterfaceModel.Properties.Add(new Property(BuiltInDataType.Int, "Prop1"));
            myInterfaceModel.Properties.Add(new Property(BuiltInDataType.String, "Prop2"));
            
            myInterfaceModel.Methods.Add(new Method(BuiltInDataType.Void, "Method1"));

            string result = myInterfaceModel.ToString() + Util.NewLine;

            string text = GetInterfaceText();

            Debug.WriteLine(result);
            Assert.Equal(result, text);
        }

        public interface SomeInt
        {
            int Ba { get; set; }

            void Order();
        }

        private string GetInterfaceText()
        {
            var lines = new List<string>
            {
                "",
                "    interface MyInterfaceModel",
                "    {",
                "        int Prop1 { get; set; }",
                "        string Prop2 { get; set; }",
                "",
                "        void Method1();",
                "    }"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        private void WriteComplexNumberFile()
        {
            var usingDirectives = new List<string>
            {
                "System;",
                "System.ComponentModel;"
            };
            string fileNameSpace = "CsCodeGenerator.Tests";
            string complexNumberText = "ComplexNumber";

            ClassModel complexNumberClass = new ClassModel(complexNumberText);
            complexNumberClass.SingleKeyWord = KeyWord.Partial; //or: complexNumberClass.KeyWords.Add(KeyWord.Partial);

            var descriptionAttribute = new AttributeModel("Description")
            {
                SingleParameter = new Parameter(@"""Some class info""")
            };
            complexNumberClass.AddAttribute(descriptionAttribute);

            complexNumberClass.DefaultConstructor.IsVisible = true;

            Constructor secondConstructor = new Constructor(complexNumberClass.Name);
            secondConstructor.Parameters.Add(new Parameter(BuiltInDataType.Double, "real"));
            secondConstructor.Parameters.Add(new Parameter(BuiltInDataType.Double, "imaginary") { Value = "0" });
            secondConstructor.BodyLines.Add("Real = real;");
            secondConstructor.BodyLines.Add("Imaginary = imaginary;");
            complexNumberClass.Constructors.Add(secondConstructor);

            var fields = new List<Field>
            {
                new Field(BuiltInDataType.Double, "PI") { SingleKeyWord = KeyWord.Const, DefaultValue = "3.14" },
                new Field(BuiltInDataType.String, "remark") { AccessModifier = AccessModifier.Private },
            };

            var properties = new List<Property>
            {
                new Property(BuiltInDataType.String, "DefaultFormat")
                {
                    SingleKeyWord = KeyWord.Static,
                    IsGetOnly = true,
                    DefaultValue = @"""a + b * i"""
                },
                new Property(BuiltInDataType.Double, "Real"),
                new Property(BuiltInDataType.Double, "Imaginary"),
                new Property(BuiltInDataType.String, "Remark")
                {
                    SingleKeyWord = KeyWord.Virtual,
                    IsAutoImplemented = false,
                    GetterBody = "remark",
                    SetterBody = "remark = value"

                },
            };

            var methods = new List<Method>
            {
                new Method(BuiltInDataType.Double, "Modul")
                {
                    BodyLines = new List<string> { "return Math.Sqrt(Real * Real + Imaginary * Imaginary);" }
                },
                new Method(complexNumberText, "Add")
                {
                    Parameters = new List<Parameter> { new Parameter("ComplexNumber", "input") },
                    BodyLines = new List<string>
                    {
                        "ComplexNumber result = new ComplexNumber();",
                        "result.Real = Real + input.Real;",
                        "result.Imaginary = Imaginary + input.Imaginary;",
                        "return result;"
                    }
                },
                new Method(BuiltInDataType.String, "ToString")
                {
                    Comment = "example of 2 KeyWords(new and virtual), usually here would be just virtual",
                    KeyWords = new List<KeyWord> { KeyWord.New, KeyWord.Virtual },
                    BodyLines = new List<string> { "return $\"({Real:0.00}, {Imaginary:0.00})\";" }
                }
            };

            complexNumberClass.Fields = fields;
            complexNumberClass.Properties = properties;
            complexNumberClass.Methods = methods;

            FileModel complexNumberFile = new FileModel(complexNumberText);
            complexNumberFile.LoadUsingDirectives(usingDirectives);
            complexNumberFile.Namespace = fileNameSpace;
            complexNumberFile.Classes.Add(complexNumberClass);

            CsGenerator csGenerator = new CsGenerator();
            csGenerator.Files.Add(complexNumberFile);
            //csGenerator.CreateFiles(); //Console.Write(complexNumberFile); 

            string result = complexNumberFile.ToString();

            string text = GetComplexNumberFileText();

            Debug.Write(result);
            Assert.Equal(result, text);
        }

        private string GetComplexNumberFileText()
        {
            var lines = new List<string>
            {
                "using System;",
                "using System.ComponentModel;",
                "",
                "namespace CsCodeGenerator.Tests",
                "{",
                "    [Description(\"Some class info\")]",
                "    public partial class ComplexNumber",
                "    {",
                "        protected const double PI = 3.14;",
                "        private string remark;",
                "",
                "        public ComplexNumber() { }",
                "",
                "        public ComplexNumber(double real, double imaginary = 0)",
                "        {",
                "            Real = real;",
                "            Imaginary = imaginary;",
                "        }",
                "",
                "        public static string DefaultFormat { get; } = \"a + b * i\";",
                "",
                "        public double Real { get; set; }",
                "",
                "        public double Imaginary { get; set; }",
                "",
                "        public virtual string Remark",
                "        {",
                "            get { return remark; }",
                "            set { remark = value; }",
                "        }",
                "",
                "        public double Modul()",
                "        {",
                "            return Math.Sqrt(Real * Real + Imaginary * Imaginary);",
                "        }",
                "",
                "        public ComplexNumber Add(ComplexNumber input)",
                "        {",
                "            ComplexNumber result = new ComplexNumber();",
                "            result.Real = Real + input.Real;",
                "            result.Imaginary = Imaginary + input.Imaginary;",
                "            return result;",
                "        }",
                "",
                "        // example of 2 KeyWords(new and virtual), usually here would be just virtual",
                "        public new virtual string ToString()",
                "        {",
                "            return $\"({Real:0.00}, {Imaginary:0.00})\";",
                "        }",
                "    }",
                "}"
            };
            string text = GetText(lines);
            return text;
        }

        [Fact]
        public void ShouldWriteEntityUserFile()
        {
            FileModel userFile = GetEntityUserFile();
            string result = userFile.ToString();

            string text = GetUserFileText();

            Assert.Equal(result, text);
        }

        private FileModel GetEntityUserFile()
        {
            string fileNameSpace = "CsCodeGenerator.Tests";
            string userText = "User";
            string textFirstName = "FirstName"; //nameof(User.FirstName);

            // Properties
            var userProperties = new List<Property>
            {
                new Property(BuiltInDataType.Int, "UserId"),
                new Property(BuiltInDataType.String, "FirstName"),
                new Property(BuiltInDataType.String, "FamilyName"),
                new Property(BuiltInDataType.String, "Address"),
                new Property(CommonDataType.DateTime.ToString(), "DateOfBirth"),
                new Property(BuiltInDataType.String, "FullName") { IsGetOnly = true, IsAutoImplemented = false, GetterBody = "FirstName + FamilyName" }
            };
            
            var lastNameColumAttributeParams = new List<Parameter>
            {
                new Parameter(@"""LastName"""),
                new Parameter() { Name = "Order", Value = "2" }
            };

            userProperties.Single(a => a.Name == "UserId").AddAttribute(new AttributeModel("Key"));
            userProperties.Single(a => a.Name == "FirstName").AddAttribute(new AttributeModel("Column") { SingleParameter = new Parameter { Name = "Order", Value = "1" } });
            userProperties.Single(a => a.Name == "FamilyName").AddAttribute(new AttributeModel("Column") { Parameters = lastNameColumAttributeParams });
            userProperties.Single(a => a.Name == "FullName").AddAttribute(new AttributeModel("NotMapped"));

            var userTableAttributeParams = new List<Parameter>
            {
                new Parameter(@"""User"""),
                new Parameter() { Name = "Schema", Value = @"""tmp""" }
            };

            /*var methods = new List<Method>
            {
                new Method(BuiltInDataType.Bool, "TestMetod")
                {
                    BodyLines = new List<string>
                    {
                        "var check = true;",
                        "if(checked == true)",
                        "{",
                        new String(' ', CsGenerator.DefaultTabSize) + "checked = false;",
                        "}",
                        "return check;"
                    }
                }
            };*/

            // Class
            ClassModel userClass = new ClassModel(userText);
            userClass.SingleKeyWord = KeyWord.Partial;
            userClass.AddAttribute(new AttributeModel("Table") { Parameters = userTableAttributeParams });
            userClass.Properties = userProperties;
            //userClass.Methods = methods;

            var usingDirectives = new List<string>
            {
                "System;",
                "System.ComponentModel.DataAnnotations;",
                "System.ComponentModel.DataAnnotations.Schema;"
            };

            // File
            FileModel userFile = new FileModel(userText);
            userFile.LoadUsingDirectives(usingDirectives);
            userFile.Namespace = fileNameSpace;
            userFile.Classes.Add(userClass);

            return userFile;
        }

        private string GetUserFileText()
        {
            var lines = new List<string>
            {
                "using System;",
                "using System.ComponentModel.DataAnnotations;",
                "using System.ComponentModel.DataAnnotations.Schema;",
                "",
                "namespace CsCodeGenerator.Tests",
                "{",
                "    [Table(\"User\", Schema = \"tmp\")]",
                "    public partial class User",
                "    {",
                "        [Key]",
                "        public int UserId { get; set; }",
                "",
                "        [Column(Order = 1)]",
                "        public string FirstName { get; set; }",
                "",
                "        [Column(\"LastName\", Order = 2)]",
                "        public string FamilyName { get; set; }",
                "",
                "        public string Address { get; set; }",
                "",
                "        public DateTime DateOfBirth { get; set; }",
                "",
                "        [NotMapped]",
                "        public string FullName",
                "        {",
                "            get { return FirstName + FamilyName; }",
                "        }",
                "    }",
                "}"
            };
            string text = GetText(lines);
            return text;
        }

        // VS has Bug that lockes files on writing
        // : Severity Code Description Project File Line Cannot open '.pdb' because it is being used by another process...
        
        /*[Theory]
        [InlineData(null)]
        [InlineData("Generated")]*/
        public void ShouldWriteToDiskEntityUserFile(string directory)
        {
            CsGenerator csGenerator = new CsGenerator();
            csGenerator.OutputDirectory = directory ?? csGenerator.OutputDirectory;

            FileModel userFile = GetEntityUserFile();
            csGenerator.Files.Add(userFile);

            csGenerator.CreateFiles();
            List<string> fileLines = ReadFileFromDisk(csGenerator.Path);

            string result = userFile.ToString();
            
            string text = GetText(fileLines);

            Assert.Equal(result, text);
        }

        private List<string> ReadFileFromDisk(string path)
        {
            throw new NotImplementedException();
        }
    }
}
