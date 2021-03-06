﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows;
using BadEcho.Fenestra.Behaviors;
using BadEcho.Fenestra.Commands;
using Xunit;

namespace BadEcho.Fenestra.Tests.Behaviors
{
    public class BehaviorActionTests
    {
        [Fact]
        public void ExecuteActions_Empty_ReturnsTrue()
        {
            var actions = new BehaviorActionCollection<DependencyObject>();

            Assert.True(actions.ExecuteActions());
        }

        [Fact]
        public void ExecuteActions_TrueCondition_ReturnsTrue()
        {
            var command = new DelegateCommand(_ => { });

            var actions = new BehaviorActionCollection<DependencyObject>
                          {
                              new ConditionAction {IsEnabled = true},
                              new CommandAction {Command = command}
                          };

            Assert.True(actions.ExecuteActions());
        }

        [Fact]
        public void ExecuteActions_TrueCondition_AllExecuted()
        {
            bool commandExecuted = false;

            var command = new DelegateCommand(_ => commandExecuted = true);

            var actions = new BehaviorActionCollection<DependencyObject>
                          {
                              new ConditionAction {IsEnabled = true},
                              new CommandAction {Command = command}
                          };

            actions.ExecuteActions();

            Assert.True(commandExecuted);
        }

        [Fact]
        public void ExecuteActions_FalseCondition_ReturnsFalse()
        {
            var command = new DelegateCommand(_ => { });

            var actions = new BehaviorActionCollection<DependencyObject>
                          {
                              new ConditionAction {IsEnabled = false},
                              new CommandAction {Command = command}
                          };

            Assert.False(actions.ExecuteActions());
        }

        [Fact]
        public void ExecuteActions_FalseCondition_NotAllExecuted()
        {
            bool commandExecuted = false;

            var command = new DelegateCommand(_ => commandExecuted = true);

            var actions = new BehaviorActionCollection<DependencyObject>
                          {
                              new ConditionAction {IsEnabled = false},
                              new CommandAction {Command = command}
                          };

            actions.ExecuteActions();

            Assert.False(commandExecuted);
        }
    }
}
