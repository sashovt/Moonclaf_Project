using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoGallery.Models
{
    public class BannerModel
    {

        public byte[] Image { get; set; }

        [StringLength(200)]
        public string Search;


    }
}