namespace OptimizingCompilers2016.GUI
{
    partial class ThreeAddressCode_Form
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
            this.ResultCode = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ResultCode
            // 
            this.ResultCode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResultCode.Location = new System.Drawing.Point(12, 12);
            this.ResultCode.Name = "ResultCode";
            this.ResultCode.Size = new System.Drawing.Size(410, 500);
            this.ResultCode.TabIndex = 0;
            this.ResultCode.Text = "";
            // 
            // ThreeAddressCode_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 562);
            this.Controls.Add(this.ResultCode);
            this.Name = "ThreeAddressCode_Form";
            this.Text = "Трёхадресный код";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ResultCode;
    }
}