using System;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using OptimizingCompilers2016.Library.BaseBlock;
//using OptimizingCompilers2016.Library.ControlFlowGraph;
using OptimizingCompilers2016.Library.Analyses;
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
                if (!b)
                    Console.WriteLine("Ошибка");
                else Console.WriteLine("Программа распознана");
                var prettyVisitor = new PrettyPrintVisitor();
                parser.root.Accept(prettyVisitor);
                Console.WriteLine(prettyVisitor.Text);
                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);
                //Console.WriteLine(linearCode.ToString());

                var blocks = BaseBlockDivider.divide(linearCode.code);

                Console.WriteLine("Blocks:");
                foreach (var block in blocks)
                {
                    Console.WriteLine(block.ToString());
                    Console.WriteLine("-------");
                }

                //Tuple<BaseBlock, List<BaseBlock>> test_tree = DOM.get_testing_tree();

                //Dictionary<BaseBlock, List<BaseBlock>> dom_relations = DOM.DOM_CREAT(test_tree.Item2, test_tree.Item1);
                //DOM.test_printing(dom_relations);
                //Console.WriteLine(DOM.get_tree_root(dom_relations, test_tree.Item1).ToString());

                var domFront = new DominanceFrontier(blocks);
                
                Console.WriteLine(domFront.ToString());

                var IDF = new HashSet<string>();

                foreach (var block in blocks)
                {
                    IDF = domFront.ComputeIDF(block);
                    Console.WriteLine("IDF(" + block.Name+ ") = {" + string.Join(", ", IDF) + "}");
                }
                    



                //Dictionary<BaseBlock, List<BaseBlock>> dom_relations = DOM.DOM_CREAT(blocks, blocks[0]);
                //DOM.test_printing(dom_relations);
                //DOM.get_tree_root(dom_relations);

                //Console.WriteLine("CFG:");
                //ControlFlowGraph cfg = new ControlFlowGraph(blocks);
                //Console.WriteLine(cfg.GenerateGraphvizDotFile());


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
