using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGallery.Models
{
    public class PhotoViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public string AuthorId { get; set; }


    }
}