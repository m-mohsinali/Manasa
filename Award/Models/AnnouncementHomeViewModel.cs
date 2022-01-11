using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Award.Models
{
    public class AnnouncementHomeViewModel
    {
        public AnnouncementHome AnnouncementHome { get; set; }

        public IFormFile ThumbnailImage { get; set; }
    }
}
