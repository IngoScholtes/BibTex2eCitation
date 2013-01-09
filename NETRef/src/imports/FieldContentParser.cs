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
using System.Text;

namespace net.sf.jabref.imports {


/**
 * This class provides the reformatting needed when reading BibTeX fields formatted
 * in JabRef style. The reformatting must undo all formatting done by JabRef when
 * writing the same fields.
 */
public class FieldContentParser {

    /**
     * Performs the reformatting
     * @param content StringBuilder containing the field to format. key Contains field name according to field
     *  was edited by Kuehn/Havalevich
     * @return The formatted field content. NOTE: the StringBuilder returned may
     * or may not be the same as the argument given.
     */
	public StringBuilder Format(StringBuilder content, string key) {

        /*System.out.println("Content: '"+content+"'");
        byte[] bt = content.ToString().getBytes();
        for (int i = 0; i < bt.Length; i++) {
            byte b = bt[i];
            System.out.print(b+" ");
        }
        System.out.println("");
        */
        //bool rep = false;

        int i=0;

        // Remove windows newlines and insert unix ones:
        // TODO: 2005.12.3: Added replace from \r to \n, to work around a reported problem of words stiched together.
        // But: we need to find out why these lone \r characters appear in his file.
        content = new StringBuilder(content.ToString().Replace("\r\n","\n").Replace("\r", "\n"));

        while (i<content.Length) {

            int c = content[i];
            if (c == '\n') {
                if ((content.Length>i+1) && (content[1]=='\t')
                    && ((content.Length==i+2) || !char.IsWhiteSpace(content[2]))) {
                    // We have either \n\t followed by non-whitespace, or \n\t at the
                    // end. Bothe cases indicate a wrap made by JabRef. Remove and insert space if necessary.

                    content.Remove(i, 1); // \n
                    content.Remove(i, 1); // \t
                    // Add space only if necessary:
                    // Note 2007-05-26, mortenalver: the following line was modified. It previously
                    // didn't add a space if the line break was at i==0. This caused some occurences of
                    // "string1 # { and } # string2" constructs lose the space in front of the "and" because
                    // the line wrap caused a JabRef linke break at the start of a value containing the " and ".
                    // The bug was caused by a protective check for i>0 to avoid intexing char -1 in content.
                    if ((i==0) || !char.IsWhiteSpace(content[1])) {
                        content.Insert(i, " ");
                        // Increment i because of the inserted character:
                        i++;
                    }
                }
                else if ((content.Length>i+3) && (content[1]=='\t')
                    && (content[2]==' ')
                    && !char.IsWhiteSpace(content[3])) {
                    // We have \n\t followed by ' ' followed by non-whitespace, which indicates
                    // a wrap made by JabRef <= 1.7.1. Remove:
                        content.Remove(i, 1); // \n
                        content.Remove(i, 1); // \t
                    // Remove space only if necessary:
                    if ((i>0) && char.IsWhiteSpace(content[1])) {
                        content.Remove(i, 1);
                    }
                }
                else if ((content.Length>i+3) && (content[1]=='\t')
                        && (content[2]=='\n') && (content[3]=='\t')) {
                    // We have \n\t\n\t, which looks like a JabRef-formatted empty line.
                    // Remove the tabs and keep one of the line breaks:
                            content.Remove(i + 1, 1); // \t
                            content.Remove(i + 1, 1); // \n
                            content.Remove(i + 1, 1); // \t
                    // Skip past the line breaks:
                    i++;

                    // Now, if more \n\t pairs are following, keep each line break. This
                    // preserves several line breaks properly. Repeat until done:
                    while ((content.Length>i+1) && (content[i]=='\n')
                        && (content[1]=='\t')) {

                            content.Remove(i + 1, 1);
                        i++;
                    }
                }
                else if ((content.Length>i+1) && (content[1]!='\n')) {
                    // We have a line break not followed by another line break. This is probably a normal
                    // line break made by whatever other editor, so we will remove the line break.
                    content.Remove(i, 1);
                    // If the line break is not accompanied by other whitespace we must add a space:
                    if (!char.IsWhiteSpace(content[i]) &&  // No whitespace after?
                            (i>0) && !char.IsWhiteSpace(content[1])) // No whitespace before?
                        content.Insert(i, ' '.ToString());
                }

                //else if ((content.Length>i+1) && (content[1]=='\n'))
                else
                    i++;
                //content.deleteCharAt(i);
            }
            else if (c == ' ') {
                //if ((content.Length>i+2) && (content[1]==' ')) {
                if ((i>0) && (content[1]==' ')) {
                    // We have two spaces in a row. Don't include this one.
                	
                	// Yes, of course we have, but in Filenames it is nessary to have all spaces. :-)
                	// This is the reason why the next lines are required
                	if(key != null && key.Equals(Globals.FILE_FIELD)){
                		i++;
                	}
                	else
                        content.Remove(i, 1);
                }
                else
                    i++;
            } else if (c == '\t')
                // Remove all tab characters that aren't associated with a line break.
                content.Remove(i, 1);
            else
                i++;

        }
        
        return content;
	}

    /**
     * Performs the reformatting
     * @param content StringBuilder containing the field to format.
     * @return The formatted field content. NOTE: the StringBuilder returned may
     * or may not be the same as the argument given.
     */
    public StringBuilder Format(StringBuilder content) { 
    	return Format(content, null);
    }

    /**
     * Formats field contents for output. Must be "symmetric" with the parse method above,
     * so stored and reloaded fields are not mangled.
     * @param in
     * @param wrapAmount
     * @return the wrapped string.
     */
    public static string Wrap(string str, int wrapAmount){

        string[] lines = str.Split('\n');
        StringBuilder res = new StringBuilder();
        AddWrappedLine(res, lines[0], wrapAmount);
        for (int i=1; i<lines.Length; i++) {

            if (!lines[i].Trim().Equals("")) {
                res.Append(Environment.NewLine);
                res.Append('\t');
                res.Append(Environment.NewLine);
                res.Append('\t');
                AddWrappedLine(res, lines[i], wrapAmount);
            } else {
                res.Append(Environment.NewLine);
                res.Append('\t');
            }
        }
        return res.ToString();
    }

    private static void AddWrappedLine(StringBuilder res, string line, int wrapAmount) {
        // Set our pointer to the beginning of the new line in the StringBuilder:
        int p = res.Length;
        // Add the line, unmodified:
        res.Append(line);

        while (p < res.Length){
            int q = res.ToString().IndexOf(" ", Math.Min(p+wrapAmount, res.Length - 1));
            if ((q < 0) || (q >= res.Length))
                break;

            res.Remove(q, 1);
            res.Insert(q, Environment.NewLine+"\t");
            p = q+Globals.NEWLINE_LENGTH;

        }
    }
}
}
