namespace APIClientes.Repository
{
    using Models;

    public interface IUserRepository
    {
        Task<int> Register(User user, string password);
        Task<string> Login(string user, string password);
        Task<bool> UserExist(string username);

    }
}
