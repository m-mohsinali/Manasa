using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class ForumCategoryViewModel
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AwardName { get; set; }
        public AwardType AwardType { get; set; }
        public long TotalTopics { get; set; }
    }
    public class ForumTopicsViewModel
    {
        public long ForumId { get; set; }
        public long Categoryid { get; set; }
        public string CategoryName { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ForumTopic { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedDate { get; set; }
        public long TotalReply { get; set; }
    }

    public class ForumTopicsReplyViewModel
    {
        public long ReplyId { get; set; }
        public long Categoryid { get; set; }
        public string CategoryName { get; set; }
        public long ForumId { get; set; }
        public string ForumTopic { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ForumTopicReply { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
