using System;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using System.Collections.Generic;

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

                var ldu = new InblockDefUse(blocks[0]);

                foreach (var block in blocks)
                {
                    ldu = new InblockDefUse(block);
                    Console.WriteLine(ldu.ToString());
                }

                List<HashSet<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>> Def = new List<HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>>();
                List<HashSet<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue>> Use = new List<HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>>();

                HashSet<OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue> set = new HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>();

                foreach (var sets in ldu.result.Values)
                    foreach (var itemSet in sets)
                        Console.WriteLine(itemSet.Item2.Value);
               
                //set.Add(new OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue(key.Item2.Value));
                   
                foreach (var item in set)
                    Console.WriteLine(item);

                for (int i = 0; i < blocks.Count; i++)
                {
                    ldu = new InblockDefUse(blocks[i]);
                   var Set = new HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>();
                    foreach (var key in ldu.result.Keys)
                        Set.Add(new OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue(key.Item2.Value));
                    Def.Add(Set);
                }

                foreach (var item in Def)
                    foreach (var item2 in item)
                        Console.WriteLine(item2);

                for (int i = 0; i < blocks.Count; i++)
                {
                    ldu = new InblockDefUse(blocks[i]);
                    var Set = new HashSet<Library.ThreeAddressCode.Values.IdentificatorValue>();
                    foreach (var sets in ldu.result.Values)
                        foreach (var itemSet in sets)
                            Set.Add(new OptimizingCompilers2016.Library.ThreeAddressCode.Values.IdentificatorValue(itemSet.Item2.Value));
                    Use.Add(Set);
                }

                foreach (var item in Use)
                    foreach (var item2 in item)
                        Console.WriteLine(item2);

                var AV = new ActiveVariables(blocks);

                foreach (var block in blocks)
                {
                    Console.WriteLine(block.ToString());
                    Console.WriteLine("-------");
                }

                AV.runAnalys();

                foreach (var block in blocks)
                {
                    Console.WriteLine(block.ToString());
                    Console.WriteLine("-------");
                }

                Console.WriteLine("------------------");

                Console.WriteLine(AV.ToString());


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
