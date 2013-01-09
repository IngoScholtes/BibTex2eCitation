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
using System;
using System.Collections.Generic;
namespace net.sf.jabref {


/**
 * 
 * A comparator for BibtexEntry fields
 * 
 * Initial Version:
 * 
 * @author alver
 * @version Date: Oct 13, 2005 Time: 10:10:04 PM To
 * 
 * Current Version:
 * 
 * @author $Author$
 * @version $Revision$ ($Date$)
 * 
 * TODO: Testcases
 * 
 */
public class FieldComparator : IComparer<BibtexEntry> {

    // TODO: Collator?
    //private static Collator collator;
    
	private string[] field;
    private string fieldName;

    bool isNameField, isTypeHeader, isYearField, isMonthField, isNumeric;

	int multiplier;

	public FieldComparator(string field) : this(field, false) {
	}

	public FieldComparator(string field, bool reversed) {
        this.fieldName = field;
        this.field = field.Split(new string[] { Globals.COL_DEFINITION_FIELD_SEPARATOR }, StringSplitOptions.None);
		multiplier = reversed ? -1 : 1;
		isTypeHeader = this.field[0].Equals(Globals.TYPE_HEADER);
        isNameField = (this.field[0].Equals("author")
                || this.field[0].Equals("editor"));
		isYearField = this.field[0].Equals("year");
		isMonthField = this.field[0].Equals("month");
        isNumeric = BibtexFields.isNumeric(this.field[0]);
    }

	public int Compare(BibtexEntry e1, BibtexEntry e2) {
		Object f1, f2;

		if (isTypeHeader) {
			// Sort by type.
			f1 = e1.getType().getName();
			f2 = e2.getType().getName();
		} else {

			// If the field is author or editor, we rearrange names so they are
			// sorted according to last name.
			f1 = getField(e1);
			f2 = getField(e2);
		}

		/*
		 * [ 1598777 ] Month sorting
		 * 
		 * http://sourceforge.net/tracker/index.php?func=detail&aid=1598777&group_id=92314&atid=600306
		 */
		int localMultiplier = multiplier;
		if (isMonthField)
			localMultiplier = -localMultiplier;
		
		// Catch all cases involving null:
		if (f1 == null)
			return f2 == null ? 0 : localMultiplier;

		if (f2 == null)
			return -localMultiplier;

		// Now we now that both f1 and f2 are != null
		if (isNameField) {
			f1 = AuthorList.fixAuthorForAlphabetization((string) f1);
			f2 = AuthorList.fixAuthorForAlphabetization((string) f2);
		} else if (isYearField) {
			/*
			 * [ 1285977 ] Impossible to properly sort a numeric field
			 * 
			 * http://sourceforge.net/tracker/index.php?func=detail&aid=1285977&group_id=92314&atid=600307
			 */
			f1 = Util.toFourDigitYear((string) f1);
			f2 = Util.toFourDigitYear((string) f2);
		} else if (isMonthField) {
			/*
			 * [ 1535044 ] Month sorting
			 * 
			 * http://sourceforge.net/tracker/index.php?func=detail&aid=1535044&group_id=92314&atid=600306
			 */
			f1 = Util.getMonthNumber((string)f1);			
			f2 = Util.getMonthNumber((string)f2);
		}

        if (isNumeric) {
            int? i1 = null, i2 = null;
            try {
                i1 = int.Parse((string)f1);
            } catch (FormatException ex) {
                // Parsing failed.
            }

            try {
                i2 = int.Parse((string)f2);
            } catch (FormatException ex) {
                // Parsing failed.
            }

            if (i2 != null && i1 != null) {
                // Ok, parsing was successful. Update f1 and f2:
                f1 = i1;
                f2 = i2;
            } else if (i1 != null) {
                // The first one was parseable, but not the second one.
                // This means we consider one < two
                f1 = i1;
                f2 = i1 + 1;
            } else if (i2 != null) {
                // The second one was parseable, but not the first one.
                // This means we consider one > two
                f2 = i2;
                f1 = i2 + 1;
            }
            // Else none of them were parseable, and we can fall back on comparing strings.    
        }

        int result = 0;
		if ((f1 is int) && (f2 is int)) {
			result = (((int) f1).CompareTo((int) f2));
		} else if (f2 is int) {
			int f1AsInteger = int.Parse(f1.ToString());
			result = -((f1AsInteger).CompareTo((int) f2));
		} else if (f1 is int) {
			int f2AsInteger = int.Parse(f2.ToString());
			result = -(((int) f1).CompareTo(f2AsInteger));
		} else {
			string ours = ((string) f1).ToLower(), theirs = ((string) f2).ToLower();
            result = ours.CompareTo(theirs);//TODO collator.compare(ours, theirs);//
		}

		return result * localMultiplier;
	}

    private Object getField(BibtexEntry entry) {
        for (int i = 0; i < field.Length; i++) {
            Object o = entry.getField(field[i]);
            if (o != null)
                return o;
        }
        return null;
    }

    /**
	 * Returns the field this IComparable compares by.
	 * 
	 * @return The field name.
	 */
	public string getFieldName() {
		return fieldName;
	}
}
}
