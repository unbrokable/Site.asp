namespace ExampleB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DietView")]
    public partial class DietView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool Subscription { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? Meat { get; set; }

        public int? Milk { get; set; }

        public int? Sugar { get; set; }

        [Column(TypeName = "image")]
        public byte[] Img { get; set; }

        public int? Amout { get; set; }

        public int? Avg { get; set; }
    }
}
