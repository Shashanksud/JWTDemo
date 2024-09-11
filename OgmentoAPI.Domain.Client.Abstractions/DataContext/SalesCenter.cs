﻿using OgmentoAPI.Domain.Common.Abstractions.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgmentoAPI.Domain.Client.Abstractions.DataContext
{
    public class SalesCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public virtual ICollection<SalesCenterUserMapping> SalesCenterUsers { get; set; }

    }

}
