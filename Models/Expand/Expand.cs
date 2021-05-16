using ExampleB.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace ExampleB.Controllers
{
    public  static class Expand
    {
        // from file to byte[]
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
        // from byte to img 
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, object dynamic = null)
        {
            var descr = (dynamic != null) ? dynamic.ToString().Replace('{', ' ').Replace('}', ' ') : "";
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            return new MvcHtmlString("<img " + descr + " src='" + img + "'  />");
        }
        // Dish table
        public static  async Task UpdateDishAsync(this GoodFit bd, Dish newdish)
        {
           
                var dish = bd.Dish.FirstOrDefault(d => d.id == newdish.id);
                dish.Img = newdish.Img?? dish.Img;
                dish.Name = newdish.Name;
                dish.Сalories = newdish.Сalories;
                dish.Contains_Meat = newdish.Contains_Meat;
                dish.Contains_Milk = newdish.Contains_Milk;
                dish.Contains_Sugar = newdish.Contains_Sugar;
            await bd.SaveChangesAsync();
            
        }
        // на прямую с бд  add dish to diet
        public  static void AddDishes(this Diet diet, int[] arr, bool save = true)
        {
            if (arr == null || arr.Length == 0) throw new ArgumentException("Массив пустой");

            using (var db = new GoodFit()) { 
            
                for (int i = 0; i < arr.Length; i++)
                {
                    int Item_Id = arr[i];
                Dish disht = db.Dish.FirstOrDefault(j => j.id == Item_Id)?? throw new Exception();
                db.Diet.FirstOrDefault(j => j.Id == diet.Id).Dish.Add(disht);
                //db.Dish.FirstOrDefault(j => j.id == Item_Id).Diet.Add(diet);
                }
           
                if(save ) db.SaveChanges();
            
           }
               
            
           
        }
      // атрибуты Diet добавить при изминении  update diet
        public static void UpdateDiet(this Diet newdiet, int[] arr)
        {

         

            using (var db = new GoodFit())
            {
                Diet diet = db.Diet.FirstOrDefault(j => j.Id == newdiet.Id);
                diet.Name = newdiet.Name;
                diet.Subscription = newdiet.Subscription;
                diet.Description = newdiet.Description;
                if (arr.Length == 0)
                {
                    db.Diet.FirstOrDefault(j => j.Id == newdiet.Id).Dish.Clear();
                    db.SaveChanges();
                    return;
                }

                foreach (var item in diet.Dish.ToList())
                {
                    if (arr.Contains(item.id))
                    {

                    }
                    else
                    {
                      
                        db.Diet.FirstOrDefault(j => j.Id == newdiet.Id).Dish.Remove(db.Dish.FirstOrDefault(i => i.id == item.id));
                    }
                    

                }
                db.SaveChanges();
                
                diet.AddDishes(arr);
                
                db.SaveChanges();

            }
            
            
           
            //db.Entry(diet).State = EntityState.Modified;
            //diet.Dish.Clear();
           
            
          //  db.Diet.FirstOrDefault(j => j.Id == diet.Id)
            
        }
    }
}