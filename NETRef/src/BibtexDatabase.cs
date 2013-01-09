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
Modified for use in JabRef

*/


// created by : ?
//
// modified : r.nagel 23.08.2004
//                - insert getEntryByKey() methode needed by AuxSubGenerator

using net.sf.jabref.export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
namespace net.sf.jabref {

public class BibtexDatabase {
    private readonly object _lock = new object();

    Dictionary<string, BibtexEntry> _entries = new Dictionary<string, BibtexEntry>();

	string _preamble = null;

	Dictionary<string, BibtexString> _strings = new Dictionary<string, BibtexString>();

	List<string> _strings_ = new List<string>();

    private bool followCrossrefs = true;

	/**
	 * use a map instead of a set since i need to know how many of each key is
	 * inthere
	 */
	private Dictionary<string, int> allKeys = new Dictionary<string, int>();

    /**
     * Returns the number of entries.
     */
    public int getEntryCount()
    {
        lock (_lock) {
            return _entries.Count;
        }
    }

    /**
     * Returns a Set containing the keys to all entries.
     * Use getKeySet().iterator() to iterate over all entries.
     */
    public ICollection<string> getKeySet()
    {
        lock (_lock) {
            return _entries.Keys;
        }
    }

    /**
     * Just temporary, for testing purposes....
     * @return
     */
    public Dictionary<string, BibtexEntry> getEntryMap() { return _entries; }

    /**
     * Returns the entry with the given ID (-> entry_type + hashcode).
     */
    public BibtexEntry getEntryById(string id)
    {
        lock (_lock) {
            return _entries.ContainsKey(id) ? _entries[id] : null;
        }
    }

    public ICollection<BibtexEntry> getEntries() {
        lock (_lock) {
            return _entries.Values;
        }
    }

    /**
     * Returns the entry with the given bibtex key.
     */
    public BibtexEntry getEntryByKey(string key)
    {
      lock (_lock) {
          BibtexEntry back = null ;

          int keyHash = key.GetHashCode() ; // key hash for better performance

          var keySet = _entries.Keys;
          if (keySet != null)
          {
              var it = keySet.GetEnumerator();
              while(it.MoveNext())
              {
                string entrieID = it.Current ;
                BibtexEntry entry = getEntryById(entrieID) ;
                if ((entry != null) && (entry.getCiteKey() != null))
                {
                  string citeKey = entry.getCiteKey() ;
                  if (citeKey != null)
                  {
                    if (keyHash == citeKey.GetHashCode() )
                    {
                      back = entry ;
                    }
                  }
                }
              }
          }
          return back ;
      }
    }

    public  BibtexEntry[] getEntriesByKey(string key) {
        lock (_lock) {
            List<BibtexEntry> entries = new List<BibtexEntry>();
        
            foreach (var k in _entries.Keys){
                var entry = _entries[k];
                if (key.Equals(entry.getCiteKey()))
                    entries.Add(entry);
            }
        
            return entries.ToArray();
        }
    }

    /**
     * Inserts the entry, given that its ID is not already in use.
     * use Util.createId(...) to make up a unique ID for an entry.
     */
    public bool insertEntry(BibtexEntry entry)
    {
        lock (_lock) {
            string id = entry.getId();
            if (getEntryById(id) != null)
            {
              throw new KeyCollisionException(
                    "ID is already in use, please choose another");
            }
            _entries.Add(id, entry);
        
            return checkForDuplicateKeyAndAdd(null, entry.getCiteKey(), false);
        }
    }
    
    public bool setCiteKeyForEntry(string id, string key) {
        if (!_entries.ContainsKey(id)) return false; // Entry doesn't exist!
        BibtexEntry entry = getEntryById(id);
        String oldKey = entry.getCiteKey();
        if (key != null)
          entry.setField(BibtexFields.KEY_FIELD, key);
        else
          entry.clearField(BibtexFields.KEY_FIELD);
        return checkForDuplicateKeyAndAdd(oldKey, entry.getCiteKey(), false);
    }

