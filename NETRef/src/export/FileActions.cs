using net.sf.jabref;
using net.sf.jabref.export;
using System;
using System.Collections.Generic;
/*  Copyright (C) 2003-2011 JabRef contributors.
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System.IO;
using System.Text.RegularExpressions;
namespace net.sf.jabref.export
{

    public class FileActions
    {

        private static Regex refPat = new Regex("(#[A-Za-z]+#)"); // Used to detect string references in strings


        private static void writePreamble(TextWriter fw, string preamble)
        {
            if (preamble != null)
            {
                fw.Write("@PREAMBLE{");
                fw.Write(preamble);
                fw.Write("}" + Environment.NewLine + Environment.NewLine);
            }
        }

        /**
         * Write all strings in alphabetical order, modified to produce a safe (for BibTeX) order of the strings
         * if they reference each other.
         * @param fw The Writer to send the output to.
         * @param database The database whose strings we should write.
         * @throws IOException If anthing goes wrong in writing.
         */
        private static void writeStrings(TextWriter fw, BibtexDatabase database) {
        List<BibtexString> strings = new List<BibtexString>();
        foreach (var s in database.getStringKeySet()) {
            strings.Add(database.getString(s));
        }
        strings.Sort(new BibtexStringComparator(true));
        // First, make a Map of all entries:
        Dictionary<string, BibtexString> remaining = new Dictionary<string, BibtexString>();
        foreach (var str in strings) {
            // TODO: possible error in translation, though this looks right...
            remaining.Add(str.getName(), str);
        }
        foreach (var bs in strings) {
            if (remaining.ContainsKey(bs.getName()))
                writeString(fw, bs, remaining);
        }
    }

        private static void writeString(TextWriter fw, BibtexString bs, Dictionary<string, BibtexString> remaining)
        {
            // First remove this from the "remaining" list so it can't cause problem with circular refs:
            remaining.Remove(bs.getName());

            // Then we go through the string looking for references to other strings. If we find references
            // to strings that we will write, but still haven't, we write those before proceeding. This ensures
            // that the string order will be acceptable for BibTeX.
            string content = bs.getContent();
            Match m;
            while ((m = refPat.Match(content)).Success)
            {
                // TODO: this was group(1)
                string foundLabel = m.Groups[0].Value;
                int restIndex = content.IndexOf(foundLabel) + foundLabel.Length;
                content = content.Substring(restIndex);
                Object referred = remaining[foundLabel.Substring(1, foundLabel.Length - 1)];
                // If the label we found exists as a key in the "remaining" Map, we go on and write it now:
                if (referred != null)
                    writeString(fw, (BibtexString)referred, remaining);
            }

            fw.Write("@STRING{" + bs.getName() + " = ");
            if (!bs.getContent().Equals(""))
            {
                try
                {
                    string formatted = (new LatexFieldFormatter()).format(bs.getContent(), Globals.BIBTEX_STRING);
                    fw.Write(formatted);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(
                            Globals.lang("The # character is not allowed in BibTeX strings unless escaped as in '\\#'.") + "\n" +
                            Globals.lang("Before saving, please edit any strings containing the # character."));
                }

            }
            else
                fw.Write("{}");

            fw.Write("}" + Environment.NewLine + Environment.NewLine);
        }

        /**
         * Writes the JabRef signature and the encoding.
         *
         * @param encoding string the name of the encoding, which is part of the header.
         */
        private static void writeBibFileHeader(TextWriter writer, string encoding)
        {
            writer.Write("% ");
            writer.Write(GUIGlobals.SIGNATURE);
            writer.Write(" " + GUIGlobals.version + "." + Environment.NewLine + "% " +
                    GUIGlobals.encPrefix + encoding + Environment.NewLine + Environment.NewLine);
        }

        /**
         * Saves the database to file. Two bool values indicate whether
         * only entries with a nonzero Globals.SEARCH value and only
         * entries with a nonzero Globals.GROUPSEARCH value should be
         * saved. This can be used to let the user save only the results of
         * a search. False and false means all entries are saved.
         */
        public static void saveDatabase(BibtexDatabase database,
            Stream file, bool checkSearch, bool checkGroup, string encoding)
        {

            var types = new Dictionary<string, BibtexEntryType>();

            BibtexEntry exceptionCause = null;
            try
            {

                // Get our data stream. This stream writes only to a temporary file,
                // until committed.
                var fw = new StreamWriter(file);

                // Write signature.
                writeBibFileHeader(fw, encoding);

                // Write preamble if there is one.
                writePreamble(fw, database.getPreamble());

                // Write strings if there are any.
                writeStrings(fw, database);

                // Write database entries. Take care, using CrossRefEntry-
                // IComparable, that referred entries occur after referring
                // ones. Apart from crossref requirements, entries will be
                // sorted as they appear on the screen.
                List<BibtexEntry> sorter = getSortedEntries(database, null, true);

                FieldFormatter ff = new LatexFieldFormatter();

                foreach (var be in sorter)
                {
                    exceptionCause = be;

                    // Check if we must write the type definition for this
                    // entry, as well. Our criterion is that all non-standard
                    // types (*not* customized standard types) must be written.
                    BibtexEntryType tp = be.getType();

                    if (BibtexEntryType.getStandardType(tp.getName()) == null)
                    {
                        types.Add(tp.getName(), tp);
                    }

                    // Check if the entry should be written.
                    bool write = true;

                    if (checkSearch && !nonZeroField(be, BibtexFields.SEARCH))
                    {
                        write = false;
                    }

                    if (checkGroup && !nonZeroField(be, BibtexFields.GROUPSEARCH))
                    {
                        write = false;
                    }

                    if (write)
                    {
                        be.write(fw, ff, true);
                        fw.Write(Environment.NewLine);
                    }
                }

                // Write type definitions, if any:
                if (types.Count > 0)
                {
                    foreach (var key in types.Keys)
                    {
                        BibtexEntryType type = types[key];
                        if (type is CustomEntryType)
                        {
                            CustomEntryType tp = (CustomEntryType)type;
                            tp.save(fw);
                            fw.Write(Environment.NewLine);
                        }
                    }

                }

                fw.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    // TODO: des error handling
                    //session.cancel();
                    // repairAfterError(file, backup, INIT_OK);
                }
                catch (IOException e)
                {
                    // Argh, another error? Can we do anything?
                    throw new SaveException(ex.Message + "\n" +
                            Globals.lang("Warning: could not complete file repair; your file may "
                            + "have been corrupted. Error message") + ": " + e.Message);

                }
                throw new SaveException(ex.Message, exceptionCause);
            }
        }

        /**
         * Saves the database to file, including only the entries included in the
         * supplied input array bes.
         * 
         * @return A List containing warnings, if any.
         */
        public static void savePartOfDatabase(BibtexDatabase database,
                                                     Stream file, JabRefPreferences prefs, BibtexEntry[] bes, string encoding)
        {

            var types = new Dictionary<string, BibtexEntryType>(); // Map
            // to
            // collect
            // entry
            // type
            // definitions
            // that we must save along with entries using them.

            BibtexEntry be = null;
            bool backup = prefs.getBoolean("backup");

            try
            {

                // Define our data stream.
                var fw = new StreamWriter(file);

                // Write signature.
                writeBibFileHeader(fw, encoding);

                // Write preamble if there is one.
                writePreamble(fw, database.getPreamble());

                // Write strings if there are any.
                writeStrings(fw, database);

                // Write database entries. Take care, using CrossRefEntry-
                // IComparable, that referred entries occur after referring
                // ones. Apart from crossref requirements, entries will be
                // sorted as they appear on the screen.
                string pri, sec, ter;

                bool priD, secD, terD;
                if (!prefs.getBoolean("saveInStandardOrder"))
                {
                    // The setting is to save according to the current table order.
                    pri = prefs.get("priSort");
                    sec = prefs.get("secSort");
                    // sorted as they appear on the screen.
                    ter = prefs.get("terSort");
                    priD = prefs.getBoolean("priDescending");
                    secD = prefs.getBoolean("secDescending");
                    terD = prefs.getBoolean("terDescending");
                }
                else
                {
                    // The setting is to save in standard order: author, editor, year
                    pri = "author";
                    sec = "editor";
                    ter = "year";
                    priD = false;
                    secD = false;
                    terD = true;
                }

                var comparators = new List<IComparer<BibtexEntry>>();
                comparators.Add(new CrossRefEntryComparator());
                comparators.Add(new FieldComparator(pri, priD));
                comparators.Add(new FieldComparator(sec, secD));
                comparators.Add(new FieldComparator(ter, terD));
                comparators.Add(new FieldComparator(Globals.KEY_FIELD));
                // Use glazed lists to get a sorted view of the entries:

                List<BibtexEntry> entries = new List<BibtexEntry>();

                if ((bes != null) && (bes.Length > 0))
                    for (int i = 0; i < bes.Length; i++)
                    {
                        entries.Add(bes[i]);
                    }

                entries.Sort(new FieldComparatorStack<BibtexEntry>(comparators));

                FieldFormatter ff = new LatexFieldFormatter();

                for (var i = entries.GetEnumerator(); i.MoveNext(); )
                {
                    be = i.Current;

                    // Check if we must write the type definition for this
                    // entry, as well. Our criterion is that all non-standard
                    // types (*not* customized standard types) must be written.
                    BibtexEntryType tp = be.getType();
                    if (BibtexEntryType.getStandardType(tp.getName()) == null)
                    {
                        types.Add(tp.getName(), tp);
                    }

                    be.write(fw, ff, true);
                    fw.Write(Environment.NewLine);
                }

                // Write type definitions, if any:
                if (types.Count > 0)
                {
                    foreach (var i in types.Keys)
                    {
                        CustomEntryType tp = (CustomEntryType)types[i];
                        tp.save(fw);
                        fw.Write(Environment.NewLine);
                    }

                }

                fw.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    // TODO:
                    //session.cancel();
                    //repairAfterError(file, backup, status);
                }
                catch (IOException e)
                {
                    // Argh, another error? Can we do anything?
                    throw new SaveException(ex.Message + "\n" +
                            Globals.lang("Warning: could not complete file repair; your file may "
                            + "have been corrupted. Error message: ") + e.Message);
                }
                throw new SaveException(ex.Message, be);
            }
        }

        /*
        * We have begun to use getSortedEntries() for both database save operations
        * and non-database save operations.  In a non-database save operation
        * (such as the exportDatabase call), we do not wish to use the
        * global preference of saving in standard order.
        */
        public static List<BibtexEntry> getSortedEntries(BibtexDatabase database, ICollection<string> keySet, bool isSaveOperation)
        {
            var comparators = new List<IComparer<BibtexEntry>>();

            bool inOriginalOrder = isSaveOperation ? Globals.prefs.getBoolean("saveInOriginalOrder") :
                Globals.prefs.getBoolean("exportInOriginalOrder");
            if (inOriginalOrder)
            {
                // Sort entries based on their creation order, utilizing the fact
                // that IDs used for entries are increasing, sortable numbers.
                comparators.Add(new CrossRefEntryComparator());
                comparators.Add(new IdComparator());

            }
            else
            {
                string pri, sec, ter;
                bool priD, secD, terD = false;

                bool inStandardOrder = isSaveOperation ? Globals.prefs.getBoolean("saveInStandardOrder") :
                    Globals.prefs.getBoolean("exportInStandardOrder");
                if (!inStandardOrder)
                {
                    // The setting is to save according to the current table order.
                    pri = Globals.prefs.get("priSort");
                    sec = Globals.prefs.get("secSort");
                    // sorted as they appear on the screen.
                    ter = Globals.prefs.get("terSort");
                    priD = Globals.prefs.getBoolean("priDescending");
                    secD = Globals.prefs.getBoolean("secDescending");
                    terD = Globals.prefs.getBoolean("terDescending");

                }
                else
                {
                    // The setting is to save in standard order: author, editor, year
                    pri = "author";
                    sec = "editor";
                    ter = "year";
                    priD = false;
                    secD = false;
                    terD = true;
                }

                if (isSaveOperation)
                    comparators.Add(new CrossRefEntryComparator());
                comparators.Add(new FieldComparator(pri, priD));
                comparators.Add(new FieldComparator(sec, secD));
                comparators.Add(new FieldComparator(ter, terD));
                comparators.Add(new FieldComparator(Globals.KEY_FIELD));
            }

            List<BibtexEntry> entries = new List<BibtexEntry>();

            if (keySet == null)
                keySet = database.getKeySet();

            if (keySet != null)
            {
                var i = keySet.GetEnumerator();

                for (; i.MoveNext(); )
                {
                    entries.Add(database.getEntryById((i.Current)));
                }
            }
            entries.Sort(new FieldComparatorStack<BibtexEntry>(comparators));
            return entries;
        }

        /** Returns true iff the entry has a nonzero value in its field.
         */
        private static bool nonZeroField(BibtexEntry be, string field)
        {
            string o = (be.getField(field));

            return ((o != null) && !o.Equals("0"));
        }
    }
}


///////////////////////////////////////////////////////////////////////////////
//  END OF FILE.
///////////////////////////////////////////////////////////////////////////////
