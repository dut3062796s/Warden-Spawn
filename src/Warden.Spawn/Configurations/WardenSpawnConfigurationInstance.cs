using System;
using System.Collections.Generic;
using System.Linq;
using Warden.Core;
using Warden.Integrations;
using Warden.Spawn.Configurations.Logger;
using Warden.Watchers;

namespace Warden.Spawn.Configurations
{
    public class WardenSpawnConfigurationInstance : IWardenSpawnConfigurationInstance
    {
        public string Name => "Warden";
        public string WardenName { get; protected set; }
        public long? IterationsCount { get; protected set; }
        public bool OverrideCustomIntervals { get; protected set; }
        public TimeSpan Interval { get; protected set; }
        public WardenLoggerConfiguration Logger { get; protected set; }
        public IEnumerable<IWatcherWithHooks> Watchers { get; protected set; }
        public IEnumerable<IIntegration> Integrations { get; protected set; }
        public Action<WardenHooksConfiguration.Builder> Hooks { get; }
        public Action<WatcherHooksConfiguration.Builder> GlobalWatcherHooks { get; }
        public Action<AggregatedWatcherHooksConfiguration.Builder> AggregatedWatcherHooks { get; set; }

        protected WardenSpawnConfigurationInstance()
        {
        }

        public WardenSpawnConfigurationInstance(
            string wardenName,
            IEnumerable<IWatcherWithHooks> watchers,
            IEnumerable<IIntegration> integrations,
            Action<WardenHooksConfiguration.Builder> hooks,
            Action<WatcherHooksConfiguration.Builder> globalWatcherHooks,
            Action<AggregatedWatcherHooksConfiguration.Builder> aggregatedWatcherHooks,
            long? iterationsCount = null,
            TimeSpan? interval = null,
            bool overrideCustomIntervals = false,
            WardenLoggerConfiguration logger = null)
        {
            WardenName = wardenName;
            Watchers = watchers ?? Enumerable.Empty<IWatcherWithHooks>();
            Integrations = integrations ?? Enumerable.Empty<IIntegration>();
            Hooks = hooks;
            GlobalWatcherHooks = globalWatcherHooks;
            AggregatedWatcherHooks = aggregatedWatcherHooks;
            IterationsCount = iterationsCount;
            Interval = interval.GetValueOrDefault();
            OverrideCustomIntervals = overrideCustomIntervals;
            Logger = logger;
        }
    }
}