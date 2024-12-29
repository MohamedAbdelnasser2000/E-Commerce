using E_Commerse_Store.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace E_Commerse_Store.ViewModels
{
    public class ProductIndexViewModel
    {

        public IPagedList<Product> Products { get; set; }
        public string Search { get; set; }
        public IEnumerable<CategoryWithCount> CatsWithCount { get; set; }
        public IEnumerable<SubCategoryWithCount> SubCatsWithCount { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

        public string SortBy { get; set; }
        public Dictionary<string, string> Sorts { get; set; }

        public IEnumerable<SelectListItem> CatFilterItems
        {
            get
            {
                var allCats = CatsWithCount.Select(cc => new SelectListItem{Value = cc.CategoryName,Text = cc.CatNameWithCount});
                return allCats;
            }
        }


        public IEnumerable<SelectListItem> SubCatFilterItems
        {
            get
            {
                var allSubCats = SubCatsWithCount.Select(ss => new SelectListItem { Value = ss.SubCategoryName, Text = ss.SubCatsWithCount });
                return allSubCats;
            }
        }
    }
    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + " (" + ProductCount.ToString() + ")";
            }
        }



    }

    public class SubCategoryWithCount
    {
        public int ProductCount { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCatsWithCount
        {
            get
            {
                return SubCategoryName + " (" + ProductCount.ToString() + ")";
            }
        }



    }




}