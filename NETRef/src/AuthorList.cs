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
namespace net.sf.jabref {
    
/**
 * @see tests.net.sf.jabref.AuthorListTest Testcases for this class.
 */
public class AuthorList {

	private List<Author> authors; 

	// Variables for storing computed strings, so they only need be created
	// once:
	private string authorsNatbib = null, authorsFirstFirstAnds = null,
		authorsAlph = null;

	private string[] authorsFirstFirst = new string[4], authorsLastOnly = new string[2],
	authorLastFirstAnds = new string[2], 
	authorsLastFirst = new string[4],
    authorsLastFirstFirstLast = new string[2];


	// The following variables are used only during parsing

	private string orig; // the raw bibtex author/editor field

	// the following variables are updated by getToken procedure
	private int token_start; // index in orig

	private int token_end; // to point 'abc' in ' abc xyz', start=2 and end=5

	// the following variables are valid only if getToken returns TOKEN_WORD
	private int token_abbr; // end of token abbreviation (always: token_start <

	// token_abbr <= token_end)

	private char token_term; // either space or dash

	private bool token_case; // true if upper-case token, false if lower-case

	// token

	// Tokens of one author name.
	// Each token occupies TGL consecutive entries in this vector (as described
	// below)
	private List<object> tokens;

	private static readonly int TOKEN_GROUP_LENGTH = 4; // number of entries for

	// a token

	// the following are offsets of an entry in a group of entries for one token
	private static readonly int OFFSET_TOKEN = 0; // string -- token itself;

	private static readonly int OFFSET_TOKEN_ABBR = 1; // string -- token

	// abbreviation;

	private static readonly int OFFSET_TOKEN_TERM = 2; // Character -- token

	// terminator (either " " or
	// "-")

	// private static readonly int OFFSET_TOKEN_CASE = 3; // Boolean --
	// true=uppercase, false=lowercase
	// the following are indices in 'tokens' vector created during parsing of
	// author name
	// and later used to properly split author name into parts
	int von_start, // first lower-case token (-1 if all tokens upper-case)
		last_start, // first upper-case token after first lower-case token (-1
		// if does not exist)
		comma_first, // token after first comma (-1 if no commas)
		comma_second; // token after second comma (-1 if no commas or only one

	// comma)

	// Token types (returned by getToken procedure)
	private const int TOKEN_EOF = 0;

	private const int TOKEN_AND = 1;

	private const int TOKEN_COMMA = 2;

	private const int TOKEN_WORD = 3;

	// Constant Hashtable containing names of TeX special characters
    private static readonly Dictionary<string, bool> tex_names = new Dictionary<string, bool>();
	// and static constructor to initialize it
	static AuthorList() {
		tex_names.Add("aa", true);
        tex_names.Add("ae", true);
        tex_names.Add("l", true);
        tex_names.Add("o", true);
        tex_names.Add("oe", true);
        tex_names.Add("i", true);
        tex_names.Add("AA", true);
        tex_names.Add("AE", true);
        tex_names.Add("L", true);
        tex_names.Add("O", true);
        tex_names.Add("OE", true);
        tex_names.Add("j", true);
	}

	static Dictionary<string, AuthorList> authorCache = new Dictionary<string, AuthorList>();

	/**
	 * Parses the parameter strings and stores preformatted author information.
	 * 
	 * Don't call this constructor directly but rather use the getAuthorList()
	 * method which caches its results.
	 * 
	 * @param bibtex_authors
	 *            contents of either <CODE>author</CODE> or <CODE>editor</CODE>
	 *            bibtex field.
	 */
	protected AuthorList(string bibtex_authors) {
		authors = new List<Author>(5); // 5 seems to be reasonable initial size
		orig = bibtex_authors; // initialization
		token_start = 0;
		token_end = 0; // of parser
		while (token_start < orig.Length) {
			Author author = getAuthor();
			if (author != null)
				authors.Add(author);
		}
		// clean-up
		orig = null;
		tokens = null;
	}

	/**
	 * Retrieve an AuthorList for the given string of authors or editors.
	 * 
	 * This function tries to cache AuthorLists by string passed in.
	 * 
	 * @param authors
	 *            The string of authors or editors in bibtex format to parse.
	 * @return An AuthorList object representing the given authors.
	 */
	public static AuthorList getAuthorList(string authors) {
		AuthorList authorList = authorCache[authors];
		if (authorList == null) {
			authorList = new AuthorList(authors);
			authorCache.Add(authors, authorList);
		}
		return authorList;
	}

