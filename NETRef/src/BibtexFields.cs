/*  Copyright (C) 2003-2011 Raik Nagel and JabRef contributors
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
//
// function : Handling of bibtex fields.
//            All bibtex-field related stuff should be placed here!
//            Because we can export these informations into additional
//            config files -> simple extension and definition of new fields....
//
// todo     : - handling of identically fields with different names
//              e.g. LCCN = lib-congress
//            - group id for each fields, e.g. standard, jurabib, bio....
//            - add a additional properties functionality into the
//              BibtexSingleField class
//
// modified : r.nagel 25.04.2006
//            export/import of some definition from/to a xml file

using System;
using System.Collections.Generic;
namespace net.sf.jabref
{

    public class BibtexFields
    {
        public static readonly string KEY_FIELD = "bibtexkey";

        // some internal fields
        public static readonly string
            SEARCH = "__search",
            GROUPSEARCH = "__groupsearch",
            MARKED = "__markedentry",
            OWNER = "owner",
            TIMESTAMP = "timestamp", // it's also definied at the JabRefPreferences class
            ENTRYTYPE = "entrytype",

            // Using this when I have no database open or when I read
            // non bibtex file formats (used by the ImportFormatReader.java)
            DEFAULT_BIBTEXENTRY_ID = "__ID";

        public static readonly string[] DEFAULT_INSPECTION_FIELDS = new string[] { "author", "title", "year", KEY_FIELD };


        // singleton instance
        private static readonly BibtexFields runtime = new BibtexFields();

        // Contains all bibtex-field objects (BibtexSingleField)
        private Dictionary<string, BibtexSingleField> fieldSet;

        // Contains all known (and public) bibtex fieldnames
        private string[] PUBLIC_FIELDS = null;

        private BibtexFields()
  {
    fieldSet = new Dictionary<string, BibtexSingleField>();
    BibtexSingleField dummy = null ;

    // FIRST: all standard fields
    // These are the fields that BibTex might want to treat, so these
    // must conform to BibTex rules.
    add( new BibtexSingleField( "address", true, GUIGlobals.SMALL_W  ) ) ;
    // An annotation. It is not used by the standard bibliography styles,
    // but may be used by others that produce an annotated bibliography.
    // http://www.ecst.csuchico.edu/~jacobsd/bib/formats/bibtex.html
    add( new BibtexSingleField( "annote", true, GUIGlobals.LARGE_W  ) ) ;
    add( new BibtexSingleField( "author", true, GUIGlobals.MEDIUM_W, 280 ) ) ;
    add( new BibtexSingleField( "booktitle", true, 175 ) ) ;
    add( new BibtexSingleField( "chapter", true, GUIGlobals.SMALL_W  ) ) ;
    add( new BibtexSingleField( "crossref", true, GUIGlobals.SMALL_W ) ) ;
    add( new BibtexSingleField( "edition", true, GUIGlobals.SMALL_W  ) ) ;
    add( new BibtexSingleField( "editor", true, GUIGlobals.MEDIUM_W, 280  ) ) ;
    add( new BibtexSingleField( "howpublished", true, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "institution", true, GUIGlobals.MEDIUM_W  ) ) ;

    dummy = new BibtexSingleField( "journal", true, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("journalNames");
    add(dummy) ;
    add( new BibtexSingleField( "key", true ) ) ;
    add( new BibtexSingleField( "month", true, GUIGlobals.SMALL_W ) ) ;
    add( new BibtexSingleField( "note", true, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "number", true, GUIGlobals.SMALL_W, 60  ).setNumeric(true) ) ;
    add( new BibtexSingleField( "organization", true, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "pages", true, GUIGlobals.SMALL_W ) ) ;
    add( new BibtexSingleField( "publisher", true, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "school", true, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "series", true, GUIGlobals.SMALL_W  ) ) ;
    add( new BibtexSingleField( "title", true, 400 ) ) ;
    add( new BibtexSingleField( "type", true, GUIGlobals.SMALL_W  ) ) ;
    add( new BibtexSingleField( "language", true, GUIGlobals.SMALL_W  ) ) ;
    add( new BibtexSingleField( "volume", true, GUIGlobals.SMALL_W, 60  ).setNumeric(true) ) ;
    add( new BibtexSingleField( "year", true, GUIGlobals.SMALL_W, 60 ).setNumeric(true) ) ;

    // some semi-standard fields
    dummy = new BibtexSingleField( KEY_FIELD, true ) ;
    dummy.setPrivate();
    add( dummy ) ;

    dummy = new BibtexSingleField( "doi", true, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("external");
    add(dummy) ;
    add( new BibtexSingleField( "eid", true, GUIGlobals.SMALL_W  ) ) ;

    dummy = new BibtexSingleField( "date", true ) ;
    dummy.setPrivate();
    add( dummy ) ;

    add(new BibtexSingleField("pmid", false, GUIGlobals.SMALL_W, 60).setNumeric(true));

    // additional fields ------------------------------------------------------
    add( new BibtexSingleField( "location", false ) ) ;
    add( new BibtexSingleField( "abstract", false, GUIGlobals.LARGE_W, 400  ) ) ;

    dummy =  new BibtexSingleField( "url", false, GUIGlobals.SMALL_W) ;
    dummy.setExtras("external");
    add(dummy) ;

    dummy = new BibtexSingleField( "pdf", false, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("browseDoc");
    add(dummy) ;

    dummy = new BibtexSingleField( "ps", false, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("browseDocZip");
    add(dummy) ;
    add( new BibtexSingleField( "comment", false, GUIGlobals.MEDIUM_W  ) ) ;
    add( new BibtexSingleField( "keywords", false, GUIGlobals.SMALL_W  ) ) ;
    //FIELD_EXTRAS.put("keywords", "selector");


    dummy = new BibtexSingleField(GUIGlobals.FILE_FIELD, false);
    dummy.setEditorType(GUIGlobals.FILE_LIST_EDITOR);
    add(dummy);


    add( new BibtexSingleField( "search", false, 75 ) ) ;


    // some internal fields ----------------------------------------------
    dummy = new BibtexSingleField( GUIGlobals.NUMBER_COL, false, 32  ) ;
    dummy.setPrivate() ;
    dummy.setWriteable(false);
    dummy.setDisplayable(false);
    add( dummy ) ;

    dummy = new BibtexSingleField( OWNER, false, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("setOwner");
    dummy.setPrivate();
    add(dummy) ;

    dummy = new BibtexSingleField( TIMESTAMP, false, GUIGlobals.SMALL_W ) ;
    dummy.setExtras("datepicker");
    dummy.setPrivate();
    add(dummy) ;

    dummy =  new BibtexSingleField( ENTRYTYPE, false, 75 ) ;
    dummy.setPrivate();
    add(dummy) ;

    dummy =  new BibtexSingleField( SEARCH, false) ;
    dummy.setPrivate();
    dummy.setWriteable(false);
    dummy.setDisplayable(false);
    add(dummy) ;

    dummy =  new BibtexSingleField( GROUPSEARCH, false) ;
    dummy.setPrivate();
    dummy.setWriteable(false);
    dummy.setDisplayable(false);
    add(dummy) ;

    dummy =  new BibtexSingleField( MARKED, false) ;
    dummy.setPrivate();
    dummy.setWriteable(true); // This field must be written to file!
    dummy.setDisplayable(false);
    add(dummy) ;

    // collect all public fields for the PUBLIC_FIELDS array
    List<string> pFields = new List<string>(fieldSet.Count) ;
    foreach (var sField in fieldSet.Values){
      if (sField.isPublic() )
      {
        pFields.Add( sField.getFieldName() );
        // or export the complet BibtexSingleField ?
        // BibtexSingleField.ToString() { return fieldname ; }
      }
    }

    PUBLIC_FIELDS = pFields.ToArray();
    // sort the entries
    Array.Sort( PUBLIC_FIELDS );
  }

        /**
         * Read the "numericFields" string array from preferences, and activate numeric
         * sorting for all fields listed in the array. If an unknown field name is included,
         * add a field descriptor for the new field.
         */
        public static void setNumericFieldsFromPrefs() {
        string[] numFields = Globals.prefs.getStringArray("numericFields");
        if (numFields == null)
            return;
        // Build a Set of field names for the fields that should be sorted numerically:
        var nF = new Dictionary<string, bool>();
        for (int i = 0; i < numFields.Length; i++) {
            nF.Add(numFields[i], true);
        }
        // Look through all registered fields, and activate numeric sorting if necessary:
        foreach (var fieldName in runtime.fieldSet.Keys) {
            BibtexSingleField field = runtime.fieldSet[fieldName];
            if (!field.isNumeric() && nF.ContainsKey(fieldName)) {
                field.setNumeric(nF.ContainsKey(fieldName));
            }
            nF.Remove(fieldName); // remove, so we clear the set of all standard fields.
        }
        // If there are fields left in nF, these must be non-standard fields. Add descriptors for them:
        foreach (var fieldName in nF.Keys) {
            BibtexSingleField field = new BibtexSingleField(fieldName, false);
            field.setNumeric(true);
            runtime.fieldSet.Add(fieldName, field);
        }

    }


        /** insert a field into the internal list */
        private void add(BibtexSingleField field)
        {
            // field == null check
            string key = field.name;
            fieldSet.Add(key, field);
        }

        // --------------------------------------------------------------------------
        //  the "static area"
        // --------------------------------------------------------------------------
        private static BibtexSingleField getField(string name)
        {
            if (name != null)
            {
                return runtime.fieldSet.ContainsKey(name.ToLower()) ? runtime.fieldSet[name.ToLower()] : null;
            }

            return null;
        }

        public static string getFieldExtras(string name)
        {
            BibtexSingleField sField = getField(name);
            if (sField != null)
            {
                return sField.getExtras();
            }
            return null;
        }


        public static int getEditorType(string name)
        {
            BibtexSingleField sField = getField(name);
            if (sField != null)
            {
                return sField.getEditorType();
            }
            return GUIGlobals.STANDARD_EDITOR;
        }

        public static double getFieldWeight(string name)
        {
            BibtexSingleField sField = getField(name);
            if (sField != null)
            {
                return sField.getWeight();
            }
            return GUIGlobals.DEFAULT_FIELD_WEIGHT;
        }

        public static void setFieldWeight(string fieldName, double weight)
        {
            BibtexSingleField sField = getField(fieldName);
            if (sField != null)
            {
                sField.setWeight(weight);
            }
        }

        public static int getFieldLength(string name)
        {
            BibtexSingleField sField = getField(name);
            if (sField != null)
            {
                return sField.getLength();
            }
            return GUIGlobals.DEFAULT_FIELD_LENGTH;
        }

        // returns an alternative name for the given fieldname
        public static string getFieldDisplayName(string fieldName)
        {
            BibtexSingleField sField = getField(fieldName);
            if (sField != null)
            {
                return sField.getAlternativeDisplayName();
            }
            return null;
        }

        public static bool isWriteableField(string field)
        {
            BibtexSingleField sField = getField(field);
            if (sField != null)
            {
                return sField.isWriteable();
            }
            return true;
        }

        public static bool isDisplayableField(string field)
        {
            BibtexSingleField sField = getField(field);
            if (sField != null)
            {
                return sField.isDisplayable();
            }
            return true;
        }

        /**
         * Returns true if the given field is a standard Bibtex field.
         *
         * @param field a <code>string</code> value
         * @return a <code>bool</code> value
         */
        public static bool isStandardField(string field)
        {
            BibtexSingleField sField = getField(field);
            if (sField != null)
            {
                return sField.isStandard();
            }
            return false;
        }

        public static bool isNumeric(string field)
        {
            BibtexSingleField sField = getField(field);
            if (sField != null)
            {
                return sField.isNumeric();
            }
            return false;
        }

        /** returns an string-array with all fieldnames */
        public static string[] getAllFieldNames()
        {
            return runtime.PUBLIC_FIELDS;
        }

        /** returns the fieldname of the entry at index t */
        public static string getFieldName(int t)
        {
            return runtime.PUBLIC_FIELDS[t];
        }

        /** returns the number of available fields */
        public static int numberOfPublicFields()
        {
            return runtime.PUBLIC_FIELDS.Length;
        }

        /*
           public static int getPreferredFieldLength(string name) {
           int l = DEFAULT_FIELD_LENGTH;
           Object o = fieldLength.get(name.ToLower());
           if (o != null)
           l = ((int)o).intValue();
           return l;
           }*/


        // --------------------------------------------------------------------------
        // a container class for all properties of a bibtex-field
        // --------------------------------------------------------------------------
        private class BibtexSingleField
        {
            private static readonly int
                STANDARD = 0x01,  // it is a standard bibtex-field
                PRIVATE = 0x02,  // internal use, e.g. owner, timestamp
                DISPLAYABLE = 0x04,  // These fields cannot be shown inside the source editor panel
                WRITEABLE = 0x08; // These fields will not be saved to the .bib file.

            // the fieldname
            internal string name;

            // Contains the standard, privat, displayable, writable infos
            // default is: not standard, public, displayable and writable
            private int flag = DISPLAYABLE | WRITEABLE;

            private int length = GUIGlobals.DEFAULT_FIELD_LENGTH;
            private double weight = GUIGlobals.DEFAULT_FIELD_WEIGHT;

            private int editorType = GUIGlobals.STANDARD_EDITOR;

            // a alternative displayname, e.g. used for
            // "citeseercitationcount"="Popularity"
            private string alternativeDisplayName = null;

            // the extras data
            // fieldExtras Contains mappings to tell the EntryEditor to add a specific
            // function to this field, for instance a "browse" button for the "pdf" field.
            private string extras = null;

            // This value defines whether contents of this field are expected to be
            // numeric values. This can be used to sort e.g. volume numbers correctly:
            private bool numeric = false;

            // a comma separated list of alternative bibtex-fieldnames, e.g.
            // "LCCN" is the same like "lib-congress"
            // private string otherNames = null ;

            // a Hashmap for a lot of additional "not standard" properties
            // todo: add the handling in a key=value manner
            // private Dictionary props = new Dictionary() ;

            // some constructors ;-)
            public BibtexSingleField(string fieldName)
            {
                name = fieldName;
            }

            public BibtexSingleField(string fieldName, bool pStandard)
            {
                name = fieldName;
                setFlag(pStandard, STANDARD);
            }

            public BibtexSingleField(string fieldName, bool pStandard, double pWeight)
            {
                name = fieldName;
                setFlag(pStandard, STANDARD);
                weight = pWeight;
            }

            public BibtexSingleField(string fieldName, bool pStandard, int pLength)
            {
                name = fieldName;
                setFlag(pStandard, STANDARD);
                length = pLength;
            }

            public BibtexSingleField(string fieldName, bool pStandard,
                                      double pWeight, int pLength)
            {
                name = fieldName;
                setFlag(pStandard, STANDARD);
                weight = pWeight;
                length = pLength;
            }


            private void setFlag(bool onOff, int flagID)
            {
                if (onOff)  // set the flag
                {
                    flag = flag | flagID;
                }
                else // unset the flag,
                {
                    flag = flag & (0xff ^ flagID);
                }
            }

            private bool isSet(int flagID)
            {
                if ((flag & flagID) == flagID)
                    return true;

                return false;
            }

            // -----------------------------------------------------------------------
            public bool isStandard()
            {
                return isSet(STANDARD);
            }

            public void setPrivate()
            {
                flag = flag | PRIVATE;
            }

            public bool isPrivate()
            {
                return isSet(PRIVATE);
            }

            public void setPublic()
            {
                setFlag(false, PRIVATE);
            }

            public bool isPublic()
            {
                return !isSet(PRIVATE);
            }

            public void setDisplayable(bool value)
            {
                setFlag(value, DISPLAYABLE);
            }

            public bool isDisplayable()
            {
                return isSet(DISPLAYABLE);
            }


            public void setWriteable(bool value)
            {
                setFlag(value, WRITEABLE);
            }

            public bool isWriteable()
            {
                return isSet(WRITEABLE);
            }

            // -----------------------------------------------------------------------
            public void setAlternativeDisplayName(string aName)
            {
                alternativeDisplayName = aName;
            }

            public string getAlternativeDisplayName()
            {
                return alternativeDisplayName;
            }
            // -----------------------------------------------------------------------

            public void setExtras(string pExtras)
            {
                extras = pExtras;
            }

            // fieldExtras Contains mappings to tell the EntryEditor to add a specific
            // function to this field, for instance a "browse" button for the "pdf" field.
            public string getExtras()
            {
                return extras;
            }

            public void setEditorType(int type)
            {
                editorType = type;
            }

            public int getEditorType()
            {
                return editorType;
            }
            // -----------------------------------------------------------------------

            public void setWeight(double value)
            {
                this.weight = value;
            }

            public double getWeight()
            {
                return this.weight;
            }

            // -----------------------------------------------------------------------
            public int getLength()
            {
                return this.length;
            }

            // -----------------------------------------------------------------------

            public string getFieldName()
            {
                return name;
            }


            /**
             * Set this field's numeric propery
             * @param numeric true to indicate that this is a numeric field.
             * @return this BibtexSingleField instance. Makes it easier to call this
             *   method on the fly while initializing without using a local variable.
             */
            public BibtexSingleField setNumeric(bool numeric)
            {
                this.numeric = numeric;
                return this;
            }

            public bool isNumeric()
            {
                return numeric;
            }

        }
    }
}
