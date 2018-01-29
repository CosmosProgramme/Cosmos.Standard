﻿using System;
using System.Collections.Generic;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core;
using Cosmos.Logging.Events;

namespace Cosmos.Logging.Sinks.Exceptionless {
    public class ExceptionlessSinkOptions : ILoggingSinkOptions<ExceptionlessSinkOptions>, ILoggingSinkOptions {
        public string Key => Internals.Constants.SinkKey;

        #region Append log minimum level

        internal readonly Dictionary<string, LogEventLevel> InternalNavigatorLogEventLevels = new Dictionary<string, LogEventLevel>();

        internal LogEventLevel? MinimumLevel { get; set; }

        public ExceptionlessSinkOptions UseMinimumLevelForType(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var typeName = TypeNameHelper.GetTypeDisplayName(type);
            if (InternalNavigatorLogEventLevels.ContainsKey(typeName)) {
                InternalNavigatorLogEventLevels[typeName] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(typeName, level);
            }

            return this;
        }

        public ExceptionlessSinkOptions UseMinimumLevelForNamespace(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var @namespace = type.Namespace;
            return UseMinimumLevelForNamespace(@namespace, level);
        }

        public ExceptionlessSinkOptions UseMinimumLevelForNamespace(string @namespace, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(@namespace)) throw new ArgumentNullException(nameof(@namespace));
            @namespace = $"{@namespace}.*";
            if (InternalNavigatorLogEventLevels.ContainsKey(@namespace)) {
                InternalNavigatorLogEventLevels[@namespace] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(@namespace, level);
            }

            return this;
        }

        public ExceptionlessSinkOptions UseMinimumLevel(LogEventLevel? level) {
            MinimumLevel = level;
            return this;
        }

        #endregion

        #region Append log level alias

        internal readonly Dictionary<string, LogEventLevel> InternalAliases = new Dictionary<string, LogEventLevel>();

        public ExceptionlessSinkOptions UseAlias(string alias, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(alias)) return this;
            if (InternalAliases.ContainsKey(alias)) {
                InternalAliases[alias] = level;
            } else {
                InternalAliases.Add(alias, level);
            }

            return this;
        }

        #endregion

        #region Append configuration file path

        internal string OriginConfigFilePath { get; set; }
        internal FileTypes OriginConfigFileType { get; set; } = FileTypes.Json;

        public ExceptionlessSinkOptions RemoveConfig() {
            OriginConfigFilePath = string.Empty;
            OriginConfigFileType = FileTypes.Json;
            return this;
        }

        public ExceptionlessSinkOptions UseAppSettings(string environmentName = "") {
            UseJsonConfig(string.IsNullOrWhiteSpace(environmentName) ? "appsettings.json" : $"appsettings.{environmentName}.json");
            return this;
        }

        public ExceptionlessSinkOptions UseJsonConfig(string path) {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            OriginConfigFilePath = path;
            OriginConfigFileType = FileTypes.Json;
            return this;
        }

        public ExceptionlessSinkOptions UseXmlConfig(string path) {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            OriginConfigFilePath = path;
            OriginConfigFileType = FileTypes.Xml;
            return this;
        }

        #endregion

        #region Append api key

        internal string ApiKey { get; set; }

        public ExceptionlessSinkOptions UseApiKey(string apiKey) {
            ApiKey = apiKey;
            return this;
        }

        #endregion

    }
}