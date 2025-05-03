using Discord;
using Frombot.Services;
using Discord.WebSocket;

namespace Frombot.Commands;


public class MessageCommand
{
    private readonly DiscordSocketClient bot;

    public MessageCommand(DiscordSocketClient bot)
    {
        this.bot = bot;
    }

    public async Task HandleMessageCommand(SocketMessage message)
    {
        var parts = message.Content.Split(' ', 3);
        if (parts.Length < 3)
        {
            await message.Channel.SendMessageAsync("Formato incorreto! Use: ``!msg [alias/userid] [mensagem]``");
            return;
        }

        var targetUsername = parts[1].ToLower().Trim();
        var messageToSend = parts[2];

        ulong userId;
        if (ulong.TryParse(targetUsername, out userId)) // Direct user ID
        {
            await SendDirectMessage(userId, messageToSend);
        }
        else // Try to find user by alias
        {
            if (Constants.Constants.UserAliases.TryGetValue(targetUsername, out userId))
            {
                await SendDirectMessage(userId, messageToSend);
            }
            else
            {
                await message.Channel.SendMessageAsync($"Alias '{targetUsername}' não encontrado!");
            }
        }
    }

    private async Task SendDirectMessage(ulong userId, string content)
    {
        var user = await bot.GetUserAsync(userId);
        if (user == null)
        {
            Console.WriteLine($"User with ID {userId} is null");
            return;
        }

        try
        {
            await user.SendMessageAsync(content);
        }
        catch (Exception)
        {
            // Handle any errors (like user having DMs disabled)
        }
    }
}
