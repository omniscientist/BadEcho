﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using BadEcho.Fenestra.Extensions;
using Xunit;

namespace BadEcho.Fenestra.Tests
{
    public class SteppedBinderTests
    {
        // A time drift of 125 ms is deemed acceptable.
        private const double DRIFT_TOLERANCE = 125;

        private readonly Binding _innerBinding;
        private readonly SourceObject _sourceObject;

        public SteppedBinderTests()
        {
            _sourceObject = new SourceObject();
            _innerBinding = new Binding(nameof(SourceObject.Value))
                                {
                                    Source = _sourceObject,
                                    Mode = BindingMode.TwoWay
                                };
        }

        [Theory]
        [InlineData(111, 11, 3000)]
        [InlineData(11, 111, 3000)]
        [InlineData(50, 60, 3000)]
        [InlineData(60, 50, 3000)]
        [InlineData(111, 11, 2500)]
        [InlineData(50, 65, 2500)]
        [InlineData(111, 11, 8000)]
        [InlineData(50, 60, 8000)]
        [InlineData(111, 11, 1000)]
        [InlineData(50, 60, 1000)]
        public void OnSourceChanged_UpdatesInTime(int initialTargetValue, int newSourceValue, double durationMilliseconds)
        {
            TimeSpan steppingDuration = TimeSpan.FromMilliseconds(durationMilliseconds);

            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateSource(initialTargetValue.ToString(), newSourceValue, steppingDuration), true);

            Assert.True(
                Math.Abs(elapsed.Subtract(steppingDuration).TotalMilliseconds) < DRIFT_TOLERANCE,
                $"Expected Duration: {steppingDuration} Actual Duration: {elapsed}");
        }

        [Fact]
        public void OnSourceChanged_UnsetTarget_TargetUpdatesToSource()
        {
            TimeSpan steppingDuration = TimeSpan.FromSeconds(1);
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateSource(string.Empty, 10000, steppingDuration), true);

