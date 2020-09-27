//
//  DiscordRestGatewayAPITests.cs
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

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Rest.API;
using Remora.Discord.Rest.Tests.Extensions;
using Remora.Discord.Rest.Tests.TestBases;
using Remora.Discord.Tests;
using RichardSzalay.MockHttp;
using Xunit;

namespace Remora.Discord.Rest.Tests.API.Gateway
{
    /// <summary>
    /// Tests the <see cref="DiscordRestGatewayAPI"/> class.
    /// </summary>
    public class DiscordRestGatewayAPITests
    {
        /// <summary>
        /// Tests the <see cref="DiscordRestGatewayAPI.GetGatewayAsync"/> method.
        /// </summary>
        public class GetGatewayAsync : RestAPITestBase
        {
            /// <summary>
            /// Tests whether the API method performs its request correctly.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
            [Fact]
            public async Task PerformsRequestCorrectly()
            {
                var services = CreateConfiguredAPIServices
                (
                    b => b
                        .Expect(HttpMethod.Get, $"{Constants.BaseURL}gateway")
                        .WithNoContent()
                        .Respond("application/json", "{ \"url\": \"wss://gateway.discord.gg/\" }")
                );

                var api = services.GetRequiredService<IDiscordRestGatewayAPI>();

                var result = await api.GetGatewayAsync();
                ResultAssert.Successful(result);
            }
        }

        /// <summary>
        /// Tests the <see cref="DiscordRestGatewayAPI.GetGatewayBotAsync"/> method.
        /// </summary>
        public class GetGatewayBotAsync : RestAPITestBase
        {
            /// <summary>
            /// Tests whether the API method performs its request correctly.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
            [Fact]
            public async Task PerformsRequestCorrectly()
            {
                var response = "{ \"url\": \"wss://gateway.discord.gg/\", \"shards\": 9, \"session_start_limit\": { \"total\": 1000, \"remaining\": 999, \"reset_after\": 14400000, \"max_concurrency\": 1 }}";
                var services = CreateConfiguredAPIServices
                (
                    b => b
                        .Expect(HttpMethod.Get, $"{Constants.BaseURL}gateway/bot")
                        .WithNoContent()
                        .WithAuthentication()
                        .Respond("application/json", response)
                );

                var api = services.GetRequiredService<IDiscordRestGatewayAPI>();

                var result = await api.GetGatewayBotAsync();
                ResultAssert.Successful(result);
            }
        }
    }
}