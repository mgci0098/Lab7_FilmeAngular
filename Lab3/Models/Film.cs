using Lab3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CentruMultimedia.Models
{

    public enum Genre
    {
        Action,
        Comedy,
        Horror,
        Thriler
    }

    public enum Watched
    {
        Yes,
        No
    }
    public class Film
    {
        public int Id { get; set; }
      
        public string Title { get; set; }
        public string Description { get; set; }

        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }

        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [EnumDataType(typeof(Watched))]
        public Watched Watched { get; set; }

        public List<Comentariu> Comentarii { get; set; }

        public User Owner { get; set; }

    }
}
