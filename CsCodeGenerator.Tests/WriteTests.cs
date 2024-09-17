using CsCodeGenerator.Enums;
using System.Diagnostics;
using Xunit;

namespace CsCodeGenerator.Tests
{
    // CONSOLE: dotnet new -t xunittest  // create
    //          dotnet test              // run
    // VS: Test -> Windows -> Test Explorer
    public class WriteTests
    {
        [Fact]
        public void ShouldWriteFields()
        {
            // Fields
            var fields = new List<Field>
            {
                new (BuiltInDataType.Int, "field1"),
                new (BuiltInDataType.Decimal, "field2") { AccessModifier = AccessModifier.Private, DefaultValue = "2.0m" },
                new (BuiltInDataType.String, "field3") { SingleKeyWord = KeyWord.Static, DefaultValue = @"""text""" },
                new (CommonDataType.DateTime.ToString(), "field4") { IndentSize = 0 },
                new ("List<Guid>", "field5") { Comment = "Remark: List used for Ids.", IndentSize = (int)IndentType.Single * CsGenerator.DefaultTabSize },
            };
            string result = string.Join("", fields) + Util.NewLine;

            string text = GetFieldsText();

            Assert.Equal(text, result);
        }

        protected string GetFieldsText()
        {
            var lines = new List<string>
            {
                "",
                "        protected int field1;",
                "        private decimal field2 = 2.0m;",
                "        protected static string field3 = \"text\";",
                "protected DateTime field4;",
                "    /// <summary>",
                "    // Remark: List used for Ids.",
                "    /// <summary>",
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
                new (BuiltInDataType.Int, "Property1"),
                new (BuiltInDataType.Decimal, "Property2") { AccessModifier = AccessModifier.Protected, DefaultValue = "2.0m" },
                new (BuiltInDataType.String, "Property3") { SingleKeyWord = KeyWord.Static, DefaultValue = @"""text""" },
                new (CommonDataType.DateTime.ToString(), "Property4"),
                new ("List<Guid>", "Property5")
                {
                    Comment = "Remark: Manual implemented Property.",
                    IsAutoImplemented = false,
                    GetterBody = "field5",
                    SetterBody = "field5 = value"
                }
            };
            properties[0].AddAttribute(new AttributeModel("Key"));
            string result = string.Join("", properties) + Util.NewLine;

            string text = GetPropertiesText();

            Assert.Equal(text, result);
        }

        protected static string GetPropertiesText()
        {
            var lines = new List<string>
            {
                "",
                "        [Key]",
                "        public int Property1 { get; set; }",
                "        protected decimal Property2 { get; set; } = 2.0m;",
                "        public static string Property3 { get; set; } = \"text\";",
                "        public DateTime Property4 { get; set; }",
                "        /// <summary>",
                "        // Remark: Manual implemented Property.",
                "        /// <summary>",
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
                new (BuiltInDataType.Void, "Method1")
                {
                    BodyLines = ["throw new NotImplementedException();"],
                    BracesInNewLine = false
                },
                new (AccessModifier.Protected, KeyWord.Virtual, BuiltInDataType.Bool, "IsOdd")
                {
                    BodyLines = ["return value % 2 == 1;"]
                }
            };

            methods.Single(a => a.Name == "IsOdd").Parameters.Add(new Parameter(BuiltInDataType.Int, "value", null));
            methods.Single(a => a.Name == "IsOdd").AddAttribute(new AttributeModel("Authorize"));

            string result = string.Join(Util.NewLine, methods) + Util.NewLine;

            string text = GetMethodsText();

            Assert.Equal(text, result);
        }

        protected static string GetMethodsText()
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
            var nestedClass = new ClassModel ("MyNestedClass");
            nestedClass.IndentSize += CsGenerator.DefaultTabSize; // increase indent to every nested element
            nestedClass.DefaultConstructor.IsVisible = true;
            nestedClass.DefaultConstructor.IndentSize += CsGenerator.DefaultTabSize; // increase indent to every nested element
            nestedClass.Comment = "Some Comment";

            var parentClass = new ClassModel ("ParentClass")
            {
                BaseClass = "BaseClass"
            };
            parentClass.Interfaces.Add("ParentInterface");
            parentClass.NestedClasses.Add(nestedClass);

            string result = parentClass.ToString() + Util.NewLine;

            string text = GetNestedClassText();

            Assert.Equal(text, result);
        }

