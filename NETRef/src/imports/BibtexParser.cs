/*
 Copyright (C) 2003-06 David Weitzman, Nizar N. Batada, Morten O. Alver, Christopher Oezbek

 All programs in this directory and
 subdirectories are published under the GNU General Public License as
 described below.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or (at
 your option) any later version.

 This program is distributed in the hope that it will be useful, but
 WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307
 USA

 Further information about the GNU GPL is available at:
 http://www.gnu.org/copyleft/gpl.ja.html

 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace net.sf.jabref.imports {

/**
 * Class for importing BibTeX-files.
 * 
 * Use:
 * 
 * BibtexParser parser = new BibtexParser(reader);
 * 
 * ParserResult result = parser.parse();
 * 
 * or
 * 
 * ParserResult result = BibtexParser.parse(reader);
 * 
 * Can be used stand-alone.
 * 
 * @author David Weitzman
 * @author Nizar N. Batada
 * @author Morten O. Alver
 * @author Christopher Oezbek 
 */
public class BibtexParser : IDisposable {
	
	private TextReader _in;

	private BibtexDatabase _db;

	private Dictionary<string, string> _meta;
	
	private Dictionary<string, BibtexEntryType> entryTypes;

	private bool _eof = false;

	private int line = 1;

	private FieldContentParser fieldContentParser = new FieldContentParser();

	private ParserResult _pr;
	
	private static readonly int LOOKAHEAD = 64;

	public BibtexParser(TextReader inFile) {

		if (inFile == null) {
			throw new NullReferenceException();
		}
		if (Globals.prefs == null) {
			Globals.prefs = JabRefPreferences.getInstance();
		}
		_in = inFile;
	}

    public void Dispose()
    {
        _in.Dispose();
    }

	/**
	 * Shortcut usage to create a Parser and read the input.
	 * 
	 * @param in -
	 *            Reader to read from
	 * @throws IOException
	 */
	public static ParserResult Parse(StreamReader inFile) {
		BibtexParser parser = new BibtexParser(inFile);
		return parser.Parse();
	}
	
	
	/**
	 * Parses BibtexEntries from the given string and returns the collection of all entries found.
	 * 
	 * @param bibtexString
	 * 
	 * @return Returns null if an error occurred, returns an empty collection if no entries where found. 
	 */
	public static ICollection<BibtexEntry> FromString(string bibtexString){
	    StringReader reader = new StringReader(bibtexString);
		BibtexParser parser = new BibtexParser(reader); 
		try {
			return parser.Parse().Database.getEntries();
		} catch (Exception e){
			return null;
		}
	}
	
	/**
	 * Parses BibtexEntries from the given string and returns one entry found (or null if none found)
	 * 
	 * It is undetermined which entry is returned, so use this in case you know there is only one entry in the string.
	 * 
	 * @param bibtexString
	 * 
	 * @return The bibtexentry or null if non was found or an error occurred.
	 */
	public static BibtexEntry SingleFromString(string bibtexString) {
		ICollection<BibtexEntry> c = FromString(bibtexString);
		if (c == null){
			return null;
		}
        var e = c.GetEnumerator();
        e.MoveNext();
		return e.Current;
	}	
	
	/**
	 * Check whether the source is in the correct format for this importer.
	 */
	public static bool IsRecognizedFormat(TextReader inStream) {
		// Our strategy is to look for the "@<type>    {" line.
		Regex pat1 = new Regex("@[a-zA-Z]*\\s*\\{");

		string str;

		while ((str = inStream.ReadLine()) != null) {

			if (pat1.Match(str).Success)
				return true;
			else if (str.StartsWith(Globals.SIGNATURE))
				return true;
		}

		return false;
	}

	private void SkipWhitespace() {
		int c;

		while (true) {
			c = Peek();
			if ((c == -1) || (c == 65535)) {
				_eof = true;
                Read();
				return;
			}

            if (char.IsWhiteSpace((char)c))
            {
                Read();
                continue;
            }
			/*
			 * try { Thread.currentThread().sleep(500); } catch
			 * (InterruptedException ex) {}
			 */
			break;
		}
	}

