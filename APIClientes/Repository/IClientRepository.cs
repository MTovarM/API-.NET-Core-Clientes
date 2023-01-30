namespace APIClientes.Repository
{
    using APIClientes.Models.Dto;

    public interface IClientRepository
    {
        public Task<List<ClientDto>> GetClients();
        public Task<ClientDto> GetClientsById(int id);
        public Task<ClientDto> CreateUpdate(ClientDto Client);
        public Task<bool> DeleteClient(int id);
    }
}
