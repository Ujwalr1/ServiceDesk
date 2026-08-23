using Microsoft.EntityFrameworkCore;
using ServiceDesk.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Data
{
    public class ServiceDeskDbContext : DbContext
    {
        public ServiceDeskDbContext(DbContextOptions<ServiceDeskDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketStatus> TicketStatuses { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship configuration will go here.

            // User -> Created Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Assigned Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category -> Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // TicketStatus -> Tickets
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> Comments
            modelBuilder.Entity<TicketComment>()
                .HasOne(c => c.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Comments
            modelBuilder.Entity<TicketComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.TicketComments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500);
            });

            // Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .IsRequired();

                entity.Property(t => t.Priority)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            // TicketStatus
            modelBuilder.Entity<TicketStatus>(entity =>
            {
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // TicketComment
            modelBuilder.Entity<TicketComment>(entity =>
            {
                entity.Property(c => c.CommentText)
                    .IsRequired();
            });

            modelBuilder.Entity<TicketStatus>().HasData(
                new TicketStatus
                {
                    Id = 1,
                    Name = "Open"
                },
                new TicketStatus
                {
                    Id = 2,
                    Name = "In Progress"
                },
                new TicketStatus
                {
                    Id = 3,
                    Name = "Resolved"
                },
                new TicketStatus
                {
                    Id = 4,
                    Name = "Closed"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Hardware",
                    Description = "Hardware related issues",
                    IsActive = true
                },
                new Category
                {
                    Id = 2,
                    Name = "Software",
                    Description = "Software related issues",
                    IsActive = true
                },
                new Category
                {
                    Id = 3,
                    Name = "Network",
                    Description = "Network and connectivity issues",
                    IsActive = true
                },
                new Category
                {
                    Id = 4,
                    Name = "Access",
                    Description = "User access and permission issues",
                    IsActive = true
                },
                new Category
                {
                    Id = 5,
                    Name = "Other",
                    Description = "Other issues",
                    IsActive = true
                }
            );

        }
    }
}
