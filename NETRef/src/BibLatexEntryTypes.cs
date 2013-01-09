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
using BET = net.sf.jabref.BibLatexEntryTypes;

namespace net.sf.jabref.BibLatexEntryTypes {

/**
 * This class defines entry types for BibLatex support.
 */
    /*
        "rare" fields?
            "annotator", "commentator", "titleaddon", "editora", "editorb", "editorc",
            "issuetitle", "issuesubtitle", "origlanguage", "version", "addendum"

     */

    public class ARTICLE : BibtexEntryType {
        private static BET.ARTICLE _instance = new BET.ARTICLE();
        private ARTICLE() { }
        public static BET.ARTICLE Instance { get { return _instance; } }

        public override string getName() {
            return "Article";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "journaltitle", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"translator", "annotator", "commentator", "subtitle", "titleaddon",
				 "editor", "editora", "editorb", "editorc", "journalsubtitle", "issuetitle",
				 "issuesubtitle", "language", "origlanguage", "series", "volume", "number",
				 "eid", "issue", "date", "month", "year", "pages", "version", "note", "issn",
				 "addendum", "pubstate", "doi", "eprint", "eprintclass", "eprinttype", "url",
				 "urldate"};
        }

        // TODO: number vs issue?
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "editor", "series", "volume", "number",
				 "eid", "issue", "date", "month", "year", "pages", "note", "issn",
				 "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    }


    public class BOOK : BibtexEntryType {
        private static BET.BOOK _instance = new BET.BOOK();
        private BOOK() { }
        public static BET.BOOK Instance { get { return _instance; } }

