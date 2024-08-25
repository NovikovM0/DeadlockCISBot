using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DeadlockCISBot.Commands.Slash
{
    [SlashCommandGroup("pool", "Создать голосование")]
    internal class PoolSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("timer", "Создать голосование с таймером")]
        public async Task TimerPoolSlashCommand(InteractionContext ctx,
            [Option("Title", "Вставьте название голосования")] string poolTitle,
            [Option("Options", "Вставьте опции голосования")] string poolOptions,
            [Option("Emojis", "Вставьте эмоджи голосования")] string poolEmojis,
            [Option("Timer", "Время действия голосования (в секундах)")] double poolTimer
            )
        {
            await ctx.DeferAsync();
            var interactivity = Program.Client.GetInteractivity();
            var poolDescription = "";
            string[] options = poolOptions.Split(',');
            string[] emojis = poolEmojis.Split(',');
            var poolTime = TimeSpan.FromSeconds(poolTimer);

            for (int i = 0; i < options.Length; i++)
            {
                poolDescription += $"{emojis[i]} | {options[i]} \n";
            }

            var poolEmbed = new DiscordEmbedBuilder
            {
                Title = poolTitle,
                Description = poolDescription,
            };

            var sentPool = await ctx.Channel.SendMessageAsync(embed: poolEmbed);

            foreach (var emoji in emojis)
            {
                await sentPool.CreateReactionAsync(DiscordEmoji.FromUnicode(Program.Client, emoji));
            }

            var totalReactions = await interactivity.CollectReactionsAsync(sentPool, poolTime);
            var poolResults = "";

            foreach (var reaction in totalReactions)
            {
                poolResults += $"{reaction.Emoji} | {reaction.Total} \n";
            }

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SapGreen,
                Title = $"Результаты голосования '{poolTitle}'",
                Description = poolResults,
            };

            await ctx.EditResponseAsync(new DSharpPlus.Entities.DiscordWebhookBuilder().AddEmbed(resultEmbed));
        }

        [SlashCommand("basic", "Создать голосование без таймером")]
        public async Task PoolSlashCommand(InteractionContext ctx,
            [Option("Title", "Вставьте название голосования")] string poolTitle,
            [Option("Options", "Вставьте опции голосования")] string poolOptions,
            [Option("Emojis", "Вставьте эмоджи голосования")] string poolEmojis
            )
        {
            await ctx.DeferAsync();
            var interactivity = Program.Client.GetInteractivity();
            var poolDescription = "";
            string[] options = poolOptions.Split(',');
            string[] emojis = poolEmojis.Split(',');

            for (int i = 0; i < options.Length; i++)
            {
                poolDescription += $"{emojis[i]} | {options[i]} \n";
            }

            var poolEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SapGreen,
                Title = poolTitle,
                Description = poolDescription,
            };

            var sentPool = await ctx.Channel.SendMessageAsync(embed: poolEmbed);

            foreach (var emoji in emojis)
            {
                await sentPool.CreateReactionAsync(DiscordEmoji.FromUnicode(Program.Client, emoji));
            }

            await ctx.EditResponseAsync(new DSharpPlus.Entities.DiscordWebhookBuilder());
        }
    }

    [SlashCommandGroup("news", "Опубликовать новость")]
    internal class NewsSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("game", "Опубликовать новость об игре")]
        public async Task GameNewsSlashCommand(InteractionContext ctx,
            [Option("Title", "Вставьте заголовок обновление")] string newsTitle,
            [Option("Description", "Краткое описание обновления")] string newsDescription,
            [Option("patchLink", "Ссылка на обновление")] string patchLink,
            [Option("imageLink", "Ссылка на картинку")] string imageLink = "https://i.ibb.co/cLWykR3/Deadlock-Image.jpg")
        {
            await ctx.DeferAsync();
            var message = new DiscordEmbedBuilder
            {
                Title = newsTitle,
                Description = $"{newsDescription}\n\n" +
                $"[**Полное обновление**]({patchLink})",
                ImageUrl = $"{imageLink}",
            };
            await ctx.EditResponseAsync(new DSharpPlus.Entities.DiscordWebhookBuilder().AddEmbed(message));
        }

        [SlashCommand("server", "Опубликовать новость о сервере")]
        public async Task ServerGameNewsSlashCommand(InteractionContext ctx,
            [Option("Title", "Вставьте заголовок новости")] string newsTitle,
            [Option("Description", "Краткое описание новости")] string newsDescription,
            [Option("imageLink", "Ссылка на картинку")] string imageLink = "https://i.ibb.co/cLWykR3/Deadlock-Image.jpg")

        {
            await ctx.DeferAsync();
            var message = new DiscordEmbedBuilder
            {
                Title = newsTitle,
                Description = newsDescription,
                ImageUrl = $"{imageLink}",
            };
            await ctx.EditResponseAsync(new DSharpPlus.Entities.DiscordWebhookBuilder().AddEmbed(message));
        }
    }
}