    /**
     * Sets the database's preamble.
     */
    public void setPreamble(string preamble)
    {
        lock (_lock) {
            _preamble = preamble;
        }
    }

    /**
     * Returns the database's preamble.
     */
    public string getPreamble()
    {
        lock (_lock) {
            return _preamble;
        }
    }

    /**
     * Inserts a Bibtex string at the given index.
     */
    public void addString(BibtexString str)
    {
        lock (_lock) {
            if (hasStringLabel(str.getName())){
    	        throw new KeyCollisionException("A string with this label already exists,");
            }

            if (_strings.ContainsKey(str.getId()))
                throw new KeyCollisionException("Duplicate BibtexString id.");

            _strings.Add(str.getId(), str);
        }
    }

    /**
     * Removes the string at the given index.
     */
    public void removeString(string id) {
        lock (_lock) {
            _strings.Remove(id);
        }
    }

    /**
     * Returns a Set of keys to all BibtexString objects in the database.
     * These are in no sorted order.
     */
    public IEnumerable<string> getStringKeySet() {
        lock (_lock) {
            return _strings.Keys;
        }
    }
    
    /**
     * Returns a Collection of all BibtexString objects in the database.
     * These are in no particular order.
     */
    public ICollection<BibtexString> getStringValues() {
        lock (_lock) {
            return _strings.Values;
        }
    }

    /**
     * Returns the string at the given index.
     */
    public BibtexString getString(string o) {
        lock (_lock) {
            return _strings[o];
        }
    }

    /**
     * Returns the number of strings.
     */
    public int getStringCount() {
        lock (_lock) {
            return _strings.Count;
        }
    }

    /**
     * Returns true if a string with the given label already exists.
     */
    public bool hasStringLabel(string label) {
        lock (_lock) {
    	    foreach (BibtexString value in _strings.Values){
                 if (value.getName().Equals(label))
                    return true;
            }
            return false;
        }
    }

    /**
     * Resolves any references to strings contained in this field content,
     * if possible.
     */
    public string resolveForStrings(string content) {
    	if (content == null){
    		throw new ArgumentNullException("Content for resolveForStrings must not be null.");
    	}
        return resolveContent(content, new Dictionary<string, bool>());
    }
    
    /**
	 * Take the given collection of BibtexEntry and resolve any string
	 * references.
	 * 
	 * @param entries
	 *            A collection of BibtexEntries in which all strings of the form
	 *            #xxx# will be resolved against the hash map of string
	 *            references stored in the databasee.
	 *            
	 * @param inPlace If inPlace is true then the given BibtexEntries will be modified, if false then copies of the BibtexEntries are made before resolving the strings.
	 * 
	 * @return a list of bibtexentries, with all strings resolved. It is dependent on the value of inPlace whether copies are made or the given BibtexEntries are modified. 
	 */
    public List<BibtexEntry> resolveForStrings(ICollection<BibtexEntry> entries, bool inPlace){
    	
    	if (entries == null)
    		throw new NullReferenceException();
    	
    	List<BibtexEntry> results = new List<BibtexEntry>(entries.Count);
    	
    	foreach (BibtexEntry entry in entries){
    		results.Add(this.resolveForStrings(entry, inPlace));
    	}
    	return results;
    }
    
    /**
	 * Take the given BibtexEntry and resolve any string references.
	 * 
	 * @param entry
	 *            A BibtexEntry in which all strings of the form #xxx# will be
	 *            resolved against the hash map of string references stored in
	 *            the databasee.
	 * 
	 * @param inPlace
	 *            If inPlace is true then the given BibtexEntry will be
	 *            modified, if false then a copy is made using close made before
	 *            resolving the strings.
	 * 
	 * @return a BibtexEntry with all string references resolved. It is
	 *         dependent on the value of inPlace whether a copy is made or the
	 *         given BibtexEntries is modified.
	 */
    public BibtexEntry resolveForStrings(BibtexEntry entry, bool inPlace) {
		
    	if (!inPlace){
    		entry = (BibtexEntry)entry.clone();
    	}
    	
    	foreach (object field in entry.getAllFields()){
    		entry.setField(field.ToString(), this.resolveForStrings(entry.getField(field.ToString()).ToString()));
    	}
    	
    	return entry;
	}

