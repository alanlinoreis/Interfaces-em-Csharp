namespace Domain.Entities.Models;

public sealed record Produto(
    int Id,
    string Nome,
    decimal Preco,
    int Qualidade
);
