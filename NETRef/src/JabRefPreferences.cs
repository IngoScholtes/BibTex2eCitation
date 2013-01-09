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
using System.Diagnostics;
using System.IO;
using System.Text;
namespace net.sf.jabref {
    
public class JabRefPreferences {
    public string WRAPPED_USERNAME, MARKING_WITH_NUMBER_PATTERN;

    // TODO: Preferences prefs;
    Dictionary<string, object> prefs;
    public Dictionary<string, object> defaults = new Dictionary<string, object>();
    public Dictionary<string, string>
        keyBinds = new Dictionary<string, string>(),
        defKeyBinds = new Dictionary<string, string>();
    private List<string> putBracesAroundCapitalsFields = new List<string>(4);
    private List<string> nonWrappableFields = new List<string>(5);
    
    // The following field is used as a global variable during the export of a database.
    // By setting this field to the path of the database's default file directory, formatters
    // that should resolve external file paths can access this field. This is an ugly hack
    // to solve the problem of formatters not having access to any context except for the
    // string to be formatted and possible formatter arguments.
    public string[] fileDirForDatabase = null;

    // Similarly to the previous variable, this is a global that can be used during
    // the export of a database if the database filename should be output. If a database
    // is tied to a file on disk, this variable is set to that file before export starts:
    public Stream databaseFile = null;

    // The following field is used as a global variable during the export of a database.
    // It is used to hold custom name formatters defined by a custom export filter.
    // It is set before the export starts:
    public Dictionary<string,string> customExportNameFormatters = null;

    // The only instance of this class:
    private static JabRefPreferences singleton = null;

    public static JabRefPreferences getInstance() {
		if (singleton == null)
			singleton = new JabRefPreferences();
		return singleton;
	}

