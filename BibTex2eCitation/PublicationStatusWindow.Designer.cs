namespace BibTex2eCitation
{
    partial class PublicationStatusWindow
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
            this.PossibleValues = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PossibleValues
            // 
            this.PossibleValues.FormattingEnabled = true;
            this.PossibleValues.Location = new System.Drawing.Point(12, 12);
            this.PossibleValues.Name = "PossibleValues";
            this.PossibleValues.Size = new System.Drawing.Size(257, 21);
            this.PossibleValues.TabIndex = 0;
            this.PossibleValues.SelectedIndexChanged += new System.EventHandler(this.PossibleValues_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PublicationStatusWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 49);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PossibleValues);
            this.Name = "PublicationStatusWindow";
            this.Text = "Chose Publication Status";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ComboBox PossibleValues;
    }
}