using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class Announcement 
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ThumbnailImagePath { get; set; }

        [Required]
        public DateTime StartAnnouncingDate { get; set; }

        [Required]
        public DateTime EndAnnouncingDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
