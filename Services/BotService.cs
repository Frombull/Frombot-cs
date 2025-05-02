using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Frombot.Commands;
using Frombot.Configuration;

namespace Frombot.Services;

public class BotService
{
    private readonly DiscordSocketClient bot;
    private readonly string? botToken;

    public BotService()
    {
        bot = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 30
        });

        botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (string.IsNullOrEmpty(botToken))
            throw new Exception("BOT_TOKEN not defined in .env file");

        bot.Ready += OnReadyAsync;
        bot.MessageReceived += OnMessageReceivedAsync;
        bot.Log += LogAsync;
    }

    public async Task InitializeAsync()
    {
        await bot.LoginAsync(TokenType.Bot, botToken);
        await bot.StartAsync();
    }

    private Task OnReadyAsync()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] I'M ALIVE!");
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot || message is not SocketUserMessage userMessage) return;

        int argPos = 0;

        if (userMessage.HasCharPrefix(BotConfig.CommandPrefix, ref argPos))
        {
            string command = userMessage.Content.Substring(argPos).ToLower();

            switch (command)
            {
                case "ping":
                    await new PingCommand().ExecuteAsync(message);
                    break;
            }
        }
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine($" {log}");
        return Task.CompletedTask;
    }
}