	/**
    * If the label represents a string contained in this database, returns
    * that string's content. Resolves references to other strings, taking
    * care not to follow a circular reference pattern.
    * If the string is undefined, returns null.
    */
    private string resolveString(string label, Dictionary<string, bool> usedIds) {
    	foreach (BibtexString str in _strings.Values){

                //Util.pr(label+" : "+string.getName());
            if (str.getName().ToLower().Equals(label.ToLower())) {

                // First check if this string label has been resolved
                // earlier in this recursion. If so, we have a
                // circular reference, and have to stop to avoid
                // infinite recursion.
                if (usedIds.ContainsKey(str.getId())) {
                    return label;
                }
                // If not, log this string's ID now.
                usedIds.Add(str.getId(), true);

                // Ok, we found the string. Now we must make sure we
                // resolve any references to other strings in this one.
                string res = str.getContent();
                res = resolveContent(res, usedIds);

                // Finished with recursing this branch, so we remove our
                // ID again:
                usedIds.Remove(str.getId());

                return res;
            }
        }

        // If we get to this point, the string has obviously not been defined locally.
        // Check if one of the standard BibTeX month strings has been used:
        object o;
        if ((o = Globals.MONTH_STRINGS[label.ToLower()]) != null) {
            return (string)o;
        }

        return null;
    }

    private string resolveContent(string res, Dictionary<string, bool> usedIds) {
        //if (res.matches(".*#[-\\^\\:\\w]+#.*")) {
    if (Regex.Match(res, ".*#[^#]+#.*").Success) {
            StringBuilder newRes = new StringBuilder();
            int piv = 0, next = 0;
            while ((next=res.IndexOf('#', piv)) >= 0) {

                // We found the next string ref. Append the text
                // up to it.
                if (next > 0)
                    newRes.Append(res.Substring(piv, next));
                int stringEnd = res.IndexOf('#', next+1);
                if (stringEnd >= 0) {
                    // We found the boundaries of the string ref,
                    // now resolve that one.
                    string refLabel = res.Substring(next+1, stringEnd);
                    string resolved = resolveString(refLabel, usedIds);
                    
                    if (resolved == null) {
                        // Could not resolve string. Display the #
                        // characters rather than removing them:
                        newRes.Append(res.Substring(next, stringEnd+1));
                    } else
                        // The string was resolved, so we display its meaning only,
                        // stripping the # characters signifying the string label:
                        newRes.Append(resolved);
                    piv = stringEnd+1;
                } else {
                    // We didn't find the boundaries of the string ref. This
                    // makes it impossible to interpret it as a string label.
                    // So we should just Append the rest of the text and finish.
                    newRes.Append(res.Substring(next));
                    piv = res.Length;
                    break;
                }

            }
            if (piv < res.Length-1)
                newRes.Append(res.Substring(piv));
            res = newRes.ToString();
        }
        return res;
    }

    //##########################################
    //  usage:
    //  isDuplicate=checkForDuplicateKeyAndAdd( null, b.getKey() , issueDuplicateWarning);
    //############################################
        // if the newkey already exists and is not the same as oldkey it will give a warning
    // else it will add the newkey to the to set and remove the oldkey
    public bool checkForDuplicateKeyAndAdd(string oldKey, string newKey, bool issueWarning){
                // Globals.logger(" checkForDuplicateKeyAndAdd [oldKey = " + oldKey + "] [newKey = " + newKey + "]");

        bool duplicate=false;
        if(oldKey==null){// this is a new entry so don't bother removing oldKey
            duplicate= addKeyToSet( newKey);
        }else{
            if(oldKey.Equals(newKey)){// were OK because the user did not change keys
                duplicate=false;
            }else{// user changed the key

                // removed the oldkey
                // But what if more than two have the same key?
                // this means that user can add another key and would not get a warning!
                // consider this: i add a key xxx, then i add another key xxx . I get a warning. I delete the key xxx. JBM
                // removes this key from the allKey. then I add another key xxx. I don't get a warning!
                // i need a way to count the number of keys of each type
                // hashmap=>int (increment each time)

                removeKeyFromSet( oldKey);
                duplicate = addKeyToSet( newKey );
            }
        }
        return duplicate;
    }

