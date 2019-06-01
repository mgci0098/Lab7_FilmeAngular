using CentruMultimedia.Models;
using Lab3.Models;
using Lab3.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Services
{

    public interface IFilmService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        PaginatedList<FilmGetModel> GetAll(DateTime? from, DateTime? to, int page);
        Film GetById(int id);
        Film Create(FilmPostModel film, User addedBy);
        Film Upsert(int id, FilmPostModel film);
        Film Delete(int id);
    }



    public class FilmService : IFilmService
    {
        private FilmeDbContext context;
        public FilmService(FilmeDbContext context)
        {
            this.context = context;
        }

        public Film Create(FilmPostModel film, User addedBy)
        {
            Film toAdd = FilmPostModel.ToFilm(film);
            toAdd.Owner = addedBy;      //adaugam persoana care a adaugat acest Film Film
            context.Filme.Add(toAdd);
            context.SaveChanges();
            return toAdd;
        }

        public Film Delete(int id)
        {
            var existing = context.Filme.Include(f => f.Comentarii)
                .FirstOrDefault(film => film.Id == id);
            if (existing == null)
            {
                return null;
            }

            ////o varianta de asterge comentariile unui film
            //foreach(var comment in existing.Comentarii)
            //{
            //    context.Comentarii.Remove(comment);
            //}

            context.Filme.Remove(existing);
            context.SaveChanges();

            return existing;
        }

        public PaginatedList<FilmGetModel> GetAll(DateTime? from, DateTime? to, int page)
        {
            IQueryable<Film> result = context
                .Filme
                .OrderBy(f => f.Id)
                .Include(f => f.Comentarii);

            PaginatedList<FilmGetModel> paginatedResult = new PaginatedList<FilmGetModel>();
            paginatedResult.CurrentPage = page;

            if (from != null)
            {
                result = result.Where(f => f.DateAdded >= from);
            }
            if (to != null)
            {
                result = result.Where(f => f.DateAdded <= to);
            }

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<FilmGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<FilmGetModel>.EntriesPerPage)
                .Take(PaginatedList<FilmGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(f => FilmGetModel.FromFilm(f)).ToList();

            return paginatedResult;
        }


        public Film GetById(int id)
        {
            return context.Filme
                .Include(f => f.Comentarii)
                .FirstOrDefault(f => f.Id == id);
        }

        public Film Upsert(int id, FilmPostModel film)
        {
            var existing = context.Filme.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                Film toAdd = FilmPostModel.ToFilm(film);
                context.Filme.Add(toAdd);
                context.SaveChanges();
                return toAdd;
            }

            Film toUpdate = FilmPostModel.ToFilm(film);
            toUpdate.Id = id;
            context.Filme.Update(toUpdate);
            context.SaveChanges();
            return toUpdate;
        }
    }
}
