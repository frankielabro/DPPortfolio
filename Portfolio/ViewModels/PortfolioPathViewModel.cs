﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.ViewModels
{
    public class PortfolioPathViewModel
    {
        public int PortfolioId { get; set; }
        public string Title { get; set; }
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Developer's initials must contain two(2) characters.")]
        public string DeveloperInitials { get; set; }
        public int CategoryId { get; set; }
        public string Link { get; set; }
    }
}
