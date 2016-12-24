using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Helpers;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using System.Threading;
using System.Text.RegularExpressions;

namespace OptimizingCompilers2016.GUI
{
    public partial class Form1 : Form
    {
        List<BaseBlock> blocks;

        Thread Blick;
        string FileName;



        public Form1()
        {
            this.DesktopLocation = new Point(10000, 10000);
            InitializeComponent();
            FileName = @"a.txt";
            Code.Text = File.ReadAllText(FileName);
            Code.TextChanged -= Code_TextChanged;
            Code.Suspend();
            PaintLoad();
                       

            Code.Resume();
            Code.TextChanged += Code_TextChanged;

            blocks = new List<BaseBlock>();

        }

        private void ConsoleBlick()
        {
            MethodInvoker Invoker = delegate
            {
                this.Console.BackColor = Color.Red;
                //this.Console.BackColor = Color.White;
            };
            MethodInvoker Invoker2 = delegate
            {
                //this.Console.BackColor = Color.Maroon;
                this.Console.BackColor = Color.White;
            };
            this.Invoke(Invoker);
            Thread.Sleep(100);
            this.Invoke(Invoker2);
        }

        private string PrintBlocks()
        {
            string result = "";
            foreach (var block in blocks)
            {
                result += block.ToString();
                result += "\r\n";
            }

            return result;
        }


        private void MenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaintLoad()
        {
            var currentSelStart = Code.SelectionStart;
            var currentSelLength = Code.SelectionLength;

            Code.SelectAll();
            Code.SelectionColor = SystemColors.WindowText;
            int lineIndex = Code.GetLineFromCharIndex(currentSelStart);
            int CharIndex = Code.GetFirstCharIndexOfCurrentLine();
            var matches = Regex.Matches(Code.Text, @"\bcycle\b|\bif\b|\bthen\b|\belse\b|\bfor\b|\bto\b|\bdo\b|\brepeat\b|\buntil\b|\bwhile\b");
            foreach (var match in matches.Cast<Match>())
            {
                Code.Select(match.Index + CharIndex, match.Length);
                Code.SelectionColor = Color.Blue;
                //Code.Select(currentSelStart, currentSelLength);
                //Code.SelectionColor = SystemColors.WindowText;
            }

            //matches = Regex.Matches(Code.Text, "\".*\"");
            //foreach (var match in matches.Cast<Match>())
            //{
            //    Code.Select(match.Index, match.Length);
            //    Code.SelectionColor = Color.Red;
            //}

            Code.Select(currentSelStart, 0);
            Code.SelectionColor = SystemColors.WindowText;
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            FileName = this.openFileDialog1.FileName;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = this.openFileDialog1.FileName;

                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                Code.Text = sr.ReadToEnd();
                sr.Close();
            }
            Code.TextChanged -= Code_TextChanged;
            Code.Suspend();
            PaintLoad();

            Code.Resume();
            Code.TextChanged += Code_TextChanged;

        }

        private void Code_TextChanged(object sender, EventArgs e)
        {
            if (Code.Text != "")
            {
                Code.Suspend();
                var currentSelStart = Code.SelectionStart;
                var currentSelLength = Code.SelectionLength;



                int lineIndex = Code.GetLineFromCharIndex(currentSelStart);
                int CharIndex = Code.GetFirstCharIndexOfCurrentLine();

                Code.Select(CharIndex, Code.Lines[lineIndex].Length);
                Code.SelectionColor = SystemColors.WindowText;

                var matches = Regex.Matches(Code.Lines[lineIndex], @"\bcycle\b|\bif\b|\bthen\b|\belse\b|\bfor\b|\bto\b|\bdo\b|\brepeat\b|\buntil\b|\bwhile\b");
                foreach (var match in matches.Cast<Match>())
                {
                    Code.Select(match.Index + CharIndex, match.Length);
                    Code.SelectionColor = Color.Blue;
                }

                //matches = Regex.Matches(Code.Text, "\".*\"");
                //foreach (var match in matches.Cast<Match>())
                //{
                //    Code.Select(match.Index + CharIndex, match.Length);
                //    Code.SelectionColor = Color.Red;
                //}


                Code.Select(currentSelStart, currentSelLength);
                Code.SelectionColor = SystemColors.WindowText;
                Code.Resume();
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                string text = Code.Text;

                Scanner scanner = new Scanner();
                scanner.SetSource(text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();

                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);

                ResultCode.Text = linearCode.ToString();

                blocks = new List<BaseBlock>();
                blocks = BaseBlockDivider.divide(linearCode.code).ToList();

                BaseBlocks.Text = PrintBlocks();
            }
            catch (FileNotFoundException)
            {
                Console.Text = String.Format("Файл {0} не найден!\r\n", FileName);

            }
            catch (LexException exception)
            {
                Console.Text = "Лексическая ошибка. " + exception.Message;
                Console.Text += "\r\n";
                Console.Select(Console.Text.Length - 1, 0);
                Blick = new Thread(ConsoleBlick);
                Blick.Start();
            }
            catch (SyntaxException exception)
            {
                Console.Text = "Синтаксическая ошибка. " + exception.Message;
                Console.Text += "\r\n";
                Console.Select(Console.Text.Length - 1, 0);
                Blick = new Thread(ConsoleBlick);
                Blick.Start();
            }
        }

        private void NewWindow_Click(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == ThreeAddrCode_TabPage)
            {
                var newWindow = new NewWindow();
                newWindow.GetSetText = ResultCode.Text;
                newWindow.Text = "Трёхадресный код";
                newWindow.Show();
                newWindow.Top = this.Top;
                newWindow.Left = this.Right;
                
            }

            if (tabControl2.SelectedTab == BaseBlock_TabPage)
            {
                var newWindow = new NewWindow();
                newWindow.GetSetText = BaseBlocks.Text;
                newWindow.Text = "Базовые блоки";
                newWindow.Show();
                newWindow.Top = this.Top;
                newWindow.Left = this.Right;
            }
            

        }

        private void NewWindow2_Click(object sender, EventArgs e)
        {
            var newWindow = new NewWindow();
            newWindow.GetSetText = Result.Text;
            newWindow.Text = "Результат";
            newWindow.Show();
            newWindow.Top = this.Top;
            newWindow.Left = this.Right;
            
        }

        private void удалениеМёртвогоКодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (var block in blocks)
            //    DeadCodeDeleting.optimizeDeadCode(block);

            //Result.Text = PrintBlocks();
        }
        
        private void свёрткаКонстантToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ConstantFolding.transform(blocks);
            //Result.Text = PrintBlocks();
        }

        private void оптимизацияОбщихПодвыраженийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var opt = new CommonExpressions();
            //foreach (var block in blocks)
            //var optCode = opt.Optimize(block);

            //Result.Text = PrintBlocks();
        }

        private void протяжкаКонстантToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var opt = new ConstantPropagationOptimizator();
            //foreach (var block in blocks)
            //var optCode = opt.Optimize(block);

            //Result.Text = PrintBlocks();
        }

        private void учетАлгебраическихТождествToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var opt = new AlgebraicIdentityOptimizator();
            //foreach (var block in blocks)
            //var optCode = opt.Optimize(block);

            //Result.Text = PrintBlocks();
        }

        private void глобальнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Result.Text = "";
        }

        private void внутриБлоковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Result.Text = "";
            //foreach (var block in blocks)
            //{
            //    var ldu = new InblockDefUse(block);
            //    Result.Text += ldu.ToString();
            //}
        }

        private void наОсновеАнализаАктивныхПеременныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var AV = new ActiveVariables(blocks);
            //foreach (var block in blocks)
            //    DeadCodeDeleting.optimizeDeadCode(block, AV[block.Name]);
            //Result.Text = PrintBlocks();

        }

        private void анализАктивныхПеременныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Result.Text = "";
            //var AV = new ActiveVariables(blocks);
            //Result.Text += AV.ToString();
        }

        private void анализДоступныхВыраженийToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void деревоДоминаторовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Result.Text = "";
            ////Tuple<BaseBlock, List<BaseBlock>> test_tree = DOM.get_testing_tree();
            //Dictionary<BaseBlock, List<BaseBlock>> dom_relations = DOM.DOM_CREAT(blocks[0], blocks);
            //DOM.test_printing(dom_relations);
            //Result.Text += DOM.get_tree_root(dom_relations, test_tree.Item1).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(0, 0);
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
public static class ControlExtensions
{
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern bool LockWindowUpdate(IntPtr hWndLock);

    public static void Suspend(this Control control)
    {
        LockWindowUpdate(control.Handle);
    }

    public static void Resume(this Control control)
    {
        LockWindowUpdate(IntPtr.Zero);
    }

}




