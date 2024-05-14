﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category() { Id = Guid.NewGuid(); }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
    }
}
