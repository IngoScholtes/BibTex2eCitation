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
using System.IO;
namespace net.sf.jabref {
    
/**
 * This class is used to represent an unknown entry type, e.g. encountered
 * during bibtex parsing. The only known information is the type name.
 * This is useful if the bibtex file Contains type definitions that are used
 * in the file - because the entries will be parsed before the type definitions
 * are found. In the meantime, the entries will be assigned an 
 * UnknownEntryType giving the name.
 */
public class UnknownEntryType : BibtexEntryType {

    private string name;
    private string[] fields = new string[0];

    public UnknownEntryType(string name_) {
	    name = name_;
    }

    public override string getName()
    {
	    return name;
    }

    public override string[] getOptionalFields()
    {
	    return fields;
    }
    public override string[] getRequiredFields()
    {
	    return fields;
    }


    public override string describeRequiredFields()
    {
	    return "unknown";
    }

    public string describeOptionalFields()
    {
	    return "unknown";
    }

    public override bool hasAllRequiredFields(BibtexEntry entry, BibtexDatabase database)
    {
	    return true;
    }

    public void save(StreamWriter outFile) {
    }
}
}
