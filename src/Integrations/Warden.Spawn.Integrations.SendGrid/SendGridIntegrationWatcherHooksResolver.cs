﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationWatcherHooksResolver : IWatcherHooksResolver
    {
        private readonly SendGridIntegration _integration;
        private readonly SendGridSpawnIntegrationConfiguration _integrationConfiguration;

        public SendGridIntegrationWatcherHooksResolver(SendGridIntegration integration, 
            SendGridSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integration = integration;
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IWatcherCheck>> OnStart()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWatcherCheck, Task>> OnStartAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnFirstSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnFailure()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnFailureAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnCompleted(object configuration)
        {
            var config = configuration as SendGridSpawnIntegrationWatcherHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var subject = string.IsNullOrWhiteSpace(config.Subject)
                ? _integrationConfiguration.DefaultSubject
                : config.Subject;
            var message = string.IsNullOrWhiteSpace(config.Message)
                ? _integrationConfiguration.DefaultMessage
                : config.Message;
            var receivers = config.Receivers == null || !config.Receivers.Any()
                ? _integrationConfiguration.DefaultReceivers
                : config.Receivers;

            return x => Task.Factory.StartNew(() => _integration.SendEmailAsync(subject, message, receivers.ToArray()));
        }

        public Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration)
        {
            var config = configuration as SendGridSpawnIntegrationWatcherHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var subject = string.IsNullOrWhiteSpace(config.Subject)
                ? _integrationConfiguration.DefaultSubject
                : config.Subject;
            var message = string.IsNullOrWhiteSpace(config.Message)
                ? _integrationConfiguration.DefaultMessage
                : config.Message;
            var receivers = config.Receivers == null || !config.Receivers.Any()
                ? _integrationConfiguration.DefaultReceivers
                : config.Receivers;

            return x => _integration.SendEmailAsync(subject, message, receivers.ToArray());
        }

        public Expression<Action<Exception>> OnError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Exception, Task>> OnErrorAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<Exception>> OnFirstError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Exception, Task>> OnFirstErrorAsync()
        {
            throw new NotImplementedException();
        }
    }
}