﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BadEcho.Odin.XmlConfiguration
{
    /// <summary>
    /// Specifies the type of entity that is missing from a configuration.
    /// </summary>
    public enum MissingConfigurationType
    {
        /// <summary>
        /// The configuration as a whole (i.e. there is no configuration file) is missing.
        /// </summary>
        Whole,
        /// <summary>
        /// A configuration section is missing.
        /// </summary>
        Section,
        /// <summary>
        /// A configuration element is missing.
        /// </summary>
        Element,
        /// <summary>
        /// A configuration element is missing an attribute or some other constituent part.
        /// </summary>
        Attribute
    }
}
