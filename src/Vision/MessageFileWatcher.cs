﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using BadEcho.Odin;
using BadEcho.Omnified.Vision.Extensibility;

namespace BadEcho.Omnified.Vision
{
    /// <summary>
    /// Provides a watcher for a Vision module's message file that will notify listeners upon any changes being made to it.
    /// </summary>
    internal sealed class MessageFileWatcher : IMessageFileProvider, IDisposable
    {
        private readonly FileSystemWatcher _watcher;

        /// <inheritdoc/>
        public event EventHandler<EventArgs<string>>? NewMessages;

        /// <inheritdoc/>
        public string? CurrentMessages 
        { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFileWatcher"/> class.
        /// </summary>
        /// <param name="module">The vision module whose message file this watcher will be monitoring.</param>
        public MessageFileWatcher(IVisionModule module)
        {
            Require.NotNull(module, nameof(module));

            if (File.Exists(module.MessageFile))
                CurrentMessages = File.ReadAllText(module.MessageFile);

            _watcher = new FileSystemWatcher
                       {
                           Filter = module.MessageFile,
                           NotifyFilter = NotifyFilters.LastWrite,
                           Path = AppContext.BaseDirectory
                       };

            _watcher.Changed += HandleMessageFileChanged;

            _watcher.EnableRaisingEvents = true;
        }

        /// <inheritdoc/>
        public void Dispose()
            => _watcher.Dispose();

        private void HandleMessageFileChanged(object sender, FileSystemEventArgs e)
        {
            CurrentMessages = File.ReadAllText(e.FullPath);
            // TODO: Add mechanism to return only new messages if ProcessNewMessagesOnly is true.
            NewMessages?.Invoke(this, new EventArgs<string>(CurrentMessages));
        }
    }
}
