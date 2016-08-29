﻿namespace Warden.Spawn.Tools.Core.Messages
{
    public class CreateWardenMessage
    {
        public string ConfigurationId { get; }
        public string Token { get; }
        public string Region { get; }

        public CreateWardenMessage(string configurationId, string token, string region)
        {
            ConfigurationId = configurationId;
            Token = token;
            Region = region;
        }
    }
}