	private string SkipAndRecordWhitespace(int j) {
		int c;
		StringBuilder sb = new StringBuilder();
		if (j != ' ')
			sb.Append((char) j);
		while (true) {
			c = Peek();
			if ((c == -1) || (c == 65535)) {
				_eof = true;
                Read();
				return sb.ToString();
			}

			if (char.IsWhiteSpace((char) c)) {
				if (c != ' ')
					sb.Append((char) c);
                Read();
				continue;
			}
			/*
			 * try { Thread.currentThread().sleep(500); } catch
			 * (InterruptedException ex) {}
			 */
			break;
		}
		return sb.ToString();
	}

	/**
	 * Will parse the BibTex-Data found when reading from reader.
	 * 
	 * The reader will be consumed.
	 * 
	 * Multiple calls to parse() return the same results
	 * 
	 * @return ParserResult
	 * @throws IOException
	 */
	public ParserResult Parse() {

		// If we already parsed this, just return it.
		if (_pr != null)
			return _pr;

        _db = new BibtexDatabase(); // Bibtex related contents.
		_meta = new Dictionary<string, string>(); // Metadata in comments for Bibkeeper.
		entryTypes = new Dictionary<string, BibtexEntryType>(); // To store custem entry types parsed.
		_pr = new ParserResult(_db, _meta, entryTypes);

        // First see if we can find the version number of the JabRef version that
        // wrote the file:
        string versionNum = ReadJabRefVersionNumber();
        if (versionNum != null) {
            _pr.JabrefVersion = versionNum;
            SetMajorMinorVersions();
        }
        else {
            // No version number found. However, we have only
        }

        SkipWhitespace();

		try {
			while (!_eof) {
				bool found = ConsumeUncritically('@');
				if (!found)
					break;
				SkipWhitespace();
				string entryType = ParseTextToken();
				BibtexEntryType tp = BibtexEntryType.getType(entryType);
				bool isEntry = (tp != null);
				// Util.pr(tp.getName());
				if (!isEntry) {
					// The entry type name was not recognized. This can mean
					// that it is a string, preamble, or comment. If so,
					// parse and set accordingly. If not, assume it is an entry
					// with an unknown type.
					if (entryType.ToLower().Equals("preamble")) {
						_db.setPreamble(ParsePreamble());
					} else if (entryType.ToLower().Equals("string")) {
						BibtexString bs = ParseString();
						try {
							_db.addString(bs);
						} catch (KeyCollisionException ex) {
							_pr.AddWarning(Globals.lang("Duplicate string name") + ": "
								+ bs.getName());
							// ex.printStackTrace();
						}
					} else if (entryType.ToLower().Equals("comment")) {
						StringBuilder commentBuf = ParseBracketedTextExactly();
						/**
						 * 
						 * Metadata are used to store Bibkeeper-specific
						 * information in .bib files.
						 * 
						 * Metadata are stored in bibtex files in the format
						 * 
						 * @comment{jabref-meta: type:data0;data1;data2;...}
						 * 
						 * Each comment that starts with the META_FLAG is stored
						 * in the meta Dictionary, with type as key. Unluckily, the
						 * old META_FLAG bibkeeper-meta: was used in JabRef 1.0
						 * and 1.1, so we need to support it as well. At least
						 * for a while. We'll always save with the new one.
						 */
                        // TODO: unicode escape sequences
						string comment = commentBuf.ToString().Replace("[\\x0d\\x0a]", "");
						if (comment.Substring(0,
							Math.Min(comment.Length, Globals.META_FLAG.Length)).Equals(
							Globals.META_FLAG)
							|| comment.Substring(0,
								Math.Min(comment.Length, Globals.META_FLAG_OLD.Length))
								.Equals(Globals.META_FLAG_OLD)) {

							string rest;
							if (comment.Substring(0, Globals.META_FLAG.Length).Equals(
								Globals.META_FLAG))
								rest = comment.Substring(Globals.META_FLAG.Length);
							else
								rest = comment.Substring(Globals.META_FLAG_OLD.Length);

							int pos = rest.IndexOf(':');

							if (pos > 0)
								_meta.Add(rest.Substring(0, pos), rest.Substring(pos + 1));
							// We remove all line breaks in the metadata - these
							// will have been inserted
							// to prevent too long lines when the file was
							// saved, and are not part of the data.
						}

						/**
						 * A custom entry type can also be stored in a
						 * 
						 * @comment:
						 */
						if (comment.Substring(0,
							Math.Min(comment.Length, Globals.ENTRYTYPE_FLAG.Length)).Equals(
							Globals.ENTRYTYPE_FLAG)) {

							CustomEntryType typ = CustomEntryType.parseEntryType(comment);
							entryTypes.Add(typ.getName().ToLower(), typ);

						}
					} else {
						// The entry type was not recognized. This may mean that
						// it is a custom entry type whose definition will
						// appear
						// at the bottom of the file. So we use an
						// UnknownEntryType
						// to remember the type name by.
						tp = new UnknownEntryType(entryType.ToLower());
						// System.out.println("unknown type: "+entryType);
						isEntry = true;
					}
				}

				if (isEntry) // True if not comment, preamble or string.
				{
					/**
					 * Morten Alver 13 Aug 2006: Trying to make the parser more
					 * robust. If an exception is thrown when parsing an entry,
					 * drop the entry and try to resume parsing. Add a warning
					 * for the user.
					 * 
					 * An alternative solution is to try rescuing the entry for
					 * which parsing failed, by returning the entry with the
					 * exception and adding it before parsing is continued.
					 */
					try {
						BibtexEntry be = ParseEntry(tp);

						bool duplicateKey = _db.insertEntry(be);
						if (duplicateKey) // JZTODO lyrics
                            _pr.AddDuplicateKey(be.getCiteKey());
							/*_pr.AddWarning(Globals.lang("duplicate BibTeX key") + ": "
								+ be.getCiteKey() + " ("
								+ Globals.lang("grouping may not work for this entry") + ")");                        */
						else if (be.getCiteKey() == null || be.getCiteKey().Equals("")) {
							_pr.AddWarning(Globals.lang("empty BibTeX key") + ": "
								+ be.getAuthorTitleYear(40) + " ("
								+ Globals.lang("grouping may not work for this entry") + ")");
						}
					} catch (IOException ex) {
						_pr.AddWarning(Globals.lang("Error occured when parsing entry") + ": '"
							+ ex.Message + "'. " + Globals.lang("Skipped entry."));

					}
				}

				SkipWhitespace();
			}

			// Before returning the database, update entries with unknown type
			// based on parsed type definitions, if possible.
			CheckEntryTypes(_pr);

			return _pr;
		} catch (KeyCollisionException kce) {
			// kce.printStackTrace();
			throw new IOException("Duplicate ID in bibtex file: " + kce.ToString());
		}
	}

