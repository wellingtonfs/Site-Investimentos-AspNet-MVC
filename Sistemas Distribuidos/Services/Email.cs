using SendGrid.Helpers.Mail;
using SendGrid;
using Sistemas_Distribuidos.Models;

namespace Sistemas_Distribuidos.Services
{
    public class Email
    {
        private readonly string? send_key = Environment.GetEnvironmentVariable("SEND_GRID_KEY");
        // Mandar email quando a conta de um usuário é criada, este é o primeiro email enviado
        public async Task SendWelcomeMail(UserModel user, string key, bool justConfirm)
        {
            if (send_key == null) throw new Exception("Variável de ambiente SEND_GRID_KEY não encontrada");

            // Configura o envio de email utilizando o SendGrid
            SendGridClient client = new SendGridClient(send_key);

            EmailAddress senderMail = new EmailAddress("wellingtonsilveira99@gmail.com", "FinMonitor");
            // Definir destino
            EmailAddress receiverMail = new EmailAddress(user.Email, user.Nick);

            // Características do email:

            // Assunto
            string emailSubject = "Confirme Seu Email";
            string textContent = "welcome";

            // Corpo
            string htmlContent = "<center>";

            if (justConfirm) htmlContent += "<h1 style=\"color: green;\">Confirmação de Email</h1>";
            else htmlContent += "<h1 style=\"color: green;\">Cadastro realizado com Sucesso!</h1>";

            htmlContent += "<br/>";
            htmlContent += "Bem-vindo, " + user.Nick + "!";
            htmlContent += "<br/>";
            htmlContent += "<a href=\"http://179.189.133.252:3005/User/ValidarEmail?key=" + key + "\">Confirmar Email</a>";
            htmlContent += "<center>";

            // Criar email e enviar
            SendGridMessage msg = MailHelper.CreateSingleEmail(senderMail, receiverMail, emailSubject, textContent, htmlContent);

            var resp = client.SendEmailAsync(msg).ConfigureAwait(false);
        }

        // Mandar email de recuperação de senha para o usuário
        public async Task SendPasswordRecoveryMail(UserModel user)
        {
            if (send_key == null) throw new Exception("Variável de ambiente SEND_GRID_KEY não encontrada");

            // Configura o envio de email utilizando o SendGrid
            SendGridClient client = new SendGridClient(send_key);

            EmailAddress senderMail = new EmailAddress("wellingtonsilveira99@gmail.com", "FinMonitor");
            // Definir destino
            EmailAddress receiverMail = new EmailAddress(user.Email, user.Nick);

            // Assunto
            string emailSubject = "Recuperação de senha";
            string textContent = emailSubject;

            // Corpo
            string htmlContent = "<center><h1>Recuperação de senha</h1><br/>Olá, " + user.Nick + "!<br/><br/>Sua senha: " + user.Password + "<center>";

            // Criar email e enviar
            SendGridMessage msg = MailHelper.CreateSingleEmail(senderMail, receiverMail, emailSubject, textContent, htmlContent);

            var resp = client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
