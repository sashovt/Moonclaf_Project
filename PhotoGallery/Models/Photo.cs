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
<<<<<<< HEAD
        public Photo(ApplicationUser Author,string Title, byte[] Image, int CategoryId)
        {
            this.DateAdded = DateTime.Now;
            this.Author = Author;
            this.Title = Title;
            this.Image = Image;
        }
=======

        public Photo(string authorId,string title,byte[] image,int categoryId)
        {
            this.Author.Id = authorId;
            this.Title = title;
            this.Image = image;
            this.CategoryId = categoryId;
        }

>>>>>>> 1245cf809a1263c5626fbb23fe26cad8a6535df1
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