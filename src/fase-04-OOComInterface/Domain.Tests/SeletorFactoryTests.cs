using Xunit;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
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
        public void Deve_retornar_Premium()
        {
            var seletor = SeletorFactory.Criar("PREMIUM");

            Assert.IsType<SeletorPremium>(seletor);
        }
    }
}
