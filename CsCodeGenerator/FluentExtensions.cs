using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CsCodeGenerator
{
    public static class FluentExtensions
    {
        // BASE ---------------------------------------
        public static BaseElement WithName(this BaseElement element, string name)
        {
            element.Name = name;
            return element;
        }

        /*public static BaseElement WithBuiltInDataType(this BaseElement element, BuiltInDataType builtInDataType, string name)
        {
            element.BuiltInDataType = builtInDataType;
            element.Name = name;
            return element;
        }

        public static BaseElement WithCustomDataType(this BaseElement element, string customDataType, string name)
        {
            element.CustomDataType = customDataType;
            element.Name = name;
            return element;
        }*/

        public static BaseElement WithKeyWord(this BaseElement element, KeyWord keyWord)
        {
            element.KeyWords.Add(keyWord);
            return element;
        }

        public static BaseElement WithComment(this BaseElement element, string comment)
        {
            element.Comment = comment;
            return element;
        }

        public static BaseElement WithIndent(this BaseElement element, int indentSize)
        {
            element.IndentSize = indentSize;
            return element;
        }

        public static BaseElement AddIndent(this BaseElement baseElement)
        {
            baseElement.IndentSize += CsGenerator.DefaultTabSize;
            return baseElement;
        }

        public static BaseElement WithAttribute(this BaseElement element, AttributeModel attribute)
        {
            element.Attributes.Add(attribute);
            return element;
        }
        public static BaseElement WithAttribute(this BaseElement element, string name)
        {
            var attribute = new AttributeModel(name);
            element.Attributes.Add(attribute);
            return element;
        }
        // BASE END -----------------------------------

        // CLASS --------------------------------------
        public static BaseElement WhichInherits(this BaseElement element, string baseClass)
        {
            ValidateFluentCall(element, nameof(WhichInherits), nameof(ClassModel) + " or " + nameof(EnumModel)); // // TODO Add BaseEnum and support for it

            (element as ClassModel).BaseClass = baseClass;
            return element;
        }
        public static BaseElement WhichImplements(this BaseElement element, string interfaceName)
        {
            ValidateFluentCall(element, nameof(WhichImplements), nameof(ClassModel));

            (element as ClassModel).Interfaces.Add(interfaceName);
            return element;
        }

        public static BaseElement WithConstructor(this BaseElement element, BaseElement constructor)
        {
            ValidateFluentCall(element, nameof(WithConstructor), nameof(ClassModel));

            (element as ClassModel).Constructors.Add(constructor as Constructor);
            return element;
        }

        public static BaseElement WithProperty(this BaseElement element, BaseElement property)
        {
            ValidateFluentCall(element, nameof(WithProperty), nameof(ClassModel));

            (element as ClassModel).Properties.Add(property as Property);
            return element;
        }

        public static BaseElement WithProperty(this BaseElement element, BuiltInDataType builtInDataType, string name)
        {
            ValidateFluentCall(element, nameof(WithProperty), nameof(ClassModel));

            var property = new Property(builtInDataType, name);
            (element as ClassModel).Properties.Add(property);
            return element;
        }

        public static BaseElement WithProperty(this BaseElement element, string customDataType, string name)
        {
            ValidateFluentCall(element, nameof(WithProperty), nameof(ClassModel));

            var property = new Property(customDataType, name);
            (element as ClassModel).Properties.Add(property);
            return element;
        }

        public static BaseElement WithMethod(this BaseElement element, Method method)
        {
            ((element as ClassModel)).Methods.Add(method);
            return method;
        }
        // CLASS END-----------------------------------


        // PARAM --------------------------------------
        public static AttributeModel WithParameter(this AttributeModel element, Parameter parameter)
        {
            element.Parameters.Add(parameter);
            return element;
        }
        public static AttributeModel WithParameter(this AttributeModel element, string value)
        {
            var parameter = new Parameter(value);
            element.Parameters.Add(parameter);
            return element;
        }

        public static AttributeModel WithParameter(this AttributeModel element, string name, string value)
        {
            var parameter = new Parameter(name, value);
            element.Parameters.Add(parameter);
            return element;
        }

        public static AttributeModel WithParameter(this AttributeModel element, BuiltInDataType builtInDataType, string name, string value)
        {
            var parameter = new Parameter(builtInDataType, name, value);
            element.Parameters.Add(parameter);
            return element;
        }
        public static AttributeModel WithParameter(this AttributeModel element, string customDataType, string name, string value)
        {
            var parameter = new Parameter(customDataType, name, value);
            element.Parameters.Add(parameter);
            return element;
        }

        // PARAM END-----------------------------------

        // VALIDATION----------------------------------
        public static void ValidateFluentCall(BaseElement element, string methodName, string validModels)
        {
            if (!(element is ClassModel))
            {
                var message = $"Method '{methodName}' called on element '{element.Name}' on can only be used on {validModels}.";
                throw new InvalidOperationException(message);
            }
        }
        // --------------------------------------------
    }
}
