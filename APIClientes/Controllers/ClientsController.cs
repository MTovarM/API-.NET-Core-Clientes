namespace APIClientes.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using APIClientes.Data;
    using APIClientes.Models;
    using APIClientes.Repository;
    using APIClientes.Models.Dto;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        protected ResponseDto _response;

        public ClientsController(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
            _response = new ResponseDto();

        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            try
            {
                var clientsList = await _clientRepository.GetClients();
                _response.Result = clientsList;
                _response.DisplayMessage = "Clients List";
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.ErrorMessage = new List<string> { ex.Message.ToString() };

            }
            return Ok(_response);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var clientt = await _clientRepository.GetClientsById(id);
            
            if (clientt == null)
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Client don't exist";
                return NotFound(_response);
            }

            _response.Result = clientt;
            _response.DisplayMessage = "Cliente";
            return Ok(_response);
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, ClientDto client)
        {

            try
            {
                var clientt = await _clientRepository.CreateUpdate(client);
                _response.Result = clientt;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Error, client dont update";
                _response.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientDto clientt)
        {

            try
            {
                var client = await _clientRepository.CreateUpdate(clientt);
                _response.Result = client;
                _response.DisplayMessage = "Created client";
                return CreatedAtAction("GetClient", new { id = client.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.DisplayMessage = "Error, client dont created";
                _response.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
           
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                bool isEliminated = await _clientRepository.DeleteClient(id);
                if (isEliminated)
                {
                    _response.Result = isEliminated;
                    _response.DisplayMessage = "Client eliminated";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSucces = false;
                    _response.DisplayMessage = "Client don't eliminated";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSucces = false;
                _response.DisplayMessage = "Client don't eliminated";
                _response.ErrorMessage = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
