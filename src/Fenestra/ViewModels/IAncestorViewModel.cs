﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using BadEcho.Odin;

namespace BadEcho.Fenestra.ViewModels
{
    /// <summary>
    /// Defines a view model which has the capability of having one or more children view models.
    /// </summary>
    /// <typeparam name="T">The type of view model that may descend from this view model.</typeparam>
    public interface IAncestorViewModel<T> : IViewModel, IParent<T, AtomicObservableCollection<T>>
        where T : class, IViewModel
    { }
}