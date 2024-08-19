using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockCISBot.Commands
{
    internal class BasicCommands : BaseCommandModule
    {
        [Cooldown(1, 5, CooldownBucketType.User)]
        [Command("test")]
        public async Task TestCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Я считаю что {ctx.User.Email} чертовски хорош");
        }

        [Command("testEmbed")]
        public async Task TestEmbed(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "test",
                Description = "test",
                Color = DiscordColor.Green,
                Timestamp = DateTime.Now,
            };
            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("testInteraction")]
        public async Task TestInteraction(CommandContext ctx)
        {
            var interactivity = Program.Client.GetInteractivity();

            var messageToRetriever = await interactivity.WaitForMessageAsync(message => message.Content == "hello");

            if (messageToRetriever.Result.Content == "hello")
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} Поздоровался");
            }
        }

        [Command("testPool")]
        public async Task TestPool(CommandContext ctx, string option1, string option2, string option3, string option4, int poolTimer, [RemainingText] string poolTitle)
        {
            var interactivity = Program.Client.GetInteractivity();
            var poolTime = TimeSpan.FromMinutes(poolTimer);

            DiscordEmoji[] emojiOptions = {
                DiscordEmoji.FromName(Program.Client, ":one:"),
                DiscordEmoji.FromName(Program.Client, ":two:"),
                DiscordEmoji.FromName(Program.Client, ":three:"),
                DiscordEmoji.FromName(Program.Client, ":four:"),
                };

            string optionDescription =
                $"{emojiOptions[0]} | {option1} \n" +
                $"{emojiOptions[1]} | {option2} \n" +
                $"{emojiOptions[2]} | {option3} \n" +
                $"{emojiOptions[3]} | {option4}";

            var poolMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SapGreen,
                Title = poolTitle,
                Description = optionDescription,
            };

            var sentPool = await ctx.Channel.SendMessageAsync(embed: poolMessage);
            foreach (var emoji in emojiOptions) 
            {
                await sentPool.CreateReactionAsync(emoji);
            }

            var totalReactions = await interactivity.CollectReactionsAsync(sentPool, poolTime);

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SapGreen,
                Title = "Голосование завершено",
                Description = "Обработка результатов",
            };

            await ctx.Channel.SendMessageAsync(embed: resultEmbed);

            List<Emoji> emojiVotes = new List<Emoji> {};

            foreach (var emoji in totalReactions) 
            {
                int cnt = 0;
                foreach (var user in emoji.Users) 
                {
                    if (user != null && !user.IsBot) 
                    {
                        cnt++;
                    }
                }
                if(cnt != 0)
                {
                    var em = new Emoji { Name = emoji.Emoji.Name, Count = cnt };
                    emojiVotes.Add(em);
                }
            }

            string resultDescription = "";

            foreach (var em in emojiVotes)
            {
                resultDescription += $"{em.Name}: {em.Count} голосов \n";
            }


            resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.SapGreen,
                Title = "Результат голосования",
                Description = resultDescription,
            };

            await ctx.Channel.SendMessageAsync(embed: resultEmbed);
        }
    }

}
