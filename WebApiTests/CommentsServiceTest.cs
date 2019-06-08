using CentruMultimedia.Models;
using Lab3.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class CommentsServiceTest
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new FilmeDbContext(options))
            {

                var commentService = new ComentariuService(context);
                var filmService = new FilmService(context);
                var addedFlower = filmService.Create(new Lab3.ViewModels.FilmPostModel
                {
                    Title = "film de test 1",
                    Director = "dir1",
                    DateAdded = new DateTime(),
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

                var allComments = commentService.GetAll(string.Empty, 1);
                Assert.AreEqual(1, allComments.NumberOfPages);
            }
        }
    }
}
