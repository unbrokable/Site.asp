namespace ExampleB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class History_user_diet
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime Date_write { get; set; }

        public int? Calories_Amount { get; set; }

        public virtual Users Users { get; set; }
    }
}
