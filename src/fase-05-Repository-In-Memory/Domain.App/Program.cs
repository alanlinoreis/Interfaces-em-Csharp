using System;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;


namespace Domain.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var repo = new InMemoryRepository<Produto, int>(p => p.Id);

            Console.WriteLine("========== FASE 05 — TESTE DO APP ==========\n");

            // ================= CRIAR PRODUTOS =================
            Console.WriteLine(">> Adicionando produtos ao repositório...");

            ProdutoService.Criar(repo, new Produto(1, "TV 50\"", 2500, 9));
            ProdutoService.Criar(repo, new Produto(2, "Fogão 4 bocas", 1200, 7));
            ProdutoService.Criar(repo, new Produto(3, "Notebook Gamer", 6500, 10));
            ProdutoService.Criar(repo, new Produto(4, "Micro-ondas", 700, 6));
            ProdutoService.Criar(repo, new Produto(5, "Ventilador", 200, 4));

            Console.WriteLine("Produtos cadastrados com sucesso!\n");

            // ================= LISTAR =================
            Console.WriteLine(">> LISTA INICIAL DE PRODUTOS:");
            foreach (var p in ProdutoService.Listar(repo))
                Console.WriteLine($"{p.Id} - {p.Nome} | Preço: {p.Preco} | Qualidade: {p.Qualidade}");

            Console.WriteLine("\n");

            // ================= BUSCAR =================
            Console.WriteLine(">> Buscando produto pelo ID 3:");
            var produto3 = ProdutoService.Buscar(repo, 3);
            Console.WriteLine(produto3 != null
                ? $"Encontrado: {produto3.Nome}"
                : "Produto não encontrado.");

            Console.WriteLine("\n");

            // ================= ATUALIZAR =================
            Console.WriteLine(">> Atualizando o produto 5 (Ventilador)...");

            var atualizado = new Produto(5, "Ventilador Turbinado", 299, 6);
            ProdutoService.Atualizar(repo, atualizado);

            var produto5 = ProdutoService.Buscar(repo, 5);
            Console.WriteLine($"Atualizado: {produto5!.Nome} - Preço: {produto5.Preco} - Qualidade: {produto5.Qualidade}");

            Console.WriteLine("\n");

            // ================= REMOVER =================
            Console.WriteLine(">> Removendo produto 2 (Fogão)...");

            var removeu = ProdutoService.Remover(repo, 2);
            Console.WriteLine(removeu ? "Removido com sucesso." : "Não encontrado.");

            Console.WriteLine("\n>> Lista após remoção:");
            foreach (var p in ProdutoService.Listar(repo))
                Console.WriteLine($"{p.Id} - {p.Nome}");

            Console.WriteLine("\n");

            // ================= SELEÇÃO (Fase 04 + Fase 05) =================

            Console.WriteLine(">> Testando seletores integrados ao repositório:\n");

            // Econômico
            var eco = ProdutoService.ExecutarSelecao(repo, "economico");
            Console.WriteLine($"Selecionado (Econômico): {eco.Nome} — R${eco.Preco}");

            // Premium
            var premium = ProdutoService.ExecutarSelecao(repo, "premium");
            Console.WriteLine($"Selecionado (Premium): {premium.Nome} — R${premium.Preco} | Qualidade {premium.Qualidade}");

            // Qualidade
            var qualidade = ProdutoService.ExecutarSelecao(repo, "qualidade");
            Console.WriteLine($"Selecionado (Qualidade): {qualidade.Nome} — Nota {qualidade.Qualidade}");

            Console.WriteLine("\n");

            // ================= CASOS DE ERRO =================

            Console.WriteLine(">> Testando casos de erro:");

            // Buscar inexistente
            var inexistente = ProdutoService.Buscar(repo, 999);
            Console.WriteLine(inexistente == null
                ? "Produto 999 corretamente retornou NULL."
                : "ERRO — Deve retornar null.");

            // Atualizar inexistente
            var atualizarInexistente = ProdutoService.Atualizar(repo, new Produto(999, "X", 1, 1));
            Console.WriteLine(!atualizarInexistente
                ? "Update em ID inexistente retornou FALSE (correto)."
                : "ERRO — Não deveria atualizar.");

            // Remover inexistente
            var removerInexistente = ProdutoService.Remover(repo, 999);
            Console.WriteLine(!removerInexistente
                ? "Remove em ID inexistente retornou FALSE (correto)."
                : "ERRO — Não deveria remover.");

            Console.WriteLine("\n========== FIM DO APP ==========");
        }
    }
}
