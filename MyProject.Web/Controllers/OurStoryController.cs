using MyProject.Data;
using MyProject.Service;
using MyProject.Web.Helpers;
using MyProject.Web.Helpers.Routing;
using MyProject.Web.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Web.Controllers{
    public class OurStoryController : Controller
    {
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;

        private readonly IIntroductionService _introductionService;
        private readonly IIntroductionImageService _introductionImageService;
        private readonly ITeamService _teamService;
        private readonly IAwardService _awardService;
        private readonly IPartnerService _partnerService;

        public OurStoryController(IIntroductionService introductionService, IIntroductionImageService introductionImageService, ITeamService teamService, IAwardService awardService, IPartnerService partnerService)
        {
            _introductionService = introductionService;
            _introductionImageService = introductionImageService;
            _teamService = teamService;
            _awardService = awardService;
            _partnerService = partnerService;
        }

		[Route("our-story", Name = "our-story")]
        public ActionResult OurStory()
        {
            return View();
        }

        [Route("introduction", Name = "introduction")]
        public ActionResult Intro()
        {
            Introduction intro = _introductionService.GetIntroductionfirstordefault();
            if (intro == null)
                intro = new Introduction();
            return View(intro);
        }

        //[Route("chairman-message", Name = "chairman-message")]
        //public ActionResult ChairmanMessage()
        //{
        //    return View();
        //}

        [Route("workforce", Name = "workforce")]
        public ActionResult Team()
        {
            List<Team> teams = _teamService.GetTeams().Where(x => x.IsActive == true).ToList();
            if (teams.Count() < 1)
                teams = new List<Team>();
            return View(teams);
        }

        [Route("workforce-person/{id}", Name = "workforce-person/{id}")]
        public ActionResult TeamsDetails(long id)
        {
            Team person = _teamService.GetTeam(id);
            if (person == null)
                person = new Team();
            return View(person);
        }

        [Route("structure", Name = "structure")]
        public ActionResult Structure()
        {
            return View();
        }

        [Route("awards", Name = "awards")]
        public ActionResult Awards()
        {
            List<Award> awards = _awardService.GetAwards().Where(x=>x.IsActive == true).ToList();
            if (awards.Count() < 1)
                awards = new List<Award>();
            return View(awards);
        }
        
        [Route("partners", Name = "partners")]
        public ActionResult Partners()
        {
            return View();
        }

        [HttpPost]
        [Route("partners/filters", Name = "partners/filters")]
        public ActionResult Index(FilterViewModel filters, string culture = "en-ae")
        {
            string lang = "en";
            if (culture.Contains('-'))
                lang = culture.Split('-')[0];

            try
            {
                string ImageServer = "";
                var partners = _partnerService.GetFilteredPartners(filters.search, filters.pageSize, filters.pageNumber, filters.sortBy, lang, ImageServer);
                return Json(new
                {
                    success = true,
                    message = "Data recieved successfully!",
                    data = partners.Select(x => new
                    {
                        x.ID,
                        x.Date,
                        x.Name,
                        x.Description,
                        x.Image,
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}