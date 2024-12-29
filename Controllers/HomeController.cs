using E_Commerse_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerse_Store.Controllers
{
    public class HomeController : Controller
    {

      private  E_Commerse_Store_DB db = new E_Commerse_Store_DB();
        List<Cart> li = new List<Cart>();

        public ActionResult Index()
        {
            if (TempData["cart"] != null)
            {
                int x = 0;
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;
                }
                TempData["total"] = x;
                TempData["item_count"] = li2.Count();
            }
            TempData.Keep();

            var query = db.v_getallproduct.ToList();
            return View(query);
        }



        public ActionResult Single(int id)
        {
            var query = db.v_getallproduct.FirstOrDefault(m =>m.Id == id);

            return View(query);
        }

        public ActionResult AddtoCart(int id)
        {
            var query = db.Products.Where(x => x.Id == id).SingleOrDefault();

            return View(query);
        }
        [HttpPost]
        public ActionResult AddtoCart(int id , int qty )
        {
            Product p = db.Products.Where(x => x.Id == id).SingleOrDefault();
            Cart c = new Cart();
            c.proid = id;
            c.proname = p.Name;
            c.price = Convert.ToInt32(p.Price);
            c.qty = qty;
            c.bill = c.price * c.qty;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.proid ==c.proid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }
                }
                if (flag ==0)
                {
                    li2.Add(c);
                }
                TempData["cart"] = li2;

            }
            TempData.Keep();
            return RedirectToAction("Index");
        }




        public ActionResult Checkout()
        {
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string contact, string address, string phone, string name)
        {
            if (ModelState.IsValid)
            {
                AspNetUser u = new AspNetUser();

                List<Cart> li2 = TempData["cart"] as List<Cart>;
                Invoice iv = new Invoice();
                iv.UserId = u.Id;
                iv.InvoiceDate = System.DateTime.Now;
                iv.Bill = (int)TempData["total"];
                iv.Payment = "cash";
                db.Invoices.Add(iv);
                db.SaveChanges();
                //order book
                foreach (var item in li2)
                {
                    Order od = new Order();
                    od.ProId = item.proid;
                    od.Name = name;
                    od.Phone = phone;
                    od.Contact = contact;
                    od.Address = address;
                    od.OrderDate = System.DateTime.Now;
                    od.InvoiceId = iv.InvoiceId;
                    od.Qty = item.qty;
                    od.Unit = item.price;
                    od.Total = item.bill;

                    db.Orders.Add(od);
                    db.SaveChanges();

                }
                TempData.Remove("total");
                TempData.Remove("cart");
                // TempData["msg"] = "Order Book Successfully!!";
                return RedirectToAction("Index");
            }

            TempData.Keep();
            return View();
        }

        public ActionResult remove(int? id)
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("total");
                TempData.Remove("cart");
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                Cart c = li2.Where(x => x.proid == id).SingleOrDefault();
                li2.Remove(c);
                int s = 0;
                foreach (var item in li2)
                {
                    s += item.bill;
                }

                TempData["total"] = s;

            }

            return RedirectToAction("Checkout");
        }


        public ActionResult GetAllOrderDetail()
        {
            var query = db.getallOrders.ToList();
            return View(query);
        }


        public ActionResult ConfirmOrder(int invoiceId)
        {
            var query = db.getallOrders.SingleOrDefault(m => m.InvoiceId == invoiceId);
            return View(query);
        }
        [HttpPost]
        public ActionResult ConfirmOrder(getallOrder o)
        {
            Invoice inv = new Invoice()
            {
                InvoiceId = o.InvoiceId,
                Bill = o.Bill,
                Payment = o.Payment,
                InvoiceDate = o.InvoiceDate,
                Status = 1,
            };



            db.Entry(inv).State = EntityState.Modified;
            db.SaveChanges();

            return View();

        }









        public ActionResult About(string id)
        {
            ViewBag.Message = "Your application description page. You entered the ID " + id;

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            //return View();


            var query = db.v_getallproduct.ToList();

            return View(query);
        }
    }
}