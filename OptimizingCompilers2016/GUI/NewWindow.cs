using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimizingCompilers2016.GUI
{
    public partial class NewWindow : Form
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        public string GetSetText
        {
            get { return ResultText.Text; }
            set { ResultText.Text = value; }
        }
    }
}