        public override string getName() {
            return "Book";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"editor", "editora", "editorb", "editorc", "translator",
				 "annotator", "commentator", "introduction",
				 "foreword", "afterword", "subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "language", "origlanguage", "volume", "part",
				 "edition", "volumes", "series", "number", "note", "publisher",
				 "location", "isbn", "chapter", "pages", "pagetotal", "addendum", "pubstate",
				 "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }

        public override string[] getPrimaryOptionalFields() {
            return new string[] {"editor", "subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "volume", "edition", "publisher", "isbn", "chapter", "pages",
				 "pagetotal", "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    }


    public class INBOOK : BibtexEntryType {
        private static BET.INBOOK _instance = new BET.INBOOK();
        private INBOOK() { }
        public static BET.INBOOK Instance { get { return _instance; } }

        public override string getName() {
            return "Inbook";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "booktitle", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"bookauthor", "editor", "editora", "editorb", "editorc",
				 "translator", "annotator", "commentator", "introduction", "foreword", "afterword",
				 "subtitle", "titleaddon", "maintitle", "mainsubtitle", "maintitleaddon",
				 "booksubtitle", "booktitleaddon", "language", "origlanguage", "volume", "part",
				 "edition", "volumes", "series", "number", "note", "publisher", "location", "isbn",
				 "chapter", "pages", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }

        public override string[] getPrimaryOptionalFields() {
            return new string[] {"bookauthor", "editor", "subtitle", "titleaddon", "maintitle",
				 "mainsubtitle", "maintitleaddon", "booksubtitle", "booktitleaddon", "volume",
				 "edition", "publisher", "isbn", "chapter", "pages", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class BOOKINBOOK : BibtexEntryType {
        private static BET.BOOKINBOOK _instance = new BET.BOOKINBOOK();
        private BOOKINBOOK() { }
        public static BET.BOOKINBOOK Instance { get { return _instance; } }

        public override string getName() {
            return "Bookinbook";
        }
	// Same fields as "INBOOK" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
	    return BibLatexEntryTypes.INBOOK.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
	    return BibLatexEntryTypes.INBOOK.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
	    return BibLatexEntryTypes.INBOOK.Instance.getPrimaryOptionalFields();
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class SUPPBOOK : BibtexEntryType {
        private static BET.SUPPBOOK _instance = new BET.SUPPBOOK();
        private SUPPBOOK() { }
        public static BET.SUPPBOOK Instance { get { return _instance; } }

        public override string getName() {
            return "Suppbook";
        }
	// Same fields as "INBOOK" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
            return BibLatexEntryTypes.INBOOK.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.INBOOK.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.INBOOK.Instance.getPrimaryOptionalFields();
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class BOOKLET : BibtexEntryType {
        private static BET.BOOKLET _instance = new BET.BOOKLET();
        private BOOKLET() { }
        public static BET.BOOKLET Instance { get { return _instance; } }

        public override string getName() {
            return "Booklet";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "editor", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "howpublished", "type", "note",
				 "location", "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }

        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "howpublished", "chapter", "pages", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }

        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class COLLECTION : BibtexEntryType {
        private static COLLECTION _instance = new COLLECTION();
        private COLLECTION() { }
        public static COLLECTION Instance { get { return _instance; } }

	public override string getName() {
	    return "Collection";
	}
	public override string[] getRequiredFields() {
	    return new string[] {"editor", "title", "year", "date"};
	}
	public override string[] getOptionalFields() {
	    return new string[] {"editora", "editorb", "editorc", "translator", "annotator", 
				 "commentator", "introduction", "foreword", "afterword", "subtitle", "titleaddon",
				 "maintitle", "mainsubtitle", "maintitleaddon", "language", "origlanguage", "volume",
				 "part", "edition", "volumes", "series", "number", "note", "publisher", "location", "isbn",
				 "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
	}
     
	public override string[] getPrimaryOptionalFields() {
	    return new string[] {"translator", "subtitle", "titleaddon", "maintitle",
				 "mainsubtitle", "maintitleaddon", "volume",
				 "edition", "publisher", "isbn", "chapter", "pages", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
	}
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class INCOLLECTION : BibtexEntryType {
        private static BET.INCOLLECTION _instance = new BET.INCOLLECTION();
        private INCOLLECTION() { }
        public static BET.INCOLLECTION Instance { get { return _instance; } }
	public override string getName() {
	    return "Incollection";
	}
	public override string[] getRequiredFields() {
	    return new string[] {"author", "editor", "title", "booktitle", "year", "date"};
	}
	public override string[] getOptionalFields() {
	    return new string[] {"editora", "editorb", "editorc", "translator", "annotator",
				 "commentator", "introduction", "foreword", "afterword", "subtitle", "titleaddon",
				 "maintitle", "mainsubtitle", "maintitleaddon", "booksubtitle", "booktitleaddon",
				 "language", "origlanguage", "volume", "part", "edition", "volumes", "series", "number",
				 "note", "publisher", "location", "isbn", "chapter", "pages", "addendum", "pubstate", "doi",
				 "eprint", "eprintclass", "eprinttype", "url", "urldate"};
	}
     
	public override string[] getPrimaryOptionalFields() {
	    return new string[] {"translator", "subtitle", "titleaddon", "maintitle",
				 "mainsubtitle", "maintitleaddon", "booksubtitle", "booktitleaddon", "volume",
				 "edition", "publisher", "isbn", "chapter", "pages", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
	}
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class SUPPCOLLECTION : BibtexEntryType {
        private static SUPPCOLLECTION _instance = new SUPPCOLLECTION();
        private SUPPCOLLECTION() { }
        public static SUPPCOLLECTION Instance { get { return _instance; } }

	public override string getName() {
	    return "Suppcollection";
	}
	// Treated as alias of "INCOLLECTION" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
	    return BibLatexEntryTypes.INCOLLECTION.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.INCOLLECTION.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
	    return BibLatexEntryTypes.INCOLLECTION.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class MANUAL : BibtexEntryType {
        private static BET.MANUAL _instance = new BET.MANUAL();
        private MANUAL() { }
        public static BET.MANUAL Instance { get { return _instance; } }

        public override string getName() {
            return "Manual";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "editor", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "edition", "type", "series",
				 "number", "version", "note", "organization", "publisher", "location", "isbn", "chapter",
				 "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "edition", "publisher", "isbn", "chapter",
				 "pages", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class MISC : BibtexEntryType {
        private static BET.MISC _instance = new BET.MISC();
        private MISC() { }
        public static BET.MISC Instance { get { return _instance; } }

        public override string getName() {
            return "Misc";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "editor", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "howpublished", "type",
				 "version", "note", "organization", "location", "date", "month", "year", "addendum",
				 "pubstate", "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "howpublished", "location", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class ONLINE : BibtexEntryType {
        private static ONLINE _instance = new ONLINE();
        private ONLINE() { }
        public static ONLINE Instance { get { return _instance; } }

        public override string getName() {
            return "Online";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "editor", "title", "year", "date", "url"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "version", "note",
				 "organization", "date", "month", "year", "addendum", "pubstate", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "note", "organization", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class PATENT : BibtexEntryType {
        private static BET.PATENT _instance = new BET.PATENT();
        private PATENT() { }
        public static BET.PATENT Instance { get { return _instance; } }

        public override string getName() {
            return "Patent";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "number", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"holder", "subtitle", "titleaddon", "type", "version", "location", "note",
				 "date", "month", "year", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"holder", "subtitle", "titleaddon", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class PERIODICAL : BibtexEntryType {
        private static BET.PERIODICAL _instance = new BET.PERIODICAL();
        private PERIODICAL() { }
        public static BET.PERIODICAL Instance { get { return _instance; } }

        public override string getName() {
            return "Periodical";
        }
        public override string[] getRequiredFields() {
            return new string[] {"editor", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"editora", "editorb", "editorc", "subtitle", "issuetitle",
				 "issuesubtitle", "language", "series", "volume", "number", "issue", "date", "month", "year",
				 "note", "issn", "addendum", "pubstate", "doi", "eprint", "eprintclass", "eprinttype", "url",
				 "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "issuetitle", "issuesubtitle", "issn", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class SUPPPERIODICAL : BibtexEntryType {
        private static SUPPPERIODICAL _instance = new SUPPPERIODICAL();
        private SUPPPERIODICAL() { }
        public static SUPPPERIODICAL Instance { get { return _instance; } }

	public override string getName() {
	    return "Suppperiodical";
	}
	// Treated as alias of "ARTICLE" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
            return BibLatexEntryTypes.ARTICLE.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.ARTICLE.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.ARTICLE.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class PROCEEDINGS : BibtexEntryType {
        private static BET.PROCEEDINGS _instance = new BET.PROCEEDINGS();
        private PROCEEDINGS() { }
        public static BET.PROCEEDINGS Instance { get { return _instance; } }

        public override string getName() {
            return "Proceedings";
        }
        public override string[] getRequiredFields() {
            return new string[] {"editor", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "eventtitle", "eventdate", "venue", "language", "volume", "part",
				 "volumes", "series", "number", "note", "organization", "publisher", "location", "month",
				 "isbn", "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "eventtitle", "volume", "publisher", "isbn", "chapter", "pages",
				 "pagetotal", "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class INPROCEEDINGS : BibtexEntryType {
        private static BET.INPROCEEDINGS _instance = new BET.INPROCEEDINGS();
        private INPROCEEDINGS() { }
        public static BET.INPROCEEDINGS Instance { get { return _instance; } }

        public override string getName() {
            return "Inproceedings";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "editor", "title", "booktitle", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "booksubtitle", "booktitleaddon", "eventtitle", "eventdate", "venue",
				 "language", "volume", "part", "volumes", "series", "number", "note", "organization",
				 "publisher", "location", "month", "isbn", "chapter", "pages", "addendum",
				 "pubstate", "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "maintitle", "mainsubtitle",
				 "maintitleaddon", "booksubtitle", "booktitleaddon", "eventtitle", "volume",
				 "publisher", "isbn", "chapter", "pages",
				 "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class REFERENCE : BibtexEntryType {
        private static REFERENCE _instance = new REFERENCE();
        private REFERENCE() { }
        public static REFERENCE Instance { get { return _instance; } }

	public override string getName() {
	    return "Reference";
	}
	// Treated as alias of "COLLECTION" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
            return BibLatexEntryTypes.COLLECTION.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
	    return BibLatexEntryTypes.COLLECTION.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.COLLECTION.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class INREFERENCE : BibtexEntryType {
        private static INREFERENCE _instance = new INREFERENCE();
        private INREFERENCE() { }
        public static INREFERENCE Instance { get { return _instance; } }

	public override string getName() {
	    return "Inreference";
	}
	// Treated as alias of "INCOLLECTION" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
	    return BibLatexEntryTypes.INCOLLECTION.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.INCOLLECTION.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.INCOLLECTION.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class REPORT : BibtexEntryType {
        private static REPORT _instance = new REPORT();
        private REPORT() { }
        public static REPORT Instance { get { return _instance; } }

        public override string getName() {
            return "Report";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "type", "institution", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "number", "version", "note",
				 "location", "month", "isrn", "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi",
				 "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "number", "isrn", "chapter", "pages", "pagetotal", "doi",
				 "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class SET : BibtexEntryType {
        private static SET _instance = new SET();
        private SET() { }
        public static SET Instance { get { return _instance; } }

        public override string getName() {
            return "Set";
        }
        public override string[] getRequiredFields() {
            return new string[] {"entryset", "crossref"};
        }
	// These are all the standard entry fields, custom fields and field aliases not included:
	/* Optional fields left out since they take up too much space - I think the set type is mainly supposed
	   to fall back on content from the entries contained in the set, so only the required fields are included.*/
        public override string[] getOptionalFields() {
	    return null;
            /*return new string[] {"abstract", "addendum", "afterword", "annotation", "annotator", "author", "authortype",
				 "bookauthor", "bookpagination", "booksubtitle", "booktitle", "booktitleaddon",
				 "chapter", "commentator", "date", "doi", "edition", "editor", "editora", "editorb",
				 "editorc", "editortype", "editoratype", "editorbtype", "editorctype", "eid", "eprint",
				 "eprintclass", "eprinttype", "eventdate", "eventtitle", "file", "foreword", "holder",
				 "howpublished", "indextitle", "insitution", "introduction", "isan", "isbn", "ismn",
				 "isrn", "issn", "issue", "issuesubtitle", "issuetitle", "iswc", "journalsubtitle",
				 "journaltitle", "label", "language", "library", "location", "mainsubtitle",
				 "maintitle", "maintitleaddon", "month", "nameaddon", "note", "number", "organization",
				 "origdate", "origlanguage", "origlocation", "origpublisher", "origtitle", "pages",
				 "pagetotal", "pagination", "part", "publisher", "pubstate", "reprinttitle", "series",
				 "shortauthor", "shorteditor", "shorthand", "shorthandintro", "shortjournal",
				 "shortseries", "shorttitle", "subtitle", "title", "titleaddon", "translator", "type",
				 "url", "urldate", "venue", "version", "volume", "volumes", "year", "crossref",
				 "entryset", "entrysubtype", "execute", "gender", "hyphenation", "indexsorttitle",
				 "keywords", "options", "presort", "sortkey", "sortname", "sorttitle", "sortyear",
				 "xref"};*/
        }
	// These are just appr. the first half of the above fields:
        public override string[] getPrimaryOptionalFields() {
	    return null;
            /*return new string[] {"abstract", "addendum", "afterword", "annotation", "annotator", "author", "authortype",
				 "bookauthor", "bookpagination", "booksubtitle", "booktitle", "booktitleaddon",
				 "chapter", "commentator", "date", "doi", "edition", "editor", "editora", "editorb",
				 "editorc", "editortype", "editoratype", "editorbtype", "editorctype", "eid", "eprint",
				 "eprintclass", "eprinttype", "eventdate", "eventtitle", "file", "foreword", "holder",
				 "howpublished", "indextitle", "insitution", "introduction", "isan", "isbn", "ismn",
				 "isrn", "issn", "issue", "issuesubtitle", "issuetitle", "iswc", "journalsubtitle",
				 "journaltitle", "label", "language", "library", "location", "mainsubtitle",
				 "maintitle", "maintitleaddon", "month", "nameaddon"};*/
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class THESIS : BibtexEntryType {
        private static THESIS _instance = new THESIS();
        private THESIS() { }
        public static THESIS Instance { get { return _instance; } }

        public override string getName() {
            return "Thesis";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "type", "institution", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "note", "location", "month",
				 "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "chapter", "pages", "pagetotal", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class UNPUBLISHED : BibtexEntryType {
        private static BET.UNPUBLISHED _instance = new BET.UNPUBLISHED();
        private UNPUBLISHED() { }
        public static BET.UNPUBLISHED Instance { get { return _instance; } }

        public override string getName() {
            return "Unpublished";
        }
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "language", "howpublished", "note",
				 "location", "date", "month", "year", "addendum", "pubstate", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "howpublished", "pubstate", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    // === Type aliases: ===

    public class CONFERENCE : BibtexEntryType {
        private static BET.CONFERENCE _instance = new BET.CONFERENCE();
        private CONFERENCE() { }
        public static BET.CONFERENCE Instance { get { return _instance; } }

	public override string getName() {
	    return "Conference";
	}
	// Treated as alias of "INPROCEEDINGS" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
            return BibLatexEntryTypes.INPROCEEDINGS.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.INPROCEEDINGS.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.INPROCEEDINGS.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class ELECTRONIC : BibtexEntryType {
        private static BET.ELECTRONIC _instance = new BET.ELECTRONIC();
        private ELECTRONIC() { }
        public static BET.ELECTRONIC Instance { get { return _instance; } }

	public override string getName() {
	    return "Electronic";
	}
	// Treated as alias of "ONLINE" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
	    return BibLatexEntryTypes.ONLINE.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.ONLINE.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.ONLINE.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    public class MASTERSTHESIS : BibtexEntryType {
        private static BET.MASTERSTHESIS _instance = new BET.MASTERSTHESIS();
        private MASTERSTHESIS() { }
        public static BET.MASTERSTHESIS Instance { get { return _instance; } }

        public override string getName() {
            return "Mastersthesis";
        }
	// Treated as alias of "THESIS", except "type" field is optional
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "institution", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "language", "note", "location", "month",
				 "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "chapter", "pages", "pagetotal", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class PHDTHESIS : BibtexEntryType {
        private static BET.PHDTHESIS _instance = new BET.PHDTHESIS();
        private PHDTHESIS() { }
        public static BET.PHDTHESIS Instance { get { return _instance; } }

        public override string getName() {
            return "Phdthesis";
        }
	// Treated as alias of "THESIS", except "type" field is optional
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "institution", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "language", "note", "location", "month",
				 "chapter", "pages", "pagetotal", "addendum", "pubstate", "doi", "eprint", "eprintclass",
				 "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "chapter", "pages", "pagetotal", "doi", "eprint",
				 "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class TECHREPORT : BibtexEntryType {
        private static BET.TECHREPORT _instance = new BET.TECHREPORT();
        private TECHREPORT() { }
        public static BET.TECHREPORT Instance { get { return _instance; } }

        public override string getName() {
            return "Techreport";
        }
	// Treated as alias of "REPORT", except "type" field is optional
        public override string[] getRequiredFields() {
            return new string[] {"author", "title", "institution", "year", "date"};
        }
        public override string[] getOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "language", "number", "version", "note",
				 "location", "month", "isrn", "chapter", "pages", "pagetotal", "addendum", "pubstate",
				 "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {"subtitle", "titleaddon", "type", "number", "isrn", "chapter", "pages", "pagetotal",
				 "doi", "eprint", "eprintclass", "eprinttype", "url", "urldate"};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };

    public class WWW : BibtexEntryType {
        private static WWW _instance = new WWW();
        private WWW() { }
        public static WWW Instance { get { return _instance; } }

	public override string getName() {
	    return "Www";
	}
	// Treated as alias of "ONLINE" according to Biblatex 1.0: 
        public override string[] getRequiredFields() {
            return BibLatexEntryTypes.ONLINE.Instance.getRequiredFields();
        }
        public override string[] getOptionalFields() {
            return BibLatexEntryTypes.ONLINE.Instance.getOptionalFields();
        }

        public override string[] getPrimaryOptionalFields() {
            return BibLatexEntryTypes.ONLINE.Instance.getPrimaryOptionalFields();
        }
     
	public override string describeRequiredFields() {
	    return "";
	}
	public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
	    return entry.allFieldsPresent(getRequiredFields(), database);
	}
    };

    // Unsupported types and custom types left out

    /*public class ARTICLE : BibtexEntryType {
        public override string getName() {
            return "Article";
        }
        public override string[] getRequiredFields() {
            return new string[] {};
        }
        public override string[] getOptionalFields() {
            return new string[] {};
        }
        public override string[] getPrimaryOptionalFields() {
            return new string[] {};
        }
        public override string describeRequiredFields() {
            return "";
        }
        public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
            return entry.allFieldsPresent(getRequiredFields(), database);
        }
    };*/
}
