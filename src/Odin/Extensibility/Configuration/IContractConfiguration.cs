﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace BadEcho.Odin.Extensibility.Configuration
{
    /// <summary>
    /// Defines configuration settings for a contract being segmented by one or more call-routable plugins.
    /// </summary>
    public interface IContractConfiguration
    {
        /// <summary>
        /// Gets the type name of the contract being segmented.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the collection of call-routable plugins that segment the represented contract.
        /// </summary>
        IEnumerable<IRoutablePluginConfiguration> RoutablePlugins { get; }
    }
}
