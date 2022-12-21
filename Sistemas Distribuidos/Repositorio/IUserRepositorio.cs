using Sistemas_Distribuidos.Models;

namespace Sistemas_Distribuidos.Repositorio
{
    public interface IUserRepositorio
    {
        // Métodos criados para acessar o banco de dados na tabela de usuários
        // Aqui tem a descrição das funcionalidades:
        UserModel Adicionar(UserModel user);
        UserModel? BuscaPorId(int id);
        UserModel? BuscaPorNick(string nick);
        UserModel? BuscaPorEmail(string email);
        UserModel? ConfirmarEmail(string email);
    }
}
