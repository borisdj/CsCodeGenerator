using System;
using System.ComponentModel;

namespace CsCodeGenerator.Tests
{
    [Description("Some class info")]
    public partial class ComplexNumber
    {
        protected const double PI = 3.14;
        private string remark;

        public ComplexNumber() { }

        public ComplexNumber(double real, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static string DefaultFormat { get; } = "a + b * i";

        public double Real { get; set; }

        public double Imaginary { get; set; }

        public virtual string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public double Modul()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public ComplexNumber Add(ComplexNumber input)
        {
            ComplexNumber result = new ComplexNumber();
            result.Real = Real + input.Real;
            result.Imaginary = Imaginary + input.Imaginary;
            return result;
        }

        // example of 2 KeyWords(new and virtual), usually here would be just virtual
        public new virtual string ToString()
        {
            return $"({Real:0.00}, {Imaginary:0.00})";
        }
    }
}
