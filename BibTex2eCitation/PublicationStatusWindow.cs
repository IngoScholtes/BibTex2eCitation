using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BibTex2eCitation
{
    public partial class PublicationStatusWindow : Form
    {
        public PublicationStatusWindow()
        {
            InitializeComponent();
        }

        public string SelectedValue = "";

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void PossibleValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedValue = PossibleValues.Items[PossibleValues.SelectedIndex].ToString();
        }
    }
}
