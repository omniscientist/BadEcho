﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BadEcho.Omnified.Vision.Extensibility
{
    /// <summary>
    /// Specifies the location of a Vision module's anchor point on the screen.
    /// </summary>
    public enum AnchorPointLocation
    {
        /// <summary>
        /// The module will be anchored at the top-left point of the screen.
        /// </summary>
        TopLeft,
        /// <summary>
        /// The module will be anchored at the top-center point of the screen.
        /// </summary>
        TopCenter,
        /// <summary>
        /// The module will be anchored at the top-right point of the screen.
        /// </summary>
        TopRight,
        /// <summary>
        /// The module will be anchored at the bottom-left point of the screen.
        /// </summary>
        BottomLeft,
        /// <summary>
        /// The module will be anchored at the bottom-center point of the screen.
        /// </summary>
        BottomCenter,
        /// <summary>
        /// The module will be anchored at the bottom-right point of the screen.
        /// </summary>
        BottomRight
    }
}
