using System;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;

namespace OptimizingCompilers2016.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string FileName = @"a.txt";
            try
            {
                string text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                //  if (!b)
                //Console.WriteLine("Ошибка");
                //  else Console.WriteLine("Программа распознана");
                //var prettyVisitor = new PrettyPrintVisitor();
                //parser.root.Accept(prettyVisitor);
                //Console.WriteLine(prettyVisitor.Text);

                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);
                Console.WriteLine(linearCode.ToString());

                var blocks = BaseBlockDivider.divide(linearCode.code);
                Console.WriteLine("Blocks:");
                foreach (var block in blocks)
                {
                    Console.WriteLine(block.ToString());
                    Console.WriteLine("-------");
                }

                var gdu = new GlobalDefUse();
                gdu.runAnalys(blocks);


            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
