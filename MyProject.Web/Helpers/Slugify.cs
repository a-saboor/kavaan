using System.Text.RegularExpressions;

namespace MyProject.Web.Helpers
{
	public static class Slugify
	{
		public static string GenerateSlug(this string phrase)
		{
			string str = phrase.RemoveAccent().ToLower().Replace("&", "and");
			// invalid chars           
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces into one space   
			str = Regex.Replace(str, @"\s+", " ").Trim();
			// cut and trim 
			str = str.Substring(0, str.Length <= 2048 ? str.Length : 2048).Trim();
			str = Regex.Replace(str, @"\s", "-"); // hyphens   
			return str;
		}

		public static string RemoveAccent(this string txt)
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
			return System.Text.Encoding.ASCII.GetString(bytes);
		}
	}
}