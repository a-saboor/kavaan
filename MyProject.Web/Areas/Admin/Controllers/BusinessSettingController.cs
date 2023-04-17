using System.Web.Mvc;
using System.Net;
using System.Collections.Generic;
using System;
using MyProject.Service;
using MyProject.Web.AuthorizationProvider;
using MyProject.Data;
using MyProject.Web.Areas.Admin.ViewModels;
using MyProject.Web.Helpers;
using System.Web;
using MyProject.Web.Helpers.Routing;

namespace MyProject.Web.Areas.Admin.Controllers
{
	[AuthorizeAdmin]
	public class BusinessSettingController : Controller
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string ErrorMessage = string.Empty;
		string SuccessMessage = string.Empty;

		private readonly IBusinessSettingService _businesssettingService;
		private readonly IBusinessBranchSettingService _businessbranchsettingService;

		public BusinessSettingController(IBusinessSettingService businesssettingService, IBusinessBranchSettingService businessbranchsettingService)
		{
			this._businesssettingService = businesssettingService;
			this._businessbranchsettingService = businessbranchsettingService;
		}

		public ActionResult Index()
		{
			ViewBag.SuccessMessage = TempData["SuccessMessage"];
			BusinessSetting businessSetting = _businesssettingService.GetDefaultBusinessSetting();
			businessSetting.Contact = !string.IsNullOrEmpty(businessSetting.Contact) ? businessSetting.Contact.Remove(0, 2) : "";
			businessSetting.Contact2 = !string.IsNullOrEmpty(businessSetting.Contact2) ? businessSetting.Contact2.Remove(0, 2) : "";

			return View(businessSetting);
		
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Update(long? id, BusinessSetting businessSetting)
		{
			string message = string.Empty;
			if (ModelState.IsValid)
			{
				BusinessSetting business = _businesssettingService.GetBusinessSetting((long)id);
				if (id.HasValue && id > 0)
				{
					businessSetting.Contact = !string.IsNullOrEmpty(businessSetting.Contact) ? "92" + businessSetting.Contact : "";
					businessSetting.Contact2 = !string.IsNullOrEmpty(businessSetting.Contact2) ? "92" + businessSetting.Contact2 : "";

					
					string previousImagepath = string.IsNullOrEmpty(business.Image) ? "" : business.Image.Replace(CustomURL.GetImageServer(), "");
					if (Request.Files.Count > 0)
					{
						try
						{
							//  Get all files from Request object  
							HttpFileCollectionBase files = Request.Files;
							if (files.Count > 0)
							{
								string absolutePath = Server.MapPath("~");
								if (businessSetting.Image != null)
								{
									string relativePath = string.Format("/Assets/AppFiles/Images/BusinessSetting/{0}/", businessSetting.Title.Replace(" ", "_"));
									business.Image = Uploader.UploadImage(files, absolutePath, relativePath, "Image", ref message, "Image");
									if (!string.IsNullOrEmpty(business.Image))
									{
										business.Image = CustomURL.GetImageServer() + business.Image.Remove(0, 1);
									}
									//delete old file
									if (System.IO.File.Exists(previousImagepath))
									{
										System.IO.File.Delete(previousImagepath);
									}
								}
							}
						}
						catch (Exception ex)
						{
							//add logs here...
						}
					}
					else
					{
						if (string.IsNullOrEmpty(businessSetting.Image))
						{
							businessSetting.Image = "";
							//delete old file
							if (System.IO.File.Exists(previousImagepath))
							{
								System.IO.File.Delete(previousImagepath);
							}
						}
					}
					if (_businesssettingService.UpdateBusinessSetting(ref businessSetting, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated business settings.");
						TempData["SuccessMessage"] = message;
						return RedirectToAction("Index");
					}
				}
				else
				{
					string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/BusinessSetting/"), businessSetting.Title.Replace(" ", "_"), "/Image");
					string absolutePath = Server.MapPath("~");
					string relativePath = string.Format("/Assets/AppFiles/Images/BusinessSetting/{0}/", businessSetting.Title.Replace(" ", "_"));
					HttpFileCollectionBase files = Request.Files;
					if (files.Count > 0)
					{
						if (businessSetting.Image != null)
						{
							relativePath = string.Format("/Assets/AppFiles/Images/Services/{0}/", businessSetting.Title.Replace(" ", "_"));
							business.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
							if (!string.IsNullOrEmpty(businessSetting.Image))
							{
								businessSetting.Image = CustomURL.GetImageServer() + businessSetting.Image.Remove(0, 1);
							}
						}
					}
					if (_businesssettingService.CreateBusinessSetting(businessSetting, ref message))
					{
						log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created business settings.");
						TempData["SuccessMessage"] = message;
						return RedirectToAction("Index");
					}
				}
			}
			else
			{
				message = "Please fill the form properly ...";
			}
			ViewBag.ErrorMessage = message;
			return View("Index", businessSetting);
		}

		#region Business Branch Settings

		public ActionResult BranchSettings(long? id)
		{
			if (id == null || id ==0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			IEnumerable<BusinessBranchSetting> branchSettings = _businessbranchsettingService.GetBusinessBranchSettings((long)id);

			ViewBag.BusinessSettingID = id;

			return PartialView(branchSettings);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult BranchSettings(HttpPostedFileBase file, BranchSettingsViewModel branchSettingsViewModel)
		{
			string message = string.Empty;
			string error = string.Empty;
			if (branchSettingsViewModel.ID < 1)//Create Branch
			{
				try
				{
					if (ModelState.IsValid && !string.IsNullOrEmpty(branchSettingsViewModel.Name))
					{
						BusinessBranchSetting branchSettings = new BusinessBranchSetting();
						branchSettings.BusinessSettingID = branchSettingsViewModel.BusinessSettingID;
						branchSettings.Image = branchSettingsViewModel.Image;
						branchSettings.Name = branchSettingsViewModel.Name;
						branchSettings.NameAr = branchSettingsViewModel.NameAr;
						branchSettings.Contact = !string.IsNullOrEmpty(branchSettingsViewModel.Contact) ? "92" + branchSettingsViewModel.Contact : "";
						branchSettings.Contact2 = !string.IsNullOrEmpty(branchSettingsViewModel.Contact2) ? "92" + branchSettingsViewModel.Contact2 : "";
						branchSettings.Fax = branchSettingsViewModel.Fax;
						branchSettings.Email = branchSettingsViewModel.Email;
						branchSettings.MapIframe = branchSettingsViewModel.MapIframe;
						branchSettings.StreetAddress = branchSettingsViewModel.StreetAddress;
						branchSettings.StreetAddressAr=branchSettingsViewModel.StreetAddressAr;
						branchSettings.WorkingDays = branchSettingsViewModel.Days;


						string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/BranchSetting/"), branchSettingsViewModel.Name.Replace(" ", "_"), "/Image");
						string absolutePath = Server.MapPath("~");
						string relativePath = string.Format("/Assets/AppFiles/Images/BranchSetting/{0}/", branchSettingsViewModel.Name.Replace(" ", "_"));

						if (Request.Files.Count > 0)
						{
							try
							{
								//  Get all files from Request object  
								HttpFileCollectionBase files = Request.Files;
								if (files.Count > 0)
								{
									if (branchSettingsViewModel.Image != null)
									{
										relativePath = string.Format("/Assets/AppFiles/Images/BranchSetting/{0}/", branchSettingsViewModel.Name.Replace(" ", "_"));
										branchSettings.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
										if (!string.IsNullOrEmpty(branchSettings.Image))
										{
											branchSettings.Image = CustomURL.GetImageServer() + branchSettings.Image.Remove(0, 1);
										}
									}
								}
							}
							catch (Exception ex)
							{
								//add logs here...
							}
						}

						if (_businessbranchsettingService.CreateBusinessBranchSetting(branchSettings, ref message))
						{
							log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} created business branch settings.");
							return Json(new
							{
								success = true,
								url = "/Admin/BusinessSetting/Index",
								message = message,
								data = new
								{
									Date = branchSettings.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
									ID = branchSettings.ID,
									BusinessSettingID = branchSettings.BusinessSettingID,
									Name = branchSettings.Name,
									NameAr=branchSettings.NameAr,
									Contact = branchSettings.Contact,
									Contact2 = branchSettings.Contact2,
									Fax = branchSettings.Fax,
									Email = branchSettings.Email,
									MapIframe = branchSettings.MapIframe,
									StreetAddress = branchSettings.StreetAddress,
									StreetAddressAr=branchSettings.StreetAddressAr,
									Days = branchSettings.WorkingDays,
								}
							});
						}
					}
					else
					{
						message = "Please fill the form properly ...";
						error = "ModelState is not valid.";
					}
					return Json(new { success = false, message = message, error = error });
				}
				catch (Exception ex)
				{
					return Json(new
					{
						success = false,
						message = "Oops! Something went wrong. Please try later.",
						error = ex.Message,
					});
				}
			}
			else if (branchSettingsViewModel.ID > 0)//Update Branch
			{
				try
				{
					if (ModelState.IsValid && !string.IsNullOrEmpty(branchSettingsViewModel.Name))
					{
						BusinessBranchSetting branchSettings = _businessbranchsettingService.GetBusinessBranchSetting((long)branchSettingsViewModel.ID);
						branchSettings.BusinessSettingID = branchSettingsViewModel.BusinessSettingID;
						branchSettings.Image = branchSettingsViewModel.Image;
						branchSettings.Name = branchSettingsViewModel.Name;
						branchSettings.NameAr = branchSettingsViewModel.NameAr;
						branchSettings.Contact = !string.IsNullOrEmpty(branchSettingsViewModel.Contact) ? "92" + branchSettingsViewModel.Contact : "";
						branchSettings.Contact2 = !string.IsNullOrEmpty(branchSettingsViewModel.Contact2) ? "92" + branchSettingsViewModel.Contact2 : "";
						branchSettings.Fax = branchSettingsViewModel.Fax;
						branchSettings.Email = branchSettingsViewModel.Email;
						branchSettings.MapIframe = branchSettingsViewModel.MapIframe;
						branchSettings.StreetAddress = branchSettingsViewModel.StreetAddress;
						branchSettings.StreetAddressAr= branchSettingsViewModel.StreetAddressAr;	
						branchSettings.WorkingDays = branchSettingsViewModel.Days;

						string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/BranchSetting/"), branchSettingsViewModel.Name.Replace(" ", "_"), "/Image");
						string absolutePath = Server.MapPath("~");
						string relativePath = string.Format("/Assets/AppFiles/Images/BranchSetting/{0}/", branchSettingsViewModel.Name.Replace(" ", "_"));

						if (Request.Files.Count > 0)
						{
							try
							{
								//  Get all files from Request object  
								HttpFileCollectionBase files = Request.Files;
								if (files.Count > 0)
								{
									if (branchSettingsViewModel.Image != null)
									{
										relativePath = string.Format("/Assets/AppFiles/Images/BranchSetting/{0}/", branchSettingsViewModel.Name.Replace(" ", "_"));
										branchSettings.Image = Uploader.UploadImage(Request.Files, absolutePath, relativePath, "Image", ref message, "Image");
										if (!string.IsNullOrEmpty(branchSettings.Image))
										{
											branchSettings.Image = CustomURL.GetImageServer() + branchSettings.Image.Remove(0, 1);
										}
									}
								}
							}
							catch (Exception ex)
							{
								//add logs here...
							}
						}

						if (_businessbranchsettingService.UpdateBusinessBranchSetting(ref branchSettings, ref message))
						{
							log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} updated business branch settings.");
							return Json(new
							{
								success = true,
								url = "/Admin/BusinessSetting/Index",
								message = message,
								data = new
								{
									Date = branchSettings.CreatedOn.Value.ToString("dd MMM yyyy, h: mm tt"),
									ID = branchSettings.ID,
									BusinessSettingID = branchSettings.BusinessSettingID,
									Name = branchSettings.Name,
									NameAr=branchSettings.NameAr,
									Contact = branchSettings.Contact,
									Contact2 = branchSettings.Contact2,
									Fax = branchSettings.Fax,
									Email = branchSettings.Email,
									MapIframe = branchSettings.MapIframe,
									StreetAddress = branchSettings.StreetAddress,
									StreetAddressAr=branchSettings.StreetAddressAr,
									Days = branchSettings.WorkingDays,
								}
							});
						}

					}
					else
					{
						message = "Please fill the form properly ...";
						error = "ModelState is not valid.";
					}
					return Json(new { success = false, message = message, error = error });
				}
				catch (Exception ex)
				{
					return Json(new
					{
						success = false,
						message = "Oops! Something went wrong. Please try later.",
						error = ex.Message
					});
				}
			}

			return Json(new
			{
				success = false,
				message = "Oops! Something went wrong. Please try later.",
				error = "Create and edit method not called ...",
			});

		}

		[HttpPost, ActionName("BranchSettingDelete")]
		[ValidateAntiForgeryToken]
		public ActionResult BranchSettingDeleteConfirmed(long id)
		{
			string message = string.Empty;
			string error = string.Empty;
			string absolutePath = Server.MapPath("~");
			BusinessBranchSetting branchSetting = _businessbranchsettingService.GetBusinessBranchSetting(id);


			if (_businessbranchsettingService.DeleteBusinessBranchSetting((Int16)id, ref message))
			{
				if (System.IO.File.Exists(absolutePath + branchSetting.Image))
				{
					System.IO.File.Delete(absolutePath + branchSetting.Image);
				}
				log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} deleted business branch settings.");
				return Json(new
				{
					success = true,
					message = message,
				}, JsonRequestBehavior.AllowGet);
			}
			return Json(new { success = false, message = message, error = error }, JsonRequestBehavior.AllowGet);
		}


		#endregion
	}
}
