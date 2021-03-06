﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace BadEcho.Fenestra.Behaviors
{
    /// <summary>
    /// Provides an action that, when executed, will execute a bound <see cref="ICommand"/> instance if possible.
    /// </summary>
    public sealed class CommandAction : BehaviorAction<DependencyObject>
    {
        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.Register(nameof(Command),
                                          typeof(ICommand),
                                          typeof(CommandAction));

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> instance that will be executed when this action is executed.
        /// </summary>
        public ICommand? Command
        {
            get => (ICommand?) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <inheritdoc/>
        public override bool Execute()
        {
            if (Command == null)
                return false;

            if (!Command.CanExecute(null))
                return false;

            Command.Execute(null);

            return true;
        }

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore() 
            => new CommandAction();
    }
}
