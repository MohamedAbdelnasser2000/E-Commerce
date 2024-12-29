using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerse_Store.Models;

namespace E_Commerse_Store.Controllers
{
    public class BannersController : Controller
    {
        private E_Commerse_Store_DB db = new E_Commerse_Store_DB();

        // GET: Banners
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }

        // GET: Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Banners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title1,Title2,Title3,Discription1,Discription2,Discription3,Picture1,Picture2,Picture3")] Banner banner, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                banner.Picture1 = file1.FileName;
            }
            //..\\Images\\
            if (file2 != null)
            {
                file2.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file2.FileName);
                banner.Picture2 = file2.FileName;
            }
            if (file3 != null)
            {
                file3.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file3.FileName);
                banner.Picture3 = file3.FileName;
            }


            if (ModelState.IsValid)
            {
                db.Banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Banners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title1,Title2,Title3,Discription1,Discription2,Discription3,Picture1,Picture2,Picture3")] Banner banner, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                banner.Picture1 = file1.FileName;
            }
            //..\\Images\\
            if (file2 != null)
            {
                file2.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file2.FileName);
                banner.Picture2 = file2.FileName;
            }
            if (file3 != null)
            {
                file3.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file3.FileName);
                banner.Picture3 = file3.FileName;
            }

            if (ModelState.IsValid)
            {
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        // GET: Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
