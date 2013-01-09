using System;
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
using System.Collections.Generic;
using System.Text;

namespace net.sf.jabref.export {

public class LatexFieldFormatter : FieldFormatter {

    StringBuilder sb;
    int col; // First line usually starts about so much further to the right.
    readonly int STARTCOL = 4;
    private bool neverFailOnHashes = false;

    public void setNeverFailOnHashes(bool neverFailOnHashes) {
        this.neverFailOnHashes = neverFailOnHashes;
    }

    public string format(string text, string fieldName) {

        if (Globals.prefs.putBracesAroundCapitals(fieldName) && !Globals.BIBTEX_STRING.Equals(fieldName)) {
            text = Util.putBracesAroundCapitals(text);
        }

        // If the field is non-standard, we will just Append braces,
        // wrap and write.
        bool resolveStrings = true;
        if (Globals.prefs.getBoolean("resolveStringsAllFields")) {
            // Resolve strings for all fields except some:
            string[] exceptions = Globals.prefs.getStringArray("doNotResolveStringsFor");
            for (int i = 0; i < exceptions.Length; i++) {
                if (exceptions[i].Equals(fieldName)) {
                    resolveStrings = false;
                    break;
                }
            }
        } else {
            // Default operation - we only resolve strings for standard fields:
            resolveStrings = BibtexFields.isStandardField(fieldName)
                    || Globals.BIBTEX_STRING.Equals(fieldName);
        }
        if (!resolveStrings) {
            int brc = 0;
            bool ok = true;
            for (int i = 0; i < text.Length; i++) {
                char c = text[i];
                //Util.pr(""+c);
                if (c == '{') brc++;
                if (c == '}') brc--;
                if (brc < 0) {
                    ok = false;
                    break;
                }
            }
            if (brc > 0)
                ok = false;
            if (!ok)
                throw new ArgumentException("Curly braces { and } must be balanced.");

            sb = new StringBuilder("{");
            // No formatting at all for these fields, to allow custom formatting?
            //if (Globals.prefs.getBoolean("preserveFieldFormatting"))
            //  sb.Append(text);
            //else
            if (!Globals.prefs.isNonWrappableField(fieldName))
                sb.Append(Util.wrap2(text, GUIGlobals.LINE_LENGTH));
            else
                sb.Append(text);

            sb.Append('}');

            return sb.ToString();
        }

        sb = new StringBuilder();
        int pivot = 0, pos1, pos2;
        col = STARTCOL;
        // Here we assume that the user encloses any bibtex strings in #, e.g.:
        // #jan# - #feb#
        // ...which will be written to the file like this:
        // jan # { - } # feb
        checkBraces(text);


        while (pivot < text.Length) {
            int goFrom = pivot;
            pos1 = pivot;
            while (goFrom == pos1) {
                pos1 = text.IndexOf('#', goFrom);
                if ((pos1 > 0) && (text[pos1 - 1] == '\\')) {
                    goFrom = pos1 + 1;
                    pos1++;
                } else
                    goFrom = pos1 - 1; // Ends the loop.
            }

            if (pos1 == -1) {
                pos1 = text.Length; // No more occurences found.
                pos2 = -1;
            } else {
                pos2 = text.IndexOf('#', pos1 + 1);
                if (pos2 == -1) {
                    if (!neverFailOnHashes) {
                        throw new ArgumentException
                                (Globals.lang("The # character is not allowed in BibTeX fields") + ".\n" +
                                        Globals.lang("In JabRef, use pairs of # characters to indicate "
                                                + "a string.") + "\n" +
                                        Globals.lang("Note that the entry causing the problem has been selected."));
                    } else {
                        pos1 = text.Length; // just write out the rest of the text, and throw no exception
                    }
                }
            }

            if (pos1 > pivot)
                writeText(text, pivot, pos1);
            if ((pos1 < text.Length) && (pos2 - 1 > pos1))
                // We check that the string label is not empty. That means
                // an occurence of ## will simply be ignored. Should it instead
                // cause an error message?
                writeStringLabel(text, pos1 + 1, pos2, (pos1 == pivot),
                        (pos2 + 1 == text.Length));

            if (pos2 > -1) pivot = pos2 + 1;
            else pivot = pos1 + 1;
            //if (tell++ > 10) System.exit(0);
        }

        if (!Globals.prefs.isNonWrappableField(fieldName))
            return Util.wrap2(sb.ToString(), GUIGlobals.LINE_LENGTH);
        else
            return sb.ToString();


    }

