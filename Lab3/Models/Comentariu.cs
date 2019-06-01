using Lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentruMultimedia.Models
{
    public class Comentariu
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }

        public Film Film { get; set; } //adaugat de mine

        public User AddedBy { get; set; }
    }
}
