using CinemaApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApi.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Student> Students { get; set; }

    }
}
