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
namespace net.sf.jabref {

/**
 * Static variables for graphics files and keyboard shortcuts.
 */
public class GUIGlobals {

	// Frame titles.
	public static string
	frameTitle = "JabRef",
	version = Globals.VERSION,
	stringsTitle = "Strings for database",
	//untitledStringsTitle = stringsTitle + Globals.lang("untitled"),
	untitledTitle = "untitled",
	helpTitle = "JabRef help",
	TYPE_HEADER = "entrytype",
	NUMBER_COL = "#",
	encPrefix = "Encoding: ", // Part of the signature in written bib files.
	linuxDefaultLookAndFeel = "com.jgoodies.looks.plastic.Plastic3DLookAndFeel",

    //linuxDefaultLookAndFeel = "com.sun.java.swing.plaf.gtk.GTKLookAndFeel",
    //linuxDefaultLookAndFeel = "org.jvnet.substance.skin.SubstanceCremeCoffeeLookAndFeel",
    //linuxDefaultLookAndFeel = "org.jvnet.substance.skin.SubstanceNebulaLookAndFeel",
    //linuxDefaultLookAndFeel = "org.jvnet.substance.skin.SubstanceBusinessLookAndFeel",
    windowsDefaultLookAndFeel = "com.jgoodies.looks.windows.WindowsLookAndFeel";
    //windowsDefaultLookAndFeel = "com.sun.java.swing.plaf.windows.WindowsLookAndFeel";

	// Signature written at the top of the .bib file.
	public static readonly string SIGNATURE =
		"This file was created with JabRef";

	// Divider size for BaseFrame split pane. 0 means non-resizable.
	public static readonly int
	SPLIT_PANE_DIVIDER_SIZE = 4,
	SPLIT_PANE_DIVIDER_LOCATION = 145 + 15, // + 15 for possible scrollbar.
	TABLE_ROW_PADDING = 4,
	KEYBIND_COL_0 = 200,
	KEYBIND_COL_1 = 80, // Added to the font size when determining table
	MAX_CONTENT_SELECTOR_WIDTH = 240; // The max width of the combobox for content selectors.

	// File names.
	public static string //configFile = "preferences.dat",
	backupExt = ".bak";

	// Image paths.
	public static string
	imageSize = "24",
	extension = ".gif",
	ex = imageSize + extension,
	pre = "/images/",
	helpPre = "/help/",
	fontPath = "/images/font/";

	//Help files (in HTML format):
	public static string
	baseFrameHelp = "BaseFrameHelp.html",
	entryEditorHelp = "EntryEditorHelp.html",
	stringEditorHelp = "StringEditorHelp.html",
	helpContents = "Contents.html",
	searchHelp = "SearchHelp.html",
	groupsHelp = "GroupsHelp.html",
	customEntriesHelp = "CustomEntriesHelp.html",
	contentSelectorHelp = "ContentSelectorHelp.html",
	labelPatternHelp = "LabelPatterns.html",
	ownerHelp = "OwnerHelp.html",
	timeStampHelp = "TimeStampHelp.html",
	pdfHelp = "ExternalFiles.html",
	exportCustomizationHelp = "CustomExports.html",
	importCustomizationHelp = "CustomImports.html",
	medlineHelp = "MedlineHelp.html",
	citeSeerHelp = "CiteSeerHelp.html",
	generalFieldsHelp = "GeneralFields.html",
	aboutPage = "About.html",
	shortPlainImport="ShortPlainImport.html",
	importInspectionHelp = "ImportInspectionDialog.html",
	shortIntegrityCheck="ShortIntegrityCheck.html",
	remoteHelp = "RemoteHelp.html",
	journalAbbrHelp = "JournalAbbreviations.html",
	regularExpressionSearchHelp = "ExternalFiles.html#RegularExpressionSearch",
	nameFormatterHelp = "CustomExports.html#NameFormatter",
	previewHelp = "PreviewHelp.html",
    pluginHelp = "Plugin.html",
    autosaveHelp = "Autosave.html";

	public static string META_FLAG = "jabref-meta: ";
	public static string META_FLAG_OLD = "bibkeeper-meta: ";
	public static string ENTRYTYPE_FLAG = "jabref-entrytype: ";

	// some fieldname constants
	public static readonly double
	DEFAULT_FIELD_WEIGHT = 1,
	MAX_FIELD_WEIGHT = 2;

    // constants for editor types:
    public static readonly int
        STANDARD_EDITOR=1,
        FILE_LIST_EDITOR=2;

    public static readonly int MAX_BACK_HISTORY_SIZE = 10; // The maximum number of "Back" operations stored.

    public static readonly string FILE_FIELD = "file";

    public static readonly double
	SMALL_W = 0.30,
	MEDIUM_W = 0.5,
	LARGE_W = 1.5 ;

	public static readonly double PE_HEIGHT = 2;

//	Size constants for EntryTypeForm; small, medium and large.
	public static int[] FORM_WIDTH = new int[] { 500, 650, 820};
	public static int[] FORM_HEIGHT = new int[] { 90, 110, 130};

//	Constants controlling formatted bibtex output.
	public static readonly int
	INDENT = 4,
	LINE_LENGTH = 65; // Maximum

	public static int DEFAULT_FIELD_LENGTH = 100,
	NUMBER_COL_LENGTH = 32,
	WIDTH_ICON_COL = 19;

	// Column widths for export customization dialog table:
	public static readonly int
	EXPORT_DIALOG_COL_0_WIDTH = 50,
	EXPORT_DIALOG_COL_1_WIDTH = 200,
	EXPORT_DIALOG_COL_2_WIDTH = 30;

	// Column widths for import customization dialog table:
	public static readonly int
	IMPORT_DIALOG_COL_0_WIDTH = 200,
	IMPORT_DIALOG_COL_1_WIDTH = 80,
	IMPORT_DIALOG_COL_2_WIDTH = 200,
	IMPORT_DIALOG_COL_3_WIDTH = 200;

	public static readonly Dictionary<string, string> LANGUAGES;

	static GUIGlobals() {
		LANGUAGES = new Dictionary<string, string>();

		// LANGUAGES Contains mappings for supported languages.
		LANGUAGES.Add("English", "en");
		LANGUAGES.Add("Dansk", "da");
		LANGUAGES.Add("Deutsch", "de");
		LANGUAGES.Add("Fran\u00E7ais", "fr");
		LANGUAGES.Add("Italiano", "it");
		LANGUAGES.Add("Japanese", "ja");
		LANGUAGES.Add("Nederlands", "nl");
		LANGUAGES.Add("Norsk", "no");
		//LANGUAGES.Add("Español", "es"); // Not complete
		//LANGUAGES.Add("Polski", "pl");
		LANGUAGES.Add("Turkish", "tr");
		LANGUAGES.Add("Simplified Chinese", "zh");
		LANGUAGES.Add("Vietnamese", "vi");
		LANGUAGES.Add("Bahasa Indonesia", "in");
        LANGUAGES.Add("Brazilian Portugese", "pt_BR");
	}
}
}
