

namespace Award.Core.Entities
{
    public class EmployeeTeams : BaseEntity
    {
        public long TeamId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long StatusId { get; set; }
        public virtual User User { get; set; }
        public virtual Teams Team { get; set; }
    }
}
