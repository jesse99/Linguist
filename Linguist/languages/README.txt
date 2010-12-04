Files in the standard directory are installed when the Linguist addin starts up. Files are installed if they are
missing or older than the Linguist assembly. Files in the custom directory are under the control of the user and
override files in the standard directory.

Language are defined using a simple line-based file format consisting of settings and language elements. The
settings are:
Language: the name of the language (e.g. makefile).
Globs: space separated list of file name glob patterns associated with the language (e.g. Makefile Makefile.am *.make *.mk).

Language elements consist of an element name followed by a .NET regex used to match the element. The element name may
be: Attribute, Command, Default, Comment, Emphasis, Header1, Header2, Header3, Header4, Header5, Italic, Keyword, 
Keyword2, Markup, 
Method, Monospace, NewText, Number, OldText, Preprocess, Region, ShellVariable, String, Target, Type, or Type2. The regexen are
compiled using RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline (so dot does not match new line characters).
If parenthesis are used within a regular expression they must be non-grouping (i.e. (?: pattern)).

Fonts and colors for language elements are specified using the Styles.field files. Styles for a language element look like:
	Name: comment
	ForeColor: Red
	Italic: true
where the name is a language element name and the remaining fields may be:
FontName: the name of a font, e.g. Consolas or Verdana.
PointSize: the font size, e.g. 12.
ForeColor: an X11 color name (e.g. Red), a hex color (e.g. #FF0000), or a decimal color (e.g. 255,0,0).
BackColor: ditto
Bold: true if the element should be in bold, and false otherwise.
Italic: true if the element should be in italic, and false otherwise.

The default style is special and applies to all text within the document. Other elements override the styles listed in
the default style. A styles.field file in the custom directory overrides styles within the standard directory.

Changes to language files in the custom directory take effect while Studio is running (although you may have to scroll 
a document to see them). Changes to the styles files require restarting studio.

Problems and other information are logged to a C:\Users\<name>\AppData\Local\Linguist\Log.txt file.

jesjones@mindspring.com
