using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        public Product() { Id = Guid.NewGuid(); }
        public string Name { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public decimal? Rate { get; set; }

        // Foreign key
        public Guid CategoryId { get; set; }
        // Navigation property
        public Category Category { get; set; }
    }
}
