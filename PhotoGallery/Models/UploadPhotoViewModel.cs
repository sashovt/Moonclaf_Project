using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace PhotoGallery.Models
{
    public class UploadPhotoViewModel
    {
        public UploadPhotoViewModel()
        {
            this.DateAdded = DateTime.Now;
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

        public IEnumerable<SelectList> Categories { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}