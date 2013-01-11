namespace BibTex2eCitation
{
    partial class MainWnd
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openECitationCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportECitationCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importBibTeXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBibTexFromClipBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AUTHOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PUBLICATIONDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EDITOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PUBLICATIONTYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PUBLICATIONSTATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ORGCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUBTITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BOOKTITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EDITION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIPTION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ETHDISSNR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ISBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JOURNALORSERIESTITLE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VOLUME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ISSUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STARTPAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ENDPAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ISSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PUBLISHER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PUBLISHERPLACE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KEYWORDS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ABSTRACT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DOI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FULLTEXTURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVENTNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVENTLOCATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVENTDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOTES = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REVIEWSTATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LANGUAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1426, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.openECitationCSVToolStripMenuItem,
            this.exportECitationCSVToolStripMenuItem,
            this.toolStripSeparator2,
            this.importBibTeXToolStripMenuItem,
            this.importBibTexFromClipBoardToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // openECitationCSVToolStripMenuItem
            // 
            this.openECitationCSVToolStripMenuItem.Name = "openECitationCSVToolStripMenuItem";
            this.openECitationCSVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openECitationCSVToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.openECitationCSVToolStripMenuItem.Text = "Open eCitation CSV";
            this.openECitationCSVToolStripMenuItem.Click += new System.EventHandler(this.openECitationCSVToolStripMenuItem_Click);
            // 
            // exportECitationCSVToolStripMenuItem
            // 
            this.exportECitationCSVToolStripMenuItem.Name = "exportECitationCSVToolStripMenuItem";
            this.exportECitationCSVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.exportECitationCSVToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.exportECitationCSVToolStripMenuItem.Text = "Save as eCitation CSV";
            this.exportECitationCSVToolStripMenuItem.Click += new System.EventHandler(this.exportECitationCSVToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(231, 6);
            // 
            // importBibTeXToolStripMenuItem
            // 
            this.importBibTeXToolStripMenuItem.Name = "importBibTeXToolStripMenuItem";
            this.importBibTeXToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.importBibTeXToolStripMenuItem.Text = "Import BibTeX From File";
            this.importBibTeXToolStripMenuItem.Click += new System.EventHandler(this.importBibTeXToolStripMenuItem_Click);
            // 
            // importBibTexFromClipBoardToolStripMenuItem
            // 
            this.importBibTexFromClipBoardToolStripMenuItem.Name = "importBibTexFromClipBoardToolStripMenuItem";
            this.importBibTexFromClipBoardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.importBibTexFromClipBoardToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.importBibTexFromClipBoardToolStripMenuItem.Text = "Import BibTex From ClipBoard";
            this.importBibTexFromClipBoardToolStripMenuItem.Click += new System.EventHandler(this.importBibTexFromClipBoardToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(231, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TITLE,
            this.AUTHOR,
            this.PUBLICATIONDATE,
            this.EDITOR,
            this.PUBLICATIONTYPE,
            this.PUBLICATIONSTATUS,
            this.ORGCODE,
            this.SUBTITLE,
            this.BOOKTITLE,
            this.EDITION,
            this.DESCRIPTION,
            this.ETHDISSNR,
            this.ISBN,
            this.JOURNALORSERIESTITLE,
            this.VOLUME,
            this.ISSUE,
            this.STARTPAGE,
            this.ENDPAGE,
            this.ISSN,
            this.PUBLISHER,
            this.PUBLISHERPLACE,
            this.KEYWORDS,
            this.ABSTRACT,
            this.DOI,
            this.FULLTEXTURL,
            this.EVENTNAME,
            this.EVENTLOCATION,
            this.EVENTDATE,
            this.NOTES,
            this.REVIEWSTATUS,
            this.LANGUAGE});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1426, 300);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // TITLE
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TITLE.DefaultCellStyle = dataGridViewCellStyle3;
            this.TITLE.HeaderText = "TITLE";
            this.TITLE.Name = "TITLE";
            // 
            // AUTHOR
            // 
            this.AUTHOR.HeaderText = "AUTHOR";
            this.AUTHOR.Name = "AUTHOR";
            // 
            // PUBLICATIONDATE
            // 
            this.PUBLICATIONDATE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PUBLICATIONDATE.HeaderText = "PUBLICATIONDATE";
            this.PUBLICATIONDATE.Name = "PUBLICATIONDATE";
            this.PUBLICATIONDATE.Width = 132;
            // 
            // EDITOR
            // 
            this.EDITOR.HeaderText = "EDITOR";
            this.EDITOR.Name = "EDITOR";
            // 
            // PUBLICATIONTYPE
            // 
            this.PUBLICATIONTYPE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PUBLICATIONTYPE.HeaderText = "PUBLICATIONTYPE";
            this.PUBLICATIONTYPE.Name = "PUBLICATIONTYPE";
            this.PUBLICATIONTYPE.ReadOnly = true;
            this.PUBLICATIONTYPE.Width = 131;
            // 
            // PUBLICATIONSTATUS
            // 
            this.PUBLICATIONSTATUS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.PUBLICATIONSTATUS.HeaderText = "PUBLICATIONSTATUS";
            this.PUBLICATIONSTATUS.Name = "PUBLICATIONSTATUS";
            this.PUBLICATIONSTATUS.ReadOnly = true;
            this.PUBLICATIONSTATUS.Width = 146;
            // 
            // ORGCODE
            // 
            this.ORGCODE.HeaderText = "ORGCODE";
            this.ORGCODE.Name = "ORGCODE";
            // 
            // SUBTITLE
            // 
            this.SUBTITLE.HeaderText = "SUBTITLE";
            this.SUBTITLE.Name = "SUBTITLE";
            // 
            // BOOKTITLE
            // 
            this.BOOKTITLE.HeaderText = "BOOKTITLE";
            this.BOOKTITLE.Name = "BOOKTITLE";
            // 
            // EDITION
            // 
            this.EDITION.HeaderText = "EDITION";
            this.EDITION.Name = "EDITION";
            // 
            // DESCRIPTION
            // 
            this.DESCRIPTION.HeaderText = "DESCRIPTION";
            this.DESCRIPTION.Name = "DESCRIPTION";
            // 
            // ETHDISSNR
            // 
            this.ETHDISSNR.HeaderText = "ETHDISSNR";
            this.ETHDISSNR.Name = "ETHDISSNR";
            // 
            // ISBN
            // 
            this.ISBN.HeaderText = "ISBN";
            this.ISBN.Name = "ISBN";
            // 
            // JOURNALORSERIESTITLE
            // 
            this.JOURNALORSERIESTITLE.HeaderText = "JOURNALORSERIESTITLE";
            this.JOURNALORSERIESTITLE.Name = "JOURNALORSERIESTITLE";
            // 
            // VOLUME
            // 
            this.VOLUME.HeaderText = "VOLUME";
            this.VOLUME.Name = "VOLUME";
            // 
            // ISSUE
            // 
            this.ISSUE.HeaderText = "ISSUE";
            this.ISSUE.Name = "ISSUE";
            // 
            // STARTPAGE
            // 
            this.STARTPAGE.HeaderText = "STARTPAGE";
            this.STARTPAGE.Name = "STARTPAGE";
            // 
            // ENDPAGE
            // 
            this.ENDPAGE.HeaderText = "ENDPAGE";
            this.ENDPAGE.Name = "ENDPAGE";
            // 
            // ISSN
            // 
            this.ISSN.HeaderText = "ISSN";
            this.ISSN.Name = "ISSN";
            // 
            // PUBLISHER
            // 
            this.PUBLISHER.HeaderText = "PUBLISHER";
            this.PUBLISHER.Name = "PUBLISHER";
            // 
            // PUBLISHERPLACE
            // 
            this.PUBLISHERPLACE.HeaderText = "PUBLISHERPLACE";
            this.PUBLISHERPLACE.Name = "PUBLISHERPLACE";
            // 
            // KEYWORDS
            // 
            this.KEYWORDS.HeaderText = "KEYWORDS";
            this.KEYWORDS.Name = "KEYWORDS";
            // 
            // ABSTRACT
            // 
            this.ABSTRACT.HeaderText = "ABSTRACT";
            this.ABSTRACT.Name = "ABSTRACT";
            // 
            // DOI
            // 
            this.DOI.HeaderText = "DOI";
            this.DOI.Name = "DOI";
            // 
            // FULLTEXTURL
            // 
            this.FULLTEXTURL.HeaderText = "FULLTEXTURL";
            this.FULLTEXTURL.Name = "FULLTEXTURL";
            // 
            // EVENTNAME
            // 
            this.EVENTNAME.HeaderText = "EVENTNAME";
            this.EVENTNAME.Name = "EVENTNAME";
            // 
            // EVENTLOCATION
            // 
            this.EVENTLOCATION.HeaderText = "EVENTLOCATION";
            this.EVENTLOCATION.Name = "EVENTLOCATION";
            // 
            // EVENTDATE
            // 
            this.EVENTDATE.HeaderText = "EVENTDATE";
            this.EVENTDATE.Name = "EVENTDATE";
            // 
            // NOTES
            // 
            this.NOTES.HeaderText = "NOTES";
            this.NOTES.Name = "NOTES";
            // 
            // REVIEWSTATUS
            // 
            this.REVIEWSTATUS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.REVIEWSTATUS.HeaderText = "REVIEWSTATUS";
            this.REVIEWSTATUS.Name = "REVIEWSTATUS";
            this.REVIEWSTATUS.ReadOnly = true;
            this.REVIEWSTATUS.Width = 118;
            // 
            // LANGUAGE
            // 
            this.LANGUAGE.HeaderText = "LANGUAGE";
            this.LANGUAGE.Name = "LANGUAGE";
            this.LANGUAGE.ReadOnly = true;
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 324);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWnd";
            this.Text = "ETH eCitation Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importBibTeXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportECitationCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem importBibTexFromClipBoardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openECitationCSVToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn TITLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn AUTHOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn PUBLICATIONDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn EDITOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn PUBLICATIONTYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn PUBLICATIONSTATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ORGCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUBTITLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn BOOKTITLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn EDITION;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIPTION;
        private System.Windows.Forms.DataGridViewTextBoxColumn ETHDISSNR;
        private System.Windows.Forms.DataGridViewTextBoxColumn ISBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn JOURNALORSERIESTITLE;
        private System.Windows.Forms.DataGridViewTextBoxColumn VOLUME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ISSUE;
        private System.Windows.Forms.DataGridViewTextBoxColumn STARTPAGE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ENDPAGE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ISSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn PUBLISHER;
        private System.Windows.Forms.DataGridViewTextBoxColumn PUBLISHERPLACE;
        private System.Windows.Forms.DataGridViewTextBoxColumn KEYWORDS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ABSTRACT;
        private System.Windows.Forms.DataGridViewTextBoxColumn DOI;
        private System.Windows.Forms.DataGridViewTextBoxColumn FULLTEXTURL;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVENTNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVENTLOCATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVENTDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOTES;
        private System.Windows.Forms.DataGridViewTextBoxColumn REVIEWSTATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn LANGUAGE;
    }
}

