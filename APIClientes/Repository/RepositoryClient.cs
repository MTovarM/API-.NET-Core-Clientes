using APIClientes.Data;
using APIClientes.Models;
using APIClientes.Models.Dto;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace APIClientes.Repository
{
    public class RepositoryClient : IClientRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private IMapper _mapper;


        public RepositoryClient(ApplicationDBContext dbContext, IMapper mapper1)
        {
            this._dbContext = dbContext;
            this._mapper = mapper1;
        }

        public async Task<ClientDto> CreateUpdate(ClientDto Client)
        {
            Client cliente = _mapper.Map<ClientDto, Client>(Client);
            if (cliente.Id > 0)
            {
                _dbContext.Clients.Update(cliente);
            }
            else 
            {
                await _dbContext.Clients.AddAsync(cliente);
            }
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Client, ClientDto>(cliente);
        }

        public async Task<bool> DeleteClient(int id)
        {
            try
            {
                Client cliente = await _dbContext.Clients.FindAsync(id);
                if (cliente == null) return false;
                _dbContext.Clients.Remove(cliente);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ClientDto>> GetClients()
        {
            List<Client> lista = await _dbContext.Clients.ToListAsync();
            return _mapper.Map<List<ClientDto>>(lista);
        }

        public async Task<ClientDto> GetClientsById(int id)
        {
            Client cliente = await _dbContext.Clients.FindAsync(id) ?? new Client();

            return _mapper.Map<ClientDto>(cliente);
        }
    }
}
