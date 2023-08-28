using System;

namespace FractionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculadora de Fracciones");
            
            Console.Write("Ingrese la primera fracción (num/den): ");
            string input1 = Console.ReadLine();
            
            Console.Write("Ingrese el operador (+, -, *, /): ");
            char oper = Console.ReadKey().KeyChar;
            Console.WriteLine();
            
            Console.Write("Ingrese la segunda fracción (num/den): ");
            string input2 = Console.ReadLine();
            
            if (!Fraction.TryParse(input1, out Fraction fraction1) || !Fraction.TryParse(input2, out Fraction fraction2))
            {
                Console.WriteLine("Error: Ingrese fracciones válidas en el formato num/den.");
                return;
            }

            Fraction result = new Fraction();

            switch (oper)
            {
                case '+':
                    result = fraction1 + fraction2;
                    break;
                case '-':
                    result = fraction1 - fraction2;
                    break;
                case '*':
                    result = fraction1 * fraction2;
                    break;
                case '/':
                    if (fraction2.Numerator == 0)
                    {
                        Console.WriteLine("Error: No es posible dividir entre cero.");
                        return;
                    }
                    result = fraction1 / fraction2;
                    break;
                default:
                    Console.WriteLine("Error: Operador no válido.");
                    return;
            }

            Console.WriteLine($"Resultado: {result}");
        }
    }

    public struct Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator != 0 ? denominator : throw new ArgumentException("El denominador no puede ser cero.");
            Simplify();
        }

        public static Fraction Parse(string input)
        {
            string[] parts = input.Split('/');
            int numerator = int.Parse(parts[0]);
            int denominator = int.Parse(parts[1]);
            return new Fraction(numerator, denominator);
        }

        public static bool TryParse(string input, out Fraction fraction)
        {
            fraction = default;
            string[] parts = input.Split('/');
            if (parts.Length != 2)
                return false;

            if (int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator) && denominator != 0)
            {
                fraction = new Fraction(numerator, denominator);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        private void Simplify()
        {
            int gcd = GCD(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;

            if (Denominator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}

