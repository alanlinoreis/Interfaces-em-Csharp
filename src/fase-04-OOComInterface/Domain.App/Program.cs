using System;
using System.Collections.Generic;
using Fase04.Domain.Entities;

namespace Fase04.Domain.App
{
    internal class Program
    {
        static void Main()
        {
            var produtos = new List<Produto>
            {
                new Produto("Produto A", 100, 50),
                new Produto("Produto B", 250, 90),
                new Produto("Produto C", 120, 70)
            };

            ISeletorDeProduto seletor;

            seletor = SeletorFactory.Criar("ECONOMICO");
            Console.WriteLine("Econômico: " + seletor.Selecionar(produtos).Nome);

            seletor = SeletorFactory.Criar("PREMIUM");
            Console.WriteLine("Premium: " + seletor.Selecionar(produtos).Nome);

            seletor = SeletorFactory.Criar("QUALIDADE");
            Console.WriteLine("Qualidade: " + seletor.Selecionar(produtos).Nome);

            seletor = SeletorFactory.Criar("qualquercoisa");
            Console.WriteLine("Fallback: " + seletor.Selecionar(produtos).Nome);
        }
    }
}
