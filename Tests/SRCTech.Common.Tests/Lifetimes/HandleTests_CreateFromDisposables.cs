using System;
using System.Collections.Generic;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class HandleTests_CreateFromDisposables
    {
        [Theory]
        [InlineData("disposables")]
        public static void Handle_CreateFromDisposables_NullEnumerable_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            NullTestingUtilities.TestNullParameters(
               () => Handle.CreateFromDisposables(value, disposables),
               parameterName);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_EnumerableWithNullDisposables_ThrowsArgumentNullException()
        {
            var value = 5;
            IEnumerable<IDisposable> disposables = new IDisposable[] { Disposable.Empty, null, null };

            var exception = Assert.Throws<ArgumentException>(
                () => Handle.CreateFromDisposables(value, disposables));

            Assert.Equal("disposables", exception.ParamName);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_EmptyEnumerable_DoesNotThrow()
        {
            var value = 5;
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            var handle = Handle.CreateFromDisposables(value, disposables);

            handle.Dispose();

            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_CreateFromDisposables_NoDisposesWithEnumerable_DisposeNeverCalled(object value)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();

            IEnumerable<IDisposable> disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            disposable1.Verify(it => it.Dispose(), Times.Never);
            disposable2.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_CreateFromDisposables_MultipleDisposesWithEnumerable_DisposeCalledOnce(int disposalCount)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();

            var value = 5;
            IEnumerable<IDisposable> disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_EnumerableWithThrowingDisposables_ExceptionsAggregated()
        {
            var exception1 = new InvalidOperationException();
            var exception2 = new NullReferenceException();

            var goodDisposable1 = new Mock<IDisposable>();
            var goodDisposable2 = new Mock<IDisposable>();
            var throwingDisposable1 = new Mock<IDisposable>();
            throwingDisposable1.Setup(it => it.Dispose()).Throws(exception1);
            var throwingDisposable2 = new Mock<IDisposable>();
            throwingDisposable2.Setup(it => it.Dispose()).Throws(exception2);

            var value = 5;
            IEnumerable<IDisposable> disposables = new IDisposable[] { throwingDisposable1.Object, goodDisposable1.Object, throwingDisposable2.Object, goodDisposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            var actualException = Assert.Throws<AggregateException>(
                () => handle.Dispose());

            Assert.Equal(2, actualException.InnerExceptions.Count);
            Assert.Contains(exception1, actualException.InnerExceptions);
            Assert.Contains(exception2, actualException.InnerExceptions);

            goodDisposable1.Verify(it => it.Dispose(), Times.Once);
            goodDisposable2.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable1.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable2.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData("disposables")]
        public static void Handle_CreateFromDisposables_NullArray_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            IDisposable[] disposables = new IDisposable[] { };

            NullTestingUtilities.TestNullParameters(
               () => Handle.CreateFromDisposables(value, disposables),
               parameterName);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_ArrayWithNullDisposables_ThrowsArgumentException()
        {
            var value = 5;
            IDisposable[] disposables = new IDisposable[] { Disposable.Empty, null, null };

            var exception = Assert.Throws<ArgumentException>(
                () => Handle.CreateFromDisposables(value, disposables));

            Assert.Equal("disposables", exception.ParamName);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_EmptyArray_DoesNotThrow()
        {
            var value = 5;
            IDisposable[] disposables = new IDisposable[] { };

            var handle = Handle.CreateFromDisposables(value, disposables);

            handle.Dispose();
            
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_CreateFromDisposables_NoDisposesWithArray_DisposeNeverCalled(object value)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();

            IDisposable[] disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            disposable1.Verify(it => it.Dispose(), Times.Never);
            disposable2.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_CreateFromDisposables_MultipleDisposesWithArray_DisposeCalledOnce(int disposalCount)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();

            var value = 5;
            IDisposable[] disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }

        [Fact]
        public static void Handle_CreateFromDisposables_ArrayWithThrowingDisposables_ExceptionsAggregated()
        {
            var exception1 = new InvalidOperationException();
            var exception2 = new NullReferenceException();

            var goodDisposable1 = new Mock<IDisposable>();
            var goodDisposable2 = new Mock<IDisposable>();
            var throwingDisposable1 = new Mock<IDisposable>();
            throwingDisposable1.Setup(it => it.Dispose()).Throws(exception1);
            var throwingDisposable2 = new Mock<IDisposable>();
            throwingDisposable2.Setup(it => it.Dispose()).Throws(exception2);

            var value = 5;
            IDisposable[] disposables = new IDisposable[] { throwingDisposable1.Object, goodDisposable1.Object, throwingDisposable2.Object, goodDisposable2.Object };

            var handle = Handle.CreateFromDisposables(value, disposables);

            var actualException = Assert.Throws<AggregateException>(
                () => handle.Dispose());

            Assert.Equal(2, actualException.InnerExceptions.Count);
            Assert.Contains(exception1, actualException.InnerExceptions);
            Assert.Contains(exception2, actualException.InnerExceptions);

            goodDisposable1.Verify(it => it.Dispose(), Times.Once);
            goodDisposable2.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable1.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable2.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }
    }
}
