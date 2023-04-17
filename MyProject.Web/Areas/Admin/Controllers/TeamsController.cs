using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OfficeOpenXml;
using MyProject.Data;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Web.Helpers;

namespace MyProject.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class TeamsController : Controller
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITeamService _teamService;
        string ErrorMessage = string.Empty;
        string SuccessMessage = string.Empty;
        public TeamsController(ITeamService teamService)
        {
            this._teamService = teamService;
        }

        // GET: Admin/Teams
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var teams = _teamService.GetTeams().ToList();
            return View(teams);
        }
        // GET: Admin/Teams/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = this._teamService.GetTeam((long)id);
            team.Contact1 = team.Contact1.Remove(0, 3);
            team.Contact2 = team.Contact2.Remove(0, 3);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Admin/Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,NameAr,Image,Designation,DesignationAr,JoiningDate,Address,Email,Contact1,Contact2,About,AboutAr,IsActive,CreatedOn,IsDeleted")] Team team)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string replacement = team.Name.Replace("?", "");
                if (team.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Team/Image/{0}/", replacement);
                    team.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                team.Contact1 = "92" + team.Contact1;
                team.Contact2 = "92" + team.Contact2;
                if (this._teamService.CreateTeam(team, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created workforce person {team.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Teams/Index",
                        message = message,
                        data = new
                        {
                            ID = team.ID,
                            CreatedOn = team.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        team.Image,
                            Name = team.Name,
                            Designation = team.Designation,
                            Email = team.Email,
                            Contact1 = team.Contact1,
                            IsActive = team.IsActive.HasValue ? team.IsActive.Value.ToString() : false.ToString(),

                        }
                    });
                }
                return Json(new { success = false, message = message });
            }

            message = "Please fill the form properly ...";
            return Json(new { success = false, message = message });

        }

        // GET: Admin/Teams/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = this._teamService.GetTeam((long)id);
            team.Contact1 = team.Contact1.Replace("92", "");
            team.Contact2 = team.Contact2.Replace("92", "");
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Admin/Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,NameAr,Image,Designation,DesignationAr,JoiningDate,Address,Email,Contact1,Contact2,About,AboutAr,IsActive,CreatedOn,IsDeleted")] Team team)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                string replacement = team.Name.Replace("?", "");
                var teamimagedb = this._teamService.GetTeam(team.ID);

                if (team.Image != null)
                {
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Team/Image/{0}/", replacement);

                    if (teamimagedb.Image != null)
                    {

                        if (System.IO.File.Exists(absolutePath + teamimagedb.Image))
                        {
                            System.IO.File.Delete(absolutePath + teamimagedb.Image);
                        }
                    }
                    team.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
                }
                team.Contact1 = "92" + team.Contact1;
                team.Contact2 = "92" + team.Contact2;
                if (this._teamService.UpdateTeam(ref team, ref message))
                {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated workforce person {team.Name}.");
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Teams/Index",
                        message = message,
                        data = new
                        {
                            ID = team.ID,
                            CreatedOn = team.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        team.Image,
                            Name = team.Name,
                            Designation = team.Designation,
                            Email = team.Email,
                            Contact1 = team.Contact1,
                            IsActive = team.IsActive.HasValue ? team.IsActive.Value.ToString() : false.ToString(),

                        }
                    });
                }
            }
            else
            {
                message = "Please Fill the form Correctly ...";
            }
            return Json(new { success = false, message = message });

        }

        // GET: Admin/Teams/Delete/5
        public ActionResult Delete(long? id)
        {
            string message = string.Empty;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View();
        }

        // POST: Admin/Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            string message = string.Empty;
            if (this._teamService.DeleteTeam(id, ref message))
            {
                    log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted workforce person ID: {id}.");
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Activate(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = this._teamService.GetTeam((long)id);
            if (team == null)
            {
                return HttpNotFound();
            }

            if (!(bool)team.IsActive)
            { 
                team.IsActive = true;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} activated workforce person {team.Name}.");
            }
            else
            {
                team.IsActive = false;
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deactivated workforce person {team.Name}.");
            }
            string message = string.Empty;
            if (_teamService.UpdateTeam(ref team, ref message))
            {
                SuccessMessage = "WorkForce " + ((bool)team.IsActive ? "activated" : "deactivated") + "  successfully ...";
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} {((bool)team.IsActive ? "activated" : "deactivated")} workforce {team.Name}.");
                return Json(new
                {
                    success = true,
                    url = "/Admin/Teams/Index",
                    message = SuccessMessage,
                    data = new
                    {
                        ID = team.ID,
                        CreatedOn = team.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt"),
                        team.Image,
                        Name = team.Name,
                        Designation = team.Designation,
                        Email = team.Email,
                        Contact1 = team.Contact1,
                        IsActive = team.IsActive.HasValue ? team.IsActive.Value.ToString() : false.ToString(),

                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ErrorMessage = "Oops! Something went wrong. Please try later...";
            }

            return Json(new { success = false, message = ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report()
        {

            var getAllTeams = _teamService.GetTeams().ToList();
            if (getAllTeams.Count() > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("BanksReport");

                    var headerRow = new List<string[]>()
                    {

                            new string[] {
                        "Creation Date"
                        ,"Name"
                        ,"Email"
                        ,"Contact"
                        ,"Status"
                        }
                    };

                    // Determine the header range (e.g. A1:D1)
                    string headerRange = "A1:" + char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["BanksReport"];

                    // Popular header row data
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    if (getAllTeams.Count != 0)
                        getAllTeams = getAllTeams.OrderByDescending(x => x.ID).ToList();


                    foreach (var i in getAllTeams)
                    {
                        cellData.Add(new object[] {
                            i.CreatedOn.HasValue ? i.CreatedOn.Value.ToString("dd MMM yyyy, h:mm tt") : "-"
                            ,!string.IsNullOrEmpty(i.Name) ? i.Name: "-"
                            ,!string.IsNullOrEmpty(i.Email) ? i.Email: "-"
                            ,!string.IsNullOrEmpty(i.Contact1) ? i.Contact1: "-"
                            ,i.IsActive == true ? "Active" : "InActive"
                            });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);

                    return File(excel.GetAsByteArray(), "application/msexcel", "Teams Report.xlsx");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
