using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelengParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "GoSample.txt";
            
            if (File.Exists(path))
            {
                string input = File.ReadAllText(path);
                {
                    Lexer lexer = new Lexer();
                    var resultTokens = lexer.Tokenize(input);

                    GoParser parser = new GoParser();
                    Dictionary<string, GoObject> instance = parser.SetAndValidateInput(resultTokens);
                    
                    //object jsonResult = parser.GetRandomObject();
                }
            }

            Console.WriteLine("Pasa un archivo valido toga");
           
        }
    }
}
