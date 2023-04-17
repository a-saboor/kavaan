using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ProjectTagController : Controller
    {
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IProjectTagService _projectTagService;
		private readonly ITagService _tagService;

		public ProjectTagController(IProjectTagService projectTagService, ITagService tagService)
		{
			this._projectTagService = projectTagService;
			this._tagService = tagService;
		}

		[HttpGet]
		public ActionResult GetProjectTags(long id)
		{
			var tags = _tagService.GetTags().Select(i => new
			{
				id = i.ID.ToString(),
				value = i.Name
			}).ToList();
			var projectTags = _projectTagService.GetProjectTags(id).Select(i => new
			{
				id = i.TagID.ToString(),
				value = i.Tag.Name,
				projecttagId = i.ID
			}).ToList();

			return Json(new
			{
				success = true,
				message = "Data recieved successfully!",
				tags = tags,
				projectTags = projectTags
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProjectTag projectTag)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				if (_projectTagService.CreateProjectTag(ref projectTag, ref message))
				{
					return Json(new
					{
						success = true,
						data = projectTag.ID,
						message = "Project tag assigned.",
					});
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			return Json(new { success = false, message = message });
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(ProjectTag projectTag)
		{
			string message = string.Empty;
			if (_projectTagService.DeleteProjectTag(projectTag, ref message))
			{
				return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
		}
	}
}