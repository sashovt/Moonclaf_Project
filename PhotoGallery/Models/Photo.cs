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
        //public bool IsAuthor(string name)
            //{
            //return this.Author.Email.Equals(name);
            //}

        public Photo()
        {
            this.DateAdded = DateTime.Now;
        }

        public Photo(string authorId,string title,byte[] image,int categoryId)
        {
            this.Author.Id = authorId;
            this.Title = title;
            this.Image = image;
            this.CategoryId = categoryId;
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

        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }

    }
}