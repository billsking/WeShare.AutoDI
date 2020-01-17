using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var services = new ServiceCollection();
            services.AutoDI();
        }
    }
}