	/**
	 * This is a convenience method for getAuthorsFirstFirst()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsFirstFirst
	 */
	public static string fixAuthor_firstNameFirstCommas(string authors, bool abbr,
		bool oxfordComma) {
		return getAuthorList(authors).getAuthorsFirstFirst(abbr, oxfordComma);
	}

	/**
	 * This is a convenience method for getAuthorsFirstFirstAnds()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsFirstFirstAnds
	 */
	public static string fixAuthor_firstNameFirst(string authors) {
		return getAuthorList(authors).getAuthorsFirstFirstAnds();
	}

	/**
	 * This is a convenience method for getAuthorsLastFirst()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsLastFirst
	 */
	public static string fixAuthor_lastNameFirstCommas(string authors, bool abbr,
		bool oxfordComma) {
		return getAuthorList(authors).getAuthorsLastFirst(abbr, oxfordComma);
	}

	/**
	 * This is a convenience method for getAuthorsLastFirstAnds(true)
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsLastFirstAnds
	 */
	public static string fixAuthor_lastNameFirst(string authors) {
		return getAuthorList(authors).getAuthorsLastFirstAnds(false);
	}
	
	/**
	 * This is a convenience method for getAuthorsLastFirstAnds()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsLastFirstAnds
	 */
	public static string fixAuthor_lastNameFirst(string authors, bool abbreviate) {
		return getAuthorList(authors).getAuthorsLastFirstAnds(abbreviate);
	}

	/**
	 * This is a convenience method for getAuthorsLastOnly()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsLastOnly
	 */
	public static string fixAuthor_lastNameOnlyCommas(string authors, bool oxfordComma) {
		return getAuthorList(authors).getAuthorsLastOnly(oxfordComma);
	}

	/**
	 * This is a convenience method for getAuthorsForAlphabetization()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsForAlphabetization
	 */
	public static string fixAuthorForAlphabetization(string authors) {
		return getAuthorList(authors).getAuthorsForAlphabetization();
	}

	/**
	 * This is a convenience method for getAuthorsNatbib()
	 * 
	 * @see net.sf.jabref.AuthorList#getAuthorsNatbib
	 */
	public static string fixAuthor_Natbib(string authors) {
		return AuthorList.getAuthorList(authors).getAuthorsNatbib();
	}

