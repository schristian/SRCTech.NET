using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public static class DisposableTests_DisposeAll
    {
        [Theory]
        [InlineData("disposables")]
        public static void Disposable_DisposeAll_NullEnumerable_ThrowsArgumentNullException(string parameterName)
        {
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            NullTestingUtilities.TestNullParameters(
               () => Disposable.DisposeAll(disposables),
               parameterName);
        }

        [Fact]
        public static void Disposable_DisposeAll_EnumerableWithNullDisposables_ThrowsArgumentNullException()
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IEnumerable<IDisposable> disposables = new IDisposable[] { null, disposable1.Object, null, disposable2.Object };

            var actualException = Assert.Throws<AggregateException>(
                () => Disposable.DisposeAll(disposables));

            Assert.Single(actualException.InnerExceptions);
            Assert.IsType<ArgumentException>(actualException.InnerExceptions.Single());

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
        }

        [Fact]
        public static void Disposable_DisposeAll_EmptyEnumerable_DoesNotThrow()
        {
            IEnumerable<IDisposable> disposables = new IDisposable[] { };

            Disposable.DisposeAll(disposables);
        }

        [Fact]
        public static void Disposable_DisposeAll_MultipleDisposesWithEnumerable_AllDisposesCalled()
        {
            var disposable1 = new Mock<IDisposable>();
            var disposable2 = new Mock<IDisposable>();
            IEnumerable<IDisposable> disposables = new IDisposable[] { disposable1.Object, disposable2.Object };

            Disposable.DisposeAll(disposables);

            disposable1.Verify(it => it.Dispose(), Times.Once);
            disposable2.Verify(it => it.Dispose(), Times.Once);
        }

        [Fact]
        public static void Disposable_DisposeAll_EnumerableWithThrowingDisposables_ExceptionsAggregated()
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

            var actualException = Assert.Throws<AggregateException>(
                () => Disposable.DisposeAll(disposables));

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
