using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelengParser.GoData;

namespace TelengParser
{
    class Program
    {
        static void Main(string[] args)
        {

            string line, archivo, input, estructura;
            while (true)
            {
                line = null;
                archivo = null;
                input = null;
                estructura = null;
                Console.WriteLine("Escriba la estructura a continuación y al finalizar escriba end." + System.Environment.NewLine +
                    "Si quiere importar desde un archivo guardelo en la misma carpeta del trabajo" + System.Environment.NewLine + 
                    "y escriba por consola \"importar nombrearchivo.txt\"");
                while ((line = Console.ReadLine()) != "end")
                {
                    if (line.Contains("importar"))
                    {
                        archivo = line.Replace("importar", "").Trim();
                        break;
                    }
                    estructura += line + System.Environment.NewLine;
                }
                if (archivo != null)
                {
                    if (File.Exists(archivo))
                        input = File.ReadAllText(archivo);                        
                }
                else
                    input = estructura;
                if (input != null)
                {
                    Console.WriteLine("Generando el JSON...");
                    try
                    {
                        Lexer lexer = new Lexer();
                        var resultTokens = lexer.Tokenize(input);

                        GoParser parser = new GoParser();

                        GoInstance instance = parser.SetAndValidateInput(resultTokens);

                        object jsonResult = instance.GetRandomMainObject();
                        Console.WriteLine(jsonResult.ToString());
                    }catch(Exception ex)
                    {
                        Console.WriteLine("ERROR AL GENERAR EL JSON:" + 
                            System.Environment.NewLine + ex.Message + 
                            System.Environment.NewLine);
                    }

                } else
                    Console.WriteLine("Error al procesar el input.");
            }
        }
    }
}

