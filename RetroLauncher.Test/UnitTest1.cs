using System;
using Xunit;

namespace RetroLauncher.Test
{
    public class UnitTest1
    {
        [Fact]
        public void IsFourTest1()
        {
            var i = 5;

            Assert.Equal(2+2, i);
        }

        [Fact]
        public void IsFourTest2()
        {
            var i = 4;

            Assert.Equal(2+2, i);
        }
    }
}
