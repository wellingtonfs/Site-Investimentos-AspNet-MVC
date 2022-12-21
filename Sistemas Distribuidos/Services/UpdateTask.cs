using Sistemas_Distribuidos.Models.Hg;
using Sistemas_Distribuidos.Repositorio;
using System.Globalization;

namespace Sistemas_Distribuidos.Services
{
    /*
        
        Esta classe serve para realizar uma requisição para as APIs a cada 15 minutos para
        atualizar os dados do banco de dados caso o dia tenha mudado
        
        Sempre que passar do meio dia (12:00), os dados são salvos no banco de dados para
        ter um histórico das moedas de acordo com o tempo

     */

    public class UpdateTask : IHostedService, IDisposable
    {
        private readonly ILogger<UpdateTask> logger;
        public IServiceProvider service { get; private set; }
        
        private Timer timer;
        private DateTime? currentDate;

        // Injeção de dependencias
        public UpdateTask(ILogger<UpdateTask> logger, IServiceProvider service)
        {
            this.logger = logger;
            this.service = service;

            // Obtém a data da última atualização feita que foi salva em um arquivo txt (caso exista)
            currentDate = GetCurrentDateFromFile();
        }

        // Limpar o timer quando tudo acabar
        public void Dispose()
        {
            timer?.Dispose();
        }

        // Chamado pelo aspnet quando iniciar este serviço
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Configurar a chamada da função UpdateHgData a cada 15 minutos
            timer = new Timer(
                e => UpdateHgData(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(15)
            );

            return Task.CompletedTask;
        }

        // Chamado quando o serviço termina
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        // Função chamada a cada 15 minutos para realizar a atualização
        public async void UpdateHgData()
        {
            // Obtém o tempo atual
            DateTime today = DateTime.Now;

            // Caso os dados já tiverem sido salvos alguma vez na vida
            if (currentDate != null)
            {
                // Obtém essa data
                DateTime current = currentDate ?? today;

                // Se a data da ultima atualização for igual a hoje, então, não atualiza
                // pois já atualizou hoje
                if (current.Day == today.Day && current.Month == today.Month)
                {
                    return;
                }

                // Caso for de manha, também não atualiza (foi escolhido o período de 12:00 para atualizar)
                if (today.ToString("tt", CultureInfo.InvariantCulture) == "AM") return;
            }

            // Obtém os dados atuais (sem cache)
            HGModelBase? hgModel = await HgAPI.GetFinancialDetails();

            // Se der erro, salva a log e retorna para tentar novamente em 15 minutos
            if (hgModel == null)
            {
                logger.LogInformation("Dados NÃO salvos (problemas na API)");
                return;
            }

            // Se os dados vierem zerados, não salva também
            if (hgModel.results.currencies.USD.buy == 0 && hgModel.results.currencies.EUR.buy == 0)
            {
                logger.LogInformation("Dados NÃO salvos (problemas na API) - dados zerados");
                return;
            }

            // Obtém o contexto do banco de dados (injetar dependencias)
            using (var scope = service.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IHgRepositorio>();

                try
                {
                    // Adicionar no banco de dados os dados vindos da API
                    context?.Adicionar(hgModel);
                } catch(Exception ex)
                {
                    logger.LogInformation("Dados NÃO salvos (problemas na AWS)");
                    return;
                }
            }

            // Salvar o dia atual como concluido
            currentDate = today;
            SetCurrentDateToFile(today);

            logger.LogInformation("Dados salvos no banco de dados");
        }

        // Obtém a data da última atualização do arquivo criado txt
        // Só é executado quando o processo inicia
        private DateTime? GetCurrentDateFromFile()
        {
            // Configurar o caminho do arquivo
            string pathFile = Path.Combine(new String[] { Directory.GetCurrentDirectory(), "datetime.txt" });

            try
            {
                // Criar um leitor do arquivo (ponteiro)
                StreamReader file = new StreamReader(pathFile);

                // Lê a primeira linha do arquivo
                string? line = file.ReadLine();

                file.Close();

                // Se não der, retorna null
                if (line == null) return null;

                // Remove a quebra de linha final, caso exista
                line = line.Trim().Replace("\n", "");

                // Retorna a data no modelo DateTime c#  
                return DateTime.Parse(line);

            } catch(Exception e)
            {
                logger.LogInformation("Erro ao ler arquivo datetime.txt: " + e.Message);
                return null;
            }
        }

        // Grava uma data no arquivo txt
        private void SetCurrentDateToFile(DateTime date)
        {
            // Configurar o caminho do arquivo
            string pathFile = Path.Combine(new String[] { Directory.GetCurrentDirectory(), "datetime.txt" });

            try
            {
                // Criar um escritor do arquivo (ponteiro)
                StreamWriter file = new StreamWriter(pathFile, false);

                // Escrever a data como string
                file.WriteLine(date.ToString());

                file.Close();

            }
            catch (Exception e)
            {
                logger.LogInformation("Erro ao escrever arquivo datetime.txt: " + e.Message);
            }
        }

        // Este é outro arquivo utilizado para criar uma log de acesso ao site
        public static void AddToLogFile(string msg)
        {
            // Configurar o caminho do arquivo
            string pathFile = Path.Combine(new String[] { Directory.GetCurrentDirectory(), "accessLog.txt" });

            try
            {
                // Criar um escritor do arquivo (ponteiro)
                StreamWriter file = new StreamWriter(pathFile, true);

                // Escreve a msg vinda do controller com o ip e a data de conexão do cliente 
                file.WriteLine(msg);

                file.Close();

            }
            catch (Exception e)
            {
                // Faz nada
            }
        }
    }
}
