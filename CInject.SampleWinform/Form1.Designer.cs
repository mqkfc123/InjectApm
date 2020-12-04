namespace CInject.SampleWinform
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
            this.txtInputValue = new System.Windows.Forms.TextBox();
            this.btnChangeValue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInputValue
            // 
            this.txtInputValue.Location = new System.Drawing.Point(107, 16);
            this.txtInputValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtInputValue.Name = "txtInputValue";
            this.txtInputValue.Size = new System.Drawing.Size(132, 25);
            this.txtInputValue.TabIndex = 0;
            // 
            // btnChangeValue
            // 
            this.btnChangeValue.Location = new System.Drawing.Point(107, 46);
            this.btnChangeValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChangeValue.Name = "btnChangeValue";
            this.btnChangeValue.Size = new System.Drawing.Size(207, 27);
            this.btnChangeValue.TabIndex = 1;
            this.btnChangeValue.Text = "Print the Value in label";
            this.btnChangeValue.UseVisualStyleBackColor = true;
            this.btnChangeValue.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Value:";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.ForeColor = System.Drawing.Color.Red;
            this.lblValue.Location = new System.Drawing.Point(385, 20);
            this.lblValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(259, 25);
            this.lblValue.TabIndex = 3;
            this.lblValue.Text = "This value should change";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(413, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 295);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChangeValue);
            this.Controls.Add(this.txtInputValue);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputValue;
        private System.Windows.Forms.Button btnChangeValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Button button1;
    }
}