    // The constructor is made private to enforce this as a singleton class:
    private JabRefPreferences() {        
        //prefs = Preferences.userNodeForPackage(JabRefPreferences.class);

        prefs = new Dictionary<string, object>();

        /*defaults.Add("useDefaultLookAndFeel", Boolean.TRUE);
        defaults.Add("lyxpipe", System.getProperty("user.home")+Stream.separator+".lyx/lyxpipe");
        defaults.Add("vim", "vim");
        defaults.Add("vimServer", "vim");
        defaults.Add("posX", int.valueOf(0));
        defaults.Add("posY", int.valueOf(0));
        defaults.Add("sizeX", int.valueOf(840));
        defaults.Add("sizeY", int.valueOf(680));
        defaults.Add("windowMaximised", Boolean.FALSE);
        defaults.Add("previewPanelHeight", 200);
        defaults.Add("entryEditorHeight", 400);
        defaults.Add("tableColorCodesOn", Boolean.TRUE);
        defaults.Add("namesAsIs", Boolean.FALSE);
        defaults.Add("namesFf", Boolean.FALSE);
        defaults.Add("namesLf", Boolean.FALSE);
        defaults.Add("namesNatbib", Boolean.TRUE);
        defaults.Add("abbrAuthorNames", Boolean.TRUE);
        defaults.Add("namesLastOnly", Boolean.TRUE);
        defaults.Add("language", "en");
        defaults.Add("showShort", Boolean.TRUE);
        defaults.Add("priSort", "author");
        defaults.Add("priDescending", Boolean.FALSE);
        defaults.Add("priBinary", Boolean.FALSE);
        defaults.Add("secSort", "year");
        defaults.Add("secDescending", Boolean.TRUE);
        defaults.Add("terSort", "author");
        defaults.Add("terDescending", Boolean.FALSE);
        defaults.Add("columnNames", "entrytype;author;title;year;journal;owner;timestamp;bibtexkey");
        defaults.Add("columnWidths","75;280;400;60;100;100;100;100");
        defaults.Add("xmpPrivacyFilters", "pdf;timestamp;keywords;owner;note;review");
        defaults.Add("useXmpPrivacyFilter", Boolean.FALSE);
        defaults.Add("workingDirectory", System.getProperty("user.home"));
        defaults.Add("exportWorkingDirectory", System.getProperty("user.home"));
        defaults.Add("importWorkingDirectory", System.getProperty("user.home"));
        defaults.Add("fileWorkingDirectory", System.getProperty("user.home"));
        defaults.Add("autoOpenForm", Boolean.TRUE);
        defaults.Add("entryTypeFormHeightFactor", int.valueOf(1));
        defaults.Add("entryTypeFormWidth", int.valueOf(1));
        defaults.Add("backup", Boolean.TRUE);
        defaults.Add("openLastEdited", Boolean.TRUE);
        defaults.Add("lastEdited", null);
        defaults.Add("stringsPosX", int.valueOf(0));
        defaults.Add("stringsPosY", int.valueOf(0));
        defaults.Add("stringsSizeX", int.valueOf(600));
        defaults.Add("stringsSizeY", int.valueOf(400));
        defaults.Add("defaultShowSource", Boolean.FALSE);
        defaults.Add("showSource", Boolean.TRUE);
        defaults.Add("defaultAutoSort", Boolean.FALSE);
        defaults.Add("enableSourceEditing", Boolean.TRUE);
        defaults.Add("caseSensitiveSearch", Boolean.FALSE);
        defaults.Add("searchReq", Boolean.TRUE);
        defaults.Add("searchOpt", Boolean.TRUE);
        defaults.Add("searchGen", Boolean.TRUE);
        defaults.Add("searchAll", Boolean.FALSE);
        defaults.Add("incrementS", Boolean.FALSE);
        defaults.Add("searchAutoComplete", Boolean.TRUE);
        defaults.Add("saveInStandardOrder", Boolean.TRUE);
        defaults.Add("saveInOriginalOrder", Boolean.FALSE);
        defaults.Add("exportInStandardOrder", Boolean.TRUE);
        defaults.Add("exportInOriginalOrder", Boolean.FALSE);
        defaults.Add("selectS", Boolean.FALSE);
        defaults.Add("regExpSearch", Boolean.TRUE);
        defaults.Add("highLightWords", Boolean.TRUE);
        defaults.Add("searchPanePosX", int.valueOf(0));
        defaults.Add("searchPanePosY", int.valueOf(0));
        defaults.Add("autoComplete", Boolean.TRUE);
        defaults.Add("autoCompleteFields", "author;editor;title;journal;publisher;keywords;crossref");
        defaults.Add("autoCompFF", Boolean.FALSE);
        defaults.Add("autoCompLF", Boolean.FALSE);
        defaults.Add("groupSelectorVisible", Boolean.TRUE);
        defaults.Add("groupFloatSelections", Boolean.TRUE);
        defaults.Add("groupIntersectSelections", Boolean.TRUE);
        defaults.Add("groupInvertSelections", Boolean.FALSE);
        defaults.Add("groupShowOverlapping", Boolean.FALSE);
        defaults.Add("groupSelectMatches", Boolean.FALSE);
        defaults.Add("groupsDefaultField", "keywords");
        defaults.Add("groupShowIcons", Boolean.TRUE);
        defaults.Add("groupShowDynamic", Boolean.TRUE);
        defaults.Add("groupExpandTree", Boolean.TRUE);
        defaults.Add("groupAutoShow", Boolean.TRUE);
        defaults.Add("groupAutoHide", Boolean.TRUE);
        defaults.Add("autoAssignGroup", Boolean.TRUE);
        defaults.Add("groupKeywordSeparator", ", ");
        defaults.Add("highlightGroupsMatchingAny", Boolean.FALSE);
        defaults.Add("highlightGroupsMatchingAll", Boolean.FALSE);
        defaults.Add("searchPanelVisible", Boolean.FALSE);
        defaults.Add("defaultEncoding", System.getProperty("file.encoding"));
        defaults.Add("groupsVisibleRows", int.valueOf(8));
        defaults.Add("defaultOwner", System.getProperty("user.name"));
        defaults.Add("preserveFieldFormatting", Boolean.FALSE);
        defaults.Add("memoryStickMode", Boolean.FALSE);
        defaults.Add("renameOnMoveFileToFileDir", Boolean.TRUE);

    // The general fields stuff is made obsolete by the CUSTOM_TAB_... entries.
        defaults.Add("generalFields", "crossref;keywords;file;doi;url;urldate;"+
                     "pdf;comment;owner");

        defaults.Add("useCustomIconTheme", Boolean.FALSE);
        defaults.Add("customIconThemeFile", "/home/alver/div/crystaltheme_16/Icons.properties");

        //defaults.Add("recentFiles", "/home/alver/Documents/bibk_dok/hovedbase.bib");
        defaults.Add("historySize", int.valueOf(8));
        defaults.Add("fontSize", int.valueOf(12));
        defaults.Add("overrideDefaultFonts", Boolean.FALSE);
        defaults.Add("menuFontFamily", "Times");
        defaults.Add("menuFontSize", int.valueOf(11));
        // Main table color settings:
        defaults.Add("tableBackground", "255:255:255");
        defaults.Add("tableReqFieldBackground", "230:235:255");
        defaults.Add("tableOptFieldBackground", "230:255:230");
        defaults.Add("tableText", "0:0:0");
        defaults.Add("gridColor", "210:210:210");
        defaults.Add("grayedOutBackground", "210:210:210");
        defaults.Add("grayedOutText", "40:40:40");
        defaults.Add("veryGrayedOutBackground", "180:180:180");
        defaults.Add("veryGrayedOutText", "40:40:40");
        defaults.Add("markedEntryBackground0", "255:255:180");
        defaults.Add("markedEntryBackground1", "255:220:180");
        defaults.Add("markedEntryBackground2", "255:180:160");
        defaults.Add("markedEntryBackground3", "255:120:120");
        defaults.Add("markedEntryBackground4", "255:75:75");
        defaults.Add("markedEntryBackground5", "220:255:220");
        defaults.Add("validFieldBackgroundColor", "255:255:255");
        defaults.Add("invalidFieldBackgroundColor", "255:0:0");
        defaults.Add("activeFieldEditorBackgroundColor", "220:220:255");
        defaults.Add("fieldEditorTextColor", "0:0:0");

        defaults.Add("incompleteEntryBackground", "250:175:175");

        defaults.Add("antialias", Boolean.FALSE);
        defaults.Add("ctrlClick", Boolean.FALSE);
        defaults.Add("disableOnMultipleSelection", Boolean.FALSE);
        defaults.Add("pdfColumn", Boolean.FALSE);
        defaults.Add("urlColumn", Boolean.TRUE);
        defaults.Add("fileColumn", Boolean.TRUE);
        defaults.Add("arxivColumn", Boolean.FALSE);
        defaults.Add("useOwner", Boolean.TRUE);
        defaults.Add("overwriteOwner", Boolean.FALSE);
        defaults.Add("allowTableEditing", Boolean.FALSE);
        defaults.Add("dialogWarningForDuplicateKey", Boolean.TRUE);
        defaults.Add("dialogWarningForEmptyKey", Boolean.TRUE);
        defaults.Add("displayKeyWarningDialogAtStartup", Boolean.TRUE);
        defaults.Add("avoidOverwritingKey", Boolean.FALSE);
        defaults.Add("warnBeforeOverwritingKey", Boolean.TRUE);
        defaults.Add("confirmDelete", Boolean.TRUE);
        defaults.Add("grayOutNonHits", Boolean.TRUE);
        defaults.Add("floatSearch", Boolean.TRUE);
        defaults.Add("showSearchInDialog", Boolean.FALSE);
        defaults.Add("searchAllBases", Boolean.FALSE);
        defaults.Add("defaultLabelPattern", "[auth][year]");
        defaults.Add("previewEnabled", Boolean.TRUE);
        defaults.Add("activePreview", 0);
        defaults.Add("preview0", "<font face=\"arial\">"
                     +"<b><i>\\bibtextype</i><a name=\"\\bibtexkey\">\\begin{bibtexkey} (\\bibtexkey)</a>"
                     +"\\end{bibtexkey}</b><br>__NEWLINE__"
                     +"\\begin{author} \\format[Authors(LastFirst,Initials,Semicolon,Amp),HTMLChars]{\\author}<BR>\\end{author}__NEWLINE__"
                     +"\\begin{editor} \\format[Authors(LastFirst,Initials,Semicolon,Amp),HTMLChars]{\\editor} "
                     +"<i>(\\format[IfPlural(Eds.,Ed.)]{\\editor})</i><BR>\\end{editor}__NEWLINE__"
                     +"\\begin{title} \\format[HTMLChars]{\\title} \\end{title}<BR>__NEWLINE__"
                     +"\\begin{chapter} \\format[HTMLChars]{\\chapter}<BR>\\end{chapter}__NEWLINE__"
                     +"\\begin{journal} <em>\\format[HTMLChars]{\\journal}, </em>\\end{journal}__NEWLINE__"
                     // Include the booktitle field for @inproceedings, @proceedings, etc.
                     +"\\begin{booktitle} <em>\\format[HTMLChars]{\\booktitle}, </em>\\end{booktitle}__NEWLINE__"
                     +"\\begin{school} <em>\\format[HTMLChars]{\\school}, </em>\\end{school}__NEWLINE__"
                     +"\\begin{institution} <em>\\format[HTMLChars]{\\institution}, </em>\\end{institution}__NEWLINE__"
                     +"\\begin{publisher} <em>\\format[HTMLChars]{\\publisher}, </em>\\end{publisher}__NEWLINE__"
                     +"\\begin{year}<b>\\year</b>\\end{year}\\begin{volume}<i>, \\volume</i>\\end{volume}"
                     +"\\begin{pages}, \\format[FormatPagesForHTML]{\\pages} \\end{pages}__NEWLINE__"
                     +"\\begin{abstract}<BR><BR><b>Abstract: </b> \\format[HTMLChars]{\\abstract} \\end{abstract}__NEWLINE__"
                     +"\\begin{review}<BR><BR><b>Review: </b> \\format[HTMLChars]{\\review} \\end{review}"
                     +"</dd>__NEWLINE__<p></p></font>");
        defaults.Add("preview1", "<font face=\"arial\">"
                     +"<b><i>\\bibtextype</i><a name=\"\\bibtexkey\">\\begin{bibtexkey} (\\bibtexkey)</a>"
                     +"\\end{bibtexkey}</b><br>__NEWLINE__"
                     +"\\begin{author} \\format[Authors(LastFirst,Initials,Semicolon,Amp),HTMLChars]{\\author}<BR>\\end{author}__NEWLINE__"
                     +"\\begin{editor} \\format[Authors(LastFirst,Initials,Semicolon,Amp),HTMLChars]{\\editor} "
                     +"<i>(\\format[IfPlural(Eds.,Ed.)]{\\editor})</i><BR>\\end{editor}__NEWLINE__"
                     +"\\begin{title} \\format[HTMLChars]{\\title} \\end{title}<BR>__NEWLINE__"
                     +"\\begin{chapter} \\format[HTMLChars]{\\chapter}<BR>\\end{chapter}__NEWLINE__"
                     +"\\begin{journal} <em>\\format[HTMLChars]{\\journal}, </em>\\end{journal}__NEWLINE__"
                     // Include the booktitle field for @inproceedings, @proceedings, etc.
                     +"\\begin{booktitle} <em>\\format[HTMLChars]{\\booktitle}, </em>\\end{booktitle}__NEWLINE__"
                     +"\\begin{school} <em>\\format[HTMLChars]{\\school}, </em>\\end{school}__NEWLINE__"
                     +"\\begin{institution} <em>\\format[HTMLChars]{\\institution}, </em>\\end{institution}__NEWLINE__"
                     +"\\begin{publisher} <em>\\format[HTMLChars]{\\publisher}, </em>\\end{publisher}__NEWLINE__"
                     +"\\begin{year}<b>\\year</b>\\end{year}\\begin{volume}<i>, \\volume</i>\\end{volume}"
                     +"\\begin{pages}, \\format[FormatPagesForHTML]{\\pages} \\end{pages}"
                     +"</dd>__NEWLINE__<p></p></font>");


        // TODO: Currently not possible to edit this setting:
        defaults.Add("previewPrintButton", Boolean.FALSE);
        defaults.Add("autoDoubleBraces", Boolean.FALSE);
        defaults.Add("doNotResolveStringsFor", "url");
        defaults.Add("resolveStringsAllFields", Boolean.FALSE);
        defaults.Add("putBracesAroundCapitals","");//"title;journal;booktitle;review;abstract");
        defaults.Add("nonWrappableFields", "pdf;ps;url;doi;file");
        defaults.Add("useImportInspectionDialog", Boolean.TRUE);
        defaults.Add("useImportInspectionDialogForSingle", Boolean.TRUE);
        defaults.Add("generateKeysAfterInspection", Boolean.TRUE);
        defaults.Add("markImportedEntries", Boolean.TRUE);
        defaults.Add("unmarkAllEntriesBeforeImporting", Boolean.TRUE);
        defaults.Add("warnAboutDuplicatesInInspection", Boolean.TRUE);
        defaults.Add("useTimeStamp", Boolean.TRUE);
        defaults.Add("overwriteTimeStamp", Boolean.FALSE);
        defaults.Add("timeStampFormat", "yyyy.MM.dd");
//        defaults.Add("timeStampField", "timestamp");
        defaults.Add("timeStampField", "timestamp");
        defaults.Add("generateKeysBeforeSaving", Boolean.FALSE);

        defaults.Add("useRemoteServer", Boolean.FALSE);
        defaults.Add("remoteServerPort", int.valueOf(6050));

        defaults.Add("personalJournalList", null);
        defaults.Add("externalJournalLists", null);
        defaults.Add("citeCommand", "cite"); // obsoleted by the app-specific ones
        defaults.Add("citeCommandVim", "\\cite");
        defaults.Add("citeCommandEmacs", "\\cite");
        defaults.Add("citeCommandWinEdt", "\\cite");
        defaults.Add("citeCommandLed", "\\cite");
        defaults.Add("floatMarkedEntries", Boolean.TRUE);

        defaults.Add("useNativeFileDialogOnMac", Boolean.FALSE);
        defaults.Add("filechooserDisableRename", Boolean.TRUE);

        defaults.Add("lastUsedExport", null);
        defaults.Add("sidePaneWidth", int.valueOf(-1));

        defaults.Add("importInspectionDialogWidth", int.valueOf(650));
        defaults.Add("importInspectionDialogHeight", int.valueOf(650));
        defaults.Add("searchDialogWidth", int.valueOf(650));
        defaults.Add("searchDialogHeight", int.valueOf(500));
        defaults.Add("showFileLinksUpgradeWarning", Boolean.TRUE);
        defaults.Add("autolinkExactKeyOnly", Boolean.TRUE);
        defaults.Add("numericFields", "mittnum;author");
        defaults.Add("runAutomaticFileSearch", Boolean.FALSE);
        defaults.Add("useLockFiles", Boolean.TRUE);
        defaults.Add("autoSave", Boolean.TRUE);
        defaults.Add("autoSaveInterval", 5);
        defaults.Add("promptBeforeUsingAutosave", Boolean.TRUE);
        defaults.Add("deletePlugins", "");
        defaults.Add("enforceLegalBibtexKey", Boolean.TRUE);
        defaults.Add("biblatexMode", Boolean.FALSE);
        defaults.Add("keyGenFirstLetterA", Boolean.TRUE);
        defaults.Add("keyGenAlwaysAddLetter", Boolean.FALSE);
        defaults.Add(JabRefPreferences.EMAIL_SUBJECT, Globals.lang("References"));
        defaults.Add(JabRefPreferences.OPEN_FOLDERS_OF_ATTACHED_FILES, Boolean.FALSE);
        defaults.Add("allowFileAutoOpenBrowse", Boolean.TRUE);
        defaults.Add("webSearchVisible", Boolean.FALSE);
        defaults.Add("selectedFetcherIndex", 0);
        defaults.Add("bibLocationAsFileDir", Boolean.TRUE);
        defaults.Add("bibLocAsPrimaryDir", Boolean.FALSE);
        defaults.Add("dbConnectServerType", "MySQL");
        defaults.Add("dbConnectHostname", "localhost");
        defaults.Add("dbConnectDatabase", "jabref");
        defaults.Add("dbConnectUsername", "root");
        //defaults.Add("lastAutodetectedImport", "");

        //defaults.Add("autoRemoveExactDuplicates", Boolean.FALSE);
        //defaults.Add("confirmAutoRemoveExactDuplicates", Boolean.TRUE);
        
        //defaults.Add("tempDir", System.getProperty("java.io.tmpdir"));
        //Util.pr(System.getProperty("java.io.tempdir"));

        //defaults.Add("keyPattern", new LabelPattern(KEY_PATTERN));
        
        restoreKeyBindings();

        //defaults.Add("oooWarning", Boolean.TRUE);
        updateSpecialFieldHandling();*/
        WRAPPED_USERNAME = "["+get("defaultOwner")+"]";
        MARKING_WITH_NUMBER_PATTERN = "\\["+get("defaultOwner")+":(\\d+)\\]";

        //string defaultExpression = "**/.*[bibtexkey].*\\\\.[extension]";
        /*defaults.Add(DEFAULT_REG_EXP_SEARCH_EXPRESSION_KEY, defaultExpression);
        defaults.Add(REG_EXP_SEARCH_EXPRESSION_KEY, defaultExpression);
        defaults.Add(USE_REG_EXP_SEARCH_KEY, Boolean.FALSE);
        defaults.Add("useIEEEAbrv", Boolean.TRUE);*/
    }
    
