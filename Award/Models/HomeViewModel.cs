using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Award.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Award.Models
{
    public class HomeViewModel
    {
        public List<Announcement> Announcements { get; set; }

        public List<Awards> Awards { get; set; }
        public List<Category> Categories { get; set; }
        public List<AnnouncementHome> AnnouncementHome { get; set; }
        public List<VideosHome> VideosHome { get; set; }
        public List<ShowWinnersViewModel> AnnouncedWinners { get; set; }
        

        public string CurrentUserRole { get; set; }
        public string CurrentUserImage { get; set; }
        public long CurrentUserRoleCount { get; set; }


    }
}
