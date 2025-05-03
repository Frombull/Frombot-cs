using Frombot.Services;

namespace Frombot;


public static class Program
{
    public static async Task Main(string[] args)
    {
        var botService = new BotService();

        await botService.InitializeAsync();
    }
}
