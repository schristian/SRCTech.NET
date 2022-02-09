using System;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public static class HandleTests_Create
    {
        [Theory]
        [InlineData("disposeAction")]
        public static void Handle_Create_NullAction_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            Action action = () => { };

            NullTestingUtilities.TestNullParameters(
               () => Handle.Create(value, action),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_Create_NoDisposesWithAction_ActionNeverCalled(object value)
        {
            int callCount = 0;
            Action action = () => ++callCount;
            var handle = Handle.Create(value, action);

            Assert.Equal(0, callCount);
            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_Create_MultipleDisposesWithAction_ActionCalledOnce(int disposalCount)
        {
            int callCount = 0;
            Action action = () => ++callCount;
            var value = 5;
            var handle = Handle.Create(value, action);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            Assert.Equal(1, callCount);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData("disposeAction")]
        public static void Handle_Create_NullGenericAction_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            Action<int> action = _ => { };

            NullTestingUtilities.TestNullParameters(
               () => Handle.Create(value, action),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_Create_NoDisposesWithGenericAction_ActionNeverCalled(object value)
        {
            int callCount = 0;
            Action<object> action = _ => ++callCount;

            var handle = Handle.Create(value, action);

            Assert.Equal(0, callCount);
            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_Create_MultipleDisposesWithGenericAction_ActionCalledOnce(int disposalCount)
        {
            int callCount = 0;
            int actualValue = 0;
            Action<int> action = it => { ++callCount; actualValue = it; };
            var value = 5;
            var handle = Handle.Create(value, action);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            Assert.Equal(1, callCount);
            Assert.Equal(value, actualValue);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData("disposable")]
        public static void Handle_Create_NullDisposable_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => Handle.Create(value, disposable),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_Create_NoDisposesWithDisposable_DisposeNeverCalled(object value)
        {
            var disposable = new Mock<IDisposable>();
            var handle = Handle.Create(value, disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_Create_MultipleDisposesWithDisposable_DisposeCalledOnce(int disposalCount)
        {
            var disposable = new Mock<IDisposable>();
            var value = 5;
            var handle = Handle.Create(value, disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData("value")]
        public static void Handle_Create_NullObject_ThrowsArgumentNullException(string parameterName)
        {
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => Handle.Create(disposable),
               parameterName);
        }

        [Fact]
        public static void Handle_Create_NoDisposesWithObject_DisposeNeverCalled()
        {
            var disposable = new Mock<IDisposable>();
            var handle = Handle.Create(disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
            Assert.Same(disposable.Object, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_Create_MultipleDisposesWithObject_DisposeCalledOnce(int disposalCount)
        {
            var disposable = new Mock<IDisposable>();
            var handle = Handle.Create(disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }
    }
}
