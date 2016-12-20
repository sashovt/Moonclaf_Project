using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoGallery.Models
{
    public class GalleryViewModel
    {
        public IEnumerable<Photo> photo { get; set; }
        public ICollection<Category> category { get; set; }

        public GalleryViewModel(IEnumerable<Photo> photo, ICollection<Category> category)
        {
            this.photo = photo;
            this.category = category;
        }
    }
}