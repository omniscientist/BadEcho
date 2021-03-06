﻿//-----------------------------------------------------------------------
// <copyright>
//      Created by Matt Weber <matt@badecho.com>
//      Copyright @ 2021 Bad Echo LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using BadEcho.Odin.Extensibility;
using BadEcho.Odin.Tests.Extensibility;

namespace BadEcho.Odin.Tests.Plugin
{
    [Routable(FakeAdapterIds.AlphaFakeIdValue, typeof(ISegmentedContract))]
    public class AlphaFakeAdapter : IPluginAdapter<ISegmentedContract>
    {
        private readonly SegmentedStub _stub = new();

        public ISegmentedContract Contract
            => _stub;

        private class SegmentedStub : ISegmentedContract
        {
            public string SomeMethod()
            {
                return ISegmentedContract.FirstSomeMethod;
            }

            public string SomeOtherMethod()
            {
                return ISegmentedContract.FirstSomeOtherMethod;
            }
        }
    }

    [Routable(FakeAdapterIds.BetaFakeIdValue, typeof(ISegmentedContract))]
    public class BetaFakeAdapter : IPluginAdapter<ISegmentedContract>
    {
        private readonly SegmentedStub _stub = new();

        public ISegmentedContract Contract
            => _stub;

        private class SegmentedStub : ISegmentedContract
        {
            public string SomeMethod()
            {
                return ISegmentedContract.SecondSomeMethod;
            }

            public string SomeOtherMethod()
            {
                return ISegmentedContract.SecondSomeOtherMethod;
            }
        }
    }
}
