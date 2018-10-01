using System.ComponentModel.DataAnnotations.Schema;


namespace Portfolio.Models
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string DeveloperInitials { get; set; }
        public string Link { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