	/**
	 * Parses one author name and returns preformatted information.
	 * 
	 * @return Preformatted author name; <CODE>null</CODE> if author name is
	 *         empty.
	 */
	private Author getAuthor() {

		tokens = new List<object>(); // initialization
		von_start = -1;
		last_start = -1;
		comma_first = -1;
		comma_second = -1;

		// First step: collect tokens in 'tokens' Vector and calculate indices
		while (true) {
			int token = getToken();
			switch (token) {
			case TOKEN_EOF:
                goto case TOKEN_AND;
			case TOKEN_AND:
				goto afterloop;
			case TOKEN_COMMA:
				if (comma_first < 0)
					comma_first = tokens.Count;
				else if (comma_second < 0)
					comma_second = tokens.Count;
				break;
			case TOKEN_WORD:
				tokens.Add(orig.Substring(token_start, token_end));
				tokens.Add(orig.Substring(token_start, token_abbr));
				tokens.Add(token_term);
				tokens.Add(token_case);
				if (comma_first >= 0)
					break;
				if (last_start >= 0)
					break;
				if (von_start < 0) {
					if (!token_case) {
						von_start = tokens.Count - TOKEN_GROUP_LENGTH;
						break;
					}
				} else if (last_start < 0 && token_case) {
					last_start = tokens.Count - TOKEN_GROUP_LENGTH;
					break;
				}
                break;
			}
		}// end token_loop

        afterloop:
		// Second step: split name into parts (here: calculate indices
		// of parts in 'tokens' Vector)
		if (tokens.Count == 0)
			return null; // no author information


		// the following negatives indicate absence of the corresponding part
		int first_part_start = -1, von_part_start = -1, last_part_start = -1, jr_part_start = -1;
		int first_part_end = 0, von_part_end = 0, last_part_end = 0, jr_part_end = 0;
        bool jrAsFirstname = false;
		if (comma_first < 0) { // no commas
			if (von_start < 0) { // no 'von part'
				last_part_end = tokens.Count;
				last_part_start = tokens.Count - TOKEN_GROUP_LENGTH;
				int index = tokens.Count - 2 * TOKEN_GROUP_LENGTH + OFFSET_TOKEN_TERM;
				if (index > 0) {
					char ch = (char)tokens[index];
					if (ch == '-')
						last_part_start -= TOKEN_GROUP_LENGTH;
				}
				first_part_end = last_part_start;
				if (first_part_end > 0) {
					first_part_start = 0;
				}
			} else { // 'von part' is present
				if (last_start >= 0) {
					last_part_end = tokens.Count;
					last_part_start = last_start;
					von_part_end = last_part_start;
				} else {
					von_part_end = tokens.Count;
				}
				von_part_start = von_start;
				first_part_end = von_part_start;
				if (first_part_end > 0)
					first_part_start = 0;
			}
		} else { // commas are present: it affects only 'first part' and
			// 'junior part'
			first_part_end = tokens.Count;
			if (comma_second < 0) { // one comma
				if (comma_first < first_part_end) {
                    first_part_start = comma_first;
                    //if (((string)tokens.get(first_part_start)).ToLower().startsWith("jr."))
                    //    jrAsFirstname = true;
                }
			} else { // two or more commas
				if (comma_second < first_part_end)
					first_part_start = comma_second;
				jr_part_end = comma_second;
				if (comma_first < jr_part_end)
					jr_part_start = comma_first;
			}
			if (von_start != 0) { // no 'von part'
				last_part_end = comma_first;
				if (last_part_end > 0)
					last_part_start = 0;
			} else { // 'von part' is present
				if (last_start < 0) {
					von_part_end = comma_first;
				} else {
					last_part_end = comma_first;
					last_part_start = last_start;
					von_part_end = last_part_start;
				}
				von_part_start = 0;
			}
		}

        if ((first_part_start == -1) && (last_part_start == -1) && (von_part_start != -1)) {
            // There is no first or last name, but we have a von part. This is likely
            // to indicate a single-entry name without an initial capital letter, such
            // as "unknown".
            // We make the von part the last name, to facilitate handling by last-name formatters:
            last_part_start = von_part_start;
            last_part_end = von_part_end;
            von_part_start = -1;
            von_part_end = -1;
        }
        if (jrAsFirstname) {
            // This variable, if set, indicates that the first name starts with "jr.", which
            // is an indication that we may have a name formatted as "Firstname Lastname, Jr."
            // which is an acceptable format for BibTeX.
        }

		// Third step: do actual splitting, construct Author object
		return new Author((first_part_start < 0 ? null : concatTokens(first_part_start,
			first_part_end, OFFSET_TOKEN, false)), (first_part_start < 0 ? null : concatTokens(
			first_part_start, first_part_end, OFFSET_TOKEN_ABBR, true)), (von_part_start < 0 ? null
			: concatTokens(von_part_start, von_part_end, OFFSET_TOKEN, false)),
			(last_part_start < 0 ? null : concatTokens(last_part_start, last_part_end,
				OFFSET_TOKEN, false)), (jr_part_start < 0 ? null : concatTokens(jr_part_start,
				jr_part_end, OFFSET_TOKEN, false)));
	}

	/**
	 * Concatenates list of tokens from 'tokens' Vector. Tokens are separated by
	 * spaces or dashes, dependeing on stored in 'tokens'. Callers always ensure
	 * that start < end; thus, there exists at least one token to be
	 * concatenated.
	 * 
	 * @param start
	 *            index of the first token to be concatenated in 'tokens' Vector
	 *            (always divisible by TOKEN_GROUP_LENGTH).
	 * @param end
	 *            index of the first token not to be concatenated in 'tokens'
	 *            Vector (always divisible by TOKEN_GROUP_LENGTH).
	 * @param offset
	 *            offset within token group (used to request concatenation of
	 *            either full tokens or abbreviation).
	 * @param dot_after
	 *            <CODE>true</CODE> -- add period after each token, <CODE>false</CODE> --
	 *            do not add.
	 * @return the result of concatenation.
	 */
	private string concatTokens(int start, int end, int offset, bool dot_after) {
		StringBuilder res = new StringBuilder();
		// Here we always have start < end
		res.Append((string) tokens[start + offset]);
		if (dot_after)
			res.Append('.');
		start += TOKEN_GROUP_LENGTH;
		while (start < end) {
			res.Append(tokens[start - TOKEN_GROUP_LENGTH + OFFSET_TOKEN_TERM]);
			res.Append((string) tokens[start + offset]);
			if (dot_after)
				res.Append('.');
			start += TOKEN_GROUP_LENGTH;
		}
		return res.ToString();
	}

