using DeadlockCISBot.Commands;
using DeadlockCISBot.Commands.Slash;
using DeadlockCISBot.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net.Models;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DeadlockCISBot
{
    internal class Program
    {
        #region Инфраструктура

        public class VoiceInvite
        {
            public int? userCount { internal get; set; }
            public ulong voiceId { internal get; set; }
            public string voiceTitle { internal get; set; }
            public string voiceUrl { internal get; set; }
            public ulong messageId { internal get; set; }
        }
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; }


        private static Task<DiscordGuild> _guild;

        public static Task<DiscordGuild> Guild
        {
            get
            {
                //if (_guild == null)
                //{
                    _guild = Client.GetGuildAsync(1264136717015977986);
                //}
                return _guild;
            }
        }

        private static Task<IReadOnlyList<DiscordChannel>> _VoiceChannelList;
        public static Task<IReadOnlyList<DiscordChannel>> VoiceChannelList
        {
            get
            {
                //if (_VoiceChannelList == null)
                //{
                    _VoiceChannelList = Guild.Result.GetChannelsAsync();
                //}
                return _VoiceChannelList;
            }
        }

        public static List<DiscordChannel> voiceChannelList = new List<DiscordChannel>();
        public static List<VoiceInvite> voiceInvitelList = new List<VoiceInvite>();

        #endregion

        static async Task Main(string[] args)
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);

            //Создание интерактивити и обозначение его стандартного время жизни
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1)
            });

            Client.Ready += OnClient_Ready;
            Client.ComponentInteractionCreated += Client_ComponentInteractionCreated;
            Client.VoiceStateUpdated += Client_VoiceStateUpdated;

            var CommandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true
            };

            Commands = Client.UseCommandsNext(CommandsConfig);
            var slashCommandsConfiguration = Client.UseSlashCommands();
            slashCommandsConfiguration.RegisterCommands<PoolSlashCommands>();
            slashCommandsConfiguration.RegisterCommands<NewsSlashCommands>();
            Commands.RegisterCommands<BasicCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task Client_ComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            var guild = e.Guild;
            var bolvanka = await guild.GetMemberAsync(817707571347324939);
            var everyoneRole = guild.GetRole(1264136717015977986);
            var currentMember = await guild.GetMemberAsync(e.User.Id);
            switch (e.Interaction.Data.CustomId)
            {
                case "addButton":
                    var addSlot = new Action<ChannelEditModel>(x => x.Userlimit = currentMember.VoiceState.Channel.UserLimit + 1);
                    await currentMember.VoiceState.Channel.ModifyAsync(addSlot);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "nameButton":



                    var changeName = new Action<ChannelEditModel>(x => { x.Name = "AAAAAA"; });
                    await currentMember.VoiceState.Channel.ModifyAsync(changeName);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "privateButton":
                    //var havePermissions = currentMember.VoiceState.Channel.UserPermissions.Value.HasPermission(Permissions.UseVoice);
                    //var voicePermition = currentMember.VoiceState.Channel.PermissionsFor(currentMember).HasPermission(Permissions.UseVoice);
                    var voicePermition = currentMember.VoiceState.Channel.PermissionsFor(bolvanka).HasPermission(Permissions.UseVoice);
                    DiscordOverwriteBuilder[] overwiteBuilder = null;
                    if (voicePermition)
                    {
                        overwiteBuilder = new DiscordOverwriteBuilder[] { new DiscordOverwriteBuilder(everyoneRole).Deny(Permissions.UseVoice) };
                    }
                    else
                    {
                        overwiteBuilder = new DiscordOverwriteBuilder[] { new DiscordOverwriteBuilder(everyoneRole).Allow(Permissions.UseVoice) };
                    }
                    var changePrivate = new Action<ChannelEditModel>(x => x.PermissionOverwrites = overwiteBuilder);
                    await currentMember.VoiceState.Channel.ModifyAsync(changePrivate);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "teamButton":
                    var sendInviteChannel = await sender.GetChannelAsync(1264143565269766224);
                    var voiceChannel = await sender.GetChannelAsync(currentMember.VoiceState.Channel.Id);
                    var voiceInvite = await voiceChannel.CreateInviteAsync(max_uses: 0, unique: true);
                    var voiceUrl = voiceInvite.ToString();
                    var usersInVoice = "";
                    foreach (var user in currentMember.VoiceState.Channel.Users)
                    {
                        usersInVoice += $"<@{user.Id}>\n";
                    }
                    var linkButton = new DiscordLinkButtonComponent(voiceUrl , $"Зайти в комнату");
                    var inviteEmbed = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder
                    {
                        Title = $"{currentMember.VoiceState.Channel.Name}",
                        Description = $"**Тайтл лобби**\n\n" +
                        $"{usersInVoice}",
                        //"**Тип лобби:**\n" +
                        //"Ранговая игра",
                    });
                    inviteEmbed.AddComponents(linkButton);
                    var message = await sendInviteChannel.SendMessageAsync(inviteEmbed);
                    voiceInvitelList.Add(new VoiceInvite()
                    {
                        userCount = voiceChannel.Users.Count,
                        voiceId = voiceChannel.Id,
                        voiceTitle = "**Тайтл лобби**",
                        voiceUrl = voiceUrl,
                        messageId = message.Id,
                    });
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "upButton":
                    var upVoice = new Action<ChannelEditModel>(x => x.Position = 1);
                    await currentMember.VoiceState.Channel.ModifyAsync(upVoice);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "reduceButton":
                    var reduceSlot = new Action<ChannelEditModel>(x => x.Userlimit = currentMember.VoiceState.Channel.UserLimit - 1);
                    await currentMember.VoiceState.Channel.ModifyAsync(reduceSlot);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "kickButton":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
                case "banButton":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break; 
                case "slotsButton":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break; 
                case "bitrateButton":
                    var changeBirate = new Action<ChannelEditModel>(x => x.Bitrate = 64);
                    if (currentMember.VoiceState.Channel.Bitrate != 64)
                    {
                        changeBirate = new Action<ChannelEditModel>(x => x.Bitrate = 96);
                    }
                    await currentMember.VoiceState.Channel.ModifyAsync(changeBirate);
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DSharpPlus.Entities.DiscordInteractionResponseBuilder());
                    break;
            }
        }

        private static async Task Client_VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            var voiceChannelForCreation = await sender.GetChannelAsync(1277177145294327820);
            var usersWantsToCreate = voiceChannelForCreation.Users;
            int cnt = (await VoiceChannelList).Where(v => v.Type == ChannelType.Voice && v.ParentId == 1277178639837954109).ToList().Count;
            if (cnt > 0)
            {
                await DeleteEmptyVoice();
            }
            if (usersWantsToCreate.Any())
            {
                foreach (var User in usersWantsToCreate)
                {
                    var category = await sender.GetChannelAsync(1277178639837954109);
                    var channelDirection = await (await Guild).CreateVoiceChannelAsync($"Войс {User.Username}", parent: category);
                    await channelDirection.PlaceMemberAsync(User).ConfigureAwait(false);
                }
            }
            if (voiceInvitelList.Any())
            {
                await InviteMessageEdit();
            }
        }

        private static async Task DeleteEmptyVoice()
        {
                foreach (var Channel in await VoiceChannelList)
                {
                    
                    if (Channel.Users.Count == 0 && Channel.Type == ChannelType.Voice && Channel.ParentId == 1277178639837954109)
                    {
                        await Channel.DeleteAsync("End of the event");
                    }
                }
        }

        private static async Task InviteMessageEdit()
        {
            foreach (var invite in voiceInvitelList)
            {
                var currentVoice = await Client.GetChannelAsync(invite.voiceId);
                var inviteChannel = await Client.GetChannelAsync(1264143565269766224);
                if (invite.userCount != currentVoice.Users.Count)
                {
                    if (currentVoice.Users.Count != 0)
                    {
                        var oldMessage = await inviteChannel.GetMessageAsync(invite.messageId);
                        var usersInVoice = "";
                        foreach (var user in currentVoice.Users)
                        {
                            usersInVoice += $"<@{user.Id}>\n";
                        }
                        var linkButton = new DiscordLinkButtonComponent(invite.voiceUrl, $"Зайти в комнату");
                        var inviteEmbed = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder
                        {
                            Title = $"{currentVoice.Name}",
                            Description = $"Updated\n\n" +
                            $"{usersInVoice}",
                            //"**Тип лобби:**\n" +
                            //"Ранговая игра",
                        });
                        inviteEmbed.AddComponents(linkButton);
                        await oldMessage.ModifyAsync(inviteEmbed);
                        invite.userCount = currentVoice.Users.Count;
                    }
                    else
                    {
                        var oldMessage = await inviteChannel.GetMessageAsync(invite.messageId);
                        await oldMessage.DeleteAsync("End of the event");
                        voiceInvitelList.Remove(invite);
                    }
                }
            }
        }

        public async Task ChannelCreate(CommandContext ctx)
        {
            var category = ctx.Channel.Guild.GetChannel(1266454424457445417);
            var channel = await ctx.Guild.CreateVoiceChannelAsync("ChannelName", parent: category);
            await channel.PlaceMemberAsync(ctx.Member).ConfigureAwait(false);
        }

        private static Task OnClient_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }


    }
}
