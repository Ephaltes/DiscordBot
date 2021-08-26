using System.Threading.Tasks;
using DiscordBot.Core.Commands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Serilog;

namespace DiscordBot.Core.Test
{
    public class ClearCommandTest
    {

        [OneTimeSetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Clear_Should_ReturnErrorMessage_WrongParameterAmount()
        {
        }
    }
}