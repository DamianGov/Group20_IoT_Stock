using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin","Admin","Member")]
    public class RequestStockController : Controller
    {
        private IoTContext db = new IoTContext();

        public List<RequestStockViewModel> getRequests(string search)
        {
            List<RequestStockViewModel> SortedRequests = new List<RequestStockViewModel>();

            if (search.IsNullOrEmpty())
            {
                SortedRequests = db.RequestStock.Include(x => x.Users)
                                .OrderByDescending(x => x.RequestDate)
                                .AsEnumerable()
                                .Select(x => new RequestStockViewModel
                                {
                                    Id = x.Id,
                                    UserId = x.UserId,
                                    UniNum = x.Users.UniNum,
                                    UserName = x.Users.GetFullName(),
                                    StockName = x.StockName,
                                    Quantity = x.Quantity,
                                    StockPrice = x.StockPrice.ToString("C"),
                                    StockLink = x.StockLink,
                                    StockImage = x.StockImage,
                                    RequestDate = x.RequestDate.ToString("yyyy/M/d"),
                                    Approved = x.IsApproved ? "Yes" : "No"
                                }).ToList();
            }
            else
            {
                SortedRequests = db.RequestStock.Include(x => x.Users)
                                .OrderByDescending(x => x.RequestDate)
                                .Where(x => x.StockName.ToLower().Contains(search.Trim().ToLower()))
                                .AsEnumerable()
                                .Select(x => new RequestStockViewModel
                                {
                                    Id = x.Id,
                                    UserId = x.UserId,
                                    UniNum = x.Users.UniNum,
                                    UserName = x.Users.GetFullName(),
                                    StockName = x.StockName,
                                    Quantity = x.Quantity,
                                    StockPrice = x.StockPrice.ToString("C"),
                                    StockLink = x.StockLink,
                                    StockImage = x.StockImage,
                                    RequestDate = x.RequestDate.ToString("yyyy/M/d"),
                                    Approved = x.IsApproved ? "Yes" : "No"
                                }).ToList();
            }

            return SortedRequests;
        }

        // GET: RequestStock
        public ActionResult Index()
        {
            return View(getRequests(null));
        }

        public ActionResult Search(string searchItem)
        {
            string UserType = Session["UserType"] as string;

            var SortedRequests = getRequests(searchItem);

            if(UserType != "Member") return PartialView("_RequestStockTableAdmin", SortedRequests);

            return PartialView("_RequestStockTable", SortedRequests);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestStock stock, HttpPostedFileBase image)
        {
            Users user = Session["User"] as Users;

            if (!stock.StockName.IsNullOrEmpty()) stock.StockName = stock.StockName.Trim();

            if (db.RequestStock.Any(r => r.StockName.ToLower() == stock.StockName.ToLower())) ModelState.AddModelError("StockName", "There is already a request with this Stock Name");

            if (image == null || image.ContentLength <= 0) ModelState.AddModelError("StockImage", "Please select an image");

            if (ModelState.IsValid)
            {
                string ImageGUID = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                string ImagePath = Path.Combine(Server.MapPath("~/Content/Request"), ImageGUID);

                image.SaveAs(ImagePath);

                stock.UserId = user.Id;
                stock.StockImage = ImageGUID;
                db.RequestStock.Add(stock);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(stock);
        }

        public ActionResult MyRequestsOnly(bool showMyRequests)
        {
            Users user = Session["User"] as Users;

            List<RequestStockViewModel> SortedRequests = new List<RequestStockViewModel>();

            if (showMyRequests)
            {
                     SortedRequests = db.RequestStock.Include(x => x.Users)
                    .OrderByDescending(x => x.RequestDate)
                    .Where(x => x.UserId == user.Id)
                    .AsEnumerable()
                    .Select(x => new RequestStockViewModel
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        UniNum = x.Users.UniNum,
                        UserName = x.Users.GetFullName(),
                        StockName = x.StockName,
                        StockPrice = x.StockPrice.ToString("C"),
                        StockLink = x.StockLink,
                        StockImage = x.StockImage,
                        RequestDate = x.RequestDate.ToString("yyyy/M/d"),
                        Approved = x.IsApproved ? "Yes" : "No"
                    }).ToList();

            }
            else
            {
                SortedRequests = getRequests(null);
            }

            if (user.Role.Type != "Member") return PartialView("_RequestStockTableAdmin", SortedRequests);

            return PartialView("_RequestStockTable", SortedRequests);
        }


        [SessionChecker("SuperAdmin","Admin")]
        [HttpPost]
        public ActionResult AcceptStockRequest(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            RequestStock stock = db.RequestStock.Find(id);

            if(stock == null) return RedirectToAction(nameof(Index));

            stock.IsApproved = true;

            db.Entry(stock).State = EntityState.Modified;
            db.SaveChanges();

            Users user = db.Users.Find(stock.UserId);

            _ = SharedMethods.SendEmail(user.GetFullName(), user.Email, "IoT System - Stock Request Accepted", $"Hello,{user.GetFullName()}.\n\nThe Stock you requested for, {stock.Quantity} x {stock.StockName}, has been accepted.\n\nThank you.\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "Stock Request has been approved" });
        }
    }
}