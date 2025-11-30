using Xunit;
using Domain.Entities.Seletores;

namespace Domain.Tests
{
    public class SeletorFactoryTests
    {
        [Fact]
        public void Deve_retornar_Economico_para_modo_economico()
        {
            var seletor = SeletorFactory.Criar(ModoSelecao.Economico);
            Assert.IsType<SeletorEconomico>(seletor);
        }

        [Fact]
        public void Deve_retornar_Premium_para_modo_premium()
        {
            var seletor = SeletorFactory.Criar(ModoSelecao.Premium);
            Assert.IsType<SeletorPremium>(seletor);
        }

        [Fact]
        public void Deve_retornar_Qualidade_para_modo_qualidade()
        {
            var seletor = SeletorFactory.Criar(ModoSelecao.Qualidade);
            Assert.IsType<SeletorQualidade>(seletor);
        }
    }
}
