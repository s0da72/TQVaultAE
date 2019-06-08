using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Linq;

namespace TQLangAE
{
	public class LanguageUtility
	{

		public static string SetUILanguage(string requestedLanguage)
		{
			if (string.IsNullOrEmpty(requestedLanguage))
			{
				//auto dectect if language not set
				return (LanguageEnglishName(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName));
			}

			var myCulture = "";
			foreach (var ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
			{
				if (ci.EnglishName.Equals(requestedLanguage, StringComparison.InvariantCultureIgnoreCase))
				{
					myCulture = ci.TextInfo.CultureName;
					break;
				}
			}

			// We found something so we will use it.
			if (!string.IsNullOrEmpty(myCulture))
			{
				// Sets the culture
				Thread.CurrentThread.CurrentCulture = new CultureInfo(myCulture);

				// Sets the UI culture
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(myCulture);
			}

			// If not then we just default to the OS UI culture.
			return (LanguageEnglishName(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName));
		}

		public static string LanguageEnglishName(string iso)
		{
			var ci = new CultureInfo(iso.ToUpperInvariant(), true);
			return(ci.EnglishName);
		}

		public static IList<KeyValuePair<string,string>> CreateSupportedLanguageList(string supportedISOLanguages)
		{
			return (supportedISOLanguages.Split(',').Select(iso =>
			{
				var ci = new CultureInfo(iso.ToUpperInvariant(), true);
				return new KeyValuePair<string, string>(ci.EnglishName, ci.DisplayName);// to keep EnglishName as baseline value
			}).OrderBy(cb => cb.Value).ToList());
		}
	}
}
