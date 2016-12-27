using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizingCompilers2016.Library;

namespace OptimizingCompilers2016.Library.Analysis
{
    public class DOM
    {
        public class tree_node
        {
            public BaseBlock block;
            public List<tree_node> children;

            public tree_node(BaseBlock block)
            {
                this.block = block;
                children = new List<tree_node>();
            }

            public tree_node(BaseBlock block, List<tree_node> children)
            {
                this.block = block;
                this.children = children;
            }

            public void add_children(tree_node child)
            {
                children.Add(child);
            }

            public string ToString(string shift)
            {
                string result = shift + block.Name + "\n";
                for (int i = 0; i < children.Count; i++)
                {
                    result += children[i].ToString(shift + "\t") + "\n";
                }
                return result;
            }

            public override string ToString()
            {
                string result = block.Name + "\n";
                for (int i = 0; i < children.Count; i++)
                {
                    result += children[i].ToString("\t") + "\n";
                }
                return result;
            }
        }

        public class link
        {
            public BaseBlock root;
            public BaseBlock child;

            public link(BaseBlock root, BaseBlock child)
            {
                this.root = root;
                this.child = child;
            }

            public override bool Equals(object obj)
            {
                if(!(obj is link))
                    return base.Equals(obj);
                link to_eq = (link)obj;
                return to_eq.child == child && to_eq.root == root;
            }

            public override string ToString()
            {
                return root.Name + " -> " + child.Name;
            }
        }

        static bool foo(BaseBlock block, List<BaseBlock> root_doms, List<BaseBlock> child_doms)
        {
            List<BaseBlock> child_doms_ch = new List<BaseBlock>(child_doms);
            child_doms_ch.Add(block);
            return value_equals(child_doms_ch, root_doms);
        }

        static private tree_node make_tree(List<link> links, BaseBlock root)
        {

            List<link> for_children = new List<link>();
            foreach (var item in links)
            {
                if (item.root != root)
                    for_children.Add(item);
            }
            tree_node root_node = new tree_node(root);
            foreach (var item in links)
            {
                if (item.root == root)
                {
                    root_node.add_children(make_tree(for_children, item.child));
                }
            }

            return root_node;
        }


        /// <summary>
        /// gets direct domination relations from relations and inner block
        /// </summary>
        /// <param name="dom_relations">domination relations between blocks</param>
        /// <param name="root">inner block</param>
        /// <returns>direct domination relations</returns>
        static public List<link> get_direct_dominators(Dictionary<BaseBlock, List<BaseBlock>> dom_relations, BaseBlock root)
        {
            Dictionary<BaseBlock, List<BaseBlock>> dom_relations_strong = new Dictionary<BaseBlock, List<BaseBlock>>(dom_relations);
            foreach (var item in dom_relations_strong)
            {
                item.Value.Remove(item.Key);
            }

            List<link> links = new List<link>();
            foreach (var item in dom_relations_strong)
            {
                foreach (var item2 in item.Value)
                {
                    link cur_link = new link(item2, item.Key);
                    if (!links.Contains(cur_link))
                        links.Add(cur_link);
                }
            }

            HashSet<link> to_delete_links = new HashSet<link>();

            foreach (var item in links)
            {
                if (!foo(item.root, dom_relations_strong[item.child], dom_relations_strong[item.root]))
                {
                    to_delete_links.Add(item);
                    //Console.WriteLine("del");
                }
            }
            foreach (var item in to_delete_links)
            {
                links.Remove(item);
            }
            return links;
        }
        
        /// <summary>
        /// makes tree from relations and inner block
        /// </summary>
        /// <param name="dom_relations">domination relations between blocks</param>
        /// <param name="root">inner block</param>
        /// <returns>tree root</returns>
        static public tree_node get_tree_root(Dictionary<BaseBlock, List<BaseBlock>> dom_relations, BaseBlock root)
        {
            return make_tree(get_direct_dominators(dom_relations,root),root);
        }

        /// <summary>
        /// Testing tree (like in lections)
        /// </summary>
        /// <returns>Tuple : inner_bloc <-> list of all blocks</returns>
        public static Tuple<BaseBlock, List<BaseBlock>> get_testing_tree()
        {
            BaseBlock b1 = new BaseBlock();
            b1.Name = "B1";

            BaseBlock b2 = new BaseBlock();
            b2.Name = "B2";

            BaseBlock b3 = new BaseBlock();
            b3.Name = "B3";

            BaseBlock b4 = new BaseBlock();
            b4.Name = "B4";

            BaseBlock b5 = new BaseBlock();
            b5.Name = "B5";

            BaseBlock b6 = new BaseBlock();
            b6.Name = "B6";

            BaseBlock b7 = new BaseBlock();
            b7.Name = "B7";

            BaseBlock b8 = new BaseBlock();
            b8.Name = "B8";

            BaseBlock b9 = new BaseBlock();
            b9.Name = "B9";

            BaseBlock b10 = new BaseBlock();
            b10.Name = "B10";

            b1.Output = b2;
            b1.JumpOutput = b3;

            b2.Output = b3;
            b2.Predecessors.Add(b1);

            b3.Output = b4;
            b3.Predecessors.Add(b1);
            b3.Predecessors.Add(b2);
            b3.Predecessors.Add(b4);
            b3.Predecessors.Add(b8);

            b4.Output = b5;
            b4.JumpOutput = b6;
            b4.Predecessors.Add(b3);
            b4.Predecessors.Add(b7);

            b5.Output = b7;
            b5.Predecessors.Add(b4);

            b6.Output = b7;
            b6.Predecessors.Add(b4);

            b7.Output = b8;
            b7.JumpOutput = b4;
            b7.Predecessors.Add(b5);
            b7.Predecessors.Add(b6);

            b8.Output = b10;
            b8.JumpOutput = b9;
            b8.Predecessors.Add(b7);

            b9.Output = b1;
            b9.Predecessors.Add(b8);

            b10.Predecessors.Add(b8);

            List<BaseBlock> blocks = new List<BaseBlock>();
            blocks.Add(b1);
            blocks.Add(b2);
            blocks.Add(b3);
            blocks.Add(b4);
            blocks.Add(b5);
            blocks.Add(b6);
            blocks.Add(b7);
            blocks.Add(b8);
            blocks.Add(b9);
            blocks.Add(b10);
            return new Tuple<BaseBlock, List<BaseBlock>>(b1,blocks);
        }