    private void writeText(string text, int start_pos,
                           int end_pos) {
        /*sb.Append("{");
        sb.Append(text.Substring(start_pos, end_pos));
        sb.Append("}");*/
        sb.Append('{');
        bool escape = false, inCommandName = false, inCommand = false,
                inCommandOption = false;
        int nestedEnvironments = 0;
        StringBuilder commandName = new StringBuilder();
        char c;
        for (int i = start_pos; i < end_pos; i++) {
            c = text[i];

            // Track whether we are in a LaTeX command of some sort.
            if (char.IsLetter(c) && (escape || inCommandName)) {
                inCommandName = true;
                if (!inCommandOption)
                    commandName.Append(c);
            } else if (char.IsWhiteSpace(c) && (inCommand || inCommandOption)) {
                //System.out.println("whitespace here");
            } else if (inCommandName) {
                // This means the command name is ended.
                // Perhaps the beginning of an argument:
                if (c == '[') {
                    inCommandOption = true;
                }
                // Or the end of an argument:
                else if (inCommandOption && (c == ']'))
                    inCommandOption = false;
                    // Or the beginning of the command body:
                else if (!inCommandOption && (c == '{')) {
                    //System.out.println("Read command: '"+commandName.ToString()+"'");
                    inCommandName = false;
                    inCommand = true;
                }
                // Or simply the end of this command altogether:
                else {
                    //System.out.println("I think I read command: '"+commandName.ToString()+"'");

                    commandName.Remove(0, commandName.Length);
                    inCommandName = false;
                }
            }
            // If we are in a command body, see if it has ended:
            if (inCommand && (c == '}')) {
                //System.out.println("nestedEnvironments = " + nestedEnvironments);
                //System.out.println("Done with command: '"+commandName.ToString()+"'");
                if (commandName.ToString().Equals("begin")) {
                    nestedEnvironments++;
                }
                if (nestedEnvironments > 0 && commandName.ToString().Equals("end")) {
                    nestedEnvironments--;
                }
                //System.out.println("nestedEnvironments = " + nestedEnvironments);

                commandName.Remove(0, commandName.Length);
                inCommand = false;
            }

            // We add a backslash before any ampersand characters, with one exception: if
            // we are inside an \\url{...} command, we should write it as it is. Maybe.
            if ((c == '&') && !escape &&
                    !(inCommand && commandName.ToString().Equals("url")) &&
                    (nestedEnvironments == 0)) {
                sb.Append("\\&");
            } else
                sb.Append(c);
            escape = (c == '\\');
        }
        sb.Append('}');
    }

    private void writeStringLabel(string text, int start_pos, int end_pos,
                                  bool first, bool last) {
        //sb.Append(Util.wrap2((first ? "" : " # ") + text.Substring(start_pos, end_pos)
        //		     + (last ? "" : " # "), GUIGlobals.LINE_LENGTH));
        putIn((first ? "" : " # ") + text.Substring(start_pos, end_pos)
                + (last ? "" : " # "));
    }

    private void putIn(string s) {
        sb.Append(Util.wrap2(s, GUIGlobals.LINE_LENGTH));
    }


    private void checkBraces(string text) {

        List<int>
                left = new List<int>(5),
                right = new List<int>(5);
        int current = -1;

        // First we collect all occurences:
        while ((current = text.IndexOf('{', current + 1)) != -1)
            left.Add(current);
        while ((current = text.IndexOf('}', current + 1)) != -1)
            right.Add(current);

        // Then we throw an exception if the error criteria are met.
        if ((right.Count > 0) && (left.Count == 0))
            throw new ArgumentException
                    ("'}' character ends string prematurely.");
        if ((right.Count > 0) && (right[0] < left[0]))
            throw new ArgumentException
                    ("'}' character ends string prematurely.");
        if (left.Count != right.Count)
            throw new ArgumentException
                    ("Braces don't match.");

    }

}
}
