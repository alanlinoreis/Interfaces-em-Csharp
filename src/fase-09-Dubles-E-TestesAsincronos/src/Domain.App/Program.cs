using Domain.Entities.Doubles;
using Domain.Entities.Services;

namespace Domain.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== Fase 09 — Dublês Avançados e Testes Assíncronos ===\n");

        // Criamos um ReaderFake com um fluxo simples
        var reader = new ReaderFake<int>(new[] { 10, 20, 30 });

        // WriterFake acumula os itens recebidos
        var writer = new WriterFake<int>();

        // ClockFake permite simular passagem de tempo sem delays reais
        var clock = new ClockFake();

        // Nosso serviço assíncrono principal
        var pump = new PumpService<int>(reader, writer, clock);

        Console.WriteLine("Iniciando PumpService...\n");

        await pump.RunAsync();

        Console.WriteLine("Itens escritos pelo WriterFake:");
        foreach (var item in writer.Written)
        {
            Console.WriteLine($"- {item}");
        }

        Console.WriteLine("\nRelógio virtual após execução:");
        Console.WriteLine($"ClockFake.Now = {clock.Now:HH:mm:ss.fff}");
    }
}
