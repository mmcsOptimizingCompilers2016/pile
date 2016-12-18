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
    public partial class ThreeAddressCode_Form : Form
    {
        public ThreeAddressCode_Form()
        {
            InitializeComponent();

        }

        public string GetSetText
        {
            get { return ResultCode.Text; }
            set { ResultCode.Text = value; }
        }
    }
}