    public static readonly string DEFAULT_REG_EXP_SEARCH_EXPRESSION_KEY = "defaultRegExpSearchExpression";
    public static readonly string REG_EXP_SEARCH_EXPRESSION_KEY = "regExpSearchExpression";
    public static readonly string USE_REG_EXP_SEARCH_KEY = "useRegExpSearch";

	public static readonly string EMAIL_SUBJECT = "emailSubject";
	public static readonly string OPEN_FOLDERS_OF_ATTACHED_FILES = "openFoldersOfAttachedFiles";


	public bool putBracesAroundCapitals(string fieldName) {
        return putBracesAroundCapitalsFields.Contains(fieldName);
    }

    public void updateSpecialFieldHandling() {
        putBracesAroundCapitalsFields.Clear();
        string fieldString = get("putBracesAroundCapitals");
        if (fieldString.Length > 0) {
            string[] fields = fieldString.Split(';');
            for (int i=0; i<fields.Length; i++)
                putBracesAroundCapitalsFields.Add(fields[i].Trim());
        }
        nonWrappableFields.Clear();
        fieldString = get("nonWrappableFields");
        if (fieldString.Length > 0) {
            string[] fields = fieldString.Split(';');
            for (int i=0; i<fields.Length; i++)
                nonWrappableFields.Add(fields[i].Trim());
        }

    }

