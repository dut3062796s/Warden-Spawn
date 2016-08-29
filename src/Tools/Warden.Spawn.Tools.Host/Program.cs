﻿using System.IO;
using System.Threading.Tasks;
using Warden.Spawn.Configurations;
using Warden.Spawn.Extensions.JsonConfigurationReader;
using Warden.Spawn.Extensions.Security;
using Warden.Spawn.Extensions.SqlTde;
using Warden.Spawn.Integrations.Console;
using Warden.Spawn.Integrations.SendGrid;
using Warden.Spawn.Tools.Core.Services;
using Warden.Spawn.Watchers.Web;

namespace Warden.Spawn.Tools.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var encrypter = new Encrypter("abcdefgh12345678");
            var credentialName = "Password";
            var credentialValue = "test";
            var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=WardenSpawn;Integrated Security=True";
            var credentialsManager = new SqlTdeCredentialsManager(connectionString, encrypter);
            var wardenName = "Warden Spawn";
            var integrationName = "console";
            credentialsManager.Save(wardenName, credentialName, credentialValue, integration: integrationName);
            var credentialsConfigurator = new CredentialsConfigurator(credentialsManager);

            var configurationReader = WardenSpawnJsonConfigurationReader
                .Create()
                .WithWatcher<WebWatcherSpawn>()
                .WithIntegration<ConsoleSpawnIntegration>()
                .WithIntegration<SendGridSpawnIntegration>()
                .WithCredentialsConfigurator(() => credentialsConfigurator)
                .Build();

            var configurator = WardenSpawnConfigurator
                .Create()
                .Build();

            var configurationTask = GetConfigurationAsync(string.Empty, string.Empty);
            Task.WaitAll(configurationTask);

            var factory = WardenSpawnFactory
                .Create()
                .WithConfigurationReader(() => configurationReader)
                .WithConfiguration(configurationTask.Result)
                .WithConfigurator(() => configurator)
                .Build();

            var spawn = factory.Resolve();
            var warden = spawn.Spawn();
            System.Console.WriteLine($"Warden: '{warden.Name}' has been created and started monitoring.");
            Task.WaitAll(warden.StartAsync());
        }

        private static async Task<string> GetConfigurationAsync(string id, string token)
        {
            var service = new WardenConfigurationManager("http://localhost:20899/api");
            var configuration = await service.GetConfigurationAsync(id, token);

            return configuration;
        }
    }
}
