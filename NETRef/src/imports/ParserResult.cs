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
namespace net.sf.jabref.imports {


public class ParserResult {
    
    private List<string> _warnings = new List<string>();
    private List<string> duplicateKeys = new List<string>();

    public ParserResult(BibtexDatabase database, Dictionary<string, string> metaData, Dictionary<string, BibtexEntryType> entryTypes) {
		Database = database;
		MetaData = metaData;
		EntryTypes = entryTypes;
    }

    /**
     * Find which version of JabRef, if any, produced this bib file.
     * @return The version number string, or null if no JabRef signature could be read.
     */
    public string JabrefVersion { get; set; }

    public int JabrefMajorVersion { get; set; }

    public int JabrefMinorVersion { get; set; }

    public int JabrefMinor2Version { get; set; }

    public BibtexDatabase Database { get; private set; }

    public Dictionary<string, string> MetaData { get; private set; }

    public Dictionary<string, BibtexEntryType> EntryTypes { get; private set; }

    public Stream File { get; set; }

    public string Encoding { get; set; }

    /**
     * Add a parser warning.
     *
     * @param s string Warning text. Must be pretranslated. Only added if there isn't already a dupe.
     */
    public void AddWarning(string s) {
        if (!_warnings.Contains(s))
            _warnings.Add(s);
    }

    public bool HasWarnings() {
        return (_warnings.Count > 0);
    }

    public string[] Warnings { get { return _warnings.ToArray(); } }

    /**
     * Add a key to the list of duplicated BibTeX keys found in the database.
     * @param key The duplicated key
     */
    public void AddDuplicateKey(string key) {
        if (!duplicateKeys.Contains(key))
            duplicateKeys.Add(key);
    }

    /**
     * Query whether any duplicated BibTeX keys have been found in the database.
     * @return true if there is at least one duplicate key.
     */
    public bool HasDuplicateKeys() {
        return duplicateKeys.Count > 0;
    }

    /**
     * Get all duplicated keys found in the database.
     * @return An array containing the duplicated keys.
     */
    public string[] DuplicateKeys { get { return duplicateKeys.ToArray(); } }


    public bool PostponedAutosaveFound { get; set; }

    public bool Invalid { get; set; }

    public string ErrorMessage { get; set; }
}
}
