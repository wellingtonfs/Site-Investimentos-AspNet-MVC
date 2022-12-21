using Sistemas_Distribuidos.Models.Hg;

namespace Sistemas_Distribuidos.Repositorio
{
    public interface IHgRepositorio
    {
        // Métodos criados para acessar o banco de dados, adicionando dados e buscando histórico
        HGModelBase Adicionar(HGModelBase data);
        List<MoedaModel>? ObterHistoricoMoeda(string tag);
        void UpdatePersonalizado();
    }
}
