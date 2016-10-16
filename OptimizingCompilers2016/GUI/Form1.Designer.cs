namespace OptimizingCompilers2016.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Code = new System.Windows.Forms.RichTextBox();
            this.Run = new System.Windows.Forms.Button();
            this.ResultCode = new System.Windows.Forms.RichTextBox();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Code
            // 
            this.Code.Location = new System.Drawing.Point(12, 27);
            this.Code.Name = "Code";
            this.Code.Size = new System.Drawing.Size(428, 442);
            this.Code.TabIndex = 0;
            this.Code.Text = "";
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(446, 220);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(75, 39);
            this.Run.TabIndex = 1;
            this.Run.Text = "Пуск";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // ResultCode
            // 
            this.ResultCode.Location = new System.Drawing.Point(527, 27);
            this.ResultCode.Name = "ResultCode";
            this.ResultCode.Size = new System.Drawing.Size(428, 442);
            this.ResultCode.TabIndex = 2;
            this.ResultCode.Text = "";
            // 
            // Console
            // 
            this.Console.Location = new System.Drawing.Point(12, 487);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(943, 104);
            this.Console.TabIndex = 3;
            this.Console.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(962, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            this.Menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuExit});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(48, 20);
            this.Menu.Text = "Файл";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(121, 22);
            this.MenuOpen.Text = "Открыть";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(121, 22);
            this.MenuExit.Text = "Выход";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 603);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.ResultCode);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "GUI";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox Code;
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.RichTextBox ResultCode;
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