    /**
     * Returns the number of occurences of the given key in this database.
     */
    public int getNumberOfKeyOccurences(string key) {
        object o = allKeys[key];
        if (o == null)
            return 0;
        else
            return ((int)o);

    }

    //========================================================
    // keep track of all the keys to warn if there are duplicates
    //========================================================
    private bool addKeyToSet(string key){
                bool exists=false;
                if((key == null) || key.Equals(""))
                        return false;//don't put empty key
                if(allKeys.ContainsKey(key)){
                        // warning
                        exists=true;
                        allKeys[key] = allKeys[key] + 1;// incrementInteger( allKeys.get(key)));
                }else
                        allKeys.Add( key, 1);
                return exists;
    }
    
    //========================================================
    // reduce the number of keys by 1. if this number goes to zero then remove from the set
    // note: there is a good reason why we should not use a hashset but use hashmap instead
    //========================================================
    private void removeKeyFromSet(string key){
                if((key == null) || key.Equals("")) return;
                if(allKeys.ContainsKey(key)){
                        int tI = allKeys[key]; // if(allKeys.get(key) is int)
                        if(tI==1)
                                allKeys.Remove( key);
                        else
                                allKeys.Add( key, tI - 1);//decrementInteger( tI ));
                }
    }
    
	/**
	 * Returns the text stored in the given field of the given bibtex entry
	 * which belongs to the given database.
	 * 
	 * If a database is given, this function will try to resolve any string
	 * references in the field-value.
     * Also, if a database is given, this function will try to find values for
     * unset fields in the entry linked by the "crossref" field, if any.
	 * 
	 * @param field
	 *            The field to return the value of.
	 * @param bibtex maybenull
	 *            The bibtex entry which Contains the field.
	 * @param database maybenull
	 *            The database of the bibtex entry.
	 * @return The resolved field value or null if not found.
	 */
	public static string getResolvedField(string field, BibtexEntry bibtex,
			BibtexDatabase database) {
	
		if (field.Equals("bibtextype"))
			return bibtex.getType().getName();

        object o = bibtex.getField(field);

        // If this field is not set, and the entry has a crossref, try to look up the
        // field in the referred entry: Do not do this for the bibtex key.
        if ((o == null) && (database != null) && database.followCrossrefs &&
                !field.Equals(Globals.KEY_FIELD) && (database != null)) {
            object crossRef = bibtex.getField("crossref");
            if (crossRef != null) {
                BibtexEntry referred = database.getEntryByKey((string)crossRef);
                if (referred != null) {
                    // Ok, we found the referred entry. Get the field value from that
                    // entry. If it is unset there, too, stop looking:
                    o = referred.getField(field);
                }
            }
        } 

        return getText((string)o, database);
	}

	/**
	 * Returns a text with references resolved according to an optionally given
	 * database.
	
	 * @param toResolve maybenull The text to resolve.
	 * @param database maybenull The database to use for resolving the text.
	 * @return The resolved text or the original text if either the text or the database are null
	 */
	public static string getText(string toResolve, BibtexDatabase database) {
		if (toResolve != null && database != null)
			return database.resolveForStrings(toResolve);
		
		return toResolve;
	}

    public void setFollowCrossrefs(bool followCrossrefs) {
        this.followCrossrefs = followCrossrefs;
    }

    public void saveDatabase(Stream file) {
    	FileActions.saveDatabase(this, file, false, false, "UTF8");
    }
}
}