        protected static string GetNestedClassText()
        {
            var lines = new List<string>
            {
                "",
                "    public class ParentClass : BaseClass, ParentInterface",
                "    {",
                "",
                "        /// <summary>",
                "        // Some Comment",
                "        /// <summary>",
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
            var myEnumModel = new EnumModel("MyEnumModel");
            myEnumModel.EnumValues.Add(new EnumValue("Val1", 1));
            myEnumModel.EnumValues.Add(new EnumValue("Val2", 2));

            string result = myEnumModel.ToString() + Util.NewLine;

            string text = GetEnumText();

            Assert.Equal(text, result);
        }

        protected static string GetEnumText()
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
            var myInterfaceModel = new InterfaceModel("MyInterfaceModel");
            myInterfaceModel.Properties.Add(new Property(BuiltInDataType.Int, "Prop1"));
            myInterfaceModel.Properties.Add(new Property(BuiltInDataType.String, "Prop2"));

            myInterfaceModel.Methods.Add(new Method(BuiltInDataType.Void, "Method1"));

            string result = myInterfaceModel.ToString() + Util.NewLine;

            string text = GetInterfaceText();

            Debug.WriteLine(result);
            Assert.Equal(text, result);
        }

        public interface ISomeInt
        {
            int Ba { get; set; }

            void Order();
        }

        protected static string GetInterfaceText()
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

            var secondConstructor = new Constructor(complexNumberClass.Name);
            secondConstructor.Parameters.Add(new Parameter(BuiltInDataType.Double, "real", null));
            secondConstructor.Parameters.Add(new Parameter(BuiltInDataType.Double, "imaginary", null) { Value = "0" });
            secondConstructor.BodyLines.Add("Real = real;");
            secondConstructor.BodyLines.Add("Imaginary = imaginary;");
            complexNumberClass.Constructors.Add(secondConstructor);

            var indent = complexNumberClass.Indent;

            var fields = new List<Field>
            {
                new (BuiltInDataType.Double, "PI") { SingleKeyWord = KeyWord.Const, DefaultValue = "3.14" },
                new (BuiltInDataType.String, "remark") { AccessModifier = AccessModifier.Private },
            };

            var properties = new List<Property>
            {
                new (BuiltInDataType.String, "DefaultFormat")
                {
                    SingleKeyWord = KeyWord.Static,
                    IsGetOnly = true,
                    DefaultValue = @"""a + b * i"""
                },
                new (BuiltInDataType.Double, "Real"),
                new (BuiltInDataType.Double, "Imaginary"),
                new (BuiltInDataType.String, "Remark")
                {
                    SingleKeyWord = KeyWord.Virtual,
                    IsAutoImplemented = false,
                    GetterBody = "remark",
                    SetterBody = "remark = value"

                },
            };

            var methods = new List<Method>
            {
                new (BuiltInDataType.Double, "Modul")
                {
                    BodyLines = ["return Math.Sqrt(Real * Real + Imaginary * Imaginary);"]
                },
                new (complexNumberText, "Add")
                {
                    Parameters = [ new Parameter("ComplexNumber", "input", null) ],
                    BodyLines = 
                    [
                        "ComplexNumber result = new ComplexNumber",
                        "{",
                        $"{indent}Real = Real + input.Real,",
                        $"{indent}Imaginary = Imaginary + input.Imaginary,",
                        "};",
                        "return result;"
                    ]
                },
                new (BuiltInDataType.String, "ToString")
                {
                    Comment = "example of 2 KeyWords(new and virtual), usually here would be just virtual",
                    KeyWords = [KeyWord.New, KeyWord.Virtual],
                    BodyLines = ["return $\"({Real:0.00}, {Imaginary:0.00})\";"]
                }
            };

            complexNumberClass.Fields = fields;
            complexNumberClass.Properties = properties;
            complexNumberClass.Methods = methods;

            var complexNumberFile = new FileModel(complexNumberText);
            complexNumberFile.LoadUsingDirectives(usingDirectives);
            complexNumberFile.Namespace = fileNameSpace;
            complexNumberFile.Classes.Add(complexNumberClass);

            var csGenerator = new CsGenerator();
            csGenerator.Files.Add(complexNumberFile);
            //csGenerator.CreateFiles(); //Console.Write(complexNumberFile); 

            string result = complexNumberFile.ToString();

            string text = GetComplexNumberFileText();

