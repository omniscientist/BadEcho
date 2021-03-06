﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;

namespace BadEcho.Odin.Collections
{
    /// <summary>
    /// Provides data for the <see cref="INotifyCollectionChanged.CollectionChanged"/> event which uses empty instead of
    /// null collections in the data exposed.
    /// </summary>
    public sealed class EmptiableNotifyCollectionChangedEventArgs : EventArgs
    {
        private readonly NotifyCollectionChangedEventArgs _eventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptiableNotifyCollectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="eventArgs">The <see cref="NotifyCollectionChangedEventArgs"/> instance to wrap.</param>
        public EmptiableNotifyCollectionChangedEventArgs(NotifyCollectionChangedEventArgs eventArgs)
        {
            Require.NotNull(eventArgs, nameof(eventArgs));

            _eventArgs = eventArgs;
        }

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.Action"/>
        public NotifyCollectionChangedAction Action
            => _eventArgs.Action;

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.NewItems"/>
        public IList NewItems
            => _eventArgs.NewItems ?? Array.Empty<object>();

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.NewStartingIndex"/>
        public int NewStartingIndex
            => _eventArgs.NewStartingIndex;

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.OldItems"/>
        public IList OldItems
            => _eventArgs.OldItems ?? Array.Empty<object>();

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.OldStartingIndex"/>
        public int OldStartingIndex
            => _eventArgs.OldStartingIndex;
    }
}
