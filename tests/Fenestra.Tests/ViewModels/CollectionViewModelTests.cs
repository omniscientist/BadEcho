﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using BadEcho.Fenestra.ViewModels;
using BadEcho.Odin.Extensions;
using Xunit;

namespace BadEcho.Fenestra.Tests.ViewModels
{
    public class CollectionViewModelTests
    {
        private readonly FakeCollectionViewModel _collectionViewModel;

        public CollectionViewModelTests() 
            => _collectionViewModel = new FakeCollectionViewModel();

        [Fact]
        public void Bind_Model_AddedToChildrenOnce()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            var child = _collectionViewModel.Children.SingleOrDefault(vm => Equals(vm.ActiveModel, model));

            Assert.NotNull(child);
            Assert.Equal(2, _collectionViewModel.Children[0].Value);
        }

        [Fact]
        public void Bind_ViewModel_AddedToChildrenOnce()
        {
            var model = new ModelStub("Test", 2);
            var viewModel = new ViewModelStub();

            viewModel.Bind(model);

            _collectionViewModel.Bind(viewModel);

            var child = _collectionViewModel.Children.Single(vm => Equals(vm.ActiveModel, model));

            Assert.NotNull(child);
        }

        [Fact]
        public void Bind_ViewModelTwice_AddedToChildrenOnce()
        {
            var model = new ModelStub("Test", 2);
            var viewModel = new ViewModelStub();

            viewModel.Bind(model);

            _collectionViewModel.Bind(viewModel);
            _collectionViewModel.Bind(viewModel);

            var child = _collectionViewModel.Children.Single(vm => Equals(vm.ActiveModel, model));

            Assert.NotNull(child);
        }

        [Fact]
        public void Bind_Model_ModelIsBound()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            Assert.True(_collectionViewModel.IsBound(model));
        }

        [Fact]
        public void Bind_ViewModel_ModelIsBound()
        {
            var model = new ModelStub("Test", 2);
            var viewModel = new ViewModelStub();

            viewModel.Bind(model);

            _collectionViewModel.Bind(viewModel);

            Assert.True(_collectionViewModel.IsBound(model));
        }

        [Fact]
        public void Bind_ExistingModel_UpdatesOnly()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            Assert.Single(_collectionViewModel.Children);
            Assert.Equal(2, _collectionViewModel.Children[0].Value);

            model.Value = 3;

            _collectionViewModel.Bind(model);

            Assert.Single(_collectionViewModel.Children);
            Assert.Equal(3, _collectionViewModel.Children[0].Value);
        }

        [Fact]
        public void BindMany_ExistingModel_UpdatesOnly()
        {
            var models = new List<ModelStub> {new("Test", 2), new("Another", 4)};

            _collectionViewModel.Bind(models);

            Assert.Equal(2, _collectionViewModel.Children.Count);
            Assert.Equal(2, _collectionViewModel.Children[0].Value);

            models[0].Value = 3;

            _collectionViewModel.Bind(models);

            Assert.Equal(2, _collectionViewModel.Children.Count);
            Assert.Equal(3, _collectionViewModel.Children[0].Value);
        }

        [Fact]
        public void AddChildren_ViewModel_ModelIsBound()
        {
            var model = new ModelStub("Test", 2);
            var viewModel = new ViewModelStub();

            viewModel.Bind(model);

            _collectionViewModel.Children.Add(viewModel);
            
            Assert.True(_collectionViewModel.IsBound(model));
        }

        [Fact]
        public void RemoveChildren_ViewModel_ModelUnbound()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            _collectionViewModel.Children.Remove(_collectionViewModel.Children.First());

            Assert.False(_collectionViewModel.IsBound(model));
        }

        [Fact]
        public void Unbind_ViewModel_ModelUnbound()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            var child = _collectionViewModel.Children.First();

            _collectionViewModel.Unbind(child);

            Assert.False(_collectionViewModel.IsBound(model));
        }

        [Fact]
        public void Unbind_ViewModel_RemovedFromChildren()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            var child = _collectionViewModel.Children.First();

            _collectionViewModel.Unbind(child);

            child = _collectionViewModel.Children.FirstOrDefault(vm => Equals(vm.ActiveModel, model));

            Assert.Null(child);
        }

        [Fact]
        public void Unbind_Model_RemovedFromChildren()
        {
            var model = new ModelStub("Test", 2);

            _collectionViewModel.Bind(model);

            _collectionViewModel.Unbind(model);

            var child = _collectionViewModel.Children.FirstOrDefault(vm => Equals(vm.ActiveModel, model));

            Assert.Null(child);
        }

        
        private sealed class FakeCollectionViewModel : CollectionViewModel<ModelStub, ViewModelStub>
        {
            public FakeCollectionViewModel() : base(new CollectionViewModelOptions{AsyncBatchBindings = false})
            { }

            public override ViewModelStub CreateChild(ModelStub model)
            {
                var stub = new ViewModelStub();

                stub.Bind(model);

                return stub;
            }

            public override void UpdateChild(ModelStub model)
            {
                var existingChild = FindChild<ViewModelStub>(model);

                existingChild?.Bind(model);
            }

            public override void OnChangeCompleted()
            { }
        }

        private sealed class ViewModelStub : ViewModel<ModelStub>
        {
            public int Value
            { get; private set; }

            protected override void OnBinding(ModelStub model) 
                => Value = model.Value;

            protected override void OnUnbound(ModelStub model) 
                => Value = 0;
        }

        private sealed class ModelStub
        {
            private readonly string _name;

            public ModelStub(string name, int value)
            {
                _name = name;
                Value = value;
            }

            public int Value
            { get; set; }

            public override bool Equals(object? obj)
            {
                if (obj is not ModelStub otherModel)
                    return false;

                return _name == otherModel._name;
            }

            public override int GetHashCode() 
                => this.GetHashCode(_name);
        }
    }
}