	/**
	 * Parses the next token.
	 * <p>
	 * The string being parsed is stored in global variable <CODE>orig</CODE>,
	 * and position which parsing has to start from is stored in global variable
	 * <CODE>token_end</CODE>; thus, <CODE>token_end</CODE> has to be set
	 * to 0 before the first invocation. Procedure updates <CODE>token_end</CODE>;
	 * thus, subsequent invocations do not require any additional variable
	 * settings.
	 * <p>
	 * The type of the token is returned; if it is <CODE>TOKEN_WORD</CODE>,
	 * additional information is given in global variables <CODE>token_start</CODE>,
	 * <CODE>token_end</CODE>, <CODE>token_abbr</CODE>, <CODE>token_term</CODE>,
	 * and <CODE>token_case</CODE>; namely: <CODE>orig.Substring(token_start,token_end)</CODE>
	 * is the thext of the token, <CODE>orig.Substring(token_start,token_abbr)</CODE>
	 * is the token abbreviation, <CODE>token_term</CODE> Contains token
	 * terminator (space or dash), and <CODE>token_case</CODE> is <CODE>true</CODE>,
	 * if token is upper-case and <CODE>false</CODE> if token is lower-case.
	 * 
	 * @return <CODE>TOKEN_EOF</CODE> -- no more tokens, <CODE>TOKEN_COMMA</CODE> --
	 *         token is comma, <CODE>TOKEN_AND</CODE> -- token is the word
	 *         "and" (or "And", or "aND", etc.), <CODE>TOKEN_WORD</CODE> --
	 *         token is a word; additional information is given in global
	 *         variables <CODE>token_start</CODE>, <CODE>token_end</CODE>,
	 *         <CODE>token_abbr</CODE>, <CODE>token_term</CODE>, and
	 *         <CODE>token_case</CODE>.
	 */
	private int getToken() {
		token_start = token_end;
		while (token_start < orig.Length) {
			char c = orig[token_start];
			if (!(c == '~' || c == '-' || char.IsWhiteSpace(c)))
				break;
			token_start++;
		}
		token_end = token_start;
		if (token_start >= orig.Length)
			return TOKEN_EOF;
		if (orig[token_start] == ',') {
			token_end++;
			return TOKEN_COMMA;
		}
		token_abbr = -1;
		token_term = ' ';
		token_case = true;
		int braces_level = 0;
		int current_backslash = -1;
		bool first_letter_is_found = false;
		while (token_end < orig.Length) {
			char c = orig[token_end];
			if (c == '{') {
				braces_level++;
			}
			if (braces_level > 0)
				if (c == '}')
					braces_level--;
			if (first_letter_is_found && token_abbr < 0 && braces_level == 0)
				token_abbr = token_end;
			if (!first_letter_is_found && current_backslash < 0 && char.IsLetter(c)) {
				token_case = char.IsUpper(c);
				first_letter_is_found = true;
			}
			if (current_backslash >= 0 && !char.IsLetter(c)) {
				if (!first_letter_is_found) {
					string tex_cmd_name = orig.Substring(current_backslash + 1, token_end);
					if (tex_names.ContainsKey(tex_cmd_name)) {
						token_case = char.IsUpper(tex_cmd_name[0]);
						first_letter_is_found = true;
					}
				}
				current_backslash = -1;
			}
			if (c == '\\')
				current_backslash = token_end;
			if (braces_level == 0)
				if (c == ',' || c == '~' || c=='-' || char.IsWhiteSpace(c))
					break;
			// Morten Alver 18 Apr 2006: Removed check for hyphen '-' above to
			// prevent
			// problems with names like Bailey-Jones getting broken up and
			// sorted wrong.
			// Aaron Chen 14 Sep 2008: Enable hyphen check for first names like Chang-Chin
			token_end++;
		}
		if (token_abbr < 0)
			token_abbr = token_end;
		if (token_end < orig.Length && orig[token_end] == '-')
			token_term = '-';
		if (orig.Substring(token_start, token_end).Equals("and", System.StringComparison.CurrentCultureIgnoreCase))
			return TOKEN_AND;
		else
			return TOKEN_WORD;
	}

