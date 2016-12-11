using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
   
    public class Category
    {
        private ICollection<Photo> photos;
        public Category()
        {
            this.photos = new HashSet<Photo>();
        }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
        
    }
}