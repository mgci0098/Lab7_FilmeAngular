using CentruMultimedia.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.ViewModels
{

    
    public class FilmGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public DateTime DateAdded { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }
        public Watched Watched { get; set; }
        public int NumberOfComments { get; set; }



        public static FilmGetModel FromFilm(Film film)
        {
            return new FilmGetModel
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                Genre = film.Genre,
                DateAdded=film.DateAdded,
                Director=film.Director,
                Duration=film.Duration,
                Rating=film.Rating,
                ReleaseYear=film.ReleaseYear,
                Watched=film.Watched,
                NumberOfComments = film.Comentarii.Count
            };
        }


    }
}
