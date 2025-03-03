using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ApiCrud.Produtos
{
    public class Produto
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public Produto(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
            Ativo = true;
        }


        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }


        public void Desativar()
        {
            Ativo = false;

        }
    }
}