	private int Peek() {		
        return _in.Peek();
	}

	private int Read() {
		int c = _in.Read();
		if (c == '\n')
			line++;
		return c;
	}
    
	public BibtexString ParseString() {
		// Util.pr("Parsing string");
		SkipWhitespace();
		Consume('{', '(');
		// while (read() != '}');
		SkipWhitespace();
		// Util.pr("Parsing string name");
		string name = ParseTextToken();
		// Util.pr("Parsed string name");
		SkipWhitespace();
		// Util.pr("Now the contents");
		Consume('=');
		string content = ParseFieldContent(name);
		// Util.pr("Now I'm going to consume a }");
		Consume('}', ')');
		// Util.pr("Finished string parsing.");
		string id = Util.createNeutralId();
		return new BibtexString(id, name, content);
	}

	public string ParsePreamble() {
		return ParseBracketedText().ToString();
	}

	public BibtexEntry ParseEntry(BibtexEntryType tp) {
		string id = Util.createNeutralId();// createId(tp, _db);
		BibtexEntry result = new BibtexEntry(id, tp);
		SkipWhitespace();
		Consume('{', '(');
        int c = Peek();
        if ((c != '\n') && (c != '\r'))
            SkipWhitespace();
		string key = null;
		bool doAgain = true;
		while (doAgain) {
			doAgain = false;
			try {
				if (key != null)
					key = key + ParseKey();// parseTextToken(),
				else
					key = ParseKey();
			} catch (NoLabelException ex) {
				// This exception will be thrown if the entry lacks a key
				// altogether, like in "@article{ author = { ...".
				// It will also be thrown if a key Contains =.
				c = (char) Peek();
				if (char.IsWhiteSpace((char)c) || (c == '{') || (c == '\"')) {
					string fieldName = ex.Message.Trim().ToLower();
					string cont = ParseFieldContent(fieldName);
					result.setField(fieldName, cont);
				} else {
					if (key != null)
						key = key + ex.Message + "=";
					else
						key = ex.Message + "=";
					doAgain = true;
				}
			}
		}

		if ((key != null) && key.Equals(""))
			key = null;

		result.setField(Globals.KEY_FIELD, key);
		SkipWhitespace();

		while (true) {
			c = Peek();
			if ((c == '}') || (c == ')')) {
				break;
			}

			if (c == ',')
				Consume(',');

			SkipWhitespace();

			c = Peek();
			if ((c == '}') || (c == ')')) {
				break;
			}
			ParseField(result);
		}

		Consume('}', ')');
		return result;
	}

