﻿using System;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using OptimizingCompilers2016.Library.Transformations;
using OptimizingCompilers2016.Library.BaseBlock;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.DeadCode;
using OptimizingCompilers2016.Library.Transformations;

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
                //if (!b)
                //    Console.WriteLine("Ошибка");
                //else Console.WriteLine("Программа распознана");
                //var prettyVisitor = new PrettyPrintVisitor();
                //parser.root.Accept(prettyVisitor);
                //Console.WriteLine(prettyVisitor.Text);
                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);
                var opt = new CommonExpressions();
                BaseBlock block = new BaseBlock();
                block.Commands.AddRange(linearCode.code);
                var optCode = opt.optimize(block);

                Console.WriteLine("Before:");

                Console.WriteLine(linearCode.ToString());
                Console.WriteLine("After:");
                foreach (var item in optCode.Commands)
                {
                    Console.WriteLine(item.ToString());
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
