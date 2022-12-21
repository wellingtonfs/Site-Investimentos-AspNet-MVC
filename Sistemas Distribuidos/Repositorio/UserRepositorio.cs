using Sistemas_Distribuidos.Data;
using Sistemas_Distribuidos.Models;
using Sistemas_Distribuidos.utils;

namespace Sistemas_Distribuidos.Repositorio
{
    public class UserRepositorio : IUserRepositorio
    {
        private BancoContext _bancoContext;

        // Injeção de dependencias
        public UserRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        // Adicionar um novo usuário ao banco de dados
        public UserModel Adicionar(UserModel user)
        {
            // Configurar a data de cadastro e setar como False a confirmação de email
            user.ExistingEmail = false;
            user.DataCadastro = DateTime.Now;

            // Salva as alterações
            _bancoContext.Users.Add(user);
            _bancoContext.SaveChanges();
            return user;
        }

        // Buscar por id do usuário
        public UserModel? BuscaPorId(int id)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Id == id);
        }

        // Buscar usuário por apelido
        public UserModel? BuscaPorNick(string nick)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Nick == nick);
        }

        // Buscar por email
        public UserModel? BuscaPorEmail(string email)
        {
            return _bancoContext.Users.FirstOrDefault(x => x.Email == email);
        }

        // Quando o email for confirmado, será salvo no banco de dados essa confirmação
        public UserModel? ConfirmarEmail(string email)
        {
            UserModel? userDB = this.BuscaPorEmail(email);

            // Se o usuário não existir:
            if (userDB == null) return null;

            // Define a confirmação de existencia do email como true
            userDB.ExistingEmail = true;

            // Salva os dados
            _bancoContext.Users.Update(userDB);
            _bancoContext.SaveChanges();

            return userDB;
        }
    }
}
