namespace Domain.Entities
{
    public static class SeletorFactory
    {
        public static ISeletorDeProduto Criar(string modo)
        {
            if (string.IsNullOrWhiteSpace(modo))
                return new SeletorEconomico();

            switch (modo.Trim().ToUpperInvariant())
            {
                case "ECONOMICO": return new SeletorEconomico();
                case "PREMIUM": return new SeletorPremium();
                case "QUALIDADE": return new SeletorQualidade();
                default: return new SeletorEconomico();
            }
        }
    }
}