	/**
	 * Returns the number of author names in this object.
	 * 
	 * @return the number of author names in this object.
	 */
	public int size() {
		return authors.Count;
	}

	/**
	 * Returns the <CODE>Author</CODE> object for the i-th author.
	 * 
	 * @param i
	 *            Index of the author (from 0 to <CODE>size()-1</CODE>).
	 * @return the <CODE>Author</CODE> object.
	 */
	public Author getAuthor(int i) {
		return authors[i];
	}

	/**
	 * Returns the list of authors in "natbib" format.
	 * <p>
	 * <ul>
	 * <li>"John Smith" -> "Smith"</li>
	 * <li>"John Smith and Black Brown, Peter" ==> "Smith and Black Brown"</li>
	 * <li>"John von Neumann and John Smith and Black Brown, Peter" ==> "von
	 * Neumann et al." </li>
	 * </ul>
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsNatbib() {
		// Check if we've computed this before:
		if (authorsNatbib != null)
			return authorsNatbib;

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getLastOnly());
			if (size() == 2) {
				res.Append(" and ");
				res.Append(getAuthor(1).getLastOnly());
			} else if (size() > 2) {
				res.Append(" et al.");
			}
		}
		authorsNatbib = res.ToString();
		return authorsNatbib;
	}

	/**
	 * Returns the list of authors separated by commas with last name only; If
	 * the list consists of three or more authors, "and" is inserted before the
	 * last author's name.
	 * <p>
	 * 
	 * <ul>
	 * <li> "John Smith" ==> "Smith"</li>
	 * <li> "John Smith and Black Brown, Peter" ==> "Smith and Black Brown"</li>
	 * <li> "John von Neumann and John Smith and Black Brown, Peter" ==> "von
	 * Neumann, Smith and Black Brown".</li>
	 * </ul>
	 * 
	 * @param oxfordComma
	 *            Whether to put a comma before the and at the end.
	 * 
	 * @see http://en.wikipedia.org/wiki/Serial_comma For an detailed
	 *      explaination about the Oxford comma.
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsLastOnly(bool oxfordComma) {
		int abbrInt = (oxfordComma ? 0 : 1);

		// Check if we've computed this before:
		if (authorsLastOnly[abbrInt] != null)
			return authorsLastOnly[abbrInt];

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getLastOnly());
			int i = 1;
			while (i < size() - 1) {
				res.Append(", ");
				res.Append(getAuthor(i).getLastOnly());
				i++;
			}
			if (size() > 2 && oxfordComma)
				res.Append(",");
			if (size() > 1) {
				res.Append(" and ");
				res.Append(getAuthor(i).getLastOnly());
			}
		}
		authorsLastOnly[abbrInt] = res.ToString();
		return authorsLastOnly[abbrInt];
	}

	/**
	 * Returns the list of authors separated by commas with first names after
	 * last name; first names are abbreviated or not depending on parameter. If
	 * the list consists of three or more authors, "and" is inserted before the
	 * last author's name.
	 * <p>
	 * 
	 * <ul>
	 * <li> "John Smith" ==> "Smith, John" or "Smith, J."</li>
	 * <li> "John Smith and Black Brown, Peter" ==> "Smith, John and Black
	 * Brown, Peter" or "Smith, J. and Black Brown, P."</li>
	 * <li> "John von Neumann and John Smith and Black Brown, Peter" ==> "von
	 * Neumann, John, Smith, John and Black Brown, Peter" or "von Neumann, J.,
	 * Smith, J. and Black Brown, P.".</li>
	 * </ul>
	 * 
	 * @param abbreviate
	 *            whether to abbreivate first names.
	 * 
	 * @param oxfordComma
	 *            Whether to put a comma before the and at the end.
	 * 
	 * @see http://en.wikipedia.org/wiki/Serial_comma For an detailed
	 *      explaination about the Oxford comma.
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsLastFirst(bool abbreviate, bool oxfordComma) {
		int abbrInt = (abbreviate ? 0 : 1);
		abbrInt += (oxfordComma ? 0 : 2);

		// Check if we've computed this before:
		if (authorsLastFirst[abbrInt] != null)
			return authorsLastFirst[abbrInt];

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getLastFirst(abbreviate));
			int i = 1;
			while (i < size() - 1) {
				res.Append(", ");
				res.Append(getAuthor(i).getLastFirst(abbreviate));
				i++;
			}
			if (size() > 2 && oxfordComma)
				res.Append(",");
			if (size() > 1) {
				res.Append(" and ");
				res.Append(getAuthor(i).getLastFirst(abbreviate));
			}
		}
		authorsLastFirst[abbrInt] = res.ToString();
		return authorsLastFirst[abbrInt];
	}
	
	public override string ToString(){
		return getAuthorsLastFirstAnds(false);
	}

	/**
	 * Returns the list of authors separated by "and"s with first names after
	 * last name; first names are not abbreviated.
	 * <p>
	 * <ul>
	 * <li>"John Smith" ==> "Smith, John"</li>
	 * <li>"John Smith and Black Brown, Peter" ==> "Smith, John and Black
	 * Brown, Peter"</li>
	 * <li>"John von Neumann and John Smith and Black Brown, Peter" ==> "von
	 * Neumann, John and Smith, John and Black Brown, Peter".</li>
	 * </ul>
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsLastFirstAnds(bool abbreviate) {
		int abbrInt = (abbreviate ? 0 : 1);
		// Check if we've computed this before:
		if (authorLastFirstAnds[abbrInt] != null)
			return authorLastFirstAnds[abbrInt];

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getLastFirst(abbreviate));
			for (int i = 1; i < size(); i++) {
				res.Append(" and ");
				res.Append(getAuthor(i).getLastFirst(abbreviate));
			}
		}

		authorLastFirstAnds[abbrInt] = res.ToString();
		return authorLastFirstAnds[abbrInt];
	}

	public string getAuthorsLastFirstFirstLastAnds(bool abbreviate) {
		int abbrInt = (abbreviate ? 0 : 1);
		// Check if we've computed this before:
		if (authorsLastFirstFirstLast[abbrInt] != null)
			return authorsLastFirstFirstLast[abbrInt];

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
            res.Append(getAuthor(0).getLastFirst(abbreviate));
			for (int i = 1; i < size(); i++) {
				res.Append(" and ");
				res.Append(getAuthor(i).getFirstLast(abbreviate));
			}
		}

		authorsLastFirstFirstLast[abbrInt] = res.ToString();
		return authorsLastFirstFirstLast[abbrInt];
	}    

	/**
	 * Returns the list of authors separated by commas with first names before
	 * last name; first names are abbreviated or not depending on parameter. If
	 * the list consists of three or more authors, "and" is inserted before the
	 * last author's name.
	 * <p>
	 * <ul>
	 * <li>"John Smith" ==> "John Smith" or "J. Smith"</li>
	 * <li>"John Smith and Black Brown, Peter" ==> "John Smith and Peter Black
	 * Brown" or "J. Smith and P. Black Brown"</li>
	 * <li> "John von Neumann and John Smith and Black Brown, Peter" ==> "John
	 * von Neumann, John Smith and Peter Black Brown" or "J. von Neumann, J.
	 * Smith and P. Black Brown" </li>
	 * </ul>
	 * 
	 * @param abbr
	 *            whether to abbreivate first names.
	 * 
	 * @param oxfordComma
	 *            Whether to put a comma before the and at the end.
	 * 
	 * @see http://en.wikipedia.org/wiki/Serial_comma For an detailed
	 *      explaination about the Oxford comma.
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsFirstFirst(bool abbr, bool oxfordComma) {

		int abbrInt = (abbr ? 0 : 1);
		abbrInt += (oxfordComma ? 0 : 2);

		// Check if we've computed this before:
		if (authorsFirstFirst[abbrInt] != null)
			return authorsFirstFirst[abbrInt];

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getFirstLast(abbr));
			int i = 1;
			while (i < size() - 1) {
				res.Append(", ");
				res.Append(getAuthor(i).getFirstLast(abbr));
				i++;
			}
			if (size() > 2 && oxfordComma)
				res.Append(",");
			if (size() > 1) {
				res.Append(" and ");
				res.Append(getAuthor(i).getFirstLast(abbr));
			}
		}
		authorsFirstFirst[abbrInt] = res.ToString();
		return authorsFirstFirst[abbrInt];
	}
	
	/**
	 * Compare this object with the given one. 
	 * 
	 * Will return true iff the other object is an Author and all fields are identical on a string comparison.
	 */
	public bool equals(object o) {
		if (!(o is AuthorList)) {
			return false;
		}
		AuthorList a = (AuthorList) o;
		
		return this.authors.Equals(a.authors);
	}
	
