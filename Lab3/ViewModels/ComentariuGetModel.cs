using CentruMultimedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.ViewModels
{
    public class ComentariuGetModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? IdFilm { get; set; }    //accepta si valori null

        public bool Important { get; set; }

        public static ComentariuGetModel DinComentariu(Comentariu comentariu)
        {
            return new ComentariuGetModel
            {
                Id = comentariu.Id,
                Text = comentariu.Text,
                IdFilm = comentariu.Film?.Id,  //evalueaza expresia daca comentariu.Film este null returneaza null, altfel evalueaza expresia si returneaza valoarea
                Important = comentariu.Important

                //IdFilm = comentariu.Film == null ? -1 : comentariu.Film.Id, //mod de a accepta ceva ce poate sa vina null

            };
        }
    }
}

