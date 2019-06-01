using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentruMultimedia.Models;
using Lab3.Models;
using Lab3.Services;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentruMultimedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {

        private IFilmService filmService;

        private IUsersService usersService;
        public FilmeController(IFilmService filmService, IUsersService usersService)
        {
            this.filmService = filmService;
            this.usersService = usersService;
        }

        /// <summary>
        /// Returneaza toate filmele
        /// </summary>
        /// <param name="from"> Optional, filtreaza dupa data adaugarii de la</param>
        /// <param name="to">Optional, filtreaza dupa data adaugarii pana la</param>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /filme
        ///     {  id: 3,
        ///        title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: 2,
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///        comentarii: [
        ///            {
        ///                    id: 1,
        ///                    text: "film grozav",
        ///                    important: false
        ///             },
        ///             {
        ///                   id: 2,
        ///                   text: "film slab",
        ///                   important: false

        ///             }
        ///         ]
        ///     }
        ///
        ///          </remarks>
        ///          <returns>lista cu filme</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public PaginatedList<FilmGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);
            return filmService.GetAll(from, to, page);
        }

        /// <summary>
        /// Cauta un film dupa ID
        /// </summary>
        /// <param name="id">Optional, filltreaza dupa ID</param>
        ///  <remarks>
        /// Sample response:
        ///
        ///     Get /filme
        ///     {  id: 3,
        ///        title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: 2,
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///        comentarii: [
        ///     {
        ///         id: 1,
        ///         text: "film grozav"
        ///     },
        ///     {
        ///         id: 2,
        ///         text: "film slab"
        ///     }
        ///     ]
        ///     }
        ///
        ///          </remarks>
        /// <returns>filmul cautat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)] //adaugat de mine
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetFilme")]
        public IActionResult Get(int id)
        {
            var found = filmService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        /// <summary>
        /// Adauga un film in baza de date
        /// </summary>
        /// <param name="film">Obiect de tip film</param>
        ///  <remarks>
        /// Sample request:
        ///
        ///     Post /filme
        ///      {  title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: "Action",
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///          comentarii: [
        ///     {
        ///         id: 1,
        ///         text: "film grozav"
        ///     },
        ///     {
        ///         id: 2,
        ///         text: "film slab"
        ///     }
        ///     ]        
        ///}
        ///</remarks>
        ///        <returns>filmul adaugat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles ="Admin,Regular")]    
        [HttpPost]
        public void Post([FromBody] FilmPostModel film)
        {
            User addedBy = usersService.GetCurentUser(HttpContext);

            //if(addedBy.UserRole == UserRole.UserManager)
            //{
            //    return Forbid();      //trebuie returnat tipul IActionResult
            //}

            filmService.Create(film, addedBy);
        }

        /// <summary>
        /// Modiffica un film existent iar daca nu exista il va adauga in baza de date
        /// </summary>
        /// <param name="id">id-ul filmului care urmeaza sa fie modificat</param>
        /// <param name="film">obiect de tip film</param>
        /// Sample request:
        ///     <remarks>
        ///     Post /filme
        ///      {  title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: "Action",
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///}
        ///        </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        /// <returns>Status 400 daca nu s-a putut face modificarea</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FilmPostModel film)
        {
            var result = filmService.Upsert(id, film);
            return Ok(result);
        }

        /// <summary>
        /// Sterge un film
        /// </summary>
        /// <param name="id">Id-ul filmului care trebuie sters</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //adaugat de mine
        [Authorize(Roles = "Admin,Regular")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = filmService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}