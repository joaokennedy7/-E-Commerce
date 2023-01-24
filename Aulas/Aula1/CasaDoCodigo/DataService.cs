using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext contexto;
        private readonly IProdutoRepository produtoRepository;

        public DataService(ApplicationContext contexto,
            IProdutoRepository produtoRepository)
        {
            this.contexto = contexto;
            this.produtoRepository = produtoRepository;
        }

        public void InicializaDb()
        {
            contexto.Database.Migrate();

            //Lendo um arquivo json sobre livros
            List<Livro> livros = GetLivros();

            SaveProdutos(livros);
        }

        private void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros) // Inserindo os dados através de uma tabela produtos
            {                         //Adicionar uma nova instancia de produto dentro da nossa lista de produto
                contexto.Set<Produto>().Add(new Produto(livro.Codigo, livro.Nome, livro.Preco));
            }
            contexto.SaveChanges();
        }

        private static List<Livro> GetLivros()
        {
            var json = File.ReadAllText("livros.json");

            var livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }

    class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }

}