	private void ParseField(BibtexEntry entry) {
		string key = ParseTextToken().ToLower();
		// Util.pr("Field: _"+key+"_");
		SkipWhitespace();
		Consume('=');
		string content = ParseFieldContent(key);
		if (content.Length > 0) {
			if (entry.getField(key) == null)
				entry.setField(key, content);
			else {
				// The following hack enables the parser to deal with multiple
				// author or
				// editor lines, stringing them together instead of getting just
				// one of them.
				// Multiple author or editor lines are not allowed by the bibtex
				// format, but
				// at least one online database exports bibtex like that, making
				// it inconvenient
				// for users if JabRef didn't accept it.
				if (key.Equals("author") || key.Equals("editor"))
					entry.setField(key, entry.getField(key) + " and " + content);
			}
		}
	}

	private string ParseFieldContent(string key) {
		SkipWhitespace();
		StringBuilder value = new StringBuilder();
		int c = '.';

		while (((c = Peek()) != ',') && (c != '}') && (c != ')')) {

			if (_eof) {
				throw new Exception("Error in line " + line + ": EOF in mid-string");
			}
			if (c == '"') {
				StringBuilder text = ParseQuotedFieldExactly();
				value.Append(fieldContentParser.Format(text));
				/*
				 * 
				 * The following code doesn't handle {"} correctly: // value is
				 * a string consume('"');
				 * 
				 * while (!((peek() == '"') && (j != '\\'))) { j = read(); if
				 * (_eof || (j == -1) || (j == 65535)) { throw new
				 * Exception("Error in line "+line+ ": EOF in
				 * mid-string"); }
				 * 
				 * value.Append((char) j); }
				 * 
				 * consume('"');
				 */
			} else if (c == '{') {
				// Value is a string enclosed in brackets. There can be pairs
				// of brackets inside of a field, so we need to count the
				// brackets to know when the string is finished.
				StringBuilder text = ParseBracketedTextExactly();
				value.Append(fieldContentParser.Format(text, key));

			} else if (char.IsDigit((char) c)) { // value is a number

				string numString = ParseTextToken();
                // Morten Alver 2007-07-04: I don't see the point of parsing the integer
                // and converting it back to a string, so I'm removing the construct below
                // the following line:
                value.Append(numString);
                /*
                try {
					// Fixme: What is this for?
					value.Append(string.valueOf(int.Parse(numString)));
				} catch (FormatException e) {
					// If int could not be parsed then just add the text
					// Used to fix [ 1594123 ] Failure to import big numbers
					value.Append(numString);
				}
				*/
			} else if (c == '#') {
				Consume('#');
			} else {
				string textToken = ParseTextToken();
				if (textToken.Length == 0)
					throw new IOException("Error in line " + line + " or above: "
						+ "Empty text token.\nThis could be caused "
						+ "by a missing comma between two fields.");
				value.Append('#').Append(textToken).Append('#');
				// Util.pr(parseTextToken());
				// throw new Exception("Unknown field type");
			}
			SkipWhitespace();
		}

		return value.ToString();

	}

	/**
	 * Originalinhalt nach parseFieldContent(string) verschoben.
	 * @return
	 * @throws IOException
	 */
//	private string parseFieldContent() throws IOException {
//		return parseFieldContent(null);
//	}

