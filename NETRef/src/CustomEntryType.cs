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
using System.IO;
using System.Text;
namespace net.sf.jabref {

/**
 * This class is used to represent customized entry types.
 *
 */
public class CustomEntryType : BibtexEntryType {

    private string name;
    private string[] req, opt, priOpt;
    private string[][] reqSets = null; // Sets of either-or required fields, if any

    public CustomEntryType(string name_, string[] req_, string[] opt_, string[] opt2_) {
        name = name_;
        parseRequiredFields(req_);
        List<string> allOpt = new List<string>();
        for (int i = 0; i < opt_.Length; i++)
            allOpt.Add(opt_[i]);
        for (int i=0; i<opt2_.Length; i++)
            allOpt.Add(opt2_[i]);
        opt = allOpt.ToArray();
        priOpt = opt_;
    }

    public CustomEntryType(string name_, string[] req_, string[] opt_) : this(name_, req_, opt_, new string[0]) {
    }

    public CustomEntryType(string name_, string reqStr, string optStr) {
        name = name_;
        if (reqStr.Length == 0)
            req = new string[0];
        else {
            parseRequiredFields(reqStr);

        }
        if (optStr.Length == 0)
            opt = new string[0];
        else
            opt = optStr.Split(';');
    }

    protected void parseRequiredFields(string reqStr) {
        string[] parts = reqStr.Split(';');
        parseRequiredFields(parts);
    }

    protected void parseRequiredFields(string[] parts) {
        List<string> fields = new List<string>();
        List<string[]> sets = new List<string[]>();
        for (int i = 0; i < parts.Length; i++) {
            string[] subParts = parts[i].Split('/');
            for (int j = 0; j < subParts.Length; j++) {
                fields.Add(subParts[j]);
            }
            // Check if we have either/or fields:
            if (subParts.Length > 1) {
                sets.Add(subParts);
            }
        }
        req = fields.ToArray();
        if (sets.Count > 0) {
            reqSets = sets.ToArray();
        }
    }

    public override string getName() {
	return name;
    }

    public override string[] getOptionalFields() {
	return opt;
    }
    public override string[] getRequiredFields() {
	return req;
    }

    public override string[] getPrimaryOptionalFields() {
        return priOpt;
    }

    public override string[] getRequiredFieldsForCustomization() {
        return getRequiredFieldsString().Split(';');
    }

    //    public bool isTemporary

    public override string describeRequiredFields() {
	StringBuilder sb = new StringBuilder();
	for (int i=0; i<req.Length; i++) {
	    sb.Append(req[i]);
	    sb.Append(((i<=req.Length-1)&&(req.Length>1))?", ":"");
	}
	return sb.ToString();
    }

    public string describeOptionalFields() {
	StringBuilder sb = new StringBuilder();
	for (int i=0; i<opt.Length; i++) {
	    sb.Append(opt[i]);
	    sb.Append(((i<=opt.Length-1)&&(opt.Length>1))?", ":"");
	}
	return sb.ToString();
    }

    /**
     * Check whether this entry's required fields are set, taking crossreferenced entries and
     * either-or fields into account:
     * @param entry The entry to check.
     * @param database The entry's database.
     * @return True if required fields are set, false otherwise.
     */
    public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database) {
        // First check if the bibtex key is set:
        if (entry.getField(Globals.KEY_FIELD) == null)
                return false;
        // Then check other fields:
        bool[] isSet = new bool[req.Length];
        // First check for all fields, whether they are set here or in a crossref'd entry:
	    for (int i=0; i<req.Length; i++)
            isSet[i] = BibtexDatabase.getResolvedField(req[i], entry, database) != null;
        // Then go through all fields. If a field is not set, see if it is part of an either-or
        // set where another field is set. If not, return false:
    	for (int i=0; i<req.Length; i++) {
            if (!isSet[i]) {
                if (!isCoupledFieldSet(req[i], entry, database))
                    return false;
            }
        }
        // Passed all fields, so return true:
        return true;
    }

