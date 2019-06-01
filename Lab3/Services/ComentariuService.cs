using CentruMultimedia.Models;
using Lab3.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Services
{

    public interface IComentariuService
    {
        PaginatedList<ComentariuGetModel> GetAll(string contine, int page);
    }


    public class ComentariuService : IComentariuService
    {
        private FilmeDbContext context;

        public ComentariuService(FilmeDbContext context)
        {
            this.context = context;
        }


        public PaginatedList<ComentariuGetModel> GetAll(string filterString, int page)
        {
            IQueryable<Comentariu> result = context
                .Comentarii
                .Where(c=> string.IsNullOrEmpty(filterString) || c.Text.Contains(filterString))
                .OrderBy(c=>c.Id)
                .Include(c => c.Film);

            PaginatedList<ComentariuGetModel> paginatedResult = new PaginatedList<ComentariuGetModel>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<ComentariuGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<ComentariuGetModel>.EntriesPerPage)
                .Take(PaginatedList<ComentariuGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(f => ComentariuGetModel.DinComentariu(f)).ToList();

            return paginatedResult;

            //List<ComentariuGetModel> comentariiFiltrate = new List<ComentariuGetModel>();
            //List<ComentariuGetModel> ComentariiNefiltrate = new List<ComentariuGetModel>();

            //foreach (Film film in result)
            //{
            //    film.Comentarii.ForEach(c =>
            //    {
            //        if (c.Text == null || contine == null)
            //        {
            //            ComentariuGetModel comentariu = ComentariuGetModel.DinComentariu(c, film);

            //            ComentariiNefiltrate.Add(comentariu);
            //        }
            //        else if (c.Text.Contains(contine))
            //        {
            //            ComentariuGetModel comentariu = ComentariuGetModel.DinComentariu(c, film);

            //            comentariiFiltrate.Add(comentariu);
            //        }
            //    });
            //}
            //if (contine == null)
            //{
            //    return ComentariiNefiltrate;
            //}
            //return comentariiFiltrate;
        }

    }
}