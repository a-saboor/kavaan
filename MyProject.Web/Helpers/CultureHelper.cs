﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MyProject.Web.Helpers
{
	public static class CultureHelper
	{
		// Valid cultures
		private static readonly List<string> _validCultures = new List<string> { "af", "af-za", "sq", "sq-al", "gsw-fr", "am-et", "ar", "ar-dz", "ar-bh", "ar-eg", "ar-iq", "ar-jo", "ar-kw", "ar-lb", "ar-ly", "ar-ma", "ar-om", "ar-qa", "ar-sa", "ar-sy", "ar-tn", "ar-ae", "ar-ye", "hy", "hy-am", "as-in", "az", "az-cyrl-az", "az-latn-az", "ba-ru", "eu", "eu-es", "be", "be-by", "bn-bd", "bn-in", "bs-cyrl-ba", "bs-latn-ba", "br-fr", "bg", "bg-bg", "ca", "ca-es", "zh-hk", "zh-mo", "zh-cn", "zh-hans", "zh-sg", "zh-tw", "zh-hant", "co-fr", "hr", "hr-hr", "hr-ba", "cs", "cs-cz", "da", "da-dk", "prs-af", "div", "div-mv", "nl", "nl-be", "nl-nl", "en", "en-ae", "en-au", "en-bz", "en-ca", "en-029", "en-in", "en-ie", "en-jm", "en-my", "en-nz", "en-ph", "en-sg", "en-za", "en-tt", "en-gb", "en-us", "en-zw", "et", "et-ee", "fo", "fo-fo", "fil-ph", "fi", "fi-fi", "fr", "fr-be", "fr-ca", "fr-fr", "fr-lu", "fr-mc", "fr-ch", "fy-nl", "gl", "gl-es", "ka", "ka-ge", "de", "de-at", "de-de", "de-li", "de-lu", "de-ch", "el", "el-gr", "kl-gl", "gu", "gu-in", "ha-latn-ng", "he", "he-il", "hi", "hi-in", "hu", "hu-hu", "is", "is-is", "ig-ng", "id", "id-id", "iu-latn-ca", "iu-cans-ca", "ga-ie", "xh-za", "zu-za", "it", "it-it", "it-ch", "ja", "ja-jp", "kn", "kn-in", "kk", "kk-kz", "km-kh", "qut-gt", "rw-rw", "sw", "sw-ke", "kok", "kok-in", "ko", "ko-kr", "ky", "ky-kg", "lo-la", "lv", "lv-lv", "lt", "lt-lt", "wee-de", "lb-lu", "mk", "mk-mk", "ms", "ms-bn", "ms-my", "ml-in", "mt-mt", "mi-nz", "arn-cl", "mr", "mr-in", "moh-ca", "mn", "mn-mn", "mn-mong-cn", "ne-np", "no", "nb-no", "nn-no", "oc-fr", "or-in", "ps-af", "fa", "fa-ir", "pl", "pl-pl", "pt", "pt-br", "pt-pt", "pa", "pa-in", "quz-bo", "quz-ec", "quz-pe", "ro", "ro-ro", "rm-ch", "ru", "ru-ru", "smn-fi", "smj-no", "smj-se", "se-fi", "se-no", "se-se", "sms-fi", "sma-no", "sma-se", "sa", "sa-in", "sr", "sr-cyrl-ba", "sr-cyrl-sp", "sr-latn-ba", "sr-latn-sp", "nso-za", "tn-za", "si-lk", "sk", "sk-sk", "sl", "sl-si", "es", "es-ar", "es-bo", "es-cl", "es-co", "es-cr", "es-do", "es-ec", "es-sv", "es-gt", "es-hn", "es-mx", "es-ni", "es-pa", "es-py", "es-pe", "es-pr", "es-es", "es-us", "es-uy", "es-ve", "sv", "sv-fi", "sv-se", "syr", "syr-sy", "tg-cyrl-tj", "tzm-latn-dz", "ta", "ta-in", "tt", "tt-ru", "te", "te-in", "th", "th-th", "bo-cn", "tr", "tr-tr", "tk-tm", "ug-cn", "uk", "uk-ua", "wen-de", "ur", "ur-pk", "uz", "uz-cyrl-uz", "uz-latn-uz", "vi", "vi-vn", "cy-gb", "wo-sn", "sah-ru", "ii-cn", "yo-ng" };

		// Include ONLY cultures you are implementing
		private static readonly List<string> _cultures = new List<string> {
		"en-ae",  // first culture is the DEFAULT
        "ar-ae"  // Arabic UAE culture
        };

		/// <summary>
		/// Returns true if the language is a right-to-left language. Otherwise, false.
		/// </summary>
		public static bool IsRighToLeft()
		{
			return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

		}
		/// <summary>
		/// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "en-US"
		/// </summary>
		/// <param name="name" />Culture's name (e.g. en-AE)</param>
		public static string GetImplementedCulture(string name)
		{
			// make sure it's not null
			if (string.IsNullOrEmpty(name))
				return GetDefaultCulture(); // return Default culture
											// make sure it is a valid culture first
			if (_validCultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
				return GetDefaultCulture(); // return Default culture if it is invalid
											// if it is implemented, accept it
			if (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
				return name; // accept it
							 // Find a close match. For example, if you have "en-AE" defined and the user requests "en-GB", 
							 // the function will return closes match that is "en-AE" because at least the language is the same (ie English)  
			var n = GetNeutralCulture(name);
			foreach (var c in _cultures)
				if (c.StartsWith(n))
					return c;
			// else 
			// It is not implemented
			return GetDefaultCulture(); // return Default culture as no match found
		}
		/// <summary>
		/// Returns default culture name which is the first name decalared (e.g. en-AE)
		/// </summary>
		/// <returns></returns>
		public static string GetDefaultCulture()
		{
			return _cultures[0]; // return Default culture
		}
		public static string GetCurrentCulture()
		{
			return Thread.CurrentThread.CurrentCulture.Name;
		}
		public static string GetCurrentNeutralCulture()
		{
			return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
		}
		public static string GetNeutralCulture(string name)
		{
			if (!name.Contains("-")) return name;

			return name.Split('-')[0]; // Read first part only. E.g. "en", "ar"
		}
	}
}