	/**
	 * This method is used to parse string labels, field names, entry type and
	 * numbers outside brackets.
	 */
	private string ParseTextToken() {
		StringBuilder token = new StringBuilder(20);

		while (true) {
			int c = Peek();
			// Util.pr(".. "+c);
			if (c == -1) {
				_eof = true;
                Read();
				return token.ToString();
			}

			if (char.IsLetterOrDigit((char) c) || (c == ':') || (c == '-') || (c == '_')
				|| (c == '*') || (c == '+') || (c == '.') || (c == '/') || (c == '\'')) {
				token.Append((char) c);
                Read();
			} else {
				return token.ToString();
			}
		}
	}
	
	
	/**
	 * Tries to restore the key
	 * 
	 * @return rest of key on success, otherwise empty string
	 * @throws IOException
	 *             on Reader-Error
	 */
    private string FixKey() {
        // TODO: fix
        StringBuilder key = new StringBuilder();
        int lookahead_used = 0;
        char currentChar;
        bool first = true;

        // Find a char which ends key (','&&'\n') or entryfield ('='):
        do {
            if (!first) Read();
            first = false;
            currentChar = (char) Peek();
            key.Append(currentChar);
            lookahead_used++;
        } while ((currentChar != ',' && currentChar != '\n' && currentChar != '=')
                && (lookahead_used < LOOKAHEAD));

        key.Remove(key.Length - 1, 1);

        // Restore if possible:
        switch (currentChar) {
            case '=':

                // Get entryfieldname, push it back and take rest as key
                key = key.reverse();

                bool matchedAlpha = false;
                for (int i = 0; i < key.Length; i++) {
                    currentChar = key[i];

                    /// Skip spaces:
                    if (!matchedAlpha && currentChar == ' ') {
                        continue;
                    }
                    matchedAlpha = true;

                    // Begin of entryfieldname (e.g. author) -> push back:
                    Unread(currentChar);
                    if (currentChar == ' ' || currentChar == '\n') {

                        /*
                         * found whitespaces, entryfieldname completed -> key in
                         * keybuffer, skip whitespaces
                         */
                        StringBuilder newKey = new StringBuilder();
                        for (int j = i; j < key.Length; j++) {
                            currentChar = key[j];
                            if (!char.IsWhiteSpace(currentChar)) {
                                newKey.Append(currentChar);
                            }
                        }

                        // Finished, now reverse newKey and remove whitespaces:
                        _pr.AddWarning(Globals.lang("Line %0: Found corrupted BibTeX-key.",
                                line.ToString()));
                        key = newKey.reverse();
                    }
                }
                break;

            case ',':

                _pr.AddWarning(Globals.lang("Line %0: Found corrupted BibTeX-key (Contains whitespaces).",
                        line.ToString()));
                goto case '\n';

            case '\n':

                _pr.AddWarning(Globals.lang("Line %0: Found corrupted BibTeX-key (comma missing).",
                        line.ToString()));

                break;

            default:

                // No more lookahead, give up:
                UnreadBuffer(key);
                return "";
        }

        return RemoveWhitespaces(key).ToString();
    }

	/**
	 * removes whitespaces from <code>sb</code>
	 * 
	 * @param sb
	 * @return
	 */
	private StringBuilder RemoveWhitespaces(StringBuilder sb) {
		StringBuilder newSb = new StringBuilder();
		char current;
		for (int i = 0; i < sb.Length; ++i) {
			current = sb[i];
			if (!char.IsWhiteSpace(current))
				newSb.Append(current);
		}
		return newSb;
	}

	/**
	 * pushes buffer back into input
	 * 
	 * @param sb
	 * @throws IOException
	 *             can be thrown if buffer is bigger than LOOKAHEAD
	 */
	private void UnreadBuffer(StringBuilder sb) {
		for (int i = sb.Length - 1; i >= 0; --i) {
			Unread(sb[i]);
		}
	}

