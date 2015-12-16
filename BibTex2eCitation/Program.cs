/*
 * Developed by Ingo Scholtes
 * (c) Copyright ETH Zürich, Chair of Systems Design, 2014-2015
 * */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BibTex2eCitation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWnd());
        }
    }
}
