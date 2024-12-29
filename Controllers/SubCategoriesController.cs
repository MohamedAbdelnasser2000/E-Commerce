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
    public class SubCategoriesController : Controller
    {
        private E_Commerse_Store_DB db = new E_Commerse_Store_DB();

        // GET: SubCategories
        public ActionResult Index()
        {
            var subCategories = db.SubCategories.Include(s => s.Category);
            return View(db.SubCategories.OrderBy(s => s.Name).ToList());
        }

        public ActionResult Index1()
        {
            var subCategories = db.SubCategories.Include(s => s.Category);
            return View(subCategories.ToList());
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryID,CategoryID,Name,Description,Picture1,Picture2,isActive")] SubCategory subCategory, HttpPostedFileBase file, HttpPostedFileBase file1)
        {
            //..\\Images\\
            if (file != null)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file.FileName);
                subCategory.Picture1 = file.FileName;
            }
            //..\\Images\\
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                subCategory.Picture2 = file1.FileName;
            }
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", subCategory.CategoryID);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryID,CategoryID,Name,Description,Picture1,Picture2,isActive")] SubCategory subCategory, HttpPostedFileBase file, HttpPostedFileBase file1)
        {


            //..\\Images\\
            if (file != null)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file.FileName);
                subCategory.Picture1 = file.FileName;
            }
            //..\\Images\\
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                subCategory.Picture2 = file1.FileName;
            }

            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategory);
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
