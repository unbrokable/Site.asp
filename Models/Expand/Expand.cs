using ExampleB.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExampleB.Controllers
{
    public  static class Expand
    {
        public static byte[] ToByte( this HttpPostedFileBase img )
        {
            if (img == null) return null;
            try { 
            var arr = new byte[img.ContentLength];
            img.InputStream.Read(arr, 0, img.ContentLength);
            return arr;
            }
            catch
            {

            }
            return null;
        }

        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, object dynamic = null)
        {
            var descr = (dynamic != null) ? dynamic.ToString().Replace('{', ' ').Replace('}', ' ') : "";
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            return new MvcHtmlString("<img " + descr + " src='" + img + "'  />");
        }

        public static  void UpdateDishAsync(this GoodFit bd, Dish newdish)
        {
           
                var dish = bd.Dish.FirstOrDefault(d => d.id == newdish.id);
                dish.Img = newdish.Img;
                dish.Name = newdish.Name;
                dish.Сalories = newdish.Сalories;
                dish.Contains_Meat = newdish.Contains_Meat;
                dish.Contains_Milk = newdish.Contains_Milk;
                dish.Contains_Sugar = newdish.Contains_Sugar;
            bd.SaveChangesAsync();
            
        }
        public  static void AddDishes(this Diet diet, int[] arr)
        {
            if (arr == null || arr.Length == 0) throw new ArgumentException("Массив пустой");

                 var db = new GoodFit();
            
                for (int i = 0; i < arr.Length; i++)
                {
                    int Item_Id = arr[i];
                Dish disht = db.Dish.FirstOrDefault(j => j.id == Item_Id)?? throw new Exception();
                db.Diet.FirstOrDefault(j => j.Id == diet.Id).Dish.Add(disht);
                //db.Dish.FirstOrDefault(j => j.id == Item_Id).Diet.Add(diet);
                }
           
                db.SaveChanges();
            
           
               
            
           
        }
        public static void DeleteDishes(this Diet diet, int[] arr)
        {
            var db = new GoodFit();
            db.Diet.FirstOrDefault(j => j.Id == diet.Id).Dish.Clear();
            db.Diet.FirstOrDefault(j => j.Id == diet.Id).AddDishes(arr);
            db.SaveChanges();
        }
    }
}