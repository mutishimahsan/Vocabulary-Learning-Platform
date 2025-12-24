using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Primary constructor for runtime
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ADD THIS: Parameterless constructor for design-time (migrations)
        public AppDbContext()
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Deck> Decks => Set<Deck>();
        public DbSet<Word> Words => Set<Word>();
        public DbSet<ReviewLog> ReviewLogs => Set<ReviewLog>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This runs during design-time (when creating migrations)
            if (!optionsBuilder.IsConfigured)
            {
                // Use this simple LocalDB connection string
                // Remove the extra backslash - use single backslash or double backslash correctly
                optionsBuilder.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=VocabLearningDb;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");

                // OR try this if above doesn't work:
                // optionsBuilder.UseSqlServer(
                //     "Server=.\\SQLEXPRESS;Database=VocabLearningDb;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");

                // Enable logging to see what's happening
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();

                // Relationships - REMOVE Cascade delete here
                entity.HasMany(e => e.Decks)
                    .WithOne(e => e.Owner)
                    .HasForeignKey(e => e.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Changed from Cascade

                entity.HasMany(e => e.ReviewLogs)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Changed from Cascade
            });

            modelBuilder.Entity<Deck>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).HasMaxLength(100);

                // Relationship - KEEP Cascade delete here
                entity.HasMany(e => e.Words)
                    .WithOne(e => e.Deck)
                    .HasForeignKey(e => e.DeckId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Term).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Definition).HasMaxLength(1000);
                entity.Property(e => e.ExampleSentence).HasMaxLength(500);
                entity.Property(e => e.PartOfSpeech).HasMaxLength(50);
                entity.Property(e => e.PronunciationUrl).HasMaxLength(500);

                // Relationships - REMOVE Cascade delete here
                entity.HasMany(e => e.ReviewLogs)
                    .WithOne(e => e.Word)
                    .HasForeignKey(e => e.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull); // Changed from Cascade
            });

            modelBuilder.Entity<ReviewLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LastReviewed).IsRequired();
                entity.Property(e => e.NextReviewDate).IsRequired();
                entity.Property(e => e.SuccessRate).HasDefaultValue(0);
                entity.Property(e => e.IntervalLevel).HasDefaultValue(1);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}