using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Data.EventSourcing;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.API.Controllers
{
    /// <summary>
    /// Controller para listar os eventos inseridos no EventStore
    /// Controller para teste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public EventosController(IEventSourcingRepository eventSourcingRepository)
        {
            _eventSourcingRepository = eventSourcingRepository;
        }

        /// <summary>
        /// Listar eventos por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("eventos/{id:guid}")]
        public async Task<IActionResult> ListarEventosPorId(Guid id)
        {
            var eventos = await _eventSourcingRepository.ObterEventos(id);
            return Ok(eventos);
        }
    }
}
