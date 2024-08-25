using DSharpPlus;
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
        [Command("newsEmbed")]
        public async Task TestEmbed(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "**Patch・1.0**",
                Description = "В **Дедлоке** убрали **НДА** 🎉🎉🎉",
                Color = DiscordColor.Gray,
                ImageUrl = "https://hawk.live/storage/post-images/deadlock-valve-full-version-available-10588.jpg",
            };
            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("voiceMenu")]
        public async Task voiceMenu(CommandContext ctx)
        {
            // Добавить слот в лобби
            var addButton = new DiscordButtonComponent(ButtonStyle.Secondary, "addButton", null, false, new DiscordComponentEmoji(1277192374761553951));
            // Переименовать лобби
            var nameButton = new DiscordButtonComponent(ButtonStyle.Secondary, "nameButton", null, false, new DiscordComponentEmoji(1277193902993969183));
            // Закрыть/открыть лобби
            var privateButton = new DiscordButtonComponent(ButtonStyle.Secondary, "privateButton", null, false, new DiscordComponentEmoji(1277198085554176033));
            // Найти напарников
            var teamButton = new DiscordButtonComponent(ButtonStyle.Secondary, "teamButton", null, false, new DiscordComponentEmoji(1277198087336624179));
            // Поднять комнату
            var upButton = new DiscordButtonComponent(ButtonStyle.Secondary, "upButton", null, false, new DiscordComponentEmoji(1277198088859025493));
            // Убрать слот из лобби
            var reduceButton = new DiscordButtonComponent(ButtonStyle.Secondary, "reduceButton", null, false, new DiscordComponentEmoji(1277198796467601519));
            // кикнуть из лобби
            var kickButton = new DiscordButtonComponent(ButtonStyle.Secondary, "kickButton", null, false, new DiscordComponentEmoji(1277198084278845450));
            // забанить в лобби
            var banButton = new DiscordButtonComponent(ButtonStyle.Secondary, "banButton", null, false, new DiscordComponentEmoji(1277198078083989537));
            // установить количество слотов в лобби
            var slotsButton = new DiscordButtonComponent(ButtonStyle.Secondary, "slotsButton", null, false, new DiscordComponentEmoji(1277198082882146346));
            // поменять битрейт комнаты
            var bitrateButton = new DiscordButtonComponent(ButtonStyle.Secondary, "bitrateButton", null, false, new DiscordComponentEmoji(1277198080952893542));

            var menuBuilder = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().
                WithTitle("Настройка голосового канала").
                WithDescription($"{DiscordEmoji.FromGuildEmote(Program.Client, 1277192374761553951)} - Добавить 1 слот в вашу комнату\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198796467601519)} - Убрать 1 слот с вашей комнаты\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277193902993969183)} - Сменить название вашей комнаты\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198085554176033)} - Закрыть/открыть доступ к комнате\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198087336624179)} - Создать приглашение в комнату\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198088859025493)} - Поднять комнату\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198084278845450)} - Кикнуть человека из комнате\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198078083989537)} - Забанить человека в комнате\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198082882146346)} - Установить количество слотов в комнате\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1277198080952893542)} - Поменять битрейт комнаты")).
                AddComponents(addButton, nameButton, privateButton, teamButton, upButton).
                AddComponents(reduceButton, kickButton, banButton, slotsButton, bitrateButton);

            await ctx.Channel.SendMessageAsync(menuBuilder);
        }
    }
}
