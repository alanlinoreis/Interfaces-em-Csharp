using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Seletores;

namespace Domain.Tests
{
    public class SeletorFactoryTests
    {
        [Fact]
        public void Deve_retornar_Economico_para_modo_invalido()
        {
            var seletor = SeletorFactory.Criar("qualquercoisa");

            Assert.IsType<SeletorEconomico>(seletor);
        }

        [Fact]
        public void Deve_retornar_Economico_para_vazio()
        {
            var seletor = SeletorFactory.Criar("");

            Assert.IsType<SeletorEconomico>(seletor);
        }

        [Fact]
        public void Deve_retornar_Premium()
        {
            var seletor = SeletorFactory.Criar("PREMIUM");

            Assert.IsType<SeletorPremium>(seletor);
        }

        [Fact]
        public void Deve_retornar_Qualidade()
        {
            var seletor = SeletorFactory.Criar("QUALIDADE");

            Assert.IsType<SeletorQualidade>(seletor);
        }
    }
}
