﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using BadEcho.Odin.Extensions;
using System.Collections.Generic;
using System.Linq;
using BadEcho.Odin;

namespace BadEcho.Fenestra.ViewModels
{
    /// <summary>
    /// Provides a base view abstraction that automates communication between a view and bound
    /// <typeparamref name="T"/>-typed data.
    /// </summary>
    /// <typeparam name="T">The type of data bound to the view model for display on a view.</typeparam>
    public abstract class ViewModel<T> : ViewModel, IViewModel<T>
    {
        private readonly List<T> _boundData = new();

        /// <inheritdoc/>
        public T? ActiveModel
        { get; private set; }

        /// <inheritdoc/>
        public override void Disconnect()
        {
            List<T> boundData = _boundData.ToList();

            foreach (T boundDatum in boundData)
            {
                Unbind(boundDatum);

                if (boundDatum is IViewModel viewModel) 
                    viewModel.Disconnect();
            }
        }

        /// <inheritdoc/>
        public bool IsBound(T model)
            => _boundData.Contains(model);

        /// <inheritdoc/>
        public void Bind(T model)
        {
            Require.NotNull(model, nameof(model));

            if (UnbindOnBind && !model.Equals<T>(ActiveModel))
                Unbind();

            OnBinding(model);

            if (!_boundData.Contains(model)) 
                _boundData.Add(model);

            ActiveModel = model;
        }

        /// <inheritdoc/>
        public void Bind(IEnumerable<T> models)
        {
            Require.NotNull(models, nameof(models));

            if (OnBatchBinding(models))
                return;

            foreach (T model in models)
            {
                Bind(model);
            }
        }

        /// <inheritdoc/>
        public bool Unbind(T? model)
        {
            model ??= ActiveModel;

            if (model == null || !_boundData.Contains(model))
                return false;

            if (model.Equals<T>(ActiveModel))
                ActiveModel = default;

            _boundData.Remove(model);

            OnUnbound(model);

            return true;
        }

        /// <inheritdoc/>
        public void Unbind(IEnumerable<T> models)
        {
            Require.NotNull(models, nameof(models));

            if (OnBatchUnbound(models))
                return;

            foreach (T model in models)
            {
                Unbind(model);
            }
        }

        /// <inheritdoc/>
        public bool Unbind()
            => Unbind(ActiveModel);

        /// <summary>
        /// Gets a value indicating if existing data must be explicitly unbound before the binding of new data.
        /// </summary>
        protected virtual bool UnbindOnBind
            => false;

        /// <summary>
        /// Called when a sequence of new data is being bound to the view model so that any work required for the data
        /// to be fully represented in a view can be performed.
        /// </summary>
        /// <param name="models">The sequence of new data being bound to the view model.</param>
        /// <returns>Value indicating if <c>models</c> has been bound to the view model.</returns>
        /// <remarks>
        /// Derived classes can override this method to perform specialized batch binding work in place of the normal
        /// binding logic. Returning true from this method will prevent the normal binding logic from occurring; returning
        /// false indicates to the caller that its normal binding logic should happen. Further derived classes are expected
        /// to participate in this behavior.
        /// </remarks>
        protected virtual bool OnBatchBinding([NoEnumeration]IEnumerable<T> models)
            => false;

        /// <summary>
        /// Called when a sequence of data is being unbound from the view model so that any work required for the data
        /// to no longer be represented in a view can be performed.
        /// </summary>
        /// <param name="models">The sequence of data unbound from the view model.</param>
        /// <returns>Value indicating if <c>models</c> has been unbound from the view model.</returns>
        /// <remarks>
        /// Like <see cref="OnBatchBinding"/>, derived classes can override this method to perform specialized batch
        /// unbinding work in place of normal binding logic. Returning true from this method will prevent the normal unbinding
        /// logic from occurring; returning false indicates to the caller that its normal unbinding logic should happen. Further
        /// derived classes are expected to participate in this behavior.
        /// </remarks>
        protected virtual bool OnBatchUnbound([NoEnumeration]IEnumerable<T> models)
            => false;

        /// <summary>
        /// Called when new data is being bound to the view model so that any work required for the data to be
        /// fully represented in a view can be performed.
        /// </summary>
        /// <param name="model">The new data being bound to the view model.</param>
        protected abstract void OnBinding(T model);

        /// <summary>
        /// Called when data has been unbound from the view model so that any work required for the data to no longer
        /// be represented in a view can be performed.
        /// </summary>
        /// <param name="model">The data unbound from the view model.</param>
        protected abstract void OnUnbound(T model);
    }
}