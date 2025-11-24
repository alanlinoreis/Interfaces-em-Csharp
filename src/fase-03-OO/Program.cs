using System;
using System.Collections.Generic;
using System.Linq;

namespace Fase03OO
{
    // ==========================================
    //         CLASSE DE DADOS (SIMPLES)
    // ==========================================
    public class Produto
    {
        public string Nome { get; }
        public decimal Preco { get; }
        public int Qualidade { get; }

        public Produto(string nome, decimal preco, int qualidade)
        {
            Nome = nome;
            Preco = preco;
            Qualidade = qualidade;
        }
    }

    // ==========================================
    //           CLASSE BASE ABSTRATA
    // ==========================================
    public abstract class SeletorBase
    {
        public Produto Selecionar(List<Produto> produtos)
        {
            if (produtos == null || produtos.Count == 0)
                throw new ArgumentException("Lista vazia.");

            var maisBarato = produtos.OrderBy(p => p.Preco).First();
            var melhorQualidade = produtos.OrderByDescending(p => p.Qualidade).First();

            return SelecionarInterno(produtos, maisBarato, melhorQualidade);
        }

        protected abstract Produto SelecionarInterno(
            List<Produto> produtos,
            Produto maisBarato,
            Produto melhorQualidade);
    }

    // ==========================================
    //              MODOS CONCRETOS
    // ==========================================

    public sealed class SeletorEconomico : SeletorBase
    {
        protected override Produto SelecionarInterno(
            List<Produto> produtos,
            Produto maisBarato,
            Produto melhorQualidade)
            => maisBarato;
    }

    public sealed class SeletorPremium : SeletorBase
    {
        protected override Produto SelecionarInterno(
            List<Produto> produtos,
            Produto maisBarato,
            Produto melhorQualidade)
        {
            if (melhorQualidade.Preco - maisBarato.Preco <= 200)
                return melhorQualidade;

            return maisBarato;
        }
    }

    public sealed class SeletorQualidade : SeletorBase
    {
        protected override Produto SelecionarInterno(
            List<Produto> produtos,
            Produto maisBarato,
            Produto melhorQualidade)
            => melhorQualidade;
    }

    // ==========================================
    //                CLIENTE
    // ==========================================
    internal class Program
    {
        private static void Main()
        {
            var produtos = new List<Produto>
            {
                new Produto("Produto A", 100, 50),
                new Produto("Produto B", 250, 90),
                new Produto("Produto C", 120, 70)
            };

            SeletorBase seletor;

            seletor = new SeletorEconomico();
            Console.WriteLine("Econômico: " + seletor.Selecionar(produtos).Nome);

            seletor = new SeletorPremium();
            Console.WriteLine("Premium: " + seletor.Selecionar(produtos).Nome);

            seletor = new SeletorQualidade();
            Console.WriteLine("Qualidade: " + seletor.Selecionar(produtos).Nome);
        }
    }
}
