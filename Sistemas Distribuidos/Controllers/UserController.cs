using Microsoft.AspNetCore.Mvc;
using Sistemas_Distribuidos.Models;
using Sistemas_Distribuidos.Repositorio;
using Sistemas_Distribuidos.Services;
using Sistemas_Distribuidos.utils;

namespace Sistemas_Distribuidos.Controllers
{
    public class UserController : Controller
    {
        private IUserRepositorio _userRepositorio;
        private ISessions _sessions;
        private Email eSender;
        private Random _randN = new Random(42);

        // Injeção de dependencias
        public UserController(IUserRepositorio userRepositorio, ISessions sessions)
        {
            _userRepositorio = userRepositorio;
            _sessions = sessions;

            // Serviço de email
            eSender = new Email();
        }

        // Rota '/Entrar'
        public IActionResult Entrar()
        {
            // Se já estiver logado, voltar para a rota Home
            if (_sessions.GetSession() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        // Rota '/Cadastro'
        public IActionResult Cadastro()
        {
            // Se já estiver logado, voltar para a rota Home
            if (_sessions.GetSession() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        // Rota '/RecuperarSenha'
        public IActionResult RecuperarSenha()
        {
            // Se já estiver logado, voltar para a rota Home
            if (_sessions.GetSession() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        //Rota '/ValidarEmail?key=key&id=id'
        public IActionResult ValidarEmail(string? key, int? id)
        {
            // Caso acesse este controller sem informar parametros (key, id)
            if (string.IsNullOrWhiteSpace(key))
            {
                // Caso esteja pendente a verificação de email
                if (_sessions.GetSession("verifyemail_key") != null)
                {
                    TempData["waiting"] = "Confirme seu email para concluir seu cadastro";
                    return View();
                }

                // Caso contrário, volta para Home
                return RedirectToAction("Index", "Home");
            }

            // Com parametros

            // Caso a pessoa não confirme o email
            if (key == "no_validation")
            {
                // Se o id não for informado, volta pra home
                if (id == null) return RedirectToAction("Index", "Home");

                TempData["no_validation"] = "Email não confirmado";
                TempData["id"] = id;

                return View();
            }

            // Buscar dados salvos na sessão
            string? email = _sessions.GetSession("verifyemail_mail");
            string? keyS = _sessions.GetSession("verifyemail_key");

            // Se algo for nulo, volta pra Home
            if (email == null || keyS == null) return RedirectToAction("Index", "Home");

            // O código usado para validar o email foi enviado para o email da pessoa
            // Caso ela acesse com uma chave não válida, executa:
            if (key != keyS)
            {
                TempData["invalidKey"] = "Código Inválido";
                return View();
            } 

            // Deu tudo certo com a confirmação de email
            TempData["ok"] = "Email Confirmado";

            // Salva no banco de dados que a conta está confirmada
            _userRepositorio.ConfirmarEmail(email);
            _sessions.RemoveSession();

            return View();
        }

        // Rota '/ReenviarEmail'
        // Caso a pessoa precise do email de confirmação novamente
        public IActionResult ReenviarEmail(int id)
        {
            // Busca o usuário no banco de dados
            UserModel? user = _userRepositorio.BuscaPorId(id);

            // Caso a conta não exista ou o email já esta confirmado retorna pra Home
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.ExistingEmail) return RedirectToAction("Index", "Home");

            // Gerar outro email de confirmação
            string key = user.Nick + _randN.Next(10000, 99999).ToString();

            // Configurar variáveis de sessão
            _sessions.RemoveSession();
            _sessions.CriarSession("verifyemail_mail", user.Email);
            _sessions.CriarSession("verifyemail_key", key);

            // Mandar email com a nova key
            eSender.SendWelcomeMail(user, key, true);

            // Redireciona para validação de email
            return RedirectToAction("ValidarEmail");
        }

        // Rota Post '/Entrar' com os dados de login
        [HttpPost]
        public IActionResult Entrar(LoginModel userLogin)
        {
            try
            {
                // Verificando se os dados não são nulos
                if (!ModelState.IsValid) return View(userLogin);

                // Verificando a senha
                if (!UserModel.IsValidPassword(userLogin.Password))
                {
                    TempData["error"] = "Credenciais inválidas!";
                    return View();
                }

                // Verificando se é email ou apelido
                bool isEmail = RegexUtils.IsEmail(userLogin.NickOrEmail);
                UserModel? user;

                // Caso for email
                if (isEmail)
                {
                    if (!UserModel.IsValidEmail(userLogin.NickOrEmail)) //email inválido
                    {
                        TempData["error"] = "Credenciais inválidas!";
                        return View();
                    }

                    user = _userRepositorio.BuscaPorEmail(userLogin.NickOrEmail);

                } //caso for apelido
                else
                {

                    if (!UserModel.IsValidNick(userLogin.NickOrEmail)) //apelido inválido
                    {
                        TempData["error"] = "Credenciais inválidas!";
                        return View();
                    }

                    user = _userRepositorio.BuscaPorNick(userLogin.NickOrEmail);
                }

                // Verificando se o usuário existe
                if (user == null)
                {
                    TempData["error"] = "Credenciais inválidas!";
                    return View();
                }

                // Verificando se a senha é igual
                if (!user.SamePassword(userLogin))
                {
                    TempData["error"] = "Credenciais inválidas!";
                    return View();
                }

                // Verificando se o email já foi confirmado
                if (!user.ExistingEmail)
                {
                    return RedirectToAction("ValidarEmail", new { key = "no_validation", id = user.Id });
                }

                // Passou em todos os testes
                _sessions.CriarSession(user);

                TempData["ok"] = "Olá, " + user.Nick + ". Redirecionando...";

                return View();

            }
            catch (Exception ex)
            {
                TempData["error"] = "Erro desconhecido. Tente novamente mais tarde";
                return View();
            }
        }

        // Rota Post '/Cadastro' com os dados de cadastro
        [HttpPost]
        public IActionResult Cadastro(UserModel user)
        {
            try
            {
                // Verificando se os dados não são nulos
                if (!ModelState.IsValid) return View(user);

                // Caso apelido inválido
                if (!UserModel.IsValidNick(user.Nick))
                {
                    TempData["error"] = "O apelido deve possuir de 3 a 30 caracteres alfanuméricos sem acentos (_ e - são permitidos)";
                    return View();
                }

                // Caso email inválido
                if (!UserModel.IsValidEmail(user.Email))
                {
                    TempData["error"] = "Email inválido";
                    return View();
                }

                // Verificando se a senha é válida
                if (!UserModel.IsValidPassword(user.Password))
                {
                    TempData["error"] = "Senha inválida: a senha deve possuir de 3 a 30 caracteres!";
                    return View();
                }

                // Buscar do banco de dados
                UserModel? userT = _userRepositorio.BuscaPorNick(user.Nick);

                // Caso o nick esteja em uso
                if (userT != null)
                {
                    TempData["error"] = "Apelido já em uso";
                    return View();
                }

                userT = _userRepositorio.BuscaPorEmail(user.Email);

                // Caso o email esteja em uso
                if (userT != null)
                {
                    TempData["error"] = "Email já em uso";
                    return View();
                }

                // Passou em todos os testes

                // Adicionar no banco de dados
                _userRepositorio.Adicionar(user);

                // Mandar email de boas vindas
                string key = user.Nick + _randN.Next(10000, 99999).ToString();

                // Configurar validação do email
                _sessions.RemoveSession();
                _sessions.CriarSession("verifyemail_mail", user.Email);
                _sessions.CriarSession("verifyemail_key", key);

                // Mandar o email
                eSender.SendWelcomeMail(user, key, false);

                return RedirectToAction("ValidarEmail");


            } catch(Exception ex)
            {
                TempData["error"] = "Erro desconhecido. Tente novamente mais tarde";
                return View();
            }
        }

        // Rota '/LogOut'
        public IActionResult LogOut()
        {
            try
            {
                // Remove a sessão, caso exista
                _sessions.RemoveSession();
            } catch (Exception ex) { }

            return RedirectToAction("Index", "Home");
        }

        // Rota Post '/RecuperarSenha' recebendo o email ou apelido do usuário
        [HttpPost]
        public IActionResult RecuperarSenha(RecSenhaModel recSenhaModel)
        {
            try
            {
                // Caso o modelo enviado for inválido, retorna com erro
                if (!ModelState.IsValid) return View(recSenhaModel);

                TempData["ok"] = "ok";

                // Verificando se é email ou apelido
                bool isEmail = RegexUtils.IsEmail(recSenhaModel.NickOrEmail);
                UserModel? user;

                // Caso for email
                if (isEmail)
                {
                    if (!UserModel.IsValidEmail(recSenhaModel.NickOrEmail)) //email inválido
                    {
                        return View();
                    }

                    user = _userRepositorio.BuscaPorEmail(recSenhaModel.NickOrEmail);

                } // Caso for apelido
                else
                {
                    if (!UserModel.IsValidNick(recSenhaModel.NickOrEmail)) //apelido inválido
                    {
                        return View();
                    }

                    user = _userRepositorio.BuscaPorNick(recSenhaModel.NickOrEmail);
                }

                // Verificando se o usuário existe
                if (user == null)
                {
                    // Mesmo que a conta não exista, a pessoa não será avisada
                    return View();
                }

                // Envia um email para a pessoa com a senha cadastrada (um sistema inseguro, mas não possui dados sensíveis)
                eSender.SendPasswordRecoveryMail(user);

            }
            catch (Exception ex) { }

            return View();
        }
    }
}
