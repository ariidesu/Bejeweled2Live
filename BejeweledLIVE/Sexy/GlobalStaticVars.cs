using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using BejeweledLIVE;

namespace Sexy
{
	public static class GlobalStaticVars
	{
		public static void initialize(Main main)
		{
			GlobalStaticVars.g = main.graphics;
			GlobalStaticVars.mGlobalContent = new GlobalContentManager(main);
			GlobalStaticVars.gSexyAppBase = new GameApp(main);
			GlobalStaticVars.g.Init();
			GlobalStaticVars.mGlobalContent.initialize();
			GlobalStaticVars.gSexyAppBase.Init();
		}

		public static int timeGetTime()
		{
			return DateTime.Now.Millisecond;
		}

		internal static string GetResourceDir()
		{
			return "";
		}

		internal static string CommaSeperate_(int mDispPoints)
		{
			if (mDispPoints == 0)
			{
				return "0";
			}
			string text;
			if (!GlobalStaticVars.commaSeparatedCache.TryGetValue(mDispPoints, out text))
			{
				text = string.Format("{0:#,#}", mDispPoints);
				GlobalStaticVars.commaSeparatedCache.Add(mDispPoints, text);
			}
			return text;
		}

		internal static void CommaSeperate_(int mDispPoints, StringBuilder sb)
		{
			sb.Remove(0, sb.Length);
			int i = 1;
			while (mDispPoints / i > 9)
			{
				i *= 10;
			}
			bool flag = true;
			while (i > 0)
			{
				if (!flag && (i == 100 || i == 100000))
				{
					sb.Append(GlobalStaticVars.commaSeparator);
				}
				sb.Append(GlobalStaticVars.DigitStrings[mDispPoints / i]);
				mDispPoints %= i;
				i /= 10;
				flag = false;
			}
		}

		internal static string GetDocumentsDir()
		{
			if (!GlobalStaticVars.checkedLegacyDocuments)
			{
				GlobalStaticVars.checkedLegacyDocuments = true;
				try
				{
					Directory.CreateDirectory(GlobalStaticVars.documentsDir);
					using IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForAssembly();
					if (store.DirectoryExists("docs"))
					{
						foreach (string name in store.GetFileNames("docs/*"))
						{
							string destination = Path.Combine(GlobalStaticVars.documentsDir, name);
							if (File.Exists(destination)) continue;
							using Stream source = store.OpenFile("docs/" + name, FileMode.Open, FileAccess.Read);
							using Stream target = File.Open(destination, FileMode.CreateNew, FileAccess.Write);
							source.CopyTo(target);
						}
					}
				}
				catch (IOException)
				{
				}
				catch (IsolatedStorageException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			return GlobalStaticVars.documentsDir;
		}

		private static readonly string documentsDir = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ariidesu", "BejeweledLIVE");

		private static bool checkedLegacyDocuments;

		private const string commaSeparatorFormat = "{0:#,#}";

		private const string zero = "0";

		public static GlobalContentManager mGlobalContent;

		public static GameApp gSexyAppBase;

		public static Graphics g;

		public static readonly string[] DigitStrings = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

		private static Dictionary<int, string> commaSeparatedCache = new Dictionary<int, string>(1000);

		private static readonly string commaSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

		public struct CGPoint
		{
			public float x;

			public float y;
		}

		public enum Phase
		{
			TOUCH_BEGAN,
			TOUCH_MOVED,
			TOUCH_STATIONARY,
			TOUCH_ENDED,
			TOUCH_CANCELLED
		}

		public struct Touch
		{
			public GlobalStaticVars.CGPoint location;
		}
	}
}
