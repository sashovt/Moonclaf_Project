using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Photo(ApplicationUser Author,string Title, byte[] Image, int CategoryId)
        {
            this.DateAdded = DateTime.Now;
            this.Author = Author;
            this.Title = Title;
            this.Image = Image;
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