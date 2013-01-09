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
using System.Text.RegularExpressions;
namespace net.sf.jabref {


public class Globals {
	public static readonly string SIGNATURE =
			"This file was created with JabRef";
    
	public static readonly string BIBTEX_STRING = "__string";

	public static readonly string KEY_FIELD = "bibtexkey" ;

	public static string META_FLAG = "jabref-meta: ";
	public static string META_FLAG_OLD = "bibkeeper-meta: ";
	public static string ENTRYTYPE_FLAG = "jabref-entrytype: ";
	
    public static readonly string FILE_FIELD = "file";

    public static string JOURNALS_FILE_BUILTIN = "/resource/journalList.txt";
    
    public static readonly string COL_DEFINITION_FIELD_SEPARATOR = "/";
    
    public static readonly string TYPE_HEADER = "entrytype";

	// TODO: public static ResourceBundle messages, menuTitles, intMessages;

	public static string VERSION, BUILD, BUILD_DATE;

	// TODO: public static Locale locale;

	public static readonly string FILETYPE_PREFS_EXT = "_dir", SELECTOR_META_PREFIX = "selector_",
        PROTECTED_FLAG_META = "protectedFlag",
        LAYOUT_PREFIX = "/resource/layout/", MAC = "Mac OS X",
		DOI_LOOKUP_PREFIX = "http://dx.doi.org/", NONE = "_non__",
		ARXIV_LOOKUP_PREFIX = "http://arxiv.org/abs/",
		FORMATTER_PACKAGE = "net.sf.jabref.export.layout.format.";

	public static string[] ENCODINGS, ALL_ENCODINGS = // (string[])
		// Charset.availableCharsets().keySet().toArray(new
		// string[]{});
		new string[] { "ISO8859_1", "UTF8", "UTF-16", "ASCII", "Cp1250", "Cp1251", "Cp1252",
			"Cp1253", "Cp1254", "Cp1257", "SJIS",
            "KOI8_R", // Cyrillic
			"EUC_JP", // Added Japanese encodings.
			"Big5", "Big5_HKSCS", "GBK", "ISO8859_2", "ISO8859_3", "ISO8859_4", "ISO8859_5",
			"ISO8859_6", "ISO8859_7", "ISO8859_8", "ISO8859_9", "ISO8859_13", "ISO8859_15" };
    public static Dictionary<string,string> ENCODING_NAMES_LOOKUP;

    // string array that maps from month number to month string label:
	public static string[] MONTHS = new string[] { "jan", "feb", "mar", "apr", "may", "jun", "jul",
		"aug", "sep", "oct", "nov", "dec" };

	// Dictionary that maps from month string labels to
	public static Dictionary<string, string> MONTH_STRINGS = new Dictionary<string, string>();
	static Globals() {
		MONTH_STRINGS.Add("jan", "January");
		MONTH_STRINGS.Add("feb", "February");
		MONTH_STRINGS.Add("mar", "March");
		MONTH_STRINGS.Add("apr", "April");
		MONTH_STRINGS.Add("may", "May");
		MONTH_STRINGS.Add("jun", "June");
		MONTH_STRINGS.Add("jul", "July");
		MONTH_STRINGS.Add("aug", "August");
		MONTH_STRINGS.Add("sep", "September");
		MONTH_STRINGS.Add("oct", "October");
		MONTH_STRINGS.Add("nov", "November");
		MONTH_STRINGS.Add("dec", "December");
    }

	public static JabRefPreferences prefs = null;

	public static readonly string NEWLINE = Environment.NewLine;
    public static readonly int NEWLINE_LENGTH = Environment.NewLine.Length;

	public static string lang(string key, string[] strs) {
		string translation = null;
		if (translation == null)
			translation = key;

		if ((translation != null) && (translation.Length != 0)) {
			translation = translation.Replace("_", " ");
			StringBuilder sb = new StringBuilder();
			bool b = false;
			char c;
			for (int i = 0; i < translation.Length; ++i) {
				c = translation[i];
				if (c == '%') {
					b = true;
				} else {
					if (!b) {
						sb.Append(c);
					} else {
						b = false;
						try {
							int index = int.Parse(c.ToString());
							if (strs != null && index >= 0 && index <= strs.Length)
								sb.Append(strs[index]);
						} catch (FormatException e) {
							// Append literally (for quoting) or insert special
							// symbol
							switch (c) {
							case 'c': // colon
								sb.Append(':');
								break;
							case 'e': // equal
								sb.Append('=');
								break;
							default: // anything else, e.g. %
								sb.Append(c);
                                break;
							}
						}
					}
				}
			}
			return sb.ToString();
		}
		return key;
	}

	public static string lang(string key) {
		return lang(key, (string[]) null);
	}

	public static string lang(string key, string s1) {
		return lang(key, new string[] { s1 });
	}

	public static string lang(string key, string s1, string s2) {
		return lang(key, new string[] { s1, s2 });
	}

	public static string lang(string key, string s1, string s2, string s3) {
		return lang(key, new string[] { s1, s2, s3 });
	}
    
    public static string SPECIAL_COMMAND_CHARS = "\"`^~'c=";

	public static Dictionary<string, string> HTML_CHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> HTMLCHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> XML_CHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> ASCII2XML_CHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> UNICODE_CHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> RTFCHARS = new Dictionary<string, string>();
	public static Dictionary<string, string> URL_CHARS = new Dictionary<string,string>();


	/**
	 * Returns a reg exp pattern in the form (w1) | (w2) | ...
	 * wi are escaped if no regex search is enabled
	 */
	public static Regex getPatternForWords(List<string> words) {
        if ((words == null) || (words.Count == 0) || (words[0].Length == 0))
            return new Regex("");
		
		bool regExSearch = Globals.prefs.getBoolean("regExpSearch");
		
		// compile the words to a regex in the form (w1) | (w2) | (w3)
        // TODO: REgex.Escape might not work...
		string searchPattern = "(" + (regExSearch ? words[0] : Regex.Escape(words[0])) + ")";
		for (int i = 1; i < words.Count; i++) {
			searchPattern = searchPattern + ("|(") + (regExSearch ? words[i] : Regex.Escape(words[i])) + (")");
		}

		Regex pattern;
		if (!Globals.prefs.getBoolean("caseSensitiveSearch")) {
            pattern = new Regex(searchPattern, RegexOptions.IgnoreCase);
		} else {
			pattern = new Regex(searchPattern);
		}
		
		return pattern;
	}

}
}