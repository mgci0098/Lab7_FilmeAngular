using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentruMultimedia.Models
{
    public class FilmeDbContext : DbContext

    {
        public FilmeDbContext(DbContextOptions<FilmeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
            });

            builder.Entity<Comentariu>()
                .HasOne(c => c.Film)
                .WithMany(c => c.Comentarii)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Comentariu>()
                .HasOne(c => c.AddedBy)
                .WithMany(c => c.Comentarii)
                .OnDelete(DeleteBehavior.Cascade);

            //TODo: daca creapa la stergere in cascada cu User owner
            //builder.Entity<Film>()
            //   .HasOne(f => f.Owner)
            //   .WithMany(u => u.Filme)
            //   .OnDelete(DeleteBehavior.Cascade);

        }


        public DbSet<Film> Filme { get; set; }

        public DbSet<Comentariu> Comentarii { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserUserRole> UserUserRoles { get; set; }

    }
}
