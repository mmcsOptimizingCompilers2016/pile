namespace OptimizingCompilers2016.GUI
{
    partial class BaseBlock_Form
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
            this.BaseBlocks = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BaseBlocks
            // 
            this.BaseBlocks.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BaseBlocks.Location = new System.Drawing.Point(12, 12);
            this.BaseBlocks.Name = "BaseBlocks";
            this.BaseBlocks.Size = new System.Drawing.Size(410, 500);
            this.BaseBlocks.TabIndex = 1;
            this.BaseBlocks.Text = "";
            // 
            // BaseBlock_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 562);
            this.Controls.Add(this.BaseBlocks);
            this.Name = "BaseBlock_Form";
            this.Text = "Базовые блоки";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox BaseBlocks;
    }
}