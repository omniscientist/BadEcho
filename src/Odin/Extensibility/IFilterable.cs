﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace BadEcho.Odin.Extensibility
{
    /// <summary>
    /// Defines support for Odin's Extensibility framework's filterable plugin capabilities, indicating that types
    /// implementing this can be isolated from others belonging to different plugin families.
    /// </summary>
    public interface IFilterable
    {
        /// <summary>
        /// Gets the identity of the filterable family that this object belongs to.
        /// </summary>
        Guid FamilyId { get; }
    }
}