	/**
	 * Returns the list of authors separated by "and"s with first names before
	 * last name; first names are not abbreviated.
	 * <p>
	 * <ul>
	 * <li>"John Smith" ==> "John Smith"</li>
	 * <li>"John Smith and Black Brown, Peter" ==> "John Smith and Peter Black
	 * Brown"</li>
	 * <li>"John von Neumann and John Smith and Black Brown, Peter" ==> "John
	 * von Neumann and John Smith and Peter Black Brown" </li>
	 * </li>
	 * 
	 * @return formatted list of authors.
	 */
	public string getAuthorsFirstFirstAnds() {
		// Check if we've computed this before:
		if (authorsFirstFirstAnds != null)
			return authorsFirstFirstAnds;

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getFirstLast(false));
			for (int i = 1; i < size(); i++) {
				res.Append(" and ");
				res.Append(getAuthor(i).getFirstLast(false));
			}
		}
		authorsFirstFirstAnds = res.ToString();
		return authorsFirstFirstAnds;
	}

	/**
	 * Returns the list of authors in a form suitable for alphabetization. This
	 * means that last names come first, never preceded by "von" particles, and
	 * that any braces are removed. First names are abbreviated so the same name
	 * is treated similarly if abbreviated in one case and not in another. This
	 * form is not intended to be suitable for presentation, only for sorting.
	 * 
	 * <p>
	 * <ul>
	 * <li>"John Smith" ==> "Smith, J.";</li>
	 * 
	 * 
	 * @return formatted list of authors
	 */
	public string getAuthorsForAlphabetization() {
		if (authorsAlph != null)
			return authorsAlph;

		StringBuilder res = new StringBuilder();
		if (size() > 0) {
			res.Append(getAuthor(0).getNameForAlphabetization());
			for (int i = 1; i < size(); i++) {
				res.Append(" and ");
				res.Append(getAuthor(i).getNameForAlphabetization());
			}
		}
		authorsAlph = res.ToString();
		return authorsAlph;
	}

	/**
	 * This is an immutable class that keeps information regarding single
	 * author. It is just a container for the information, with very simple
	 * methods to access it.
	 * <p>
	 * Current usage: only methods <code>getLastOnly</code>,
	 * <code>getFirstLast</code>, and <code>getLastFirst</code> are used;
	 * all other methods are provided for completeness.
	 */
	public class Author {
		
		private readonly string first_part;

		private readonly string first_abbr;

		private readonly string von_part;

		private readonly string last_part;

		private readonly string jr_part;

		/**
		 * Compare this object with the given one. 
		 * 
		 * Will return true iff the other object is an Author and all fields are identical on a string comparison.
		 */
		public bool equals(object o) {
			if (!(o is Author)) {
				return false;
			}
			Author a = (Author) o;
			return Util.Equals(first_part, a.first_part)
					&& Util.Equals(first_abbr, a.first_abbr)
					&& Util.Equals(von_part, a.von_part)
					&& Util.Equals(last_part, a.last_part)
					&& Util.Equals(jr_part, a.jr_part);
		}
		
		/**
		 * Creates the Author object. If any part of the name is absent, <CODE>null</CODE>
		 * must be passes; otherwise other methods may return erroneous results.
		 * 
		 * @param first
		 *            the first name of the author (may consist of several
		 *            tokens, like "Charles Louis Xavier Joseph" in "Charles
		 *            Louis Xavier Joseph de la Vall{\'e}e Poussin")
		 * @param firstabbr
		 *            the abbreviated first name of the author (may consist of
		 *            several tokens, like "C. L. X. J." in "Charles Louis
		 *            Xavier Joseph de la Vall{\'e}e Poussin"). It is a
		 *            responsibility of the caller to create a reasonable
		 *            abbreviation of the first name.
		 * @param von
		 *            the von part of the author's name (may consist of several
		 *            tokens, like "de la" in "Charles Louis Xavier Joseph de la
		 *            Vall{\'e}e Poussin")
		 * @param last
		 *            the lats name of the author (may consist of several
		 *            tokens, like "Vall{\'e}e Poussin" in "Charles Louis Xavier
		 *            Joseph de la Vall{\'e}e Poussin")
		 * @param jr
		 *            the junior part of the author's name (may consist of
		 *            several tokens, like "Jr. III" in "Smith, Jr. III, John")
		 */
		public Author(string first, string firstabbr, string von, string last, string jr) {
			first_part = first;
			first_abbr = firstabbr;
			von_part = von;
			last_part = last;
			jr_part = jr;
		}

		/**
		 * Returns the first name of the author stored in this object ("First").
		 * 
		 * @return first name of the author (may consist of several tokens)
		 */
		public string getFirst() {
			return first_part;
		}

		/**
		 * Returns the abbreviated first name of the author stored in this
		 * object ("F.").
		 * 
		 * @return abbreviated first name of the author (may consist of several
		 *         tokens)
		 */
		public string getFirstAbbr() {
			return first_abbr;
		}

		/**
		 * Returns the von part of the author's name stored in this object
		 * ("von").
		 * 
		 * @return von part of the author's name (may consist of several tokens)
		 */
		public string getVon() {
			return von_part;
		}

		/**
		 * Returns the last name of the author stored in this object ("Last").
		 * 
		 * @return last name of the author (may consist of several tokens)
		 */
		public string getLast() {
            return last_part;
		}

		/**
		 * Returns the junior part of the author's name stored in this object
		 * ("Jr").
		 * 
		 * @return junior part of the author's name (may consist of several
		 *         tokens) or null if the author does not have a Jr. Part
		 */
		public string getJr() {
			return jr_part;
		}

		/**
		 * Returns von-part followed by last name ("von Last"). If both fields
		 * were specified as <CODE>null</CODE>, the empty string <CODE>""</CODE>
		 * is returned.
		 * 
		 * @return 'von Last'
		 */
		public string getLastOnly() {
			if (von_part == null) {
				return (last_part == null ? "" : last_part);
			} else {
				return (last_part == null ? von_part : von_part + " " + last_part);
			}
		}

		/**
		 * Returns the author's name in form 'von Last, Jr., First' with the
		 * first name full or abbreviated depending on parameter.
		 * 
		 * @param abbr
		 *            <CODE>true</CODE> - abbreviate first name, <CODE>false</CODE> -
		 *            do not abbreviate
		 * @return 'von Last, Jr., First' (if <CODE>abbr==false</CODE>) or
		 *         'von Last, Jr., F.' (if <CODE>abbr==true</CODE>)
		 */
		public string getLastFirst(bool abbr) {
			string res = getLastOnly();
			if (jr_part != null)
				res += ", " + jr_part;
			if (abbr) {
				if (first_abbr != null)
					res += ", " + first_abbr;
			} else {
				if (first_part != null)
					res += ", " + first_part;
			}
			return res;
		}

		/**
		 * Returns the author's name in form 'First von Last, Jr.' with the
		 * first name full or abbreviated depending on parameter.
		 * 
		 * @param abbr
		 *            <CODE>true</CODE> - abbreviate first name, <CODE>false</CODE> -
		 *            do not abbreviate
		 * @return 'First von Last, Jr.' (if <CODE>abbr==false</CODE>) or 'F.
		 *         von Last, Jr.' (if <CODE>abbr==true</CODE>)
		 */
		public string getFirstLast(bool abbr) {
			string res = getLastOnly();
			if (abbr) {
				res = (first_abbr == null ? "" : first_abbr + " ") + res;
			} else {
				res = (first_part == null ? "" : first_part + " ") + res;
			}
			if (jr_part != null)
				res += ", " + jr_part;
			return res;
		}

		/**
		 * Returns the name as "Last, Jr, F." omitting the von-part and removing
		 * starting braces.
		 * 
		 * @return "Last, Jr, F." as described above or "" if all these parts
		 *         are empty.
		 */
		public string getNameForAlphabetization() {
			StringBuilder res = new StringBuilder();
			if (last_part != null)
				res.Append(last_part);
			if (jr_part != null) {
				res.Append(", ");
				res.Append(jr_part);
			}
			if (first_abbr != null) {
				res.Append(", ");
				res.Append(first_abbr);
			}
			while ((res.Length > 0) && (res[0] == '{'))
				res.Remove(0, 1);
			return res.ToString();
		}
	}// end Author
}// end AuthorList
}
