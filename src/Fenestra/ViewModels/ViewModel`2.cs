﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace BadEcho.Fenestra.ViewModels
{
    /// <summary>
    /// Provides a base view abstraction that automates communication between a view and bound
    /// <typeparamref name="TModelImpl"/>-typed data and preserves assignment compatibility with other providers of
    /// <typeparamref name="TModel"/>-typed data.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TModelImpl"></typeparam>
    public abstract class ViewModel<TModel,TModelImpl> : ViewModel<TModelImpl>, IModelProvider<TModel>
        where TModelImpl : TModel
    {
        TModel? IModelProvider<TModel>.ActiveModel
            => ActiveModel;
    }
}
