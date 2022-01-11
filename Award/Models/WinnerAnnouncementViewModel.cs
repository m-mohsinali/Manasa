using Award.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class WinnerAnnouncementViewModel
    {
        public WinnerAnnouncement winnerAnnouncement { get; set; }
        public IFormFile winnerImagePath { get; set; }
    }
}
