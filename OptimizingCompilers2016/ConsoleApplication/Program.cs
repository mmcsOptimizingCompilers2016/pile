using System;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.DeadCode;
using OptimizingCompilers2016.Library.Transformations;
using OptimizingCompilers2016.Library.Analysis.ConstantPropagation;
using OptimizingCompilers2016.Library.Optimizators;

namespace OptimizingCompilers2016.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string FileName = @"a1.txt";
            try
            {
                string text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();

                if (!b)
                    Console.WriteLine("Ошибка");
                else Console.WriteLine("Программа распознана");
                //var prettyVisitor = new PrettyPrintVisitor();
                //parser.root.Accept(prettyVisitor);
                //Console.WriteLine(prettyVisitor.Text);


                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);
                //Console.WriteLine(linearCode.ToString());

                var blocks = BaseBlockDivider.divide(linearCode.code);
                //Console.WriteLine("Blocks:");
                //foreach (var block in blocks)
                //{
                //InblockDefUse DU = new InblockDefUse(block);
                //foreach (var item in DU.result)
                //{
                //    Console.Write(item.Key + " :");
                //    Console.Write("{");
                //    foreach (var item2 in item.Value)
                //    {
                //        Console.Write(item2 + "  ");
                //    }
                //    Console.Write("}");
                //    Console.WriteLine();
                //}

                //    Console.WriteLine(block.ToString());
                //    DeadCodeDeleting.optimizeDeadCode(block);
                //    Console.WriteLine("After optimization:");
                //    Console.WriteLine(block.ToString());

                //    //console.writeline(block.tostring());
                //    Console.WriteLine("-------");
                //}

                //var gdu = new GlobalDefUse();
                //gdu.runAnalys(blocks);
                //gdu.getDefUses();

                //ConstantFolding.transform(blocks);
                //foreach (var block in blocks)
                //{
                //    Console.WriteLine(block.ToString());
                //    Console.WriteLine("-------");
                //    Console.WriteLine("-------");
                //}

                var constantPropagation = new GlobalConstantPropagation();
                constantPropagation.RunAnalysis(blocks);
                foreach (var block in blocks) {
                    Console.WriteLine("Block " + block.Name + "\n");
                    Console.WriteLine(block.ToString());
                }
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
