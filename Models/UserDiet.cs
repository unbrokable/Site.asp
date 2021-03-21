namespace ExampleB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserDiet")]
    public partial class UserDiet
    {
        public int Id { get; set; }

        public int? Diet_Id { get; set; }

        public int? User_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_Start { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_End { get; set; }

        public bool Used { get; set; }

        public virtual Diet Diet { get; set; }

        public virtual Users Users { get; set; }
    }
}
