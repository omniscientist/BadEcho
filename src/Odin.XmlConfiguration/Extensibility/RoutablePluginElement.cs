﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using BadEcho.Odin.Extensibility.Configuration;

namespace BadEcho.Odin.XmlConfiguration.Extensibility
{
    /// <summary>
    /// Provides a configuration element for a call-routable plugin.
    /// </summary>
    internal sealed class RoutablePluginElement : GuidConfigurationElement, IRoutablePluginConfiguration
    {
        private const string METHOD_CLAIMS_CHILD_SCHEMA = "methodClaims";
        private const string PRIMARY_ATTRIBUTE_SCHEMA = "primary";

        private static readonly Lazy<ConfigurationPropertyCollection> _Properties
            = new(InitializeProperties, LazyThreadSafetyMode.PublicationOnly);

        IEnumerable<string> IRoutablePluginConfiguration.MethodClaims
            => MethodClaims.Select(m => m.Name);

        /// <summary>
        /// Gets the collection of methods claimed by the plugin represented by this element.
        /// </summary>
        public NamedElementCollection<MethodClaimElement> MethodClaims
            => (NamedElementCollection<MethodClaimElement>) base[METHOD_CLAIMS_CHILD_SCHEMA];

        /// <summary>
        /// Gets a value indicating whether the plugin represented by this element is the primary call-routable plugin
        /// for a particular contract.
        /// </summary>
        public bool Primary
            => (bool) base[PRIMARY_ATTRIBUTE_SCHEMA];

        /// <summary>
        /// Gets the schema name for the attribute indicating whether the plugin represented by this element is the primary
        /// call-routable plugin for a particular contract.
        /// </summary>
        internal static string PrimaryAttributeSchema
            => "primary";

        /// <inheritdoc/>
        protected override ConfigurationPropertyCollection Properties
            => _Properties.Value;

        private static ConfigurationPropertyCollection InitializeProperties()
            => new()
               {
                   new ConfigurationProperty(METHOD_CLAIMS_CHILD_SCHEMA,
                                             typeof(NamedElementCollection<MethodClaimElement>),
                                             null,
                                             null,
                                             null,
                                             ConfigurationPropertyOptions.None),
                   new ConfigurationProperty(PRIMARY_ATTRIBUTE_SCHEMA,
                                             typeof(bool),
                                             false,
                                             null,
                                             null,
                                             ConfigurationPropertyOptions.None),
                   CreateIdProperty(),
                   CreateNameProperty()
               };
    }
}