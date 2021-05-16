using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleB.Models
{
    public class DishTime
    {
        public DateTime Time { get; set; }
        public decimal Opacity { get; set; }
        public int id { get; set; }

        public string Name { get; set; }

        public int Сalories { get; set; }

        public byte[] Img { get; set; }

        public bool? Contains_Meat { get; set; }

        public bool? Contains_Milk { get; set; }

        public bool? Contains_Sugar { get; set; }

        public DishTime()
        {

        }
        public DishTime(Dish dish , DateTime date)
        {
            this.id = dish.id;
            this.Img = dish.Img;
            this.Contains_Meat = dish.Contains_Meat;
            this.Contains_Milk = dish.Contains_Milk;
            this.Contains_Sugar = dish.Contains_Sugar;
            this.Name = dish.Name;
            this.Сalories = dish.Сalories;
            DateTime now = DateTime.Now;
            TimeSpan divide = now - date;

            this.Opacity = ((decimal)(24 - divide.Hours) / 24);

            this.Time = date;
        }
    }
}