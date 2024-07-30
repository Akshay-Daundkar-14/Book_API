using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class Book
    {
        
            public int BookID { get; set; }

            [Required]
            [StringLength(100)]
            public string Title { get; set; }

            [Required]
            [StringLength(100)]
            public string Author { get; set; }

            [StringLength(50)]
            public string Genre { get; set; }

            [Range(0, int.MaxValue)]
            public int PublishedYear { get; set; }

            [Range(0, int.MaxValue)]
            public int Price { get; set; }

            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
            public bool IsDeleted { get; set; }
        
    }

}