    /**
     * Check whether a key is set (differently from null).
     * @param key The key to check.
     * @return true if the key is set, false otherwise.
     */
    public bool hasKey(string key) {
        return prefs.ContainsKey(key) && prefs[key] != null;
    }

    public string get(string key) {
        return (string)(hasKey(key) ? prefs[key] : defaults.ContainsKey(key) ? defaults[key] : string.Empty);
    }

    public string get(string key, string def)
    {
        return (string)(hasKey(key) ? prefs[key] : def);
    }

    public bool getBoolean(string key)
    {
        //Debug.WriteLine("getBoolean " + key);
        return (bool)(hasKey(key) ? prefs[key] : defaults.ContainsKey(key) ? defaults[key] : false);
    }
    
    public double getDouble(string key) {
        //Debug.WriteLine("getDouble " + key);
        return (double)(hasKey(key) ? prefs[key] : defaults.ContainsKey(key) ? defaults[key] : 0.0);
    }

    public int getInt(string key) {
        return (int)(hasKey(key) ? prefs[key] : defaults.ContainsKey(key) ? defaults[key] : 0);
    }

    public byte[] getByteArray(string key) {
        return (byte[])(hasKey(key) ? prefs[key] : defaults.ContainsKey(key) ? defaults[key] : new byte[0]);
    }
        
