using System;
using System.Collections.Generic;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class DisposableTests_CreateFromDisposables
    {
        [Theory]
        [InlineData("disposables")]
        public static void Disposable_CreateFromDisposables_NullEnumerable_ThrowsArgumentNullException(string parameterName)
        {
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            NullTestingUtilities.TestNullParameters(
               () => Disposable.CreateFromDisposables(disposables),
               parameterName);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_EnumerableWithNullDisposables_ThrowsArgumentException()
        {
            IEnumerable<IDisposable> disposables = new IDisposable[] { Disposable.Empty, null, null };

            var exception = Assert.Throws<ArgumentException>(
                () => Disposable.CreateFromDisposables(disposables));

            Assert.Equal("disposables", exception.ParamName);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_EmptyEnumerable_DoesNotThrow()
        {
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            mainDisposable.Dispose();
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_NoDisposesWithEnumerable_DisposeNeverCalled()
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IEnumerable<IDisposable> disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            disposable1.Verify(it => it.Dispose(), Times.Never);
            disposable2.Verify(it => it.Dispose(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_CreateFromDisposables_MultipleDisposesWithEnumerable_DisposeCalledOnce(int disposalCount)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IEnumerable<IDisposable> disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            for (int i = 0; i < disposalCount; i++)
            {
                mainDisposable.Dispose();
            }

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_EnumerableWithThrowingDisposables_ExceptionsAggregated()
        {
            var exception1 = new InvalidOperationException();
            var exception2 = new NullReferenceException();

            var goodDisposable1 = new Mock<IDisposable>();
            var goodDisposable2 = new Mock<IDisposable>();
            var throwingDisposable1 = new Mock<IDisposable>();
            throwingDisposable1.Setup(it => it.Dispose()).Throws(exception1);
            var throwingDisposable2 = new Mock<IDisposable>();
            throwingDisposable2.Setup(it => it.Dispose()).Throws(exception2);

            IEnumerable<IDisposable> disposables = new IDisposable[] { throwingDisposable1.Object, goodDisposable1.Object, throwingDisposable2.Object, goodDisposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            var actualException = Assert.Throws<AggregateException>(
                () => mainDisposable.Dispose());

            Assert.Equal(2, actualException.InnerExceptions.Count);
            Assert.Contains(exception1, actualException.InnerExceptions);
            Assert.Contains(exception2, actualException.InnerExceptions);

            goodDisposable1.Verify(it => it.Dispose(), Times.Once);
            goodDisposable2.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable1.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable2.Verify(it => it.Dispose(), Times.Once);
        }

        [Theory]
        [InlineData("disposables")]
        public static void Disposable_CreateFromDisposables_NullArray_ThrowsArgumentNullException(string parameterName)
        {
           IDisposable[] disposables = new IDisposable[] { };

            NullTestingUtilities.TestNullParameters(
               () => Disposable.CreateFromDisposables(disposables),
               parameterName);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_ArrayWithNullDisposables_ThrowsArgumentException()
        {
            IDisposable[] disposables = new IDisposable[] { Disposable.Empty, null, null };

            var exception = Assert.Throws<ArgumentException>(
                () => Disposable.CreateFromDisposables(disposables));

            Assert.Equal("disposables", exception.ParamName);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_EmptyArray_DoesNotThrow()
        {
            IDisposable[] disposables = new IDisposable[] { };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            mainDisposable.Dispose();
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_NoDisposesWithArray_DisposeNeverCalled()
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IDisposable[] disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            disposable1.Verify(it => it.Dispose(), Times.Never);
            disposable2.Verify(it => it.Dispose(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_CreateFromDisposables_MultipleDisposesWithArray_DisposeCalledOnce(int disposalCount)
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IDisposable[] disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            for (int i = 0; i < disposalCount; i++)
            {
                mainDisposable.Dispose();
            }

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
        }

        [Fact]
        public static void Disposable_CreateFromDisposables_ArrayWithThrowingDisposables_ExceptionsAggregated()
        {
            var exception1 = new InvalidOperationException();
            var exception2 = new NullReferenceException();

            var goodDisposable1 = new Mock<IDisposable>();
            var goodDisposable2 = new Mock<IDisposable>();
            var throwingDisposable1 = new Mock<IDisposable>();
            throwingDisposable1.Setup(it => it.Dispose()).Throws(exception1);
            var throwingDisposable2 = new Mock<IDisposable>();
            throwingDisposable2.Setup(it => it.Dispose()).Throws(exception2);

            IDisposable[] disposables = new IDisposable[] { throwingDisposable1.Object, goodDisposable1.Object, throwingDisposable2.Object, goodDisposable2.Object };

            var mainDisposable = Disposable.CreateFromDisposables(disposables);

            var actualException = Assert.Throws<AggregateException>(
                () => mainDisposable.Dispose());

            Assert.Equal(2, actualException.InnerExceptions.Count);
            Assert.Contains(exception1, actualException.InnerExceptions);
            Assert.Contains(exception2, actualException.InnerExceptions);

            goodDisposable1.Verify(it => it.Dispose(), Times.Once);
            goodDisposable2.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable1.Verify(it => it.Dispose(), Times.Once);
            throwingDisposable2.Verify(it => it.Dispose(), Times.Once);
        }
    }
}
