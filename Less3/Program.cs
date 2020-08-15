using System;
using System.Collections.Generic;
using System.Text;

namespace Less3
{
    class Program
    {
        private static UserGenerator userGenerator;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<String> result = new List<string>();
            if (args.Length > 0) userGenerator = new UserGenerator(args[0]);

            if (args.Length == 2)
                result = TwoArgsGenerate(args);
            else if (args.Length == 3)
                result = ThreeArgsGenerate(args);

            result.ForEach(Console.WriteLine);
        }

        private static List<String> TwoArgsGenerate(string[] args) => 
            (Int32.TryParse(args[1], out int count) && count >= 0) ?
                userGenerator.GenerateUser(count) 
            : new List<string>();

        private static List<String> ThreeArgsGenerate(string[] args) => 
            (Double.TryParse(args[2], out double mistakeCount) && mistakeCount >= 0 
                && Int32.TryParse(args[1], out int count) && count >= 0)
                ? userGenerator.GenerateUser(count, mistakeCount)
                : new List<string>();
    }
}
