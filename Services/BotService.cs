using Discord;
using DotNetEnv;
using Discord.Commands;
using Frombot.Commands;
using Discord.WebSocket;
using Frombot.Configuration;

namespace Frombot.Services;


public class BotService
{
    protected readonly DiscordSocketClient bot;
    private readonly string? botToken;

    public BotService()
    {
        bot = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 30
        });

        Env.Load();
        botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (string.IsNullOrEmpty(botToken))
            throw new Exception("BOT_TOKEN not defined in .env file");

        bot.Ready += OnReadyAsync;
        bot.MessageReceived += OnMessageReceivedAsync;
        bot.Log += Logger.Log;
    }

    public async Task InitializeAsync()
    {
        await bot.LoginAsync(TokenType.Bot, botToken);
        await bot.StartAsync();
        await bot.SetActivityAsync(new Game("with your mom"));

        await Task.Delay(Timeout.Infinite);
    }

    private Task OnReadyAsync()
    {
        Console.WriteLine($"------------------------------");
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] I'M ALIVE!");
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot || message is not SocketUserMessage userMessage) return;

        if (!userMessage.Content.StartsWith(BotConfig.CommandPrefix)) return;

        string command = userMessage.Content.Split(' ')[0].Substring(1).ToLower().Trim();

        switch (command)
        {
            case "ping":
                await new PingCommand().ExecuteAsync(message);
                break;
            
            case "msg" or "message":
                await Logger.Log("Message command received");
                await new MessageCommand(bot).HandleMessageCommand(message);
                break;
            
        }
    }
}
