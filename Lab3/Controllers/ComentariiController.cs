using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Services;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariiController : ControllerBase
    {
        private IComentariuService cometariuService;

        public ComentariiController(IComentariuService cometariuService)
        {
            this.cometariuService = cometariuService;
        }

        /// <summary>
        /// Afiseaza toate comentatriile
        /// </summary>
        /// <param name="filterString">cuvantul in functie de care se filtreaza un comentariu</param>
        /// <param name="page">cuvantul in functie de care se filtreaza un comentariu</param>
        /// <remarks>
        /// Sample response:   
        ///      {
        ///         id: 3,
        ///         text: "cel mai bun",
        ///         idFilm: 2
        ///         }
        /// </remarks>
        /// 
        /// <returns>lista cu comentarii</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public PaginatedList<ComentariuGetModel> Get([FromQuery]string filterString, [FromQuery]int page = 1)
        {
            return cometariuService.GetAll(filterString, page);
        }


    }
}