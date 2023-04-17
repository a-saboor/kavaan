using System.Configuration;

namespace MyProject.Service.Helpers.Routing
{
	class CustomURL
	{
		public static string GetFormatedURL(string url)
		{
			var ServiceProviderUrl = ConfigurationManager.AppSettings["DomainUrl"];
			return string.Format("{0}{1}", ServiceProviderUrl, url.Replace("~", ""));
		}
	}
}
