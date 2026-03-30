using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<BookType> BookTypes { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<Recipt> Recipts { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === BOOK ===
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Author).IsRequired();
                entity.Property(e => e.Price).HasColumnType("numeric(10,2)");

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Books)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.BookType)
                    .WithMany(bt => bt.Books)
                    .HasForeignKey(b => b.TypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === BOOKTYPE ===
            modelBuilder.Entity<BookType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.Property(e => e.TypeName).IsRequired().HasMaxLength(150);
            });

            // === USER ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(u => u.UserRole)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.UserRoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === USERROLE ===
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(30);
            });

            // === RECIPT ===
            modelBuilder.Entity<Recipt>(entity =>
            {
                entity.HasKey(e => e.ReciptId);
                entity.Property(e => e.BillNumber).IsRequired().HasMaxLength(160);
                entity.Property(e => e.TotalAmount).IsRequired();
                entity.Property(e => e.PaymentType).IsRequired().HasMaxLength(100);
               

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Recipts)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Book)
                    .WithMany(b => b.Recipts)
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookType>().HasData(
                new BookType { TypeId = 1, TypeName = "Fiction" }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserRoleId = 1, RoleName = "Admin" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Password = "admin123",
                    Email = "admin@bookstore.com",
                    CreateDate = DateTime.UtcNow,
                    UserRoleId = 1
                }
            );

        }
    }
}
