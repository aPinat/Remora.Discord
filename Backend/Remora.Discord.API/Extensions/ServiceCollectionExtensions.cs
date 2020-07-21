//
//  ServiceCollectionExtensions.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Activities;
using Remora.Discord.API.Abstractions.Commands;
using Remora.Discord.API.Abstractions.Events;
using Remora.Discord.API.Abstractions.Gateway;
using Remora.Discord.API.Abstractions.Presence;
using Remora.Discord.API.API.Commands;
using Remora.Discord.API.API.Events;
using Remora.Discord.API.API.Objects.Activities;
using Remora.Discord.API.API.Objects.Gateway;
using Remora.Discord.API.Json;

namespace Remora.Discord.API.Extensions
{
    /// <summary>
    /// Defines various extension methods to the <see cref="IServiceCollection"/> class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds supporting services for the Discord API.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>The service collection, with the services.</returns>
        public static IServiceCollection AddDiscordApi(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .Configure<JsonSerializerOptions>
                (
                    o =>
                    {
                        o.Converters.Add(new OptionalConverterFactory());
                        o.Converters.Add(new NullableConverterFactory());

                        o.Converters.Add(new UnixDateTimeConverter());
                        o.Converters.Add(new UnixDateTimeOffsetConverter());
                        o.Converters.Add(new PartySizeConverter());
                        o.Converters.Add(new HeartbeatConverter());
                        o.Converters.Add(new ShardIdentificationConverter());
                        o.Converters.Add(new SnowflakeConverter());
                        o.Converters.Add(new PayloadConverter());
                        o.Converters.Add(new HeartbeatConverter());

                        o.Converters.Add
                        (
                            new DataObjectConverter<ISessionStartLimit, SessionStartLimit>()
                                .WithPropertyConverter(st => st.ResetAfter, new MillisecondTimeSpanConverter())
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IActivity, Activity>()
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IGatewayEndpoint, GatewayEndpoint>()
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IIdentify, Identify>()
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IRequestGuildMembers, RequestGuildMembers>()
                                .WithPropertyName(r => r.GuildID, "guild_id")
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IResume, Resume>()
                                .WithPropertyName(r => r.SequenceNumber, "seq")
                                .WithPropertyName(r => r.SessionID, "session_id")
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IUpdateStatus, UpdateStatus>()
                                .WithPropertyName(u => u.IsAFK, "afk")
                                .WithPropertyConverter
                                (
                                    u => u.Status,
                                    new JsonStringEnumConverter(new SnakeCaseNamingPolicy())
                                )
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IUpdateVoiceState, UpdateVoiceState>()
                                .WithPropertyName(u => u.IsSelfMuted, "self_mute")
                                .WithPropertyName(u => u.IsSelfDeafened, "self_deaf")
                                .WithPropertyName(u => u.GuildID, "guild_id")
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IHello, Hello>()
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IConnectionProperties, ConnectionProperties>()
                                .WithPropertyName(p => p.OperatingSystem, "$os")
                                .WithPropertyName(p => p.Browser, "$browser")
                                .WithPropertyName(p => p.Device, "$device")
                        );

                        o.Converters.Add
                        (
                            new DataObjectConverter<IPresenceUpdate, PresenceUpdate>()
                                .WithPropertyConverter(p => p.Status, new JsonStringEnumConverter())
                        );

                        var snakeCasePolicy = new SnakeCaseNamingPolicy();
                        o.PropertyNamingPolicy = snakeCasePolicy;
                        o.DictionaryKeyPolicy = snakeCasePolicy;
                    }
                );

            return serviceCollection;
        }
    }
}