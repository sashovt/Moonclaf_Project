using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGallery.Models
{
    public class Photo
    {
        public Photo()
        {
            this.DateAdded = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public ApplicationUser Author { get; set; }


    }
}