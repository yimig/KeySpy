namespace KeySpy
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.cbKeyDown = new System.Windows.Forms.CheckBox();
            this.cbKeyUp = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 104);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(775, 441);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "";
            // 
            // cbKeyDown
            // 
            this.cbKeyDown.AutoSize = true;
            this.cbKeyDown.Checked = true;
            this.cbKeyDown.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeyDown.Location = new System.Drawing.Point(19, 36);
            this.cbKeyDown.Name = "cbKeyDown";
            this.cbKeyDown.Size = new System.Drawing.Size(97, 22);
            this.cbKeyDown.TabIndex = 1;
            this.cbKeyDown.Text = "KeyDown";
            this.cbKeyDown.UseVisualStyleBackColor = true;
            // 
            // cbKeyUp
            // 
            this.cbKeyUp.AutoSize = true;
            this.cbKeyUp.Location = new System.Drawing.Point(135, 36);
            this.cbKeyUp.Name = "cbKeyUp";
            this.cbKeyUp.Size = new System.Drawing.Size(79, 22);
            this.cbKeyUp.TabIndex = 2;
            this.cbKeyUp.Text = "KeyUp";
            this.cbKeyUp.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbKeyUp);
            this.groupBox1.Controls.Add(this.cbKeyDown);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 76);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "filter";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(670, 38);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 557);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "KeySpy";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox1;
        private System.Windows.Forms.CheckBox cbKeyDown;
        private System.Windows.Forms.CheckBox cbKeyUp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
    }
}

