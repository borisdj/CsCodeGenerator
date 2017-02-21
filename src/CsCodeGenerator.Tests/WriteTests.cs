﻿using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
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
                new Field() { CustomDataType = CommonDataType.DateTime.ToString(), Name = "field4", IndentSize = 0 },
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
                new Property() { CustomDataType = CommonDataType.DateTime.ToString(), Name = "Property4" },
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
            }.ToDictionary(a => a.Name, a => a);

            methods["IsOdd"].Parameters.Add(new Parameter(BuiltInDataType.Int, "value"));
            methods["IsOdd"].AddAttribute(new AttributeModel("Authorize"));

            string result = String.Join(Util.NewLine, methods.Values) + Util.NewLine;

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
            nestedClass.Constructor.IsVisible = true;
            nestedClass.Constructor.IndentSize += CsGenerator.DefaultTabSize; // increase indent to every nested element
            nestedClass.Comment = "Some Comment";

            ClassModel parentClass = new ClassModel("ParentClass");
            parentClass.NestedClasses.Add(nestedClass.Name, nestedClass);

            string result = parentClass.ToString() + Util.NewLine;

            string text = GetNestedClassText();

            Assert.Equal(result, text);
        }

        private string GetNestedClassText()
        {
            var lines = new List<string>
            {
                "",
                "    public class ParentClass",
                "    {",
                "",
                "        // Some Comment",
                "        public class MyNestedClass",
                "        {",
                "            public MyNestedClass()",
                "            {",
                "            }",
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
            myEnumModel.EnumValues.Add("Val1", new EnumValue("Val1", 1));
            myEnumModel.EnumValues.Add("Val2", new EnumValue("Val2", 2));

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
        public void ShouldWriteEntityUserFile()
        {
            FileModel userFile = GetEntityUserFile();
            string result = userFile.ToString();

            string text = GetUserFileText();

            Assert.Equal(result, text);
        }

        private FileModel GetEntityUserFile()
        {
            string fileNameSpace = $"{Util.Namespace} CsCodeGenerator.Tests";
            string userText = "User";

            // Properties
            var userProperties = new Property[]
            {
                new Property(BuiltInDataType.Int, "UserId"),
                new Property(BuiltInDataType.String, "FirstName"),
                new Property(BuiltInDataType.String, "FamilyName"),
                new Property(BuiltInDataType.String, "Address"),
                new Property(CommonDataType.DateTime.ToString(), "DateOfBirth"),
                new Property(BuiltInDataType.String, "FullName") { IsGetOnly = true, IsAutoImplemented = false, GetterBody = "FirstName + FamilyName" }
            }.ToDictionary(a => a.Name, a => a);

            var firstNameColumnAttributeParams = new List<Parameter> { new Parameter() { Name = "Order", Value = "1" } };
            var lastNameColumAttributeParams = new List<Parameter> { new Parameter(@"""LastName"""), new Parameter() { Name = "Order", Value = "2" } };

            userProperties["UserId"].AddAttribute(new AttributeModel("Key"));
            userProperties["FirstName"].AddAttribute(new AttributeModel("Column") { Parameters = firstNameColumnAttributeParams });
            userProperties["FamilyName"].AddAttribute(new AttributeModel("Column") { Parameters = lastNameColumAttributeParams });
            userProperties["FullName"].AddAttribute(new AttributeModel("NotMapped"));

            var userTableAttributeParams = new List<Parameter> { new Parameter(@"""User"""), new Parameter() { Name = "Schema", Value = @"""tmp""" } };

            // Class
            ClassModel userClass = new ClassModel("User");
            userClass.KeyWords.Add(KeyWord.Partial);
            userClass.AddAttribute(new AttributeModel("Table") { Parameters = userTableAttributeParams });
            userClass.Properties = userProperties;

            var usingDirectives = new List<string>
            {
                "using System;",
                "using System.ComponentModel.DataAnnotations;",
                "using System.ComponentModel.DataAnnotations.Schema;"
            };

            // File
            FileModel userFile = new FileModel();
            userFile.LoadUsingDirectives(usingDirectives);
            userFile.Namespace = fileNameSpace;
            userFile.Name = userText;
            userFile.Classes.Add(userClass.Name, userClass);

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
        [InlineData("Generated")]
        public void ShouldWriteToDiskEntityUserFile(string directory)
        {
            CsGenerator csGenerator = new CsGenerator();
            csGenerator.OutputDirectory = directory ?? csGenerator.OutputDirectory;

            FileModel userFile = GetEntityUserFile();
            csGenerator.Files.Add(userFile.Name, userFile);

            csGenerator.CreateFiles();
            List<string> fileLines = ReadFileFromDisk(csGenerator.Path);

            string result = userFile.ToString();
            
            string text = GetText(fileLines);

            Assert.Equal(result, text);
        }

        private List<string> ReadFileFromDisk(string path)
        {
            throw new NotImplementedException();
        }*/
    }
}