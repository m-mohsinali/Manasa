using Microsoft.EntityFrameworkCore;
using Award.Core.Entities;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Award.Infrastructure.Data
{

    public class AwardDbContext : IdentityDbContext
    {
        public AwardDbContext(DbContextOptions<AwardDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.HasDefaultSchema("award");

            #region Sector

            builder.Entity<Sector>().HasKey(a => a.Id);
            builder.Entity<Sector>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Sector>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Sector>().Property(a => a.NameEn).HasMaxLength(300);
            builder.Entity<Sector>().Property(a => a.CreateDate);

            #endregion

            #region Department

            builder.Entity<Department>().HasKey(a => a.Id);
            builder.Entity<Department>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Department>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Department>().Property(a => a.NameEn).HasMaxLength(300);
            builder.Entity<Department>().Property(a => a.CreateDate);

            #endregion

            #region Section

            builder.Entity<Section>().HasKey(a => a.Id);
            builder.Entity<Section>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Section>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Section>().Property(a => a.NameEn).HasMaxLength(300);
            builder.Entity<Section>().Property(a => a.CreateDate);

            #endregion

            #region Branch

            builder.Entity<Branch>().HasKey(a => a.Id);
            builder.Entity<Branch>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Branch>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Branch>().Property(a => a.NameEn).HasMaxLength(300);
            builder.Entity<Branch>().Property(a => a.CreateDate);

            #endregion

            #region Unit

            builder.Entity<Unit>().HasKey(a => a.Id);
            builder.Entity<Unit>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Unit>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Unit>().Property(a => a.NameEn).HasMaxLength(300);
            builder.Entity<Unit>().Property(a => a.CreateDate);

            #endregion

            #region Employee

            builder.Entity<Employee>().HasKey(a => a.Id);
            builder.Entity<Employee>().Property(a => a.Id).ValueGeneratedOnAdd();
            //builder.Entity<Employee>().HasOne(e => e.Sector).WithMany(e => e.Employee).HasForeignKey(e => e.IdSector);
            //builder.Entity<Employee>().HasOne(e => e.Department).WithMany(e => e.Employees).HasForeignKey(e => e.IdDepartment).IsRequired(false);
            //builder.Entity<Employee>().HasOne(e => e.Section).WithMany(e => e.Employees).HasForeignKey(e => e.IdSection)
            //    .IsRequired(false);
            //builder.Entity<Employee>().HasOne(e => e.Branch).WithMany(e => e.Employees).HasForeignKey(e => e.IdBranch)
            //    .IsRequired(false);
            //builder.Entity<Employee>().HasOne(e => e.Unit).WithMany(e => e.Employees).HasForeignKey(e => e.IdUnit)
            //    .IsRequired(false);
            builder.Entity<Employee>().HasIndex(a => a.UserDomain).IsUnique();
            builder.Entity<Employee>().Property(a => a.UserDomain).HasMaxLength(50).IsRequired();
            builder.Entity<Employee>().Property(a => a.Grp).HasMaxLength(50).IsRequired();
            builder.Entity<Employee>().Property(a => a.SexEn).HasMaxLength(12);
            builder.Entity<Employee>().Property(a => a.SexAr).HasMaxLength(12);
            builder.Entity<Employee>().Property(a => a.NameAr).HasMaxLength(300);
            builder.Entity<Employee>().Property(a => a.NameEn).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.RankAr).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.RankEn).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.JobAr).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.JobEn).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.ClassAr).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.ClassEn).HasMaxLength(300).IsRequired(false);
            builder.Entity<Employee>().Property(a => a.EmployeePhotoUrl).HasMaxLength(500);
            builder.Entity<Employee>().Property(a => a.LastUpdateAt);

            #endregion

            #region EmployeeTeams
            builder.Entity<EmployeeTeams>().HasKey(a => a.Id);
            builder.Entity<EmployeeTeams>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<EmployeeTeams>().HasIndex(b => new { b.UserId, b.TeamId,b.RoleId }).IsUnique();
            builder.Entity<EmployeeTeams>().HasOne(e => e.Team).WithMany(e => e.EmployeeTeams)
                .HasForeignKey(e => e.TeamId);

            #endregion

            #region Teams

            builder.Entity<Teams>().HasKey(a => a.Id);
            builder.Entity<Teams>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<Teams>().Property(a => a.Name).HasMaxLength(300).IsRequired();
            builder.Entity<Teams>().Property(a => a.CreateDate).IsRequired();
            builder.Entity<Teams>().Property(a => a.DeletedDate).IsRequired(false);

            #endregion


            #region CategoryTeamEntry

            builder.Entity<CategoryTeamEntry>().HasKey(a => a.Id);
            builder.Entity<CategoryTeamEntry>().Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Entity<CategoryTeamEntry>().Property(a => a.CreateDate).IsRequired();
            //builder.Entity<CategoryTeamEntries>().Property(a => a.DeletedDate).IsRequired(false);

            #endregion

            #region User

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            #endregion

            #region Role
            builder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
            #endregion

            #region UserRole
            builder.Entity<UserRole>(entity =>
               {
                   entity.HasKey(e => new { e.UserId, e.RoleId })
                       .HasName("PK_UserRoles");

                   entity.HasOne(d => d.Role)
                       .WithMany(p => p.UserRoles)
                       .HasForeignKey(d => d.RoleId)
                       .HasConstraintName("FK_UserRoles_Roles");

                   entity.HasOne(d => d.User)
                       .WithMany(p => p.UserRoles)
                       .HasForeignKey(d => d.UserId)
                       .HasConstraintName("FK_UserRoles_Users");
               });
            #endregion

            //builder.Entity<ManasaEmployee>().ToTable("vwManasaEmployees");

            builder.Entity<CategoryCriteria>().HasOne(c => c.Category).WithMany(c => c.CategoryCriteria)
            .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CategorySubCriteria>().HasOne(c => c.CategoryCriteria).WithMany(c => c.CategorySubCriteria)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QualitySectorManager>(entity =>
            {
                builder.Entity<Section>().HasKey(a => a.Id);

                entity.HasIndex(p => new { p.SectorId, p.UserId }).IsUnique();

                entity.HasOne(d => d.Sector)
                       .WithMany(p => p.QualitySectorManagers)
                       .HasForeignKey(d => d.SectorId);
            });

            //builder.Entity<CategoryTeamEntries>().HasKey(a => a.Id);  
            


        }

        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<AnnouncementHome> AnnouncementHome { get; set; }
        public virtual DbSet<VideosHome> VideosHome { get; set; }
        public virtual DbSet<Awards> Awards { get; set; }
        public virtual DbSet<AwardDocuments> AwardDocuments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryDocument> CategoryDocuments { get; set; }
        public virtual DbSet<CategoryCriteria> CategoryCriterias { get; set; }
        public virtual DbSet<CategoryCriteriaDocument> CategoryCriteriaDocuments { get; set; }
        public virtual DbSet<CategorySubCriteria> CategorySubCriterias { get; set; }

        public virtual DbSet<CategoryEntry> CategoryEntries { get; set; }

        public virtual DbSet<CategorySectorEntry> CategorySectorEntries { get; set; }
        public virtual DbSet<CategoryTeamEntry> CategoryTeamEntries { get; set; }

        public virtual DbSet<TeamEntry> TeamEntry { get; set; }
        public virtual DbSet<AuditingUserAnswers> AuditingUserAnswer { get; set; }
        public virtual DbSet<QSMEntries> QSMEntries { get; set; }
        public virtual DbSet<UserAnswers> UserAnswers { get; set; }
        public virtual DbSet<UserAnswersComments> UserAnswersComments { get; set; }
        //public virtual DbSet<QsmSector> QsmSector { get; set; }

        //public virtual DbSet<EntryStatus> EntryStatus { get; set; }
        //public virtual DbSet<CategoryEntryComments> CategoryEntryComments { get; set; }

        public virtual DbSet<Sector> Sectors { get; set; }
        //public virtual DbSet<UserSector> UserSector { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        //public virtual DbSet<CategorySector> CategoriesSectors { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<LastLogInUser> LastLogInUser { get; set; }

        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<EmployeeTeams> EmployeeTeams { get; set; }
        //public virtual DbSet<CategoryTeamEntry> CategoryTeams { get; set; }
        //public virtual DbSet<ManasaEmployee> ManasaEmployees { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Winner> Winners { get; set; }
        public virtual DbSet<QualitySectorManager> QualitySectorManagers { get; set; }
        public virtual DbSet<CriteriaDocument> CriteriaDocuments { get; set; }
        public virtual DbSet<Forum> Forum { get; set; }
        public virtual DbSet<ForumDetails> ForumDetails { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<UserEvent> UserEvents { get; set; }
        public virtual DbSet<WinnerAnnouncement> WinnerAnnouncements { get; set; }

        public virtual DbSet<AwardsType> AwardsType { get; set; }


        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<JuryTeam> JuryTeam { get; set; }

    }
}
