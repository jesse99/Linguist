using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Linguist
{
	// MEF magic used to register our styles with studio.
	internal static class Definitions
	{
		[Export(typeof(ClassificationTypeDefinition))]
		[Name("Linguist")]
		internal static ClassificationTypeDefinition BaseDefinition = null;

		internal abstract class BaseFormat : ClassificationFormatDefinition
		{
			protected BaseFormat(string name)
			{
				DisplayName = name;

				Style style = Styles.FindStyle(name);
				if (style != null)
				{
					if (style.FontName != null)
						this.FontTypeface = new Typeface(style.FontName);

					if (style.Bold)
						IsBold = true;
					if (style.Italic)
						IsItalic = true;

					if (style.BackColor.HasValue)
						BackgroundColor = style.BackColor.Value;
					if (style.ForeColor.HasValue)
						ForegroundColor = style.ForeColor.Value;

					if (style.PointSize > 0.0)
						FontRenderingSize = style.PointSize;
				}
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.attribute")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition AttributeDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.attribute")]
		[Name("Linguist.attribute")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class AttributeFormat : BaseFormat
		{
			public AttributeFormat()
				: base("attribute")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.command")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition CommandDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.command")]
		[Name("Linguist.command")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class CommandFormat : BaseFormat
		{
			public CommandFormat()
				: base("command")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.command2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Command2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.command2")]
		[Name("Linguist.command2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Command2Format : BaseFormat
		{
			public Command2Format() 
				: base("command2")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.comment2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Comment2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.comment2")]
		[Name("Linguist.comment2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Comment2Format : BaseFormat
		{
			public Comment2Format()
				: base("comment2")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.comment")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition CommentDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.comment")]
		[Name("Linguist.comment")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class CommentFormat : BaseFormat
		{
			public CommentFormat()
				: base("comment")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.default")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition DefaultDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.default")]
		[Name("Linguist.default")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class DefaultFormat : BaseFormat
		{
			public DefaultFormat()
				: base("default")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.diffloc")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition DiffLocDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.diffloc")]
		[Name("Linguist.diffloc")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class DiffLocFormat : BaseFormat
		{
			public DiffLocFormat()
				: base("diffloc")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.emphasis")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition EmphasisDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.emphasis")]
		[Name("Linguist.emphasis")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class EmphasisFormat : BaseFormat
		{
			public EmphasisFormat()
				: base("emphasis")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header0")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header0Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header0")]
		[Name("Linguist.header0")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header0Format : BaseFormat
		{
			public Header0Format()
				: base("header0")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header1")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header1Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header1")]
		[Name("Linguist.header1")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header1Format : BaseFormat
		{
			public Header1Format()
				: base("header1")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header2")]
		[Name("Linguist.header2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header2Format : BaseFormat
		{
			public Header2Format()
				: base("header2")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header3")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header3Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header3")]
		[Name("Linguist.header3")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header3Format : BaseFormat
		{
			public Header3Format()
				: base("header3")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header4")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header4Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header4")]
		[Name("Linguist.header4")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header4Format : BaseFormat
		{
			public Header4Format()
				: base("header4")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.header5")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Header5Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.header5")]
		[Name("Linguist.header5")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Header5Format : BaseFormat
		{
			public Header5Format()
				: base("header5")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.italic")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition ItalicDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.italic")]
		[Name("Linguist.italic")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class ItalicFormat : BaseFormat
		{
			public ItalicFormat()
				: base("italic")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.keyword")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition KeywordDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.keyword")]
		[Name("Linguist.keyword")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class KeywordFormat : BaseFormat
		{
			public KeywordFormat()
				: base("keyword")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.keyword2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Keyword2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.keyword2")]
		[Name("Linguist.keyword2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Keyword2Format : BaseFormat
		{
			public Keyword2Format()
				: base("keyword2")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.markup")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition MarkupDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.markup")]
		[Name("Linguist.markup")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class MarkupFormat : BaseFormat
		{
			public MarkupFormat()
				: base("markup")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.method")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition MehodDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.method")]
		[Name("Linguist.method")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class MethodFormat : BaseFormat
		{
			public MethodFormat()
				: base("method")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.monospace")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition MonospaceDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.monospace")]
		[Name("Linguist.monospace")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class MonospaceFormat : BaseFormat
		{
			public MonospaceFormat()
				: base("monospace")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.newtext")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition NewTextDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.newtext")]
		[Name("Linguist.newtext")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class NewTextFormat : BaseFormat
		{
			public NewTextFormat()
				: base("newtext")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.number")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition NumberDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.number")]
		[Name("Linguist.number")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class NumberFormat : BaseFormat
		{
			public NumberFormat()
				: base("number")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.oldtext")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition OldTextDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.oldtext")]
		[Name("Linguist.oldtext")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class OldTextFormat : BaseFormat
		{
			public OldTextFormat()
				: base("oldtext")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.preprocess")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition PreprocessDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.preprocess")]
		[Name("Linguist.preprocess")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class PreprocessFormat : BaseFormat
		{
			public PreprocessFormat()
				: base("preprocess")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.region")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition RegionDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.region")]
		[Name("Linguist.region")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class RegionFormat : BaseFormat
		{
			public RegionFormat()
				: base("region")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.shellvariable")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition ShellVariableDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.shellvariable")]
		[Name("Linguist.shellvariable")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class ShellVariableFormat : BaseFormat
		{
			public ShellVariableFormat()
				: base("shellvariable")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.string")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition StringDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.string")]
		[Name("Linguist.string")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class StringFormat : BaseFormat
		{
			public StringFormat()
				: base("string")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.target")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition TargetDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.target")]
		[Name("Linguist.target")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class TargetFormat : BaseFormat
		{
			public TargetFormat()
				: base("target")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.type")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition TypeDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.type")]
		[Name("Linguist.type")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class TypeFormat : BaseFormat
		{
			public TypeFormat()
				: base("type")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.type2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Type2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.type2")]
		[Name("Linguist.type2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Type2Format : BaseFormat
		{
			public Type2Format()
				: base("type2")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.prerequisite")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition PrerequisiteDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.prerequisite")]
		[Name("Linguist.prerequisite")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class PrerequisiteFormat : BaseFormat
		{
			public PrerequisiteFormat()

				: base("prerequisite")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.operator")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition OperatorDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.operator")]
		[Name("Linguist.operator")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class OperatorFormat : BaseFormat
		{
			public OperatorFormat()
				: base("operator")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.variable")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition VariableDefinition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.variable")]
		[Name("Linguist.variable")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class VariableFormat : BaseFormat
		{
			public VariableFormat()
				: base("variable")
			{
			}
		}

		// ----------------------------------------------------------------
		[Export]
		[Name("Linguist.variable2")]
		[BaseDefinition("Linguist")]
		internal static ClassificationTypeDefinition Variable2Definition = null;

		[Export(typeof(EditorFormatDefinition))]
		[ClassificationType(ClassificationTypeNames = "Linguist.variable2")]
		[Name("Linguist.variable2")]
		[UserVisible(true)]
		[Order(After = Priority.High)]
		internal sealed class Variable2Format : BaseFormat
		{
			public Variable2Format()
				: base("variable2")
			{
			}
		}
	}
}
