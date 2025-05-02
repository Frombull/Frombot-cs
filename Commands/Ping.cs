using Discord;
using Discord.WebSocket;

namespace Frombot.Commands;

public class PingCommand
{
    public async Task ExecuteAsync(SocketMessage message)
    {
        await message.Channel.SendMessageAsync("Pong!");
    }
}
