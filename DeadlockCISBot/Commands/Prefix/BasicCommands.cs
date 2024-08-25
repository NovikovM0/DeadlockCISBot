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
            var addButton = new DiscordButtonComponent(ButtonStyle.Secondary, "addButton", null, false, new DiscordComponentEmoji(1266393654847606868));
            // Переименовать лобби
            var nameButton = new DiscordButtonComponent(ButtonStyle.Secondary, "nameButton", null, false, new DiscordComponentEmoji(1266428910564802694));
            // Закрыть/открыть лобби
            var privateButton = new DiscordButtonComponent(ButtonStyle.Secondary, "privateButton", null, false, new DiscordComponentEmoji(1266428912351838252));
            // Найти напарников
            var teamButton = new DiscordButtonComponent(ButtonStyle.Secondary, "teamButton", null, false, new DiscordComponentEmoji(1266428897776369726));
            // Поднять комнату
            var upButton = new DiscordButtonComponent(ButtonStyle.Secondary, "upButton", null, false, new DiscordComponentEmoji(1266428909214371973));
            // Убрать слот из лобби
            var reduceButton = new DiscordButtonComponent(ButtonStyle.Secondary, "reduceButton", null, false, new DiscordComponentEmoji(1266428903002738778));
            // кикнуть из лобби
            var kickButton = new DiscordButtonComponent(ButtonStyle.Secondary, "kickButton", null, false, new DiscordComponentEmoji(1266428905883963452));
            // забанить в лобби
            var banButton = new DiscordButtonComponent(ButtonStyle.Secondary, "banButton", null, false, new DiscordComponentEmoji(1266428920933122143));
            // установить количество слотов в лобби
            var slotsButton = new DiscordButtonComponent(ButtonStyle.Secondary, "slotsButton", null, false, new DiscordComponentEmoji(1266406168595533855));
            // поменять битрейт комнаты
            var bitrateButton = new DiscordButtonComponent(ButtonStyle.Secondary, "bitrateButton", null, false, new DiscordComponentEmoji(1266428912351838252));

            var menuBuilder = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().
                WithTitle("Настройка голосового канала").
                WithDescription($"{DiscordEmoji.FromGuildEmote(Program.Client, 1266393654847606868)} - Добавить 1 слот в вашу комнату\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428903002738778)} - Убрать 1 слот с вашей комнаты\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428910564802694)} - Сменить название вашей комнаты\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428912351838252)} - Закрыть/открыть доступ к войсу\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428897776369726)} - Создать приглашение в войс\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428909214371973)} - Поднять комнату\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428905883963452)} - Кикнуть человека из войса\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428920933122143)} - Забанить человека в войсе\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266406168595533855)} - Установить количество слотов в комнате\n" +
                $"{DiscordEmoji.FromGuildEmote(Program.Client, 1266428912351838252)} - Поменять битрейт войса")).
                AddComponents(addButton, nameButton, privateButton, teamButton, upButton).
                AddComponents(reduceButton, kickButton, banButton, slotsButton, bitrateButton);

            await ctx.Channel.SendMessageAsync(menuBuilder);
        }
    }
}
