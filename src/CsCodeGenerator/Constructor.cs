using CsCodeGenerator.Enums;
using System;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public class Constructor : Method
    {
        public Constructor(string name)
        {
            base.BuiltInDataType = null;
            base.Name = name;
        }
        
        public override bool IsVisible { get; set; } = true;

        public new string Name => base.Name;

        public new BuiltInDataType? BuiltInDataType => base.BuiltInDataType;
        public new string CustomDataType => base.CustomDataType;

        public override string Signature => AccessFormated + Name + Parameters.ToStringList();
    }
}
