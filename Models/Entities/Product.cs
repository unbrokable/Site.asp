using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleB
{
    public class Product
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public Product(string name, int calories)
        {
            this.Name = name;
            this.Calories = calories;
        }
    }
}