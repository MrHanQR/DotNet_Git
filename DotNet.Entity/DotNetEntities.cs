namespace DotNet.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DotNetEntities : DbContext
    {
        public DotNetEntities()
            : base("name=DbConnectionString")
        {
        }
        public static void InitDataBase()
        {
            DotNetEntities db=new DotNetEntities();
            db.Database.CreateIfNotExists();
        }
        public virtual DbSet<StateBug> StateBug { get; set; }
        public virtual DbSet<PermissButton> PermissButton { get; set; }
        public virtual DbSet<PermissDepartment> PermissDepartment { get; set; }
        public virtual DbSet<StateLoginLog> StateLoginLog { get; set; }
        public virtual DbSet<PermissMenu> PermissMenu { get; set; }
        public virtual DbSet<PermissRefMenuButton> PermissRefMenuButton { get; set; }
        public virtual DbSet<PermissRefRoleMenuButton> PermissRefRoleMenuButton { get; set; }
        public virtual DbSet<PermissRefUserDepartment> PermissRefUserDepartment { get; set; }
        public virtual DbSet<PermissRefUserMenuButton> PermissRefUserMenuButton { get; set; }
        public virtual DbSet<PermissRefUserRole> PermissRefUserRole { get; set; }
        public virtual DbSet<PermissRole> PermissRole { get; set; }
        public virtual DbSet<PermissUserLogin> PermissUserLogin { get; set; }
        public virtual DbSet<StateOperateLog> StateOperateLog { get; set; }
        public virtual DbSet<PermissUserDetails> PermissUserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StateBug>()
                .Property(e => e.UserIp)
                .IsUnicode(false);

            modelBuilder.Entity<PermissButton>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<PermissButton>()
                .Property(e => e.HttpMethod)
                .IsUnicode(false);

            modelBuilder.Entity<PermissButton>()
                .Property(e => e.ActionNameCode)
                .IsUnicode(false);

            modelBuilder.Entity<StateLoginLog>()
                .Property(e => e.UserIp)
                .IsUnicode(false);

            modelBuilder.Entity<PermissMenu>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<PermissMenu>()
                .Property(e => e.ControllerNameCode)
                .IsUnicode(false);

            modelBuilder.Entity<PermissMenu>()
                .Property(e => e.ActionNameCode)
                .IsUnicode(false);
            modelBuilder.Entity<PermissMenu>().Property(e => e.HaveChild).IsOptional();
            modelBuilder.Entity<PermissMenu>().Property(e => e.ParentId).IsOptional();
            modelBuilder.Entity<PermissUserLogin>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<PermissUserLogin>()
                .Property(e => e.LoginPwd)
                .IsUnicode(false);

            modelBuilder.Entity<PermissUserLogin>()
                .Property(e => e.UserEmail)
                .IsUnicode(false);

            modelBuilder.Entity<StateOperateLog>()
                .Property(e => e.UserIp)
                .IsUnicode(false);

            modelBuilder.Entity<PermissUserDetails>().Property(e => e.UserId).IsOptional();
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.RealName).IsOptional().IsUnicode(true);
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.Birth).IsOptional();
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.Gender).IsOptional();
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.IdentityCardNumber).IsOptional().IsUnicode(false);
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.Address).IsOptional().IsUnicode(true);
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.PhoneNumber).IsOptional().IsUnicode(false);
            modelBuilder.Entity<PermissUserDetails>().Property(e => e.Description).IsOptional().IsUnicode(true);

            modelBuilder.Entity<PermissDepartment>().Property(e => e.ParentId).IsOptional();

            modelBuilder.Entity<PermissRole>().Property(e => e.DepartmentId).IsRequired();
        }
    }
}