    public void put(string key, object value) {
        prefs.Add(key, value);
    }

    public void remove(string key) {
        prefs.Remove(key);
    }

    /**
     * Puts a string array into the Preferences, by linking its elements
     * with ';' into a single string. Escape characters make the process
     * transparent even if strings contain ';'.
     */
    public void putStringArray(string key, string[] value) {
        if (value == null) {
            remove(key);
            return;
        }

        if (value.Length > 0) {
            StringBuilder linked = new StringBuilder();
            for (int i=0; i<value.Length-1; i++) {
                linked.Append(makeEscape(value[i]));
                linked.Append(';');
            }
            linked.Append(makeEscape(value[value.Length-1]));
            put(key, linked.ToString());
        } else {
            put(key, "");
        }
    }

    /**
     * Returns a string[] containing the chosen columns.
     */
    public string[] getStringArray(string key) {
        string names = get(key);
        if (names == null)
            return null;

        var rd = new StringReader(names);
        List<string> arr = new List<string>();
        string rs;
        try {
            while ((rs = getNextUnit(rd)) != null) {
                arr.Add(rs);
            }
        } catch (IOException ex) {}
        string[] res = new string[arr.Count];
        for (int i=0; i<res.Length; i++)
            res[i] = arr[i];

        return res;
    }
    
