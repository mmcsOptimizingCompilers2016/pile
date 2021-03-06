﻿using System;
using System.IO;
using System.Collections.Generic;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using OptimizingCompilers2016.Library.Optimizators;
using OptimizingCompilers2016.Library.Analysis.DefUse;
using OptimizingCompilers2016.Library.Analysis.ConstantPropagation;
using OptimizingCompilers2016.Library.ThreeAddressCode;
using OptimizingCompilers2016.Library.Analysis;
using OptimizingCompilers2016.Library.DeadCode;
using OptimizingCompilers2016.Library.InterBlockOptimizators;

namespace OptimizingCompilers2016.ConsoleApplication
{
    class Program
    {
        static void print(List<IThreeAddressCode> code)
        {
            String text = "";
            foreach (IThreeAddressCode lr in code)
            {

                text += lr.ToString() + Environment.NewLine;
            }
            Console.WriteLine(text);
        }

        static void print(List<BaseBlock> blocks)
        {
            foreach (var block in blocks)
            {
                Console.WriteLine("Block: {0}", block.Name);
                foreach (var command in block.Commands)
                {
                    Console.WriteLine(command);
                }
                Console.WriteLine();
            }

        }

        static Parser parse(string FileName)
        {
            string text = File.ReadAllText(FileName);

            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);

            Parser parser = new Parser(scanner);

            var b = parser.Parse();

            if (!b)
            {
                Console.WriteLine("Ошибка");
                return null;
            }
            else
            {
                Console.WriteLine("Программа распознана");
            }

            return parser;
        }

        static void prettyPrint(Parser parser)
        {
            var prettyVisitor = new PrettyPrintVisitor();
            parser.root.Accept(prettyVisitor);
            Console.WriteLine(prettyVisitor.Text);
        }

        static List<LinearRepresentation> getLinearCode(Parser parser)
        {
            var linearCode = new LinearCodeVisitor();
            parser.root.Accept(linearCode);
            //Console.WriteLine(linearCode.ToString());
            return linearCode.code;
        }

        static ControlFlowGraph getBlocks(List<LinearRepresentation> linearCode)
        {
            var blocks = BaseBlockDivider.divide(linearCode);
            //Console.WriteLine("Blocks:");
            //foreach (var block in blocks)
            //{
            //    Console.WriteLine(block.ToString());
            //    Console.WriteLine("-------");
            //}
            return blocks;
        }

        static List<BaseBlock> getListOfBB(ControlFlowGraph graph)
        {
            List<BaseBlock> result = new List<BaseBlock>();
            Queue<BaseBlock> blocks = new Queue<BaseBlock>();
            blocks.Enqueue(graph.GetRoot());
            HashSet<BaseBlock> used = new HashSet<BaseBlock>();

            while (blocks.Count > 0)
            {
                var current = blocks.Dequeue();
                if (used.Contains(current))
                    continue;

                result.Add(current);

                used.Add(current);
                if (current.Output != null && !used.Contains(current.Output))
                {
                    blocks.Enqueue(current.Output);
                }
                if (current.JumpOutput != null && !used.Contains(current.JumpOutput))
                {
                    blocks.Enqueue(current.JumpOutput);
                }
            }
            //Debug.Assert(graph.Count == result.Count);
            return result;

        }
                

        static void Main(string[] args)
        {
            string FileName = @"a.txt";

            try
            {
                var parser = parse(FileName);
                if (parser == null)
                    return;

                //prettyPrint(parser);
                var linearCode = getLinearCode(parser);
                var blocks = getBlocks(linearCode);

                //Console.WriteLine("Edge Types:");
                //Console.WriteLine(blocks.EdgeTypes);

                //var AV = new ActiveVariables(blocks);

                //AV.runAnalys();

                //Console.WriteLine(AV.ToString());

                //var opt = new CommonExpressions();

                //BaseBlock block = new BaseBlock();

                //block.Commands.AddRange(linearCode.code);


                //Console.WriteLine("Before:");
                //print(block.Commands);
                //var optCode = opt.Optimize(block);
                //Console.WriteLine("After:");
                //print(block.Commands);

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

                //Tuple<BaseBlock, List<BaseBlock>> test_tree = DOM.get_testing_tree();

                //Dictionary<BaseBlock, List<BaseBlock>> dom_relations = DOM.DOM_CREAT(test_tree.Item2, test_tree.Item1);
                //DOM.test_printing(dom_relations);
                //Console.WriteLine(DOM.get_tree_root(dom_relations, test_tree.Item1).ToString());

                //var domFront = new DominanceFrontier(blocks.ToList());

                //Console.WriteLine(domFront.ToString());

                //var domFront = new DominanceFrontier(blocks.ToList());

                //Console.WriteLine(domFront.ToString());

                //var IDF = new HashSet<string>();
                //foreach (var block in blocks)
                //{
                //    IDF = domFront.ComputeIDF(block);
                //    Console.WriteLine("IDF(" + block.Name + ") = {" + string.Join(", ", IDF) + "}");
                //}

                //Dictionary<BaseBlock, List<BaseBlock>> dom_relations = DOM.DOM_CREAT(blocks, blocks[0]);
                //DOM.test_printing(dom_relations);
                //DOM.get_tree_root(dom_relations);

                //Console.WriteLine("CFG:");
                //ControlFlowGraph cfg = blocks;
                //Console.WriteLine(cfg.GenerateGraphvizDotFile());

                //var CFG = blocks;

                //var BackEdge = ControlFlowGraph.MakeEdge(blocks.ToList()[2], blocks.ToList()[0]);

                //Console.WriteLine(CFG.ToString());

                //var NatLoop = new NaturalLoop(CFG, BackEdge);

                //Console.WriteLine(NatLoop.ToString());

                //Console.WriteLine("GlobalDefUse:");
                //GlobalDefUse gdf = new GlobalDefUse();
                //gdf.RunAnalysis(blocks.ToList());
                //Console.WriteLine(gdf.ToString());

                //var constantPropagation = new GlobalConstantPropagation();
                //constantPropagation.RunAnalysis(blocks.ToList());
                //foreach (var block in blocks) {
                //    Console.WriteLine("Block " + block.Name + "\n");
                //    Console.WriteLine(block.ToString());
                //}

                //var opt = new Library.InterBlockOptimizators.CommonExpressions();

                //var graph = BaseBlockDivider.divide(linearCode);


                //Console.WriteLine("Before:");
                //print(getListOfBB(graph));
                //var optCode = opt.Optimize(graph);
                //Console.WriteLine("After:");
                //print(getListOfBB(graph));

                var gd = new GlobalDefUse();
                gd.RunAnalysis(blocks.ToList());
                Console.WriteLine(gd.ToString());

                //foreach (var block in blocks)
                //{
                //    InblockDefUse df = new InblockDefUse(block);
                //    Console.WriteLine(df.ToString());
                //}
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
