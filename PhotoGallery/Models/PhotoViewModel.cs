using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoGallery.Models
{
    public class PhotoViewModel
    {
        public PhotoViewModel()
        {
            this.DateAdded = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }

    }
}