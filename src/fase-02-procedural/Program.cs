using System;
using System.Collections.Generic;
using System.Linq;

// Dados do produto (tipo simples, sem comportamento)
public record Produto(string Nome, decimal Preco, int Qualidade);

// ===============================
//   CÓDIGO PROCEDURAL PURO
// ===============================

// Lista de produtos para teste
var produtos = new List<Produto>
{
    new Produto("Produto A", 100, 50),
    new Produto("Produto B", 250, 90),
    new Produto("Produto C", 120, 70)
};

Console.WriteLine("=== Teste dos Modos ===");

Console.WriteLine("Modo ECONOMICO: " + SelecionarProduto(produtos, "ECONOMICO").Nome);
Console.WriteLine("Modo PREMIUM: " + SelecionarProduto(produtos, "PREMIUM").Nome);
Console.WriteLine("Modo QUALIDADE: " + SelecionarProduto(produtos, "QUALIDADE").Nome);
Console.WriteLine("Modo PADRÃO (modo inválido): " + SelecionarProduto(produtos, "xxxx").Nome);


// ============================================
//        Função procedural (local function)
// ============================================

Produto SelecionarProduto(List<Produto> produtos, string modo)
{
    if (produtos == null || produtos.Count == 0)
        throw new ArgumentException("Lista de produtos vazia.");

    // Calcula mais barato e maior qualidade
    var maisBarato = produtos.OrderBy(p => p.Preco).First();
    var melhorQualidade = produtos.OrderByDescending(p => p.Qualidade).First();

    string m = (modo ?? "").Trim().ToUpperInvariant();

    switch (m)
    {
        case "ECONOMICO":
            return maisBarato;

        case "PREMIUM":
            if (melhorQualidade.Preco - maisBarato.Preco <= 200)
                return melhorQualidade;
            return maisBarato;

        case "QUALIDADE":
            return melhorQualidade;

        default:
            // fallback — ECONOMICO
            return maisBarato;
    }
}
