using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using net.sf.jabref;
using net.sf.jabref.imports;
using System.IO;

namespace BibTex2eCitation
{
    public partial class MainWnd : Form
    {

        BibtexDatabase db = null;

        public MainWnd()
        {
            InitializeComponent();

            foreach (var col in dataGridView1.Columns)
            {
                (col as DataGridViewColumn).HeaderCell.Style.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                (col as DataGridViewColumn).DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            }

            // Make the mandatory columns bold ... 
            dataGridView1.Columns["TITLE"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["AUTHOR"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["EDITOR"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["PUBLICATIONDATE"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["PUBLICATIONTYPE"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["PUBLICATIONSTATUS"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.Columns["ORGCODE"].HeaderCell.Style.Font = new Font("Arial", 9, FontStyle.Bold);
        }

        private void importBibTeXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "BibTeX Files|*.bib";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            BibtexParser parser = new BibtexParser(new StreamReader(openFileDialog1.FileName));
            var result = parser.Parse();
            db = result.Database;

            foreach (var entry in db.getEntries())
                AddBibTeXEntry(entry);
        }

        private string TrimEntry(string entry)
        {
            string trimmed = entry.TrimStart('{', ' ');
            trimmed = trimmed.TrimEnd('}', ' ');
            return trimmed;
        }

        private void AddBibTeXEntry(BibtexEntry entry)
        {
            string title = entry.getField("title");
            string subtitle = "";
            string author = entry.getField("author");
            string editor = entry.getField("editor");

            title = TrimEntry(title);
            author = TrimEntry(author);

            if (editor != null)
                editor = TrimEntry(editor);

            string[] authors = author.Split(new string[] { " and ", "," }, StringSplitOptions.RemoveEmptyEntries);
            string new_author = "";
            foreach (string a in authors)
            {
                if (a.Length > 0 && new_author != "")
                    new_author = new_author + ";" + TrimEntry(a);
                else if (a.Length > 0)
                    new_author = TrimEntry(a);
            }

            if (editor == null)
                editor = new_author;
            if (new_author == "")
                new_author = editor;

            string booktitle = entry.getField("booktitle");
            string edition = entry.getField("edition");
            string isbn = entry.getField("isbn");
            string journalorseriestitle = entry.getType() == BibtexEntryType.getType("ARTICLE") ? entry.getField("journal") : entry.getField("series");
            string volume = entry.getField("volume");
            string issue = entry.getField("issue");
            string pages = entry.getField("pages");
            string startpage = "";
            string endpage = "";   

            if (pages != null)
            {
                string[] splittedPages = pages.Split(new string[] { "-", "--" }, StringSplitOptions.RemoveEmptyEntries);
                if (splittedPages.Length > 0)
                    startpage = splittedPages[0].Trim();
                if (splittedPages.Length > 1)
                    endpage = splittedPages[1].Trim();
            }
                     

            string issn = entry.getField("issn");
            string publisher = entry.getField("publisher");
            string publicationdate = entry.getField("year");
            string abstract_txt = entry.getField("abstract");
            string doi = entry.getField("doi");
            string fulltexturl = entry.getField("url");
            string notes = entry.getField("notes");
            string publicationtype = "";
            string orgcode = BibTex2eCitation.Properties.Settings.Default.DefaultOrgCode.ToString();

            if (entry.getType() == BibtexEntryType.getType("ARTICLE"))
                publicationtype = "Journal Items";
            else if (entry.getType() == BibtexEntryType.getType("BOOK"))
                publicationtype = "Monographs/ Monograph Items";
            else if (entry.getType() == BibtexEntryType.getType("PHDTHESIS"))
                publicationtype = "Doctoral Thesis and Habilitation";
            else if (entry.getType() == BibtexEntryType.getType("MASTERSTHESIS"))
                publicationtype = "Master Thesis and Bachelor Thesis";
            else if (entry.getType() == BibtexEntryType.getType("INPROCEEDINGS"))
                publicationtype = "Conference Contributions";
            string language = "English";

            dataGridView1.Rows.Add(new string[] { title, new_author, publicationdate, editor, publicationtype, "", orgcode, subtitle, booktitle, edition, "", "", isbn, journalorseriestitle, volume, issue, startpage, endpage, issn, publisher, "", "", abstract_txt, doi, fulltexturl, "", "", "", notes, "", language });
        }

        private void exportECitationCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var col in dataGridView1.Columns)
            {
                sb.Append((col as DataGridViewColumn).HeaderText.ToString());
                sb.Append("\t");
            }
            sb.AppendLine();

            foreach (var row in dataGridView1.Rows)
            {
                foreach (var cell in (row as DataGridViewRow).Cells)
                {
                    if((cell as DataGridViewCell).Value!= null)
                        sb.Append((cell as DataGridViewCell).Value.ToString());
                    sb.Append("\t");
                }
                sb.AppendLine();
            }

            string text = sb.ToString();

            saveFileDialog1.Filter = "CSV Files|*.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllText(saveFileDialog1.FileName, text, Encoding.Unicode);
        }

        private void importBibTexFromClipBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                BibtexParser parser = new BibtexParser(new StringReader(Clipboard.GetDataObject().GetData(DataFormats.Text).ToString()));
                var result = parser.Parse();
                db = result.Database;

                foreach (var entry in db.getEntries())
                    AddBibTeXEntry(entry);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openECitationCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV Files|*.txt;*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] data = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                foreach (string row in data)
                {
                    if(row != data[0] && row.Length>0)
                        dataGridView1.Rows.Add(row.Split(','));
                }                
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "PUBLICATIONSTATUS")
            {
                PublicationStatusWindow wnd = new PublicationStatusWindow();
                wnd.TopMost = true;
                wnd.Text = "Chose Publication Status";
                wnd.PossibleValues.Items.AddRange(new string[] { "Accepted", "Published", "Unpublished" });

                if (wnd.ShowDialog() == DialogResult.OK)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = wnd.SelectedValue;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "PUBLICATIONTYPE")
            {
                PublicationStatusWindow wnd = new PublicationStatusWindow();
                wnd.TopMost = true;
                wnd.Text = "Chose Publication Type";
                wnd.PossibleValues.Items.AddRange(new string[] { "Journal Items", "Monographs/ Monograph Items", "Conference Contributions", "Doctoral Thesis and Habilitation", "Thesis", "Master Thesis and Bachelor Thesis", "Reports", "Working Papers", "Edited Works", "Educational Material", "Other" });

                if (wnd.ShowDialog() == DialogResult.OK)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = wnd.SelectedValue;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "LANGUAGE")
            {
                PublicationStatusWindow wnd = new PublicationStatusWindow();
                wnd.TopMost = true;
                wnd.Text = "Chose Language";
                wnd.PossibleValues.Items.AddRange(new string[] { "Bulgarian", "Catalan", "Chinese", "Croatian", "Dutch", "English", "Finnish", "French", "German", "Greek - Modern", "Italian", "Japanese", "Polish", "Portuguese", "Romanian", "Russian", "Slovak", "Spanish", "Swedish", "Turkish" });

                if (wnd.ShowDialog() == DialogResult.OK)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = wnd.SelectedValue;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "REVIEWSTATUS")
            {
                PublicationStatusWindow wnd = new PublicationStatusWindow();
                wnd.TopMost = true;
                wnd.Text = "Chose Review Status";
                wnd.PossibleValues.Items.AddRange(new string[] { "Peer reviewed", "Internally reviewed", "Not reviewed", "Unknown" });

                if (wnd.ShowDialog() == DialogResult.OK)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = wnd.SelectedValue;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" eCitation Editor Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " \n Developed at the Chair of Systems Design, 2013-2014\n http://www.sg.ethz.ch", "About eCitation Editor");
        }
    }
}
