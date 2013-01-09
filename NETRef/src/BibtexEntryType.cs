/*
Copyright (C) 2003 David Weitzman, Morten O. Alver

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

Note:
Modified for use in JabRef.

*/
using System;
using System.Collections.Generic;
namespace net.sf.jabref {


public abstract class BibtexEntryType : IComparable<BibtexEntryType>
{

    public class OTHER : BibtexEntryType
        {
            private static OTHER _instance = new OTHER();
            private OTHER() { }
            public static OTHER Instance { get { return _instance; } }

            public override string getName()
            {
                return "Other";
            }

            public override string[] getOptionalFields()
            {
                return new string[0];
            }

            public override string[] getRequiredFields()
            {
                return new string[0];
            }


            public override string describeRequiredFields()
            {
                return "";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return true;
            }
        };


    public class ARTICLE : BibtexEntryType
        {
            private static ARTICLE _instance = new ARTICLE();
            private ARTICLE() { }
            public static ARTICLE Instance { get { return _instance; } }

            public override string getName()
            {
                return "Article";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "number", "month", "part", "eid", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "journal", "year", "volume", "pages"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, JOURNAL and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "author", "title", "journal", "year", "bibtexkey", "volume", "pages"
                    }, database);
            }
        };

    public class BOOKLET : BibtexEntryType
        {
            private static BOOKLET _instance = new BOOKLET();
            private BOOKLET() { }
            public static BOOKLET Instance { get { return _instance; } }

            public override string getName()
            {
                return "Booklet";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "howpublished", "lastchecked", "address", "month", "year", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title"
                };
            }

            public override string describeRequiredFields()
            {
                return "TITLE";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "bibtexkey"
                    }, database);
            }
        };


   public class INBOOK : BibtexEntryType
        {
            private static INBOOK _instance = new INBOOK();
            private INBOOK() { }
            public static INBOOK Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Inbook";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "volume", "number", "pages", "series", "type", "address", "edition",
		    "month", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "chapter", "pages", "title", "publisher", "year", "editor",
		    "author"
                };
            }

            public override string[] getRequiredFieldsForCustomization() {
                return new string[] {"author/editor", "title", "chapter/pages", "year", "publisher"};
            }

            public override string describeRequiredFields()
            {
                return "TITLE, CHAPTER and/or PAGES, PUBLISHER, YEAR, and an "
		    +"EDITOR and/or AUTHOR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "publisher", "year", "bibtexkey"
                    }, database) &&
		    (((entry.getField("author") != null) ||
		      (entry.getField("editor") != null)) &&
		     ((entry.getField("chapter") != null) ||
		      (entry.getField("pages") != null)));
            }
        };

    public class BOOK : BibtexEntryType
        {
            private static BOOK _instance = new BOOK();
            private BOOK() { }
            public static BOOK Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Book";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "volume", "number", "pages", "series", "address", "edition", "month",
                    "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title", "publisher", "year", "editor", "author"
                };
            }

            public override string[] getRequiredFieldsForCustomization()
            {
                return new string[]
                {
                    "title", "publisher", "year", "author/editor"
                };
            }

            public override string describeRequiredFields()
            {
                return "TITLE, PUBLISHER, YEAR, and an EDITOR and/or AUTHOR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "publisher", "year", "bibtexkey"
                    }, database) &&
                ((entry.getField("author") != null) ||
                (entry.getField("editor") != null));
            }
        };


    public class INCOLLECTION : BibtexEntryType
        {
            private static INCOLLECTION _instance = new INCOLLECTION();
            private INCOLLECTION() { }
            public static INCOLLECTION Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Incollection";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "editor", "volume", "number", "series", "type", "chapter",
		    "pages", "address", "edition", "month", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "booktitle", "publisher", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, BOOKTITLE, PUBLISHER and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"author", "title", "booktitle", "publisher", "year",
			"bibtexkey"

                    }, database);
            }
        };

    public class CONFERENCE : BibtexEntryType
        {
            private static CONFERENCE _instance = new CONFERENCE();
            private CONFERENCE() { }
            public static CONFERENCE Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Conference";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "editor", "volume", "number", "series", "pages",
		    "address", "month", "organization", "publisher", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "booktitle", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, BOOKTITLE and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"author", "title", "booktitle", "year" , "bibtexkey"
                    }, database);
            }
        };

    public class INPROCEEDINGS : BibtexEntryType
        {
            private static INPROCEEDINGS _instance = new INPROCEEDINGS();
            private INPROCEEDINGS() { }
            public static INPROCEEDINGS Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Inproceedings";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "editor", "volume", "number", "series", "pages",
		    "address", "month", "organization", "publisher", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "booktitle", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, BOOKTITLE and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"author", "title", "booktitle", "year" , "bibtexkey"
                    }, database);
            }
        };

    public class PROCEEDINGS : BibtexEntryType
        {
            private static PROCEEDINGS _instance = new PROCEEDINGS();
            private PROCEEDINGS() { }
            public static PROCEEDINGS Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Proceedings";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "editor", "volume", "number", "series", "address",
		    "publisher", "note", "month", "organization"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "TITLE and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"title", "year", "bibtexkey"
                    }, database);
            }
        };


    public class MANUAL : BibtexEntryType
        {
            private static MANUAL _instance = new MANUAL();
            private MANUAL() { }
            public static MANUAL Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Manual";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "organization", "address", "edition",
		    "month", "year", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title"
                };
            }

            public override string describeRequiredFields()
            {
                return "TITLE";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "bibtexkey"
                    }, database);
            }
        };

    public class TECHREPORT : BibtexEntryType
        {
            private static TECHREPORT _instance = new TECHREPORT();
            private TECHREPORT() { }
            public static TECHREPORT Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Techreport";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "type", "number", "address", "month", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "institution", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, INSTITUTION and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"author", "title", "institution", "year",
			"bibtexkey"
                    }, database);
            }
        };


    public class MASTERSTHESIS : BibtexEntryType
        {
            private static MASTERSTHESIS _instance = new MASTERSTHESIS();
            private MASTERSTHESIS() { }
            public static MASTERSTHESIS Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Mastersthesis";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "type", "address", "month", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "school", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, SCHOOL and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "author", "title", "school", "year", "bibtexkey"
                    }, database);
            }
        };


    public class PHDTHESIS : BibtexEntryType
        {
            private static PHDTHESIS _instance = new PHDTHESIS();
            private PHDTHESIS() { }
            public static PHDTHESIS Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Phdthesis";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "type", "address", "month", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "school", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE, SCHOOL and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "author", "title", "school", "year", "bibtexkey"
                    }, database);
            }
        };

    public class UNPUBLISHED : BibtexEntryType
        {
            private static UNPUBLISHED _instance = new UNPUBLISHED();
            private UNPUBLISHED() { }
            public static UNPUBLISHED Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Unpublished";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "month", "year"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "author", "title", "note"
                };
            }

            public override string describeRequiredFields()
            {
                return "AUTHOR, TITLE and NOTE";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
			"author", "title", "note", "bibtexkey"
                    }, database);
            }
        };

     public class PERIODICAL : BibtexEntryType
        {
            private static PERIODICAL _instance = new PERIODICAL();
            private PERIODICAL() { }
            public static PERIODICAL Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Periodical";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "editor", "language", "series", "volume", "number", "organization", "month", "note", "url"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title", "year"
                };
            }

            public override string describeRequiredFields()
            {
                return "TITLE and YEAR";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "year", "bibtexkey"
                    }, database);
            }
        };

     public class PATENT : BibtexEntryType
        {
            private static PATENT _instance = new PATENT();
            private PATENT() { }
            public static PATENT Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Patent";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "title", "language", "assignee", "address", "type", "number", "day", "dayfiled", "month", "monthfiled", "note", "url"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "nationality", "number", "year", "yearfiled"
                };
            }

            public override string describeRequiredFields()
            {
                return "NATIONALITY, NUMBER, YEAR or YEARFILED";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "number", "bibtexkey"
                    }, database) &&
                ((entry.getField("year") != null) ||
                (entry.getField("yearfiled") != null));
            }
        };

   public class STANDARD : BibtexEntryType
        {
            private static STANDARD _instance = new STANDARD();
            private STANDARD() { }
            public static STANDARD Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Standard";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "language", "howpublished", "type", "number", "revision", "address", "month", "year", "note", "url"
                };
            }

            public override string[] getRequiredFields()
            {
                return new string[]
                {
                    "title", "organization", "institution"
                };
            }

            public override string[] getRequiredFieldsForCustomization() {
                return new string[] {"title", "organization/institution"};
            }

            public override string describeRequiredFields()
            {
                return "TITLE, ORGANIZATION or INSTITUTION";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "title", "bibtexkey"
                    }, database) &&
                ((entry.getField("organization") != null) ||
                (entry.getField("institution") != null));
            }
        };

    public class ELECTRONIC : BibtexEntryType
        {
            private static ELECTRONIC _instance = new ELECTRONIC();
            private ELECTRONIC() { }
            public static ELECTRONIC Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Electronic";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "month", "year", "title", "language", "howpublished", "organization", "address", "note", "url"
                };
            }

            public override string[] getRequiredFields()
            {
                return null;
            }

            public override string describeRequiredFields()
            {
                return "None";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
                return entry.allFieldsPresent(new string[]
                    {
                        "bibtexkey"
                    }, database);
            }
        };

    public class MISC : BibtexEntryType
        {
            private static MISC _instance = new MISC();
            private MISC() { }
            public static MISC Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Misc";
            }

            public override string[] getOptionalFields()
            {
                return new string[]
                {
                    "author", "title", "howpublished", "month", "year", "note"
                };
            }

            public override string[] getRequiredFields()
            {
                return null;
            }

            public override string describeRequiredFields()
            {
                return "None";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
		return entry.allFieldsPresent(new string[]
                    {
			"bibtexkey"
                    }, database);
            }
        };

    /**
     * This type is provided as an emergency choice if the user makes
     * customization changes that remove the type of an entry.
     */
    public class TYPELESS : BibtexEntryType
        {
            private static TYPELESS _instance = new TYPELESS();
            private TYPELESS() { }
            public static TYPELESS Instance { get { return _instance; } }

            
            public override string getName()
            {
                return "Typeless";
            }

            public override string[] getOptionalFields()
            {
                return null;
            }

            public override string[] getRequiredFields()
            {
                return null;
            }

            public override string describeRequiredFields()
            {
                return "None";
            }

            public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
            {
		return false;
           }
        };


    public abstract string getName();

    public int CompareTo(BibtexEntryType o) {
	    return getName().CompareTo(o.getName());
    }

    public abstract string[] getOptionalFields();

    public abstract string[] getRequiredFields();

    public virtual string[] getPrimaryOptionalFields() {
        return new string[0];
    }

    public abstract string describeRequiredFields();

    public abstract bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database);


    public string[] getUtilityFields(){
        return new string[] {"search" } ;
    }


    public bool isRequired(string field) {
	string[] req = getRequiredFields();
	if (req == null) return false;
	for (int i=0; i<req.Length; i++)
	    if (req[i].Equals(field)) return true;
	return false;
    }

    public bool isOptional(string field) {
	string[] opt = getOptionalFields();
	if (opt == null) return false;
	for (int i=0; i<opt.Length; i++)
	    if (opt[i].Equals(field)) return true;
	return false;
    }

    public static Dictionary<string, BibtexEntryType> ALL_TYPES = new Dictionary<string, BibtexEntryType>();
    public static Dictionary<string, BibtexEntryType> STANDARD_TYPES = new Dictionary<string, BibtexEntryType>();
    static BibtexEntryType() {
        // Put the standard entry types into the type map.
        if (!Globals.prefs.getBoolean("biblatexMode")) {
            ALL_TYPES.Add("article", ARTICLE.Instance);
            ALL_TYPES.Add("inbook", INBOOK.Instance);
            ALL_TYPES.Add("book", BOOK.Instance);
            ALL_TYPES.Add("booklet", BOOKLET.Instance);
            ALL_TYPES.Add("incollection", INCOLLECTION.Instance);
            ALL_TYPES.Add("conference", CONFERENCE.Instance);
            ALL_TYPES.Add("inproceedings", INPROCEEDINGS.Instance);
            ALL_TYPES.Add("proceedings", PROCEEDINGS.Instance);
            ALL_TYPES.Add("manual", MANUAL.Instance);
            ALL_TYPES.Add("mastersthesis", MASTERSTHESIS.Instance);
            ALL_TYPES.Add("phdthesis", PHDTHESIS.Instance);
            ALL_TYPES.Add("techreport", TECHREPORT.Instance);
            ALL_TYPES.Add("unpublished", UNPUBLISHED.Instance);
            ALL_TYPES.Add("patent", PATENT.Instance);
            ALL_TYPES.Add("standard", STANDARD.Instance);
            ALL_TYPES.Add("electronic", ELECTRONIC.Instance);
            ALL_TYPES.Add("periodical", PERIODICAL.Instance);
            ALL_TYPES.Add("misc", MISC.Instance);
            ALL_TYPES.Add("other", OTHER.Instance);
        }
        else {
            ALL_TYPES.Add("article", BibLatexEntryTypes.ARTICLE.Instance);
	    ALL_TYPES.Add("book", BibLatexEntryTypes.BOOK.Instance);
	    ALL_TYPES.Add("inbook", BibLatexEntryTypes.INBOOK.Instance);
	    ALL_TYPES.Add("bookinbook", BibLatexEntryTypes.BOOKINBOOK.Instance);
	    ALL_TYPES.Add("suppbook", BibLatexEntryTypes.SUPPBOOK.Instance);
	    ALL_TYPES.Add("booklet", BibLatexEntryTypes.BOOKLET.Instance);
	    ALL_TYPES.Add("collection", BibLatexEntryTypes.COLLECTION.Instance);
	    ALL_TYPES.Add("incollection", BibLatexEntryTypes.INCOLLECTION.Instance);
	    ALL_TYPES.Add("suppcollection", BibLatexEntryTypes.SUPPCOLLECTION.Instance);
	    ALL_TYPES.Add("manual", BibLatexEntryTypes.MANUAL.Instance);
	    ALL_TYPES.Add("misc", BibLatexEntryTypes.MISC.Instance);
	    ALL_TYPES.Add("online", BibLatexEntryTypes.ONLINE.Instance);
	    ALL_TYPES.Add("patent", BibLatexEntryTypes.PATENT.Instance);
	    ALL_TYPES.Add("periodical", BibLatexEntryTypes.PERIODICAL.Instance);
	    ALL_TYPES.Add("suppperiodical", BibLatexEntryTypes.SUPPPERIODICAL.Instance);
	    ALL_TYPES.Add("proceedings", BibLatexEntryTypes.PROCEEDINGS.Instance);
	    ALL_TYPES.Add("inproceedings", BibLatexEntryTypes.INPROCEEDINGS.Instance);
	    ALL_TYPES.Add("reference", BibLatexEntryTypes.REFERENCE.Instance);
	    ALL_TYPES.Add("inreference", BibLatexEntryTypes.INREFERENCE.Instance);
	    ALL_TYPES.Add("report", BibLatexEntryTypes.REPORT.Instance);
	    ALL_TYPES.Add("set", BibLatexEntryTypes.SET.Instance);
	    ALL_TYPES.Add("thesis", BibLatexEntryTypes.THESIS.Instance);
	    ALL_TYPES.Add("unpublished", BibLatexEntryTypes.UNPUBLISHED.Instance);
	    ALL_TYPES.Add("conference", BibLatexEntryTypes.CONFERENCE.Instance);
	    ALL_TYPES.Add("electronic", BibLatexEntryTypes.ELECTRONIC.Instance);
	    ALL_TYPES.Add("mastersthesis", BibLatexEntryTypes.MASTERSTHESIS.Instance);
	    ALL_TYPES.Add("phdthesis", BibLatexEntryTypes.PHDTHESIS.Instance);
	    ALL_TYPES.Add("techreport", BibLatexEntryTypes.TECHREPORT.Instance);
	    ALL_TYPES.Add("www", BibLatexEntryTypes.WWW.Instance);
        }

        // We need a record of the standard types, in case the user wants
        // to remove a customized version. Therefore we clone the map.
        STANDARD_TYPES = new Dictionary<string, BibtexEntryType>(ALL_TYPES);
    }

    /**
     * This method returns the BibtexEntryType for the name of a type,
     * or null if it does not exist.
     */
    public static BibtexEntryType getType(string name) {
	//Util.pr("'"+name+"'");
        var key = name.ToLower();
	object o = ALL_TYPES.ContainsKey(key) ? ALL_TYPES[key] : null;
	if (o == null)
	    return null;
	else return (BibtexEntryType)o;
    }

    /**
     * This method returns the standard BibtexEntryType for the
     * name of a type, or null if it does not exist.
     */
    public static BibtexEntryType getStandardType(string name) {
	//Util.pr("'"+name+"'");
	object o = STANDARD_TYPES[(name.ToLower())];
	if (o == null)
	    return null;
	else return (BibtexEntryType)o;
    }

    /**
     * Removes a customized entry type from the type map. If this type
     * overrode a standard type, we reinstate the standard one.
     *
     * @param name The customized entry type to remove.
     */
    public static void removeType(string name) {
	//BibtexEntryType type = getType(name);
	string nm = name.ToLower();
        //System.out.println(ALL_TYPES.Count);
	ALL_TYPES.Remove(nm);
        //System.out.println(ALL_TYPES.Count);
	if (STANDARD_TYPES[nm] != null) {
	    // In this case the user has removed a customized version
	    // of a standard type. We reinstate the standard type.
	    ALL_TYPES.Add(nm, STANDARD_TYPES[(nm)]);
	}

    }

    /**
     * Get an array of the required fields in a form appropriate for the entry customization
     * dialog - that is, thie either-or fields together and separated by slashes.
     * @return Array of the required fields in a form appropriate for the entry customization dialog.
     */
    public virtual string[] getRequiredFieldsForCustomization() {
        return getRequiredFields();
    }
}
}
