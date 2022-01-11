using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class MeetingInvitationViewModel
    {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public string EventDescription { get; set; }
        //public DateTime StartTime { get; set; }
        public string ErrorMessage { get; set; }
        public long SaveStatus { get; set; }
        private DateTime _returnDateStart = DateTime.MinValue;
        private DateTime _returnDateEnd = DateTime.MinValue;

        public DateTime StartTime
        {
            get
            {
                return (_returnDateStart == DateTime.MinValue) ? DateTime.Now : _returnDateStart;
            }
            set { _returnDateStart = value; }
        }
        public DateTime EndTime
    {
            get
            {
                return (_returnDateEnd == DateTime.MinValue) ? DateTime.Now : _returnDateEnd;
            }
            set { _returnDateEnd = value; }
        }
    }
}
