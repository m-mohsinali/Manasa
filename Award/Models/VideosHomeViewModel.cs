using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Award.Models
{
    public class VideosHomeViewModel
    {
        public VideosHome VideosHome { get; set; }

        public IFormFile ThumbnailImage { get; set; }
    }
}
