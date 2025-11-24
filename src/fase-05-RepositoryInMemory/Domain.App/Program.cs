using System;
using Domain.Entities;
using Domain.Entities.Services;
using Domain.Entities.Repositories;

namespace Domain.App
{
    internal class Program
    {
        static void Main()
        {
            // Repositório InMemory real da Fase 5
            var repo = new InMemoryProdutoRepository();

            // Adicionando produtos no repositório
            repo.Add(new Produto(1, "Produto A", 100, 50));
            repo.Add(new Produto(2, "Produto B", 250, 90));
            repo.Add(new Produto(3, "Produto C", 120, 70));

            Console.WriteLine("Produtos cadastrados no repositório:");
            foreach (var p in repo.ListAll())
            {
                Console.WriteLine($"#{p.Id} - {p.Nome} (Preço: {p.Preco}, Qualidade: {p.Qualidade})");
            }

            Console.WriteLine();
            Console.WriteLine("Seleção de produtos:");
            Console.WriteLine("-------------------------");

            // Cliente usa APENAS interface + factory
            ISeletorDeProduto seletorEconomico = SeletorFactory.Criar("ECONOMICO");
            ISeletorDeProduto seletorPremium   = SeletorFactory.Criar("PREMIUM");
            ISeletorDeProduto seletorQualidade = SeletorFactory.Criar("QUALIDADE");

            // Serviço combina Repository + Seletor (regra da Fase 5)
            var serviceEconomico = new ProdutoService(repo, seletorEconomico);
            var servicePremium   = new ProdutoService(repo, seletorPremium);
            var serviceQualidade = new ProdutoService(repo, seletorQualidade);

            Console.WriteLine("Modo Econômico:  " + serviceEconomico.SelecionarMelhorProduto().Nome);
            Console.WriteLine("Modo Premium:    " + servicePremium.SelecionarMelhorProduto().Nome);
            Console.WriteLine("Modo Qualidade:  " + serviceQualidade.SelecionarMelhorProduto().Nome);

            Console.WriteLine();
            Console.WriteLine("Modo inválido (fallback):");
            var seletorFallback = SeletorFactory.Criar("??");
            var serviceFallback = new ProdutoService(repo, seletorFallback);
            Console.WriteLine("Fallback: " + serviceFallback.SelecionarMelhorProduto().Nome);
        }
    }
}
