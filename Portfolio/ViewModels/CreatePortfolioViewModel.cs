using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.ViewModels
{
    public class CreatePortfolioViewModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string DeveloperInitials { get; set; }
        public int CategoryId { get; set; }
    }
}
