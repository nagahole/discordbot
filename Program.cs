using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Utils;
using System.IO;
using System.Diagnostics;

namespace Discord_Bot_2
{
    class Program
    {
        public static Random rnd = new Random();

        private static string _folderPath = Path.GetDirectoryName(
            Process.GetCurrentProcess().MainModule.FileName) + @"\data\";
        public static string folderPath => _folderPath;

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "NjgxNDIzODYwMTA4ODg2MDY2.XlOPfw.xtw--G1LE4-NTFTiK8s2CrrBNLI";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.MessageReceived += LogMessageIntoConsoleAsync;
            await _commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), _services);
        }
        private Task LogMessageIntoConsoleAsync(SocketMessage msg)
        {
            Console.WriteLine($"{msg.Timestamp.ToString()} {msg.Author.ToString()} : {(msg as SocketUserMessage).ToString()}");
            return Task.CompletedTask;
        }


        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("i'", ref argPos) || message.HasStringPrefix("i", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (result.IsSuccess) return;
                Console.WriteLine(result.ErrorReason);
            }

                
            if (message.HasStringPrefix(@"`", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }
    }
}