    private void Unread(char c)
    {
        // TODO: fix
        throw new Exception();
    }

	
	/**
	 * This method is used to parse the bibtex key for an entry.
	 */
	private string ParseKey() {
		StringBuilder token = new StringBuilder(20);

		while (true) {
			int c = Peek();
			// Util.pr(".. '"+(char)c+"'\t"+c);
			if (c == -1) {
				_eof = true;
                Read();
				return token.ToString();
			}

			// Ikke: #{}\uFFFD~\uFFFD
			//
			// G\uFFFDr: $_*+.-\/?"^
			if (!char.IsWhiteSpace((char) c)
				&& (char.IsLetterOrDigit((char) c) || ((c != '#') && (c != '{') && (c != '}')
					&& (c != '\uFFFD') && (c != '~') && (c != '\uFFFD') && (c != ',') && (c != '=')))) {
                Read();
				token.Append((char) c);
			} else {

				if (char.IsWhiteSpace((char) c)) {
					// We have encountered white space instead of the comma at
					// the end of
					// the key. Possibly the comma is missing, so we try to
					// return what we
					// have found, as the key and try to restore the rest in fixKey().
                    Read();
					return token.ToString()+FixKey();
				} else if (c == ',') {
					return token.ToString();
					// } else if (char.IsWhiteSpace((char)c)) {
					// throw new NoLabelException(token.ToString());
				} else if (c == '=') {
					// If we find a '=' sign, it is either an error, or
					// the entry lacked a comma signifying the end of the key.
                    Read();
					return token.ToString();
					// throw new NoLabelException(token.ToString());

				} else
					throw new IOException("Error in line " + line + ":" + "Character '" + (char) c
						+ "' is not " + "allowed in bibtex keys.");

			}
		}

	}

	private class NoLabelException : Exception {
	}

	private StringBuilder ParseBracketedText() {
		// Util.pr("Parse bracketed text");
		StringBuilder value = new StringBuilder();

		Consume('{');

		int brackets = 0;

		while (!((Peek() == '}') && (brackets == 0))) {

			int j = Read();
			if ((j == -1) || (j == 65535)) {
				throw new Exception("Error in line " + line + ": EOF in mid-string");
			} else if (j == '{')
				brackets++;
			else if (j == '}')
				brackets--;

			// If we encounter whitespace of any kind, read it as a
			// simple space, and ignore any others that follow immediately.
			/*
			 * if (j == '\n') { if (peek() == '\n') value.Append('\n'); } else
			 */
			if (char.IsWhiteSpace((char) j)) {
				string whs = SkipAndRecordWhitespace(j);

				// System.out.println(":"+whs+":");

				if (!whs.Equals("") && !whs.Equals("\n\t")) { // &&
																// !whs.Equals("\n"))

					whs = whs.Replace("\t", ""); // Remove tabulators.

					// while (whs.EndsWith("\t"))
					// whs = whs.Substring(0, whs.Length-1);

					value.Append(whs);

				} else {
					value.Append(' ');
				}

			} else
				value.Append((char) j);

		}

		Consume('}');

		return value;
	}

	private StringBuilder ParseBracketedTextExactly() {

		StringBuilder value = new StringBuilder();

		Consume('{');

		int brackets = 0;

		while (!((Peek() == '}') && (brackets == 0))) {

			int j = Read();
			if ((j == -1) || (j == 65535)) {
				throw new Exception("Error in line " + line + ": EOF in mid-string");
			} else if (j == '{')
				brackets++;
			else if (j == '}')
				brackets--;

			value.Append((char) j);

		}

		Consume('}');

		return value;
	}

	private StringBuilder ParseQuotedFieldExactly() {

		StringBuilder value = new StringBuilder();

		Consume('"');

		int brackets = 0;

		while (!((Peek() == '"') && (brackets == 0))) {

			int j = Read();
			if ((j == -1) || (j == 65535)) {
				throw new Exception("Error in line " + line + ": EOF in mid-string");
			} else if (j == '{')
				brackets++;
			else if (j == '}')
				brackets--;

			value.Append((char) j);

		}

		Consume('"');

		return value;
	}

	private void Consume(char expected) {
		int c = Read();

		if (c != expected) {
			throw new Exception("Error in line " + line + ": Expected " + expected
				+ " but received " + (char) c);
		}

	}

