using DotNetEnv;
using Frombot.Services;

namespace Frombot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Env.Load();
        
        var bot = new BotService();

        await bot.InitializeAsync();

        await Task.Delay(Timeout.Infinite);
    }
}
