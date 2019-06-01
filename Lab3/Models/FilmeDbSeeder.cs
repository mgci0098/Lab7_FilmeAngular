using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentruMultimedia.Models
{
    public class FilmeDbSeeder
    {

        public static void Initialize(FilmeDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any flowers.
            if (context.Filme.Any())
            {
                return;   // DB has been seeded
            }

            context.Filme.AddRange(

                new Film
               {
                    Title= "film 2",
                    Description="test descriere 2",
                    Genre= Genre.Action,
                    Duration= 120,
                    ReleaseYear=2000,
                    Director="John Doe",
                    DateAdded=DateTime.Now,
                    Rating=5,
                    Watched= Watched.Yes
                }
            
                );
            context.SaveChanges();



        }
    }
}
