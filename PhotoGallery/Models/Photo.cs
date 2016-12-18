using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PhotoGallery.Models
{
    public class Photo
    {

        public Photo()
        {
        }

        public Photo(string authorId,string title, byte[] image,int categoryId,DateTime dateAdded)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Image = image;
            this.CategoryId = categoryId;
            this.DateAdded = dateAdded;

        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
       
        [Required]
        public byte[] Image { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}