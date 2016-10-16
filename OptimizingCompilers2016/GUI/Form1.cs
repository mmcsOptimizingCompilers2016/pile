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


namespace OptimizingCompilers2016.GUI
{
    public partial class Form1 : Form
    {

        string FileName;
        public Form1()
        {
            InitializeComponent();
            FileName = @"a.txt";

        }

        private void Run_Click(object sender, EventArgs e)
        {
            try
            {
                string text = File.ReadAllText(FileName);
               
                Scanner scanner = new Scanner();
                scanner.SetSource(text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();

                var linearCode = new LinearCodeVisitor();
                parser.root.Accept(linearCode);

                ResultCode.Text = linearCode.ToString();


            }
            catch (FileNotFoundException)
            {
                Console.Text += String.Format("Файл {0} не найден!\r\n", FileName);

            }
            catch (LexException exception)
            {
                Console.Text += String.Format("Лексическая ошибка. " + exception.Message+ "\r\n");
            }
            catch (SyntaxException exception)
            {
                Console.Text += String.Format("Синтаксическая ошибка. " + exception.Message+ "\r\n");
            }


            System.Console.ReadLine();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
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


        }
    }
}