    protected bool isCoupledFieldSet(string field, BibtexEntry entry, BibtexDatabase database) {
        if (reqSets == null)
            return false;
        for (int i=0; i<reqSets.Length; i++) {
            bool takesPart = false, oneSet = false;
            for (int j=0; j<reqSets[i].Length; j++) {
                // If this is the field we're looking for, note that the field is part of the set:
                if (reqSets[i][j].Equals(field, System.StringComparison.CurrentCultureIgnoreCase))
                    takesPart = true;
                // If it is a different field, check if it is set:
                else if (BibtexDatabase.getResolvedField(reqSets[i][j], entry, database) != null)
                    oneSet = true;
            }
            // Ths the field is part of the set, and at least one other field is set, return true:
            if (takesPart && oneSet)
                return true;
        }
        // No hits, so return false:
        return false;
    }

    /**
     * Get a string describing the required field set for this entry type.
     * @return Description of required field set for storage in preferences or bib file.
     */
    public string getRequiredFieldsString() {
        StringBuilder sb = new StringBuilder();
        int reqSetsPiv = 0;
        for (int i=0; i<req.Length; i++) {
            if ((reqSets == null) || (reqSetsPiv == reqSets.Length)) {
                sb.Append(req[i]);
            }
            else if (req[i].Equals(reqSets[reqSetsPiv][0])) {
                for (int j = 0; j < reqSets[reqSetsPiv].Length; j++) {
                    sb.Append(reqSets[reqSetsPiv][j]);
                    if (j < reqSets[reqSetsPiv].Length-1)
                        sb.Append('/');
                }
                // Skip next n-1 fields:
                i += reqSets[reqSetsPiv].Length-1;
                reqSetsPiv++;
            }
            else sb.Append(req[i]);
            if (i < req.Length-1)
                sb.Append(';');

        }
        return sb.ToString();
    }


    public void save(TextWriter outFile) {
	outFile.Write("@comment{");
    outFile.Write(Globals.ENTRYTYPE_FLAG);
    outFile.Write(getName());
    outFile.Write(": req[");
    outFile.Write(getRequiredFieldsString());
	/*StringBuilder sb = new StringBuilder();
	for (int i=0; i<req.Length; i++) {
	    sb.Append(req[i]);
	    if (i<req.Length-1)
		sb.Append(";");
	}
	out.Write(sb.ToString());*/
    outFile.Write("] opt[");
	StringBuilder sb = new StringBuilder();
	for (int i=0; i<opt.Length; i++) {
	    sb.Append(opt[i]);
	    if (i<opt.Length-1)
		sb.Append(';');
	}
    outFile.Write(sb.ToString());
    outFile.Write("]}" + Environment.NewLine);
    }

    public static CustomEntryType parseEntryType(string comment) { 
	try {
	    //if ((comment.Length < 9+GUIGlobals.ENTRYTYPE_FLAG.Length)
	    //	|| comment
	    //System.out.println(">"+comment+"<");
	    string rest;
	    rest = comment.Substring(Globals.ENTRYTYPE_FLAG.Length);
	    int nPos = rest.IndexOf(':');
	    string name = rest.Substring(0, nPos);
	    rest = rest.Substring(nPos+2);

	    int rPos = rest.IndexOf(']');
	    if (rPos < 4)
		throw new IndexOutOfRangeException();
	    string reqFields = rest.Substring(4, rPos);
	    //System.out.println(name+"\nr '"+reqFields+"'");
	    int oPos = rest.IndexOf(']', rPos+1);
	    string optFields = rest.Substring(rPos+6, oPos);
	    //System.out.println("o '"+optFields+"'");
	    return new CustomEntryType(name, reqFields, optFields);
	} catch (IndexOutOfRangeException ex) {
	    return null;
	}

    }
}
}
