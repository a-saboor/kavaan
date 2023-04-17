using Project.Data;
using Project.Service;
using Project.Web.Helpers;
using Project.Web.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Web.AuthorizationProvider;

namespace Project.Web.Areas.Admin.Controllers
{
    public class TicketController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITicketService _ticketService;
        private readonly ITicketDocumentService _ticketDocumentService;
        private readonly ITicketMessageService _ticketMessageService;
        private readonly IUserService _userService;

        public TicketController(ITicketService ticketService, IUserService userService, ITicketDocumentService ticketDocumentService, ITicketMessageService ticketMessageService)
        {
            this._ticketService = ticketService;
            this._ticketDocumentService = ticketDocumentService;
            this._ticketMessageService = ticketMessageService;
            this._userService = userService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            long userId = (long)Session["AdminUserID"];
            var user = _userService.GetUser(userId);
            var role = Session["Role"].ToString();
            ViewBag.Role = role;
            if (role == "Admin")
            {
                var tickets = _ticketService.GetTickets();
                return View(tickets);
            }

            else
            {
                if (user.TicketManagement == true)
                {
                    var tickets = _ticketService.GetTickets();
                    return View(tickets);
                }
                else
                {
                    var tickets = _ticketService.GetTicketsByUser(userId);
                    return View(tickets);
                }

            }
        }

        public ActionResult Details(long? id)
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            long test = (long)(Session["AdminUserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var conversation = _ticketMessageService.GetTicketConversation((int)id);
            var ticket = _ticketService.GetTicket((long)id);
            TicketDetailsViewModel Details = new TicketDetailsViewModel();


            Details.CreatedOn = (DateTime)ticket.CreatedOn;
            Details.TicketID = ticket.ID;
            Details.TicketNo = ticket.TicketNo;
            Details.Priority = ticket.Priority;
            Details.Status = ticket.Status;
            Details.Description = ticket.Description;
            Details.ticketConversation = conversation.ToList();


            if (Details == null)
            {
                return HttpNotFound();
            }

            ViewBag.Referrer = Request.UrlReferrer;
            return View(Details);
        }



        [HttpPost]
        public ActionResult Message(string Message, long TicketID, string SenderName)
        {
            HttpFileCollectionBase file = Request.Files;
            string message = string.Empty;
            long userID = (long)(Session["AdminUserID"]);
            TicketMessage ticketMessage = new TicketMessage();
            ticketMessage.Message = Message;
            ticketMessage.TicketID = TicketID;
            ticketMessage.SenderType = "User";
            ticketMessage.SenderID = userID;
            ticketMessage.CreatedOn = Helpers.TimeZone.GetLocalDateTime();



            if (_ticketMessageService.CreateTicketMessage(ticketMessage, ref message))
            {
                if (Request.Files["Image"] != null)
                {
                    string FilePath = string.Format("{0}{1}{2}", Server.MapPath("~/Assets/AppFiles/Images/Tickets/"), ticketMessage.ID, "/attachment");
                    TicketDocument document = new TicketDocument();
                    string absolutePath = Server.MapPath("~");
                    string relativePath = string.Format("/Assets/AppFiles/Images/Tickets/{0}/", ticketMessage.ID);

                    document.Url = Uploader.UploadImage(Request.Files["Image"], absolutePath, relativePath, "attachment", ref message, "Attachment");
                    if (_ticketDocumentService.CreateTicketDocument(document, ref message))
                    {
                        ticketMessage.TicketDocumentID = document.ID;
                        if (_ticketMessageService.UpdateTicketMessage(ref ticketMessage, ref message))
                        {
                            return Json(new
                            {
                                success = true,
                                message = "Message sent successfully ...",
                                response = new
                                {
                                    senderName = SenderName,
                                    date = ticketMessage.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                                    file = document.Url,
                                    message = ticketMessage.Message,
                                }
                            });
                        }
                    }


                }

                return Json(new
                {
                    success = true,
                    message = "Message sent successfully ...",
                    response = new
                    {
                        senderName = SenderName,
                        date = ticketMessage.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        file = "No Image",
                        message = ticketMessage.Message,
                        ID = ticketMessage.ID
                    }
                });


            }

            else
            {
                return Json(new
                {
                    success = true,
                    message = "Message failed ...",

                });

            }



        }
        public ActionResult AssignUser(long? Id)
        {
            TempData["TicketID"] = Id;
            ViewBag.UserID = new SelectList(_userService.GetUserDropDown(), "value", "text");
            return PartialView();
        }

        [HttpPost]
        public ActionResult AssignUser(long UserID, string message)
        {
            long ticketID = (long)TempData["TicketID"];
            var ticket = _ticketService.GetTicket(ticketID);
            ticket.UserID = UserID;

            if (_ticketService.UpdateTicket(ref ticket, ref message))
            {


                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} assing user ticket {ticket.TicketNo}.");
                return Json(new
                {
                    success = true,
                    url = "/Admin/Ticket/Index",
                    message = "Ticket assign successfully ...",
                    data = new
                    {
                        ID = ticketID,
                        UserID = ticket.UserID,
                        Description = ticket.Description,
                        VendorName = ticket.Vendor.Name,
                        Priority = ticket.Priority,
                        Status = ticket.Status,
                        UserName = ticket.User.Name,
                        Date = ticket.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        TicketNo = ticket.TicketNo,


                    }
                });
            }

            else
            {
                message = "Please select the student properly ...";
            }

            return Json(new { success = false, message = message });
        }



        public ActionResult StatusChange(long Id)
        {
            Ticket ticket = _ticketService.GetTicket(Id);
            TempData["TicketID"] = Id;
            return PartialView(ticket);
        }

        [HttpPost]
        public ActionResult StatusChange(Ticket ticket, string status)
        {
            string message = string.Empty;
            Ticket current = _ticketService.GetTicket((long)ticket.ID);
            current.Status = status;
            if (_ticketService.UpdateTicket(ref current, ref message))
            {

                var VendorID = Convert.ToInt64(Session["VendorID"]);
                log.Info($"{AdminSessionHelper.UserName} | {AdminSessionHelper.Email} ticket status change {ticket.Status}.");
                return Json(new
                {
                    success = true,
                    url = "/Admin/Ticket/Index",
                    message = "Status updated successfully ...",
                    data = new
                    {
                        ID = current.ID,
                        UserID = current.UserID != null ? current.UserID : 0,
                        VendorName = current.Vendor.Name,
                        Description = current.Description,
                        Priority = current.Priority,
                        Status = current.Status,
                        UserName = current.User != null ? current.User.Name : "-",
                        Date = current.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
                        TicketNo = current.TicketNo == null ? "-" : current.TicketNo,


                    }
                });
            }
            else

                return Json(new
                {
                    success = false,
                    message = "Ooops! something went wrong..."
                });
        }

    }
}