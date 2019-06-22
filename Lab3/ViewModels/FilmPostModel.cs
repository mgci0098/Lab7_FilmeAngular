using CentruMultimedia.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.ViewModels
{
    public class FilmPostModel
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        public int Rating { get; set; }
        [EnumDataType(typeof(Watched))]
        public Watched Watched { get; set; }

        public List<Comentariu> Comentarii { get; set; } // adaugat de mine


        public static Film ToFilm(FilmPostModel film)
        {
            Genre genre = CentruMultimedia.Models.Genre.Action;

            if (film.Genre == "Comedy")
            {
                genre = CentruMultimedia.Models.Genre.Comedy;
            }
            else if (film.Genre == "Horror")
            {
                genre = CentruMultimedia.Models.Genre.Horror;
            }
            else if (film.Genre == "Thriler")
            {
                genre = CentruMultimedia.Models.Genre.Thriler;
            }

            return new Film
            {
                Title = film.Title,
                Description = film.Description,
                Genre = genre,
                Duration = film.Duration,
                ReleaseYear = film.ReleaseYear,
                DateAdded = film.DateAdded,
                Director = film.Director,
                Rating = film.Rating,
                Watched = film.Watched,
                Comentarii = film.Comentarii
            };
        }


    }
}
