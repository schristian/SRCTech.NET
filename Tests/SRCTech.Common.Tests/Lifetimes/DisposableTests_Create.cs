using System;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public static class DisposableTests_Create
    {
        [Theory]
        [InlineData("disposeAction")]
        public static void Disposable_Create_NullAction_ThrowsArgumentNullException(string parameterName)
        {
            Action action = () => { };

            NullTestingUtilities.TestNullParameters(
               () => Disposable.Create(action),
               parameterName);
        }

        [Fact]
        public static void Disposable_Create_NoDisposesWithAction_ActionNeverCalled()
        {
            int callCount = 0;
            Action action = () => ++callCount;
            var disposable = Disposable.Create(action);

            Assert.Equal(0, callCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_Create_MultipleDisposesWithAction_ActionCalledOnce(int disposalCount)
        {
            int callCount = 0;
            Action action = () => ++callCount;
            var disposable = Disposable.Create(action);

            for (int i = 0; i < disposalCount; i++)
            {
                disposable.Dispose();
            }

            Assert.Equal(1, callCount);
        }

        [Theory]
        [InlineData("disposeAction")]
        public static void Disposable_Create_NullFunc_ThrowsArgumentNullException(string parameterName)
        {
            Func<int> func = () => 5;

            NullTestingUtilities.TestNullParameters(
               () => Disposable.Create(func),
               parameterName);
        }

        [Fact]
        public static void Disposable_Create_NoDisposesWithFunc_ActionNeverCalled()
        {
            int callCount = 0;
            Func<int> func = () => ++callCount;
            var disposable = Disposable.Create(func);

            Assert.Equal(0, callCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_Create_MultipleDisposesWithFunc_ActionCalledOnce(int disposalCount)
        {
            int callCount = 0;
            Func<int> func = () => ++callCount;
            var disposable = Disposable.Create(func);

            for (int i = 0; i < disposalCount; i++)
            {
                disposable.Dispose();
            }

            Assert.Equal(1, callCount);
        }
    }
}