        /// <summary>
        /// Contains of in and out lists of dominator blocks for block
        /// </summary>
        private class block_working
        {
            public List<BaseBlock> out_b = new List<BaseBlock>();
            public List<BaseBlock> in_b = new List<BaseBlock>();           

            public block_working()
            {

            }

            public block_working(block_working old)
            {
                out_b = new List<BaseBlock>(old.out_b);
                in_b = new List<BaseBlock>(old.in_b);
            }

            public void set_new(block_working old)
            {
                out_b = new List<BaseBlock>(old.out_b);
                in_b = new List<BaseBlock>(old.in_b);
            }

            public bool equal(block_working bw)
            {
                return value_equals(bw.out_b, out_b) && value_equals(bw.in_b, in_b);
            }
        }

        /// <summary>
        /// Creates dom - relations between blocks
        /// </summary>
        /// <param name="blocks">All programm blocks</param>
        /// <param name="root_block">Inner block</param>
        /// <returns>Dictionary: key - block, value - list of dominators for block</returns>
        public static Dictionary<BaseBlock, List<BaseBlock>> DOM_CREAT(List<BaseBlock> blocks, BaseBlock root_block)
        {
            //key is block, value: block`s dominators
            Dictionary<BaseBlock, List<BaseBlock>> dominators = new Dictionary<BaseBlock, List<BaseBlock>>();
            Dictionary<BaseBlock, block_working> dominators_workng = new Dictionary<BaseBlock, block_working>();
            foreach (var item in blocks)
            {
                dominators_workng.Add(item, new block_working());
                dominators_workng[item].in_b = new List<BaseBlock>();
                if (item == root_block)
                {
                    dominators_workng[item].out_b = new List<BaseBlock>();
                }
                else
                {
                    dominators_workng[item].out_b = new List<BaseBlock>(blocks);
                }
            }

            bool changed = true;

            while (changed)
            {
                changed = false;
                foreach (var item in dominators_workng)
                {
                    block_working new_value = new block_working();
                    if(item.Key.Predecessors.Count == 0)
                    {
                        new_value.in_b = new List<BaseBlock>();
                    }
                    else
                    {
                        new_value.in_b = new List<BaseBlock>(blocks);
                    }
                    
                    foreach (var prev in item.Key.Predecessors)
                    {
                        new_value.in_b = new List<BaseBlock>(new_value.in_b.Intersect(dominators_workng[prev].out_b));
                    }
                    new_value.out_b = new List<BaseBlock>(new_value.in_b);
                    if (!new_value.out_b.Contains(item.Key))
                        new_value.out_b.Add(item.Key);
                    if(!new_value.equal(item.Value))
                    {
                        //Console.WriteLine("____________________");
                        //Console.WriteLine(item.Key.Name);
                        //Console.WriteLine("===");
                        //test_printing(dominators_workng);
                        //Console.WriteLine("===");
                        //Console.WriteLine("\t" + string.Join(",", new_value.out_b.Select((BaseBlock bl) => { return bl.Name; })));
                        //Console.WriteLine("____________________");
                        changed = true;
                        item.Value.set_new(new_value);
                    }

                }
            }

            foreach (var item in dominators_workng)
            {
                dominators.Add(item.Key, item.Value.out_b);
            }

            //test_printing(dominators);
            return dominators;
        }

        List<BaseBlock> intersect_lists(List<BaseBlock> first, List<BaseBlock> second)
        {
            List<BaseBlock> result = new List<BaseBlock>();
            foreach (var item in first)
            {
                if (second.Contains(item))
                    result.Add(item);
            }
            return result;
        }

        static bool value_equals(List<BaseBlock> first, List<BaseBlock>  second)
        {
            return first.Count == second.Count;
        }

        private static void test_printing(Dictionary<BaseBlock, block_working> dominators_working)
        {
            foreach (var item in dominators_working)
            {
                Console.WriteLine("D(" + item.Key.Name + ") = {" + string.Join(", ", item.Value.out_b.Select((BaseBlock bl) => { return bl.Name; })) + "}");
            }
        }

        public static void test_printing(Dictionary<BaseBlock, List<BaseBlock>> dominators)
        {
            foreach (var item in dominators)
            {
                Console.WriteLine("D(" + item.Key.Name + ") = {" + string.Join(", ", item.Value.Select((BaseBlock bl) => { return bl.Name; })) + "}");
            }
        }
    }
}