    /**
     * Set the default value for a key. This is useful for plugins that need to
     * add default values for the prefs keys they use.
     * @param key The preferences key.
     * @param value The default value.
     */
    public void putDefaultValue(string key, object value) {
        defaults.Add(key, value);
    }

    /**
     * Returns the Dictionary containing all key bindings.
     */
    public Dictionary<string, string> getKeyBindings() {
        return keyBinds;
    }

    /**
     * Returns the Dictionary containing default key bindings.
     */
    public Dictionary<string, string> getDefaultKeys() {
        return defKeyBinds;
    }


    private string getNextUnit(TextReader data) {
        int c;
        bool escape = false, done = false;
        StringBuilder res = new StringBuilder();
        while (!done && ((c = data.Read()) != -1)) {
            if (c == '\\') {
                if (!escape)
                    escape = true;
                else {
                    escape = false;
                    res.Append('\\');
                }
            } else {
                if (c == ';') {
                    if (!escape)
                        done = true;
                    else
                        res.Append(';');
                } else {
                    res.Append((char)c);
                }
                escape = false;
            }
        }
        if (res.Length > 0)
            return res.ToString();
        else
            return null;
    }

    private string makeEscape(string s) {
        StringBuilder sb = new StringBuilder();
        int c;
        for (int i=0; i<s.Length; i++) {
            c = s[i];
            if ((c == '\\') || (c == ';'))
                sb.Append('\\');
            sb.Append((char)c);
        }
        return sb.ToString();
    }
    
    /**
     * Removes all entries keyed by prefix+number, where number
     * is equal to or higher than the given number.
     * @param number or higher.
     */
    public void purgeSeries(string prefix, int number) {
        while (get(prefix+number) != null) {
            remove(prefix+number);
            number++;
        }
    }

      /**
       * Imports Preferences from an XML file.
       *
       * @param filename string Stream to import from
       */
      /*public void importPreferences(string filename) {
        Stream f = new Stream(filename);
        InputStream is = new FileInputStream(f);
        try {
          Preferences.importPreferences(is);
        } catch (InvalidPreferencesFormatException ex) {
          throw new IOException(ex.Message);
        }
      }*/

    /**
     * Determines whether the given field should be written without any sort of wrapping.
     * @param fieldName The field name.
     * @return true if the field should not be wrapped.
     */
    public bool isNonWrappableField(string fieldName) {
        return nonWrappableFields.Contains(fieldName);
    }
}
}
