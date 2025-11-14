using Microsoft.EntityFrameworkCore;
using RoomReservation_Item_I13L.Data.Entities;

namespace RoomReservation_Item_I13L.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Rooms");
                entity.HasKey(r => r.Id);
                
                entity.Property(r => r.RoomName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("RoomName");
                
                entity.Property(r => r.RoomType)
                    .HasMaxLength(100)
                    .HasColumnName("RoomType");
                
                entity.Property(r => r.Capacity)
                    .IsRequired()
                    .HasColumnName("Capacity");
                
                entity.Property(r => r.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Status");
                
                entity.Property(r => r.Description)
                    .HasColumnName("Description");
                
                entity.Property(r => r.CreatedAt)
                    .IsRequired()
                    .HasColumnName("CreatedAt");
                
                entity.Property(r => r.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("UpdatedAt");

                entity.HasIndex(r => r.RoomName).HasDatabaseName("IX_Room_RoomName");
                entity.HasIndex(r => r.Status).HasDatabaseName("IX_Room_Status");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservations");
                entity.HasKey(r => r.Id);
                
                entity.Property(r => r.RoomName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("RoomName");
                
                entity.Property(r => r.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("CustomerName");
                
                entity.Property(r => r.ContactNumber)
                    .HasMaxLength(20)
                    .HasColumnName("ContactNumber");
                
                entity.Property(r => r.Email)
                    .HasMaxLength(255)
                    .HasColumnName("Email");
                
                entity.Property(r => r.CheckInDate)
                    .IsRequired()
                    .HasColumnName("CheckInDate");
                
                entity.Property(r => r.CheckOutDate)
                    .IsRequired()
                    .HasColumnName("CheckOutDate");
                
                entity.Property(r => r.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Status");
                
                entity.Property(r => r.PaymentStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("PaymentStatus");
                
                entity.Property(r => r.CreatedAt)
                    .IsRequired()
                    .HasColumnName("CreatedAt");
                
                entity.Property(r => r.UpdatedAt)
                    .IsRequired()
                    .HasColumnName("UpdatedAt");

                entity.HasIndex(r => r.Status).HasDatabaseName("IX_Reservation_Status");
                entity.HasIndex(r => r.CustomerName).HasDatabaseName("IX_Reservation_CustomerName");
                entity.HasIndex(r => r.CheckInDate).HasDatabaseName("IX_Reservation_CheckInDate");
                entity.HasIndex(r => r.CheckOutDate).HasDatabaseName("IX_Reservation_CheckOutDate");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");
            });
        }
    }
}
