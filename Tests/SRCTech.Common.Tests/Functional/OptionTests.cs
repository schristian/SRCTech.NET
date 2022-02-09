using System;
using Moq;
using SRCTech.Common.Functional;
using Xunit;

namespace SRCTech.Common.Tests.Functional
{
    public static class OptionTests
    {
        [Fact]
        public static void Option_None_ReturnsOptionWithNoValue()
        {
            var option = Option.None<int>();

            Assert.False(option.HasValue);
            Assert.Throws<InvalidOperationException>(() => option.Value);
        }

        [Fact]
        public static void Option_None_ReturnsSameInstance()
        {
            var option1 = Option.None<int>();
            var option2 = Option.None<int>();

            Assert.Same(option1, option2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Option_Some_ReturnsOptionWithValue(object value)
        {
            var option = Option.Some(value);

            Assert.True(option.HasValue);
            Assert.Equal(value, option.Value);
        }

        [Fact]
        public static void Option_FromNullable_NullStruct_ReturnsNone()
        {
            int? value = null;
            var option = Option.FromNullable(value);

            Assert.False(option.HasValue);
            Assert.Throws<InvalidOperationException>(() => option.Value);
        }

        [Fact]
        public static void Option_FromNullable_NonNullStruct_ReturnsSome()
        {
            int? value = 5;
            var option = Option.FromNullable(value);

            Assert.True(option.HasValue);
            Assert.Equal(value, option.Value);
        }

        [Fact]
        public static void Option_FromNullable_NullClass_ReturnsNone()
        {
            string value = null;
            var option = Option.FromNullable(value);

            Assert.False(option.HasValue);
            Assert.Throws<InvalidOperationException>(() => option.Value);
        }

        [Fact]
        public static void Option_FromNullable_NonNullClass_ReturnsSome()
        {
            string value = "String Value";
            var option = Option.FromNullable(value);

            Assert.True(option.HasValue);
            Assert.Equal(value, option.Value);
        }

        [Theory]
        [InlineData("option")]
        public static void Option_TryGetValue_NullOption_ThrowsArgumentNullException(string parameterName)
        {
            var option = Option.Some(5);
            int value;

            NullTestingUtilities.TestNullParameters(
               () => Option.TryGetValue(option, out value),
               parameterName);
        }

        [Fact]
        public static void Option_TryGetValue_NoneOption_ReturnsFalse()
        {
            var option = new Mock<IOption<int>>(MockBehavior.Strict);
            option.Setup(it => it.HasValue).Returns(false);

            Assert.False(Option.TryGetValue(option.Object, out int value));
        }

        [Fact]
        public static void Option_TryGetValue_SomeOption_ReturnsTrue()
        {
            var expectedValue = 5;
            var option = new Mock<IOption<int>>(MockBehavior.Strict);
            option.Setup(it => it.HasValue).Returns(true);
            option.Setup(it => it.Value).Returns(expectedValue);

            Assert.True(Option.TryGetValue(option.Object, out int actualValue));
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