            Assert.True(elapsed < steppingDuration,
                        $"Actual duration of {elapsed} greater than expected duration of {steppingDuration} when no stepping should have occurred.");
        }

        [Fact]
        public void OnSourceChanged_IdenticalSourceTarget_NoUpdate()
        {
            TimeSpan steppingDuration = TimeSpan.FromSeconds(1);
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateSource("10000", 10000, steppingDuration), true);

            Assert.True(elapsed < steppingDuration,
                        $"Actual duration of {elapsed} greater than expected duration of {steppingDuration} when no stepping should have occurred.");
        }

        [Fact]
        public void OnSourceChanged_ZeroDuration_TargetUpdatesToSource()
        {
            TimeSpan steppingDuration = TimeSpan.Zero;
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateSource("10", 15, steppingDuration), true);

            Assert.True(
                Math.Abs(elapsed.Subtract(steppingDuration).TotalMilliseconds) < DRIFT_TOLERANCE,
                $"Expected Duration: {steppingDuration} Actual Duration: {elapsed}");
        }

        [Theory]
        [InlineData(111, 11, 3000)]
        [InlineData(11, 111, 3000)]
        [InlineData(50, 60, 3000)]
        [InlineData(60, 50, 3000)]
        [InlineData(111, 11, 2500)]
        [InlineData(50, 65, 2500)]
        [InlineData(111, 11, 8000)]
        [InlineData(50, 60, 8000)]
        [InlineData(111, 11, 1000)]
        [InlineData(50, 60, 1000)]
        public void OnTargetChanged_UpdatesInTime(int initialSourceValue, int newTargetValue, double durationMilliseconds)
        {
            TimeSpan steppingDuration = TimeSpan.FromMilliseconds(durationMilliseconds);
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed =
                                            UpdateTarget(initialSourceValue,
                                                         initialSourceValue.ToString(),
                                                         newTargetValue.ToString(),
                                                         steppingDuration),
                                        true);
            Assert.True(
                Math.Abs(elapsed.Subtract(steppingDuration).TotalMilliseconds) < DRIFT_TOLERANCE,
                $"Expected Duration: {steppingDuration} Actual Duration: {elapsed}");
        }

        [Fact]
        public void OnTargetChanged_UnsetTarget_SourceUpdatesToTarget()
        {
            TimeSpan steppingDuration = TimeSpan.FromSeconds(1);
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateTarget(10, string.Empty, "1000", steppingDuration), true);

            Assert.True(
                Math.Abs(elapsed.Subtract(steppingDuration).TotalMilliseconds) < DRIFT_TOLERANCE,
                $"Expected Duration: {steppingDuration} Actual Duration: {elapsed}");
        }

        [Fact]
        public void OnTargetChanged_IdenticalSourceTarget_NoUpdate()
        {
            TimeSpan steppingDuration = TimeSpan.FromSeconds(1);
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateTarget(10, string.Empty, "10", steppingDuration), true);

            Assert.True(elapsed < steppingDuration,
                        $"Actual duration of {elapsed} greater than expected duration of {steppingDuration} when no stepping should have occurred.");
        }

        [Fact]
        public void OnTargetChanged_ZeroDuration_TargetUpdatesToSource()
        {
            TimeSpan steppingDuration = TimeSpan.Zero;
            TimeSpan elapsed = default;

            UserInterface.RunUIFunction(() => elapsed = UpdateTarget(10, "10", "15", steppingDuration), true);

            Assert.True(
                Math.Abs(elapsed.Subtract(steppingDuration).TotalMilliseconds) < DRIFT_TOLERANCE,
                $"Expected Duration: {steppingDuration} Actual Duration: {elapsed}");
        }

        [Fact]
        public void Initialize_NegativeDuration_ThrowsException()
        {
            UserInterface.RunUIFunction(
                () =>
                {
                    Assert.Throws<ArgumentException>(() => InitializeBinder(TimeSpan.FromSeconds(-2.0)));
                    Dispatcher.CurrentDispatcher.InvokeShutdown();
                },
                true);
        }

        private SteppedBinder InitializeBinder(TimeSpan steppingDuration)
        {
            var textBox = new TextBox {Text = null};
            return new SteppedBinder(textBox, TextBox.TextProperty, _innerBinding, steppingDuration);
        }

        private TimeSpan UpdateSource(string initialTargetValue, int newSourceValue, TimeSpan steppingDuration)
        {
            bool updatedToFinalValue = false;

            var textBox = new TextBox { Text = initialTargetValue };
            var binder = new SteppedBinder(textBox, TextBox.TextProperty, _innerBinding, steppingDuration);
            var stopwatch = new Stopwatch();

            binder.Changed += (_, _) =>
                              {
                                  if (Convert.ToInt32(textBox.Text) != newSourceValue)
                                      return;

                                  stopwatch.Stop();
                                  updatedToFinalValue = true;
                              };

            stopwatch.Start();

            _sourceObject.Value = newSourceValue;

            while (!updatedToFinalValue) 
                textBox.ProcessMessages();

            Dispatcher.CurrentDispatcher.InvokeShutdown();

            return stopwatch.Elapsed;
        }

        private TimeSpan UpdateTarget(int initialSourceValue, string initialTargetValue, string newTargetValue, TimeSpan steppingDuration)
        {
            bool updatedToFinalValue = false;

            _sourceObject.Value = initialSourceValue;
            var textBox = new TextBox { Text = initialTargetValue };
            var binder = new SteppedBinder(textBox, TextBox.TextProperty, _innerBinding, steppingDuration);
            var stopwatch = new Stopwatch();
            
            binder.Changed += (_, _) =>
                              {
                                  if (_sourceObject.Value.ToString() != newTargetValue)
                                      return;

                                  stopwatch.Stop();
                                  updatedToFinalValue = true;
                              };

            stopwatch.Start();

            binder.SetValue(TransientBinder.TargetProperty, newTargetValue);

            while (!updatedToFinalValue)
                textBox.ProcessMessages();

            Dispatcher.CurrentDispatcher.InvokeShutdown();

            return stopwatch.Elapsed;
        }
        
        private sealed class SourceObject : INotifyPropertyChanged
        {
            private int _value;

            public event PropertyChangedEventHandler? PropertyChanged;

            public int Value
            {
                get => _value;
                set 
                {
                    if (_value == value)
                        return;

                    _value = value;

                    OnPropertyChanged();
                }
            }

            private void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}