using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerse_Store.Models;
using E_Commerse_Store.ViewModels;
using PagedList;

namespace E_Commerse_Store.Controllers
{
    public class ProductsController : Controller
    {

        private E_Commerse_Store_DB db = new E_Commerse_Store_DB();



        // GET: Products
        public ActionResult Index(string category, string subcategory, string search, string sortBy, int? page)
        {
            //instantiate a new view model 
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            var products = db.Products.Include(p => p.Category );
            var product = db.Products.Include(k => k.SubCategory);

            //search
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) ||p.Discription.Contains(search) || p.Category.Name.Contains(search)|| p.SubCategory.Name.Contains(search));
                product = product.Where( k =>k.SubCategory.Name.Contains(search));

                viewModel.Search = search;
            }


            //group search results into categories and count how many items in each category 
            viewModel.CatsWithCount = from matchingProducts in products where matchingProducts.CategoryID != null group matchingProducts by matchingProducts.Category.Name into catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };

            var categories = products.OrderBy(p => p.Category.Name).Select(p =>p.Category.Name).Distinct();

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }



            //group search results into subcategories and count how many items in each subcategory 
            viewModel.SubCatsWithCount = from matchingProducts in products
                                      where matchingProducts.SubCategoryID != null
                                      group matchingProducts by matchingProducts.SubCategory.Name into subcatGroup
                                      select new SubCategoryWithCount()
                                      {
                                          SubCategoryName = subcatGroup.Key,
                                          ProductCount = subcatGroup.Count()
                                      };

            var subcategories = products.OrderBy(k => k.SubCategory.Name).Select(k => k.SubCategory.Name).Distinct();

            if (!String.IsNullOrEmpty(subcategory))
            {
                products = products.Where(k => k.SubCategory.Name == subcategory);
                viewModel.SubCategory = subcategory;
            }

            //sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            const int PageItems = 4;
            int currentPage = (page ?? 1);
            viewModel.Products = products.ToPagedList(currentPage, PageItems);
            viewModel.SortBy = sortBy;

            viewModel.Sorts = new Dictionary<string, string>
             {
             {"Price low to high", "price_lowest" },{"Price high to low", "price_highest" }
             };
            return View(viewModel);


           
        }



        public ActionResult Index11()
        {
            return View(db.Products.ToList());
        }




        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            ViewBag.SubCategoryID = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Discription,Price,CategoryID,SubCategoryID,SupplierID,QuantityPerUnit,UnitPrice,OldPrice,UnitWeight,Size,Discount,UnitInStock,UnitOnOrder,ProductAvailable,ImageURL,AltText,AddBadge,OfferTitle,OfferBadgeClass,LongDescription,Picture1,Picture2,Picture3,Picture4,Note")] Product product, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4)
        {

            //..\\Images\\
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                product.Picture1 = file1.FileName;
            }
            //..\\Images\\
            if (file2 != null)
            {
                file2.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file2.FileName);
                product.Picture2 = file2.FileName;
            }

            //..\\Images\\
            if (file3 != null)
            {
                file3.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file3.FileName);
                product.Picture3 = file3.FileName;
            }
            //..\\Images\\
            if (file4 != null)
            {
                file4.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file4.FileName);
                product.Picture4 = file4.FileName;
            }



            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index11");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
            ViewBag.SubCategoryID = new SelectList(db.SubCategories, "SubCategoryID", "Name", product.SubCategoryID);
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            ViewBag.SubCategoryID = new SelectList(db.SubCategories, "SubCategoryID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Discription,Price,CategoryID,SubCategoryID,SupplierID,QuantityPerUnit,UnitPrice,OldPrice,UnitWeight,Size,Discount,UnitInStock,UnitOnOrder,ProductAvailable,ImageURL,AltText,AddBadge,OfferTitle,OfferBadgeClass,LongDescription,Picture1,Picture2,Picture3,Picture4,Note")] Product product, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4)
        {

            //..\\Images\\
            if (file1 != null)
            {
                file1.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file1.FileName);
                product.Picture1 = file1.FileName;
            }
            //..\\Images\\
            if (file2 != null)
            {
                file2.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file2.FileName);
                product.Picture2 = file2.FileName;
            }

            //..\\Images\\
            if (file3 != null)
            {
                file3.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file3.FileName);
                product.Picture3 = file3.FileName;
            }
            //..\\Images\\
            if (file4 != null)
            {
                file4.SaveAs(HttpContext.Server.MapPath("~/Content/Image/") + file4.FileName);
                product.Picture4 = file4.FileName;
            }



            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index11");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", product.SupplierID);
            ViewBag.SubCategoryID = new SelectList(db.SubCategories, "SubCategoryID", "Name", product.SubCategoryID);
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index11");
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
