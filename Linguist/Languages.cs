﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Classification;

namespace Linguist
{
	// Loads *.lang file and creates a table mapping language globs to Languages.
	internal static class Languages
	{
		static public void Init(IClassificationTypeRegistryService registry)
		{
			if (ms_elements.Count == 0)
			{
				ms_elements.Add("Attribute", registry.GetClassificationType("Linguist.attribute"));
				ms_elements.Add("Command", registry.GetClassificationType("Linguist.command"));
				ms_elements.Add("Command2", registry.GetClassificationType("Linguist.command2"));
				ms_elements.Add("Default", registry.GetClassificationType("Linguist.default"));
				ms_elements.Add("Comment", registry.GetClassificationType("Linguist.comment"));
				ms_elements.Add("Comment2", registry.GetClassificationType("Linguist.comment2"));
				ms_elements.Add("DiffLoc", registry.GetClassificationType("Linguist.diffloc"));
				ms_elements.Add("Emphasis", registry.GetClassificationType("Linguist.emphasis"));
				ms_elements.Add("Header0", registry.GetClassificationType("Linguist.header0"));
				ms_elements.Add("Header1", registry.GetClassificationType("Linguist.header1"));
				ms_elements.Add("Header2", registry.GetClassificationType("Linguist.header2"));
				ms_elements.Add("Header3", registry.GetClassificationType("Linguist.header3"));
				ms_elements.Add("Header4", registry.GetClassificationType("Linguist.header4"));
				ms_elements.Add("Header5", registry.GetClassificationType("Linguist.header5"));
				ms_elements.Add("Italic", registry.GetClassificationType("Linguist.italic"));
				ms_elements.Add("Keyword", registry.GetClassificationType("Linguist.keyword"));
				ms_elements.Add("Keyword2", registry.GetClassificationType("Linguist.keyword2"));
				ms_elements.Add("Markup", registry.GetClassificationType("Linguist.markup"));
				ms_elements.Add("Method", registry.GetClassificationType("Linguist.method"));
				ms_elements.Add("Monospace", registry.GetClassificationType("Linguist.monospace"));
				ms_elements.Add("NewText", registry.GetClassificationType("Linguist.newtext"));
				ms_elements.Add("Number", registry.GetClassificationType("Linguist.number"));
				ms_elements.Add("OldText", registry.GetClassificationType("Linguist.oldtext"));
				ms_elements.Add("Preprocess", registry.GetClassificationType("Linguist.preprocess"));
				ms_elements.Add("Region", registry.GetClassificationType("Linguist.region"));
				ms_elements.Add("ShellVariable", registry.GetClassificationType("Linguist.shellvariable"));
				ms_elements.Add("String", registry.GetClassificationType("Linguist.string"));
				ms_elements.Add("Target", registry.GetClassificationType("Linguist.target"));
				ms_elements.Add("Type", registry.GetClassificationType("Linguist.type"));
				ms_elements.Add("Type2", registry.GetClassificationType("Linguist.type2"));
				ms_elements.Add("Prerequisite", registry.GetClassificationType("Linguist.prerequisite"));
				ms_elements.Add("Variable", registry.GetClassificationType("Linguist.variable"));
				ms_elements.Add("Variable2", registry.GetClassificationType("Linguist.variable2"));
				ms_elements.Add("Operator", registry.GetClassificationType("Linguist.operator"));

				DoLoad(null, null);

				// This is very poorly documented, but:
				// 1) We need to use NotifyFilters.FileName in order to get the Deleted event to fire.
				// 2) Events are fired on a worker thread.
				ms_watcher = new FileSystemWatcher(Constants.CustomPath, "*.lang");
				ms_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName;
				ms_watcher.Changed += Languages.DoLoad;
				ms_watcher.Created += Languages.DoLoad;
				ms_watcher.Deleted += Languages.DoLoad;
				ms_watcher.EnableRaisingEvents = true;
			}
		}

		public static Language FindLanguage(string fileName)
		{
			lock (ms_mutex)
			{
				foreach (Language candidate in ms_languages.Values)
				{
					if (candidate.Glob.IsMatch(fileName))
						return candidate;
				}
			}

			return null;
		}

		#region Private Methods
		private static void DoLoad(object sender, FileSystemEventArgs e)
		{
			lock (ms_mutex)
			{
				ms_languages.Clear();

				var files = new List<string>();
				DoLoadLanguages(files, Constants.CustomPath);
				DoLoadLanguages(files, Constants.StandardPath);
			}
		}

		private static void DoLoadLanguages(List<string> files, string dir)
		{
			foreach (string path in Directory.GetFiles(dir, "*.lang", SearchOption.TopDirectoryOnly))
			{
				try
				{
					string file = Path.GetFileName(path);
					if (!files.Contains(file))
					{
						Log.WriteLine("Loading {0}", path);
						Log.Indent();
						files.Add(file);
						DoLoadLanguage(path);
					}
					else
					{
						Log.WriteLine("Ignoring {0} (there is a custom version)", path);
					}
				}
				catch (Exception e)
				{
					Log.WriteLine(e.Message);
				}
				finally
				{
					Log.Unndent();
				}
			}
		}

		private static void DoLoadLanguage(string path)
		{
			string contents = File.ReadAllText(path);
			Field[] fields = FieldParser.Parse(contents);

			string name = DoGetField(fields, "Language");
			string[] globs = DoGetField(fields, "Globs").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var elements = DoGetElements(fields);

			foreach (string glob in globs)
			{
				Language lang;
				if (!ms_languages.TryGetValue(glob, out lang))
				{
					string pattern = string.Format("^{0}$", Regex.Escape(glob));
					pattern = pattern.Replace(@"\*", ".*").Replace(@"\?", ".");
					var re = new Regex(pattern, RegexOptions.IgnoreCase);
					ms_languages.Add(glob, new Language(name, re, elements));
				}
				else
				{
					Log.WriteLine("{0} glob was already defined in language {1},", glob, lang.Name);
				}
			}

			DoUnusedCheck(fields);
		}

		private static List<KeyValuePair<string, IClassificationType>> DoGetElements(Field[] fields)
		{
			var elements = new List<KeyValuePair<string, IClassificationType>>();

			elements.Add(new KeyValuePair<string, IClassificationType>(null, ms_elements["Default"]));

			foreach (var entry in ms_elements)
			{
				var candidates = from f in fields where f.Name == entry.Key select f;
				foreach (Field candidate in candidates)
				{
					string regex = candidate.Value;
					elements.Add(new KeyValuePair<string, IClassificationType>(regex, entry.Value));
				}
			}

			return elements;
		}


		private static string DoGetField(Field[] fields, string name)
		{
			var candidates = from f in fields where f.Name == name select f;
			int count = candidates.Count();
			if (count == 1)
				return candidates.First().Value;
			else if (count == 0)
				throw new Exception(name + " field is missing.");
			else
				throw new Exception(name + " field was defined more than once.");
		}

		private static void DoUnusedCheck(Field[] fields)
		{
			foreach (Field field in fields)
			{
				if (field.Name != "Language" && field.Name != "Globs")
					if (!ms_elements.Keys.Contains(field.Name))
						Log.WriteLine("Ignoring unused field {0}.", field.Name);
			}
		}
		#endregion

		#region Fields
		private static FileSystemWatcher ms_watcher;
		private static Dictionary<string, IClassificationType> ms_elements = new Dictionary<string, IClassificationType>();
		private static object ms_mutex = new object();
			private static Dictionary<string, Language> ms_languages = new Dictionary<string, Language>();	// glob to language
		#endregion
	}
}
