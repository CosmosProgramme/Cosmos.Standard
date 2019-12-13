﻿using System;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core.Enrichers;
using Cosmos.Logging.MessageTemplates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Logging.Core {
    public interface ILogServiceCollection {
        bool BeGivenConfigurationBuilder { get; }
        bool BeGivenConfigurationRoot { get; }
        IServiceCollection ExposeServices();
        ILoggingOptions ExposeLogSettings();
        LoggingConfiguration ExposeLoggingConfiguration();
        ILogServiceCollection AddDependency(Action<IServiceCollection> dependencyAction);

        ILogServiceCollection AddEnricher(Func<ILogEventEnricher> enricherProvider);

        ILogServiceCollection AddSinkSettings<TSinkSettings, TSinkConfiguration>(TSinkSettings settings, Action<IConfiguration, TSinkConfiguration> configAct)
            where TSinkSettings : class, ILoggingSinkOptions, new()
            where TSinkConfiguration : SinkConfiguration, new();

        ILogServiceCollection AddExtraSinkSettings<TExtraSinkSettings, TExtraSinkConfiguration>(
            TExtraSinkSettings settings,
            Action<IConfiguration, TExtraSinkConfiguration, LoggingConfiguration> configAct)
            where TExtraSinkSettings : class, ILoggingSinkOptions, new()
            where TExtraSinkConfiguration : SinkConfiguration, new();

        ILogServiceCollection PreheatMessageTemplates(Action<MessageTemplateCachePreheater> preheatAct);
        ILogServiceCollection AddOriginalConfigAction(Action<IConfiguration> configAction);
        ILogServiceCollection ModifyConfigurationBuilder(Action<LoggingConfigurationBuilder> builderAct);
    }
}