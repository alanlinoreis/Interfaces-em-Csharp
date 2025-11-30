using System.Globalization;
using Domain.Entities.Models;

namespace Domain.Entities.Repository;

public sealed class CsvProdutoRepository : IRepository<Produto, int>
{
    private readonly string _path;

    public CsvProdutoRepository(string path)
    {
        _path = path;
    }

    // -----------------------------
    // METODO AUXILIAR: LER LINHAS CSV
    // -----------------------------
    private List<Produto> ReadAll()
    {
        if (!File.Exists(_path))
            return new List<Produto>();

        var lines = File.ReadAllLines(_path);
        var list = new List<Produto>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = ParseCsvLine(line);

            int id = int.Parse(parts[0]);
            string nome = parts[1];
            decimal preco = decimal.Parse(parts[2], CultureInfo.InvariantCulture);
            int qualidade = int.Parse(parts[3]);

            list.Add(new Produto(id, nome, preco, qualidade));
        }

        return list;
    }

    // -----------------------------
    // METODO AUXILIAR: SALVAR CSV
    // -----------------------------
    private void WriteAll(IEnumerable<Produto> produtos)
    {
        var lines = produtos.Select(p =>
            $"{p.Id},{Escape(p.Nome)},{p.Preco.ToString(CultureInfo.InvariantCulture)},{p.Qualidade}");

        File.WriteAllLines(_path, lines);
    }

    // -----------------------------
    // CRUD
    // -----------------------------
    public Produto Add(Produto entity)
    {
        var produtos = ReadAll();

        // se já existe ID → sobrescreve
        produtos.RemoveAll(p => p.Id == entity.Id);

        produtos.Add(entity);
        WriteAll(produtos);

        return entity;
    }


    public Produto? GetById(int id)
    {
        return ReadAll().FirstOrDefault(p => p.Id == id);
    }

    public IReadOnlyList<Produto> ListAll()
    {
        return ReadAll();
    }

    public bool Update(Produto entity)
    {
        var produtos = ReadAll();

        var index = produtos.FindIndex(p => p.Id == entity.Id);
        if (index == -1)
            return false;

        produtos[index] = entity;
        WriteAll(produtos);
        return true;
    }

    public bool Remove(int id)
    {
        var produtos = ReadAll();
        var removed = produtos.RemoveAll(p => p.Id == id) > 0;

        if (removed)
            WriteAll(produtos);

        return removed;
    }

    // -----------------------------
    // Funções auxiliares para CSV
    // -----------------------------
    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }

    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool insideQuotes = false;
        var current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"' && !insideQuotes)
            {
                insideQuotes = true;
                continue;
            }

            if (c == '"' && insideQuotes)
            {
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++;
                }
                else
                {
                    insideQuotes = false;
                }
                continue;
            }

            if (c == ',' && !insideQuotes)
            {
                result.Add(current);
                current = "";
                continue;
            }

            current += c;
        }

        result.Add(current);
        return result.ToArray();
    }
}
