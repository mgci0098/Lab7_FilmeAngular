using CentruMultimedia.Models;
using Lab3.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class FilmeServiceTest
    {

        [Test]
        public void GetAllShouldReturnCorrectNumberOfPagesForFilme()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForFilme))
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                var filmService = new FilmService(context);
                var addedFilm = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "film de test 1",
                    Director = "dir1",
                    DateAdded = DateTime.Parse("2019-06-11T00:00:00"),
                    Duration = 100,
                    Description = "asdvadfbdbsb",
                    Genre = "Comedy",
                    ReleaseYear = 2000,
                    Rating = 3,
                    Watched = 0,
                    Comentarii = new List<Comentariu>()
                    {
                        new Comentariu
                        {
                            Important = true,
                            Text = "asd",
                            AddedBy = null
                        }
                    },

                }, null);

                DateTime from = DateTime.Parse("2019-06-10T00:00:00");
                DateTime to = DateTime.Parse("2019-06-12T00:00:00");

                var allFilms = filmService.GetAll(from, to, 1);
                Assert.AreEqual(1, allFilms.Entries.Count);
            }
        }


        [Test]
        public void GetByIdShouldReturnFilmWithCorrectIdNumber()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnFilmWithCorrectIdNumber))
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                var filmService = new FilmService(context);
                var addedFilm = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "Testare",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    Duration = 100,
                    Description = "asdvadfbdbsb",
                    Genre = "Comedy",
                    ReleaseYear = 2000,
                    Rating = 3,
                    Watched = 0
                }, null);

                var theFilm = filmService.GetById(1);
                Assert.AreEqual("Testare", theFilm.Title);
            }
        }


        [Test]
        public void CreateShouldAddAndReturnTheFilmCreated()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheFilmCreated))
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                var filmService = new FilmService(context);
                var addedFilm = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "Create",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    Duration = 100,
                    Description = "asdvadfbdbsb",
                    Genre = "Comedy",
                    ReleaseYear = 2000,
                    Rating = 3,
                    Watched = 0
                }, null);

                Assert.IsNotNull(addedFilm);
                Assert.AreEqual("Create", addedFilm.Title);
            }
        }


        //TODO: nu stiu de ce nu functioneaza testul, Postman functioneaza !!!
        [Test]
        public void UpsertShouldChangeTheFildValues()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldChangeTheFildValues))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                var filmService = new FilmService(context);
                var original = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "Original",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    Duration = 100,
                    Description = "asdvadfbdbsb",
                    Genre = "Comedy",
                    ReleaseYear = 2000,
                    Rating = 3,
                    Watched = 0
                }, null);


                var film = new Lab3.ViewModels.FilmPostModel
                {
                    Title = "upsert"
                };

                context.Entry(original).State = EntityState.Detached;

                var result = filmService.Upsert(1, film);

                Assert.IsNotNull(original);
                Assert.AreEqual("upsert", result.Title);
            }
        }

        [Test]
        public void DeleteShouldRemoveAndReturnFilm()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldRemoveAndReturnFilm))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                var filmService = new FilmService(context);
                var toAdd = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "DeSters",
                    Director = "dir1",
                    DateAdded = new DateTime(),
                    Duration = 100,
                    Description = "asdvadfbdbsb",
                    Genre = "Comedy",
                    ReleaseYear = 2000,
                    Rating = 3,
                    Watched = 0
                }, null);

                Assert.IsNotNull(toAdd);
                Assert.AreEqual(1, filmService.GetAll(new DateTime(), new DateTime(), 1).Entries.Count);

                var deletedFilm = filmService.Delete(1);

                Assert.IsNotNull(deletedFilm);
                Assert.AreEqual(0, filmService.GetAll(new DateTime(), new DateTime(), 1).Entries.Count);
            }
        }



    }
}
