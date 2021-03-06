﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Threading;
using BadEcho.Fenestra.Extensions;
using Xunit;

namespace BadEcho.Fenestra.Tests
{
    public class AtomicObservableCollectionTests
    {
        private readonly INotifyCollectionChanged _collectionChanged;
        private readonly AtomicObservableCollection<int> _collection;

        public AtomicObservableCollectionTests()
        {
            _collection = new AtomicObservableCollection<int>();
            _collectionChanged = _collection;
        }

        [Fact]
        public void AddRange_Empty_SingleChange()
        {
            int numberOfChanges = 0;

            _collectionChanged.CollectionChanged += (_, _) => numberOfChanges++;

            _collection.AddRange(new List<int> {1, 2, 3, 4, 5, 3, 1,2,4,5,1,23,4,3,2,1,2,3});

            Assert.Equal(1, numberOfChanges);
        }

        [Fact]
        public void RemoveRange_NotEmpty_SingleChange()
        {
            _collection.AddRange(new List<int> { 2, 5, 1, 3, 9, 4 });

            int numberOfChanges = 0;

            _collectionChanged.CollectionChanged += (_, _) => numberOfChanges++;

            _collection.RemoveRange(new List<int> {5, 3, 9, 2});

            Assert.Equal(1, numberOfChanges);
        }

        [Fact]
        public void OrderBy_Unordered_SingleChange()
        {
            _collection.AddRange(new List<int> {2, 5, 1, 3, 9, 4});
            
            int numberOfChanges = 0;

            _collectionChanged.CollectionChanged += (_, _) => numberOfChanges++;
            _collection.OrderBy(i => i);

            Assert.Equal(1, numberOfChanges);
        }

        [Fact]
        public void OrderByDescending_Unordered_SingleChange()
        {
            _collection.AddRange(new List<int> { 2, 5, 1, 3, 9, 4 });

            int numberOfChanges = 0;

            _collectionChanged.CollectionChanged += (_, _) => numberOfChanges++;
            _collection.OrderByDescending(i => i);

            Assert.Equal(1, numberOfChanges);
        }

        [Fact]
        public void Add_ForeignThread_NoException()
        {
            _collection.ChangeDispatcher(Dispatcher.CurrentDispatcher);

            bool done = false;

            Task.Factory.StartNew(() => _collection.Add(2))
                .ContinueWith(_ => done = true);

            while (!done)
                Dispatcher.CurrentDispatcher.ProcessMessages();
        }

        [Fact]
        public void Add_ForeignThreadWithDispatcher_NoException()
        {
            _collection.ChangeDispatcher(Dispatcher.CurrentDispatcher);

            bool done = false;

            Task.Factory.StartNew(() =>
                                  {
                                      var unused = Dispatcher.CurrentDispatcher;
                                      _collection.Add(2);
                                  })
                .ContinueWith(_ => done = true);

            while (!done)
                Dispatcher.CurrentDispatcher.ProcessMessages();
        }
    }
}
