namespace Core
{
    using Data;
    using System.Data.Entity;
    public class SDContext : DbContext
    {
        public SDContext()
            : base("StudentDrive")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Statistics> Statistics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Share>()
               .HasRequired(s => s.File)
               .WithMany(f => f.Shares)
               .HasForeignKey(s => s.FileId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Share>()
               .HasRequired(s => s.Owner)
               .WithMany(o => o.MyShares)
               .HasForeignKey(s => s.OwnerId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Share>()
               .HasRequired(s => s.ToUser)
               .WithMany(o => o.FriendShares)
               .HasForeignKey(s => s.ToUserId)
               .WillCascadeOnDelete(false);
        }
    }
}