            Debug.Write(result);
            Assert.Equal(text, result);
        }

        [Fact]
        private void WriteComplexNumberFileWithFluent()
        {
            var usingDirectives = new List<string>
            {
                "System;",
                "System.ComponentModel;"
            };
            string fileNameSpace = "CsCodeGenerator.Tests";
            string complexNumberText = "ComplexNumber";

            // Class
            var classModel = new ClassModel();
            var indent = classModel.Indent;

            var complexNumberClass = (ClassModel)classModel
                .WithAttribute(new AttributeModel("Description")
                    .WithParameter(@"""Some class info"""))
                .WithKeyWord(KeyWord.Partial)
                .WithName(complexNumberText)
                .WithConstructor(new Constructor(complexNumberText) { BracesInNewLine = false })
                .WithConstructor(new Constructor(complexNumberText)
                    .WithParameter(BuiltInDataType.Double, "real", null)
                    .WithParameter(BuiltInDataType.Double, "imaginary", "0")
                    .WithBodyLine("Real = real;")
                    .WithBodyLine("Imaginary = imaginary;"))
                .WithField(new Field(BuiltInDataType.Double, "PI") { SingleKeyWord = KeyWord.Const, DefaultValue = "3.14" })
                .WithField(new Field(BuiltInDataType.String, "remark") { AccessModifier = AccessModifier.Private })
                .WithProperty(new Property(BuiltInDataType.String, "DefaultFormat") 
                    { SingleKeyWord = KeyWord.Static, IsGetOnly = true, DefaultValue = @"""a + b * i""" })
                .WithProperty(BuiltInDataType.Double, "Real")
                .WithProperty(BuiltInDataType.Double, "Imaginary")
                .WithProperty(new Property(BuiltInDataType.String, "Remark")
                    { SingleKeyWord = KeyWord.Virtual, IsAutoImplemented = false }
                    .WithGetter("remark")
                    .WithSetter("remark = value"))
                .WithMethod(new Method(BuiltInDataType.Double, "Modul")
                    .WithBodyLine("return Math.Sqrt(Real * Real + Imaginary * Imaginary);"))
                .WithMethod(new Method(complexNumberText, "Add")
                    .WithParameter(complexNumberText, "input", null)
                    .WithBodyLine("ComplexNumber result = new ComplexNumber") // WAY 2
                    .WithBodyLine("{")
                    .AddIndent()
                        .WithBodyLine("Real = Real + input.Real,")
                        .WithBodyLine("Imaginary = Imaginary + input.Imaginary,")
                    .RemoveIndent()
                    .WithBodyLine("};")
                    .WithBodyLine("return result;")
                    /*.WithBodyLines([ // WAY 2 alternatively
                     "ComplexNumber result = new ComplexNumber",
                     "{",
                        $"{indent}Real = Real + input.Real,",
                        $"{indent}Imaginary = Imaginary + input.Imaginary,",
                     "};",
                     "return result;"
                     ])*/
                 )
                .WithMethod(new Method(BuiltInDataType.String, "ToString") { 
                                KeyWords = [KeyWord.New, KeyWord.Virtual],
                                Comment = "example of 2 KeyWords(new and virtual), usually here would be just virtual" }
                    .WithBodyLine("return $\"({Real:0.00}, {Imaginary:0.00})\";"));

            var complexNumberFile = new FileModel(complexNumberText);
            complexNumberFile.LoadUsingDirectives(usingDirectives);
            complexNumberFile.Namespace = fileNameSpace;
            complexNumberFile.Classes.Add(complexNumberClass);

            var csGenerator = new CsGenerator();
            csGenerator.Files.Add(complexNumberFile);
            //csGenerator.CreateFiles(); //Console.Write(complexNumberFile); 

            string result = complexNumberFile.ToString();

            string text = GetComplexNumberFileText();

            Debug.Write(result);
            Assert.Equal(text, result);
        }

        protected static string GetComplexNumberFileText()
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
                "            ComplexNumber result = new ComplexNumber",
                "            {",
                "                Real = Real + input.Real,",
                "                Imaginary = Imaginary + input.Imaginary,",
                "            };",
                "            return result;",
                "        }",
                "",
                "        /// <summary>",
                "        // example of 2 KeyWords(new and virtual), usually here would be just virtual",
                "        /// <summary>",
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
            FileModel userFile = GetEntityUserFileWithFluent(); //GetEntityUserFile
            string result = userFile.ToString();

            string text = GetUserFileText();

            Assert.Equal(text, result);
        }

        protected static FileModel GetEntityUserFile()
        {
            string fileNameSpace = "CsCodeGenerator.Tests";
            string userText = "User";
            //string textFirstName = "FirstName"; //nameof(User.FirstName);

            // Properties
            var userProperties = new List<Property>
            {
                new(BuiltInDataType.Int, "UserId"),
                new(BuiltInDataType.String, "FirstName"),
                new(BuiltInDataType.String, "FamilyName"),
                new(BuiltInDataType.String, "Address"),
                new(CommonDataType.DateTime.ToString(), "DateOfBirth"),
                new(BuiltInDataType.String, "FullName") { IsGetOnly = true, IsAutoImplemented = false, GetterBody = "FirstName + FamilyName" }
            };

            var lastNameColumAttributeParams = new List<Parameter>
            {
                new(@"""LastName"""),
                new() { Name = "Order", Value = "2" }
            };

            userProperties.Single(a => a.Name == "UserId").AddAttribute(new AttributeModel("Key"));
            userProperties.Single(a => a.Name == "FirstName").AddAttribute(new AttributeModel("Column") { SingleParameter = new Parameter { Name = "Order", Value = "1" } });
            userProperties.Single(a => a.Name == "FamilyName").AddAttribute(new AttributeModel("Column") { Parameters = lastNameColumAttributeParams });
            userProperties.Single(a => a.Name == "FullName").AddAttribute(new AttributeModel("NotMapped"));

            var userTableAttributeParams = new List<Parameter>
            {
                new(@"""User"""),
                new() { Name = "Schema", Value = @"""tmp""" }
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

        protected static FileModel GetEntityUserFileWithFluent()
        {
            string fileNameSpace = "CsCodeGenerator.Tests";
            string userText = "User";

            // Class
            var userClass = (ClassModel)new ClassModel()
                .WithAttribute(new AttributeModel("Table")
                    .WithParameter(@"""User""")
                    .WithParameter("Schema", @"""tmp"""))
                .WithKeyWord(KeyWord.Partial)
                .WithName(userText)
                    .WithProperty(new Property(BuiltInDataType.Int, "UserId") // needs 'new Property' when having deeper element such as Att.
                        .WithAttribute("Key"))
                    .WithProperty(new Property(BuiltInDataType.String, "FirstName")
                        .WithAttribute(new AttributeModel("Column") 
                            .WithParameter("Order", "1")))
                    .WithProperty(new Property(BuiltInDataType.String, "FamilyName")
                        .WithAttribute(new AttributeModel("Column")
                            .WithParameter(@"""LastName""")
                            .WithParameter("Order", "2")))
                    .WithProperty(BuiltInDataType.String, "Address") // no need for 'new Property', can be set with args directly, no deeper element
                    .WithProperty(CommonDataType.DateTime.ToString(), "DateOfBirth")
                    .WithProperty(new Property(BuiltInDataType.String, "FullName") { IsGetOnly = true, IsAutoImplemented = false }
                        .WithGetter("FirstName + FamilyName")    
                        .WithAttribute("NotMapped"));

            var textSystem = "System";
            var textSystem_ComponentModel = $"{textSystem}.ComponentModel";
            var textSystem_ComponentModel_DataAnnotations = $"{textSystem_ComponentModel}.DataAnnotations";
            var textSystem_ComponentModel_DataAnnotations_Schema = $"{textSystem_ComponentModel_DataAnnotations}.Schema";
            var usingDirectives = new List<string>
            {
                textSystem + ";",
                textSystem_ComponentModel_DataAnnotations + ";",
                textSystem_ComponentModel_DataAnnotations_Schema + ";",
            };

            // File
            var userFile = new FileModel(userText);
            userFile.LoadUsingDirectives(usingDirectives);
            userFile.Namespace = fileNameSpace;
            userFile.Classes.Add(userClass);

            return userFile;
        }

        protected static string GetUserFileText()
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

        //[Theory]
        //[InlineData(null)]
        //[InlineData("Generated")]
        public static void ShouldWriteToDiskEntityUserFile(string directory) // TODO
        {
            CsGenerator csGenerator = new CsGenerator();
            csGenerator.OutputDirectory = directory ?? csGenerator.OutputDirectory;

            FileModel userFile = GetEntityUserFile();
            csGenerator.Files.Add(userFile);

            csGenerator.CreateFiles();
            List<string> fileLines = ReadFileFromDisk(csGenerator.Path);

            string result = userFile.ToString();

            string text = GetText(fileLines);

            Assert.Equal(text, result);
        }

        protected static string GetText(List<string> lines)
        {
            string text = "";
            foreach (var line in lines)
            {
                text += line + Util.NewLine;
            }
            return text;
        }

        private List<string> ReadFileFromDisk(string path)
        {
            throw new NotImplementedException();
        }
    }
}
