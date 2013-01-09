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

// created by : Morten O. Alver 2003

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace net.sf.jabref {

/**
 * utility functions
 */
public class Util {

    private readonly static object _lock = new object();

	// TODO: readonly static NumberFormat idFormat;

    public static Regex remoteLinkPattern = new Regex("[a-z]+://.*");

    public static int MARK_COLOR_LEVELS = 6,
            MAX_MARKING_LEVEL = MARK_COLOR_LEVELS-1,
            IMPORT_MARK_LEVEL = MARK_COLOR_LEVELS;
    public static Regex markNumberPattern = new Regex(Globals.prefs.MARKING_WITH_NUMBER_PATTERN);


	static Util() {
		/*idFormat = NumberFormat.getInstance();
		idFormat.setMinimumIntegerDigits(8);
		idFormat.setGroupingUsed(false);*/
	}
        
	public static string nCase(string s) {
		// Make first character of string uppercase, and the
		// rest lowercase.
		if (s.Length > 1)
			return s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length).ToLower();
		else
			return s.ToUpper();

	}

	public static string checkName(string s) {
		// Append '.bib' to the string unless it ends with that.
		if (s.Length < 4 || !s.Substring(s.Length - 4).Equals(".bib", System.StringComparison.CurrentCultureIgnoreCase)) {
			return s + ".bib";
		}
		return s;
	}

	private static int idCounter = 0;

	public static string createNeutralId() {
        lock (_lock) {
		    // TODO: return idFormat.format(idCounter++);
            return (idCounter++).ToString("00000000"); 
        }
	}

	/**
	 * This method translates a field or string from Bibtex notation, with
	 * possibly text contained in " " or { }, and string references,
	 * concatenated by '#' characters, into Bibkeeper notation, where string
	 * references are enclosed in a pair of '#' characters.
	 */
	public static string parseField(string content) {
		
		if (content.Length == 0)
			return content;
		
		string[] strings = content.Split('#');
		StringBuilder result = new StringBuilder();
		for (int i = 0; i < strings.Length; i++){
			string s = strings[i].Trim();
			if (s.Length > 0){
				char c = s[0];
				// string reference or not?
				if (c == '{' || c == '"'){
					result.Append(shaveString(strings[i]));	
				} else {
					// This part should normally be a string reference, but if it's
					// a pure number, it is not.
					string s2 = shaveString(s);
					try {
						int.Parse(s2);
						// If there's no exception, it's a number.
						result.Append(s2);
					} catch (FormatException ex) {
						// otherwise Append with hashes...
						result.Append('#').Append(s2).Append('#');
					}
				}
			}
		}
		return result.ToString();
	}


	public static string shaveString(string s) {
		// returns the string, after shaving off whitespace at the beginning
		// and end, and removing (at most) one pair of braces or " surrounding
		// it.
		if (s == null)
			return null;
		char ch, ch2;
		int beg = 0, end = s.Length;
		// We start out assuming nothing will be removed.
		bool begok = false, endok = false;
		while (!begok) {
			if (beg < s.Length) {
				ch = s[beg];
				if (char.IsWhiteSpace(ch))
					beg++;
				else
					begok = true;
			} else
				begok = true;

		}
		while (!endok) {
			if (end > beg + 1) {
				ch = s[end - 1];
				if (char.IsWhiteSpace(ch))
					end--;
				else
					endok = true;
			} else
				endok = true;
		}

		if (end > beg + 1) {
			ch = s[beg];
			ch2 = s[end - 1];
			if (((ch == '{') && (ch2 == '}')) || ((ch == '"') && (ch2 == '"'))) {
				beg++;
				end--;
			}
		}
		s = s.Substring(beg, end);
		return s;
	}


	static public string _wrap2(string inStr, int wrapAmount) {
		// The following line cuts out all whitespace and replaces them with
		// single
		// spaces:
		// in = in.Replace("[ ]+"," ").Replace("[\\t]+"," ");
		// StringBuilder out = new StringBuilder(in);
		StringBuilder outStr = new StringBuilder(inStr.Replace("[ \\t\\r]+", " "));

		int p = inStr.Length - wrapAmount;
		int lastInserted = -1;
		while (p > 0) {
			p = outStr.ToString().LastIndexOf(" ", p);
			if (p <= 0 || p <= 20)
				break;
			int lbreak = outStr.ToString().IndexOf("\n", p);
			if ((lbreak > p) && ((lastInserted >= 0) && (lbreak < lastInserted))) {
				p = lbreak - wrapAmount;
			} else {
				outStr.Insert(p, "\n\t");
				lastInserted = p;
				p -= wrapAmount;
			}
		}
		return outStr.ToString();
	}

	static public string wrap2(string inStr, int wrapAmount) {
		return net.sf.jabref.imports.FieldContentParser.Wrap(inStr, wrapAmount);
	}

	/**
	 * Takes a string array and returns a string with the array's elements
	 * delimited by a certain string.
	 * 
	 * @param strs
	 *            string array to convert.
	 * @param delimiter
	 *            string to use as delimiter.
	 * @return Delimited string.
	 */
	public static string stringArrayToDelimited(string[] strs, string delimiter) {
		if ((strs == null) || (strs.Length == 0))
			return "";
		if (strs.Length == 1)
			return strs[0];
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < strs.Length - 1; i++) {
			sb.Append(strs[i]);
			sb.Append(delimiter);
		}
		sb.Append(strs[strs.Length - 1]);
		return sb.ToString();
	}

	/**
	 * Takes a delimited string, splits it and returns
	 * 
	 * @param names
	 *            a <code>string</code> value
	 * @return a <code>string[]</code> value
	 */
	public static string[] delimToStringArray(string names, string delimiter) {
		if (names == null)
			return null;
		return names.Split(new string[] { delimiter }, StringSplitOptions.None);
	}


	/**
	 * Removes optional square brackets from the string s
	 *
	 * @param s
	 * @return
	 */
	public static string stripBrackets(string s) {
		int beginIndex = (s.Length > 0 && s[0] == '[' ? 1 : 0);
		int endIndex = (s.EndsWith("]") ? s.Length - 1 : s.Length);
		return s.Substring(beginIndex, endIndex);
	}

	/*public static List<string[]> parseMethodsCalls(string calls) {
        // TODO: this is going to do some weird stuff...
		List<string[]> result = new List<string[]>();

		char[] c = calls.ToCharArray();

		int i = 0;

		while (i < c.Length) {

			int start = i;
			if (char.isJavaIdentifierStart(c[i])) {
				i++;
				while (i < c.Length && (char.IsJavaIdentifierPart(c[i]) || c[i] == '.')) {
					i++;
				}
				if (i < c.Length && c[i] == '(') {

					string method = calls.Substring(start, i);

					// Skip the brace
					i++;

					if (i < c.Length){
						if (c[i] == '"'){
							// Parameter is in format "xxx"

							// Skip "
							i++;

							int startParam = i;
							i++;
		                    bool escaped = false;
							while (i + 1 < c.Length &&
                                    !(!escaped && c[i] == '"' && c[i + 1] == ')')) {
                                if (c[i] == '\\') {
                                    escaped = !escaped;
                                }
                                else
                                    escaped = false;
                                i++;

                            }

							string param = calls.Substring(startParam, i);
		
							result.Add(new string[] { method, param });
						} else {
							// Parameter is in format xxx

							int startParam = i;

							while (i < c.Length && c[i] != ')') {
								i++;
							}

							string param = calls.Substring(startParam, i);

							result.Add(new string[] { method, param });


						}
					} else {
						// Incorrecly terminated open brace
						result.Add(new string[] { method });
					}
				} else {
					string method = calls.Substring(start, i);
					result.Add(new string[] { method });
				}
			}
			i++;
		}

		return result;
	}*/


	static Regex squareBracketsPattern = new Regex("\\[.*?\\]");

	/**
	 * Concatenate all strings in the array from index 'from' to 'to' (excluding
	 * to) with the given separator.
	 * 
	 * Example:
	 * 
	 * string[] s = "ab/cd/ed".split("/"); join(s, "\\", 0, s.Length) ->
	 * "ab\\cd\\ed"
	 * 
	 * @param strings
	 * @param separator
	 * @param from
	 * @param to
	 *            Excluding strings[to]
	 * @return
	 */
	public static string join(string[] strings, string separator, int from, int to) {
		if (strings.Length == 0 || from >= to)
			return "";
		
		from = Math.Max(from, 0);
		to = Math.Min(strings.Length, to);

		StringBuilder sb = new StringBuilder();
		for (int i = from; i < to - 1; i++) {
			sb.Append(strings[i]).Append(separator);
		}
		return sb.Append(strings[to - 1]).ToString();
	}


	/**
	 * This method is called at startup, and makes necessary adaptations to
	 * preferences for users from an earlier version of Jabref.
	 */
	public static void performCompatibilityUpdate() {

		// Make sure "abstract" is not in General fields, because
		// Jabref 1.55 moves the abstract to its own tab.
		string genFields = Globals.prefs.get("generalFields");
		// pr(genFields+"\t"+genFields.IndexOf("abstract"));
		if (genFields.IndexOf("abstract") >= 0) {
			// pr(genFields+"\t"+genFields.IndexOf("abstract"));
			string newGen;
			if (genFields.Equals("abstract"))
				newGen = "";
			else if (genFields.IndexOf(";abstract;") >= 0) {
				newGen = genFields.Replace(";abstract;", ";");
			} else if (genFields.IndexOf("abstract;") == 0) {
				newGen = genFields.Replace("abstract;", "");
			} else if (genFields.IndexOf(";abstract") == genFields.Length - 9) {
				newGen = genFields.Replace(";abstract", "");
			} else
				newGen = genFields;
			// pr(newGen);
			Globals.prefs.put("generalFields", newGen);
		}

	}

    // -------------------------------------------------------------------------------

	/**
	 * extends the filename with a default Extension, if no Extension '.x' could
	 * be found
	 */
	public static string getCorrectFileName(string orgName, string defaultExtension) {
		if (orgName == null)
			return "";

		string back = orgName;
		int t = orgName.IndexOf(".", 1); // hidden files Linux/Unix (?)
		if (t < 1)
			back = back + "." + defaultExtension;

		return back;
	}

	/**
	 * Quotes each and every character, e.g. '!' as &#33;. Used for verbatim
	 * display of arbitrary strings that may contain HTML entities.
	 */
	public static string quoteForHTML(string s) {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < s.Length; ++i) {
			sb.Append("&#" + (int) s[i] + ";");
		}
		return sb.ToString();
	}

	public static string quote(string s, string specials, char quoteChar) {
		return quote(s, specials, quoteChar, 0);
	}

	/**
	 * Quote special characters.
	 * 
	 * @param s
	 *            The string which may contain special characters.
	 * @param specials
	 *            A string containing all special characters except the quoting
	 *            character itself, which is automatically quoted.
	 * @param quoteChar
	 *            The quoting character.
	 * @param linewrap
	 *            The number of characters after which a linebreak is inserted
	 *            (this linebreak is undone by unquote()). Set to 0 to disable.
	 * @return A string with every special character (including the quoting
	 *         character itself) quoted.
	 */
	public static string quote(string s, string specials, char quoteChar, int linewrap) {
		StringBuilder sb = new StringBuilder();
		char c;
		int linelength = 0;
		bool isSpecial;
		for (int i = 0; i < s.Length; ++i) {
			c = s[i];
			isSpecial = specials.IndexOf(c) >= 0 || c == quoteChar;
			// linebreak?
			if (linewrap > 0
				&& (++linelength >= linewrap || (isSpecial && linelength >= linewrap - 1))) {
				sb.Append(quoteChar);
				sb.Append('\n');
				linelength = 0;
			}
			if (isSpecial) {
				sb.Append(quoteChar);
				++linelength;
			}
			sb.Append(c);
		}
		return sb.ToString();
	}

	/**
	 * Unquote special characters.
	 * 
	 * @param s
	 *            The string which may contain quoted special characters.
	 * @param quoteChar
	 *            The quoting character.
	 * @return A string with all quoted characters unquoted.
	 */
	public static string unquote(string s, char quoteChar) {
		StringBuilder sb = new StringBuilder();
		char c;
		bool quoted = false;
		for (int i = 0; i < s.Length; ++i) {
			c = s[i];
			if (quoted) { // Append literally...
				if (c != '\n') // ...unless newline
					sb.Append(c);
				quoted = false;
			} else if (c != quoteChar) {
				sb.Append(c);
			} else { // quote char
				quoted = true;
			}
		}
		return sb.ToString();
	}

	/**
	 * Quote all regular expression meta characters in s, in order to search for
	 * s literally.
	 */
	public static string quoteMeta(string s) {
		// work around a bug: trailing backslashes have to be quoted
		// individually
		int i = s.Length - 1;
		StringBuilder bs = new StringBuilder("");
		while ((i >= 0) && (s[i] == '\\')) {
			--i;
			bs.Append("\\\\");
		}
		s = s.Substring(0, i + 1);
		return "\\Q" + s.Replace("\\\\E", "\\\\E\\\\\\\\E\\\\Q") + "\\E" + bs.ToString();
	}

	/*
	 * This method "tidies" up e.g. a keyword string, by alphabetizing the words
	 * and removing all duplicates.
	 */
	public static string sortWordsAndRemoveDuplicates(string text) {

		string[] words = text.Split(new string[] {", "}, StringSplitOptions.None);
		Dictionary<string, bool> set = new Dictionary<string, bool>();
		for (int i = 0; i < words.Length; i++)
			set.Add(words[i], true);
		StringBuilder sb = new StringBuilder();
		for (var i = set.GetEnumerator(); i.MoveNext();) {
			sb.Append(i.Current);
			sb.Append(", ");
		}
		if (sb.Length > 2)
			sb.Remove(sb.Length - 2, sb.Length);
		string result = sb.ToString();
		return result.Length > 2 ? result : "";
	}

	// ========================================================
	// lot of abreviations in medline
	// PKC etc convert to {PKC} ...
	// ========================================================
	static Regex titleCapitalPattern = new Regex("[A-Z]+");

	/**
	 * Wrap all uppercase letters, or sequences of uppercase letters, in curly
	 * braces. Ignore letters within a pair of # character, as these are part of
	 * a string label that should not be modified.
	 * 
	 * @param s
	 *            The string to modify.
	 * @return The resulting string after wrapping capitals.
	 */
	public static string putBracesAroundCapitals(string s) {

		bool inString = false, isBracing = false, escaped = false;
		int inBrace = 0;
		StringBuilder buf = new StringBuilder();
		for (int i = 0; i < s.Length; i++) {
			// Update variables based on special characters:
			int c = s[i];
			if (c == '{')
				inBrace++;
			else if (c == '}')
				inBrace--;
			else if (!escaped && (c == '#'))
				inString = !inString;

			// See if we should start bracing:
			if ((inBrace == 0) && !isBracing && !inString && char.IsLetter((char) c)
				&& char.IsUpper((char) c)) {

				buf.Append('{');
				isBracing = true;
			}

			// See if we should close a brace set:
			if (isBracing && !(char.IsLetter((char) c) && char.IsUpper((char) c))) {

				buf.Append('}');
				isBracing = false;
			}

			// Add the current character:
			buf.Append((char) c);

			// Check if we are entering an escape sequence:
			if ((c == '\\') && !escaped)
				escaped = true;
			else
				escaped = false;

		}
		// Check if we have an unclosed brace:
		if (isBracing)
			buf.Append('}');

		return buf.ToString();

		/*
		 * if (s.Length == 0) return s; // Protect against ArrayIndexOutOf....
		 * StringBuilder buf = new StringBuilder();
		 * 
		 * Matcher mcr = titleCapitalPattern.matcher(s.Substring(1)); while
		 * (mcr.find()) { string replaceStr = mcr.group();
		 * mcr.AppendReplacement(buf, "{" + replaceStr + "}"); }
		 * mcr.AppendTail(buf); return s.Substring(0, 1) + buf.ToString();
		 */
	}


    /**
     * Will convert a two digit year using the following scheme (describe at
     * http://www.filemaker.com/help/02-Adding%20and%20view18.html):
     * 
     * If a two digit year is encountered they are matched against the last 69
     * years and future 30 years.
     * 
     * For instance if it is the year 1992 then entering 23 is taken to be 1923
     * but if you enter 23 in 1993 then it will evaluate to 2023.
     * 
     * @param year
     *            The year to convert to 4 digits.
     * @return
     */
    public static String toFourDigitYear(String year)
    {
        // TODO: check Calendar year thing returns teh same
        return toFourDigitYear(year, System.DateTime.Now.Year);
    }

	/**
	 * Will convert a two digit year using the following scheme (describe at
	 * http://www.filemaker.com/help/02-Adding%20and%20view18.html):
	 * 
	 * If a two digit year is encountered they are matched against the last 69
	 * years and future 30 years.
	 * 
	 * For instance if it is the year 1992 then entering 23 is taken to be 1923
	 * but if you enter 23 in 1993 then it will evaluate to 2023.
	 * 
	 * @param year
	 *            The year to convert to 4 digits.
	 * @return
	 */
	public static string toFourDigitYear(string year, int thisYear) {
		if (year.Length != 2)
			return year;
		try {
			int thisYearTwoDigits = thisYear % 100;
			int thisCentury = thisYear - thisYearTwoDigits;

			int yearNumber = int.Parse(year);

			if (yearNumber == thisYearTwoDigits) {
                return (thisYear).ToString();
			}
			// 20 , 90
			// 99 > 30
			if ((yearNumber + 100 - thisYearTwoDigits) % 100 > 30) {
				if (yearNumber < thisYearTwoDigits) {
                    return (thisCentury + yearNumber).ToString();
				} else {
					return (thisCentury - 100 + yearNumber).ToString();
				}
			} else {
				if (yearNumber < thisYearTwoDigits) {
                    return (thisCentury + 100 + yearNumber).ToString();
				} else {
                    return (thisCentury + yearNumber).ToString();
				}
			}
		} catch (FormatException e) {
			return year;
		}
	}

	/**
	 * Will return an integer indicating the month of the entry from 0 to 11.
	 * 
	 * -1 signals a unknown month string.
	 * 
	 * This method accepts three types of months given:
	 *  - Single and Double Digit months from 1 to 12 (01 to 12)
	 *  - 3 Digit BibTex strings (jan, feb, mar...)
	 *  - Full English Month identifiers.
	 * 
	 * @param month
	 * @return
	 */
	public static int getMonthNumber(string month) {

		month = month.Replace("#", "").ToLower();

		for (int i = 0; i < Globals.MONTHS.Length; i++) {
			if (month.StartsWith(Globals.MONTHS[i])) {
				return i;
			}
		}

		try {
			return int.Parse(month) - 1;
		} catch (FormatException e) {
		}
		return -1;
	}


    /**
     * Encodes a two-dimensional string array into a single string, using ':' and
     * ';' as separators. The characters ':' and ';' are escaped with '\'.
     * @param values The string array.
     * @return The encoded string.
     */
    public static string encodeStringArray(string[][] values) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < values.Length; i++) {
            sb.Append(encodeStringArray(values[i]));
            if (i < values.Length-1)
                sb.Append(';');
        }
        return sb.ToString();
    }

    /**
     * Encodes a string array into a single string, using ':' as separator.
     * The characters ':' and ';' are escaped with '\'.
     * @param entry The string array.
     * @return The encoded string.
     */
    public static string encodeStringArray(string[] entry) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < entry.Length; i++) {
            sb.Append(encodeString(entry[i]));
            if (i < entry.Length-1)
                sb.Append(':');

        }
        return sb.ToString();
    }

    /**
     * Decodes an encoded double string array back into array form. The array
     * is assumed to be square, and delimited by the characters ';' (first dim) and
     * ':' (second dim).
     * @param value The encoded string to be decoded.
     * @return The decoded string array.
     */
    public static string[][] decodeStringDoubleArray(string value) {
        List<List<string>> newList = new List<List<string>>();
        StringBuilder sb = new StringBuilder();
        List<string> thisEntry = new List<string>();
        bool escaped = false;
        for (int i=0; i<value.Length; i++) {
            char c = value[i];
            if (!escaped && (c == '\\')) {
                escaped = true;
                continue;
            }
            else if (!escaped && (c == ':')) {
                thisEntry.Add(sb.ToString());
                sb = new StringBuilder();
            }
            else if (!escaped && (c == ';')) {
                thisEntry.Add(sb.ToString());
                sb = new StringBuilder();
                newList.Add(thisEntry);
                thisEntry = new List<string>();
            }
            else sb.Append(c);
            escaped = false;
        }
        if (sb.Length > 0)
            thisEntry.Add(sb.ToString());
        if (thisEntry.Count > 0)
            newList.Add(thisEntry);

        // Convert to string[][]:
        string[][] res = new string[newList.Count][];
        for (int i = 0; i < res.Length; i++) {
            res[i] = new string[newList[i].Count];
            for (int j = 0; j < res[i].Length; j++) {
                res[i][j] = newList[i][j];
            }
        }
        return res;
    }

    private static string encodeString(string s) {
        if (s == null)
            return null;
        StringBuilder sb = new StringBuilder();
        for (int i=0; i<s.Length; i++) {
            char c = s[i];
            if ((c == ';') || (c == ':') || (c == '\\'))
                sb.Append('\\');
            sb.Append(c);
        }
        return sb.ToString();
    }

    /**
	 * Static equals that can also return the right result when one of the
	 * objects is null.
	 * 
	 * @param one
	 *            The object whose equals method is called if the first is not
	 *            null.
	 * @param two
	 *            The object passed to the first one if the first is not null.
	 * @return <code>one == null ? two == null : one.Equals(two);</code>
	 */
	public static bool equals(object one, object two) {
		return one == null ? two == null : one.Equals(two);
	}

	/**
	 * Returns the given string but with the first character turned into an
	 * upper case character.
	 * 
	 * Example: testTest becomes TestTest
	 * 
	 * @param string
	 *            The string to change the first character to upper case to.
	 * @return A string has the first character turned to upper case and the
	 *         rest unchanged from the given one.
	 */
	public static string toUpperFirstLetter(string str){
		if (str == null)
			throw new ArgumentNullException();
		
		if (str.Length == 0)
			return str;
		
		return char.ToUpper(str[0]) + str.Substring(1);
	}

    /**
     * Build a string array containing all those elements of all that are not
     * in subset.
     * @param all The array of all values.
     * @param subset The subset of values.
     * @return The remainder that is not part of the subset.
     */
    public static string[] getRemainder(string[] all, string[] subset) {
        List<string> al = new List<string>();
        for (int i = 0; i < all.Length; i++) {
            bool found = false;
            for (int j = 0; j < subset.Length; j++) {
                if (subset[j].Equals(all[i])) {
                    found = true;
                    break;
                }
            }
            if (!found) al.Add(all[i]);
        }
        return al.ToArray();
    }

    public static T[] concat<T> (T[] first, T[] second) {
        T[] result = new T[first.Length + second.Length];
        Array.Copy(first, 0, result, 0, first.Length);
        Array.Copy(second, 0, result, first.Length, second.Length);
	    return result;
    }
}
}
