﻿using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace PayApp.Test.Helpers
{
    public class InlineAutoMoqDataAttribute : CompositeDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values)
            : base(new InlineDataAttribute(values), new AutoMoqDataAttribute())
        {
        }
    }
}
