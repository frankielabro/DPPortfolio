using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.ViewModels
{
    public class PortfolioWithCategoryViewModel
    {

        public int PortfolioId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string DeveloperInitials { get; set; }
        public string Link { get; set; }
        public virtual Category Category { get; set; }
    }
}
