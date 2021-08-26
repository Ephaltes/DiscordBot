using DiscordBot.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DiscordBot.Configuration.Test
{
    public class ConsoleConfigurationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConsoleConfiguration_Should_LoadDependencies()
        {
            ConsoleConfiguration consoleConfiguration = new ConsoleConfiguration();
            ServiceProvider services = consoleConfiguration.Setup();

            IUploadOnlyRepository uploadOnlyRepository = services.GetRequiredService<IUploadOnlyRepository>();
            Core.DiscordBot discordBot = services.GetRequiredService<Core.DiscordBot>();
            IConfigurationRoot configurationRoot = services.GetRequiredService<IConfigurationRoot>();

            uploadOnlyRepository.Should().NotBeNull();
            discordBot.Should().NotBeNull();

            configurationRoot.GetSection("ApiToken").Value.Should().Be("testToken");
            configurationRoot.GetSection("SqlitePath").Value.Should().Be("TestDB");
        }
    }
}