	private bool ConsumeUncritically(char expected) {
		int c;
		while (((c = Read()) != expected) && (c != -1) && (c != 65535)){
		    // do nothing
		}
			
		if ((c == -1) || (c == 65535))
			_eof = true;

		// Return true if we actually found the character we were looking for:
		return c == expected;
	}

	private void Consume(char expected1, char expected2) {
		// Consumes one of the two, doesn't care which appears.

		int c = Read();

		if ((c != expected1) && (c != expected2)) {
			throw new Exception("Error in line " + line + ": Expected " + expected1 + " or "
				+ expected2 + " but received " + c);

		}

	}

	public void CheckEntryTypes(ParserResult _pr) {
		
		foreach (BibtexEntry be in _db.getEntries()){
			if (be.getType() is UnknownEntryType) {
				// Look up the unknown type name in our map of parsed types:

				object o = entryTypes[(be.getType().getName().ToLower())];
				if (o != null) {
					BibtexEntryType type = (BibtexEntryType) o;
					be.setType(type);
				} else {
					// System.out.println("Unknown entry type:
					// "+be.getType().getName());
					_pr
						.AddWarning(Globals.lang("unknown entry type") + ": "
							+ be.getType().getName() + ". " + Globals.lang("Type set to 'other'")
							+ ".");
					be.setType(BibtexEntryType.OTHER.Instance);
				}
			}
		}
	}

    /**
     * Read the JabRef signature, if any, and find what version number is given.
     * This method advances the file reader only as far as the end of the first line of
     * the JabRef signature, or up until the point where the read characters don't match
     * the signature. This should ensure that the parser can continue from that spot without
     * resetting the reader, without the risk of losing important contents.
     *
     * @return The version number, or null if not found.
     * @throws IOException
     */
    private string ReadJabRefVersionNumber() {
        StringBuilder headerText = new StringBuilder();
        
        bool keepon = true;
        int piv = 0;
        int c;

        // We start by reading the standard part of the signature, which precedes
        // the version number:
        //                     This file was created with JabRef X.y.
        while (keepon) {
            c = Peek();
            headerText.Append((char) c);
            if ((piv == 0) && (char.IsWhiteSpace((char) c) || (c == '%')))
                Read();
            else if (c == Globals.SIGNATURE[piv]) {
                piv++;
                Read();
            }
            else {
                keepon = false;
                return null;
            }

            // Check if we've reached the end of the signature's standard part:
            if (piv == Globals.SIGNATURE.Length) {
                keepon = false;

                // Found the standard part. Now read the version number:
                StringBuilder sb = new StringBuilder();
                while (((c=Read()) != '\n') && (c != -1))
                    sb.Append((char)c);
                string versionNum = sb.ToString().Trim();
                // See if it fits the X.y. pattern:
                if (Regex.Match(versionNum, "[1-9]+\\.[1-9A-Za-z ]+\\.").Success) {
                    // It matched. Remove the last period and return:
                    return versionNum.Substring(0, versionNum.Length-1);
                }
                else if (Regex.Match(versionNum, "[1-9]+\\.[1-9]\\.[1-9A-Za-z ]+\\.").Success) {
                    // It matched. Remove the last period and return:
                    return versionNum.Substring(0, versionNum.Length-1);
                }

            }
        }

        return null;
    }

    /**
     * After a JabRef version number has been parsed and put into _pr,
     * parse the version number to determine the JabRef major and minor version
     * number
     */
    private void SetMajorMinorVersions() {
        string v = _pr.JabrefVersion;
        Regex p = new Regex("([0-9]+)\\.([0-9]+).*");
        Regex p2 = new Regex("([0-9]+)\\.([0-9]+)\\.([0-9]+).*");
        var m = p.Match(v);
        var m2 = p2.Match(v);
        if (m.Success)
            if (m.Groups.Count >= 2) {
                // TODO: zero-based indexing?
                _pr.JabrefMajorVersion = int.Parse(m.Groups[0].Value);
                _pr.JabrefMinorVersion = int.Parse(m.Groups[1].Value);
            }
        if (m2.Success)
            if (m2.Groups.Count >= 3) {
                _pr.JabrefMinor2Version = int.Parse(m2.Groups[2].Value);
            }
    }
}
}
