using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.Api.Suggestion
{
	public class SuggestionViewModel
	{
		[Required]
		public string Suggestion { get; set; }
		
	}
}