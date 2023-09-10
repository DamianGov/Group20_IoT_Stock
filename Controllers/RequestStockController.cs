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
    [SessionChecker]
    public class RequestStockController : Controller
    {
        private IoTContext db = new IoTContext();
        // GET: RequestStock
        public ActionResult Index()
        {
            var SortedRequests = db.RequestStock.Include(x => x.Users)
                                .OrderByDescending(x => x.RequestDate)
                                .AsEnumerable()
                                .Select(x => new RequestStockViewModel
                                {
                                    Id = x.Id,
                                    UserId = x.UserId,
                                    UserName = x.Users.Name,
                                    StockName = x.StockName,
                                    StockPrice = x.StockPrice.ToString("C"),
                                    StockLink = x.StockLink,
                                    StockImage = x.StockImage,
                                    RequestDate = x.RequestDate.ToString("yyyy/M/d"),
                                    Approved = x.IsApproved ? "Yes" : "No"
                                }).ToList();

            return View(SortedRequests);
        }

        public ActionResult Search(string searchItem)
        {
            var SortedRequests = db.RequestStock.Include(x => x.Users)
                                .OrderByDescending(x => x.RequestDate)
                                .Where(x => x.StockName.ToLower().Contains(searchItem.Trim().ToLower()))
                                .AsEnumerable()
                                .Select(x => new RequestStockViewModel
                                {
                                    Id = x.Id,
                                    UserId = x.UserId,
                                    UserName = x.Users.Name,
                                    StockName = x.StockName,
                                    StockPrice = x.StockPrice.ToString("C"),
                                    StockLink = x.StockLink,
                                    StockImage = x.StockImage,
                                    RequestDate = x.RequestDate.ToString("yyyy/M/d"),
                                    Approved = x.IsApproved ? "Yes" : "No"
                                }).ToList();

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

    }
}