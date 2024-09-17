using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CsCodeGenerator
{
    public static class FluentExtensions
    {
        #region BASE --------------------------------------------------------------------------------------
        public static BaseElement WithName(this BaseElement baseElement, string name)
        {
            baseElement.Name = name;
            return baseElement;
        }

        public static BaseElement WithKeyWord(this BaseElement baseElement, KeyWord keyWord)
        {
            baseElement.KeyWords.Add(keyWord);
            return baseElement;
        }

        public static BaseElement WithComment(this BaseElement baseElement, string comment)
        {
            baseElement.Comment = comment;
            return baseElement;
        }

        public static BaseElement WithIndent(this BaseElement baseElement, int indentSize)
        {
            baseElement.IndentSize = indentSize;
            return baseElement;
        }

        public static BaseElement AddIndent(this BaseElement baseElement)
        {
            baseElement.IndentSize += CsGenerator.DefaultTabSize;
            return baseElement;
        }

        public static BaseElement WithAttribute(this BaseElement baseElement, string name)
        {
            var component = new AttributeModel(name);
            baseElement.Attributes.Add(component);
            return baseElement;
        }
        public static BaseElement WithAttribute(this BaseElement baseElement, AttributeModel attribute)
        {
            baseElement.Attributes.Add(attribute);
            return baseElement;
        }
        #endregion

        #region CLASS -------------------------------------------------------------------------------------
        public static BaseElement WhichInherits(this BaseElement baseElement, string baseClass)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WhichInherits), nameof(ClassModel) + " or " + nameof(EnumModel)); // TODO Add BaseEnum and support for it

            (baseElement as ClassModel).BaseClass = baseClass;
            return baseElement;
        }
        public static BaseElement WhichImplements(this BaseElement baseElement, string interfaceName)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WhichImplements));

            (baseElement as ClassModel).Interfaces.Add(interfaceName);
            return baseElement;
        }

        public static BaseElement WithConstructor(this BaseElement baseElement, BaseElement constructor)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithConstructor));

            (baseElement as ClassModel).Constructors.Add(constructor as Constructor);
            return baseElement;
        }
        public static BaseElement WithConstructor(this BaseElement baseElement, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithConstructor));

            var constructor = new Constructor(name);
            (baseElement as ClassModel).Constructors.Add(constructor);
            return baseElement;
        }

        public static BaseElement WithField(this BaseElement baseElement, BaseElement field)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithField));

            (baseElement as ClassModel).Fields.Add(field as Field);
            return baseElement;
        }
        public static BaseElement WithField(this BaseElement baseElement, BuiltInDataType builtInDataType, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithField));

            var field = new Field(builtInDataType, name);
            (baseElement as ClassModel).Fields.Add(field);
            return baseElement;
        }
        public static BaseElement WithField(this BaseElement baseElement, string customDataType, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithField));

            var field = new Field(customDataType, name);
            (baseElement as ClassModel).Fields.Add(field);
            return baseElement;
        }

        public static BaseElement WithProperty(this BaseElement baseElement, BaseElement property)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithProperty));

            (baseElement as ClassModel).Properties.Add(property as Property);
            return baseElement;
        }
        public static BaseElement WithProperty(this BaseElement baseElement, BuiltInDataType builtInDataType, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithProperty));

            var property = new Property(builtInDataType, name);
            (baseElement as ClassModel).Properties.Add(property);
            return baseElement;
        }
        public static BaseElement WithProperty(this BaseElement baseElement, string customDataType, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithProperty));

            var property = new Property(customDataType, name);
            (baseElement as ClassModel).Properties.Add(property);
            return baseElement;
        }

        // Sample for adding option to call deeper fluent methods (without new instance) as return is child element instead of parent
        /*public static Property WithPropertyReturn(this BaseElement baseElement, BuiltInDataType builtInDataType, string name)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithProperty));

            var property = new Property(builtInDataType, name);
            (baseElement as ClassModel).Properties.Add(property);
            return property;
        }*/

        public static BaseElement WithMethod(this BaseElement baseElement, BaseElement method)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithMethod));

            (baseElement as ClassModel).Methods.Add(method as Method);
            return baseElement;
        }
        public static BaseElement WithMethod(this BaseElement baseElement, BuiltInDataType builtInDataType, string name, AccessModifier? accessModifier = null, KeyWord? singleKeyWord = null)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithMethod));

            var method = new Method(builtInDataType, name);
            method.AccessModifier = accessModifier ?? method.AccessModifier;
            if (singleKeyWord != null)
                method.KeyWords.Add((KeyWord)singleKeyWord);

            (baseElement as ClassModel).Methods.Add(method);
            return baseElement;
        }
        public static BaseElement WithMethod(this BaseElement baseElement, string customDataType, string name, AccessModifier? accessModifier = null, KeyWord? singleKeyWord = null)
        {
            ValidateFluentCall<ClassModel>(baseElement, nameof(WithMethod));

            var method = new Method(customDataType, name);
            method.AccessModifier = accessModifier ?? method.AccessModifier;
            if (singleKeyWord != null)
                method.KeyWords.Add((KeyWord)singleKeyWord);

            (baseElement as ClassModel).Methods.Add(method);
            return baseElement;
        }

        public static BaseElement AddIndent(this BaseElement baseElement, IndentType indentType = IndentType.Single)
        {
            baseElement.FluentIndentSize += (int)indentType * CsGenerator.DefaultTabSize;
            return baseElement;
        }
        public static BaseElement RemoveIndent(this BaseElement baseElement, IndentType indentType = IndentType.Single)
        {
            baseElement.FluentIndentSize -= (int)indentType * CsGenerator.DefaultTabSize;
            return baseElement;
        }
        public static Method AddIndent(this Method baseElement, IndentType indentType = IndentType.Single)
        {
            baseElement.FluentIndentSize += (int)indentType * CsGenerator.DefaultTabSize;
            return baseElement;
        }
        public static Method RemoveIndent(this Method baseElement, IndentType indentType = IndentType.Single)
        {
            baseElement.FluentIndentSize -= (int)indentType * CsGenerator.DefaultTabSize;
            return baseElement;
        }

        #endregion

        #region PROPERTY ---------------------------------------------------------------------------------
        public static Property WithGetter(this Property baseElement, string getterBody)
        {
            ValidateFluentCall<Property>(baseElement, nameof(WithGetter));

            baseElement.GetterBody = getterBody;
            return baseElement;
        }
        public static Property WithSetter(this Property baseElement, string setterBody)
        {
            ValidateFluentCall<Property>(baseElement, nameof(WithSetter));

            baseElement.SetterBody = setterBody;
            return baseElement;
        }
        #endregion

        #region PARAMETER ---------------------------------------------------------------------------------
        public static AttributeModel WithParameter(this AttributeModel baseElement, Parameter parameter)
        {
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }
        public static AttributeModel WithParameter(this AttributeModel baseElement, string value)
        {
            var parameter = new Parameter(value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }

        public static AttributeModel WithParameter(this AttributeModel baseElement, string name, string value)
        {
            var parameter = new Parameter(name, value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }

        public static AttributeModel WithParameter(this AttributeModel baseElement, BuiltInDataType builtInDataType, string name, string value)
        {
            var parameter = new Parameter(builtInDataType, name, value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }
        public static AttributeModel WithParameter(this AttributeModel baseElement, string customDataType, string name, string value)
        {
            var parameter = new Parameter(customDataType, name, value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }

        public static Method WithParameter(this Method baseElement, Parameter parameter)
        {
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }
        public static Method WithParameter(this Method baseElement, BuiltInDataType builtInDataType, string name, string value)
        {
            var parameter = new Parameter(builtInDataType, name, value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }
        public static Method WithParameter(this Method baseElement, string customDataType, string name, string value)
        {
            var parameter = new Parameter(customDataType, name, value);
            baseElement.Parameters.Add(parameter);
            return baseElement;
        }

        public static Method WithBodyLine(this Method baseElement, string bodyLine)
        {
            baseElement.BodyLines.Add(baseElement.FluentIndent + bodyLine);
            return baseElement;
        }
        public static Method WithBodyLines(this Method baseElement, List<string> bodyLines)
        {
            if (baseElement.FluentIndentSize == 0)
            {
                baseElement.BodyLines.AddRange(bodyLines);
            }
            else
            {
                foreach (var bodyLine in bodyLines)
                {
                    baseElement.BodyLines.Add(baseElement.FluentIndent + bodyLine);
                }
            }
            return baseElement;
        }
        #endregion

        #region VALIDATION --------------------------------------------------------------------------------
        public static void ValidateFluentCall<T>(BaseElement baseElement, string methodName, string validModels = null) where T : class, new()
        {
            if (!(baseElement is T))
            {
                var validModel = new T().GetType().Name;
                var message = $"Method '{methodName}' called on baseElement '{baseElement.Name}' on can only be used on {validModels ?? validModel}.";
                throw new InvalidOperationException(message);
            }
        }
        #endregion
    }
}
