using System.Threading.Tasks;
using SRCTech.Common.Functional;
using Xunit;

namespace SRCTech.Common.Tests.Functional
{
    public sealed class UnitTests
    {
        [Fact]
        public static void Unit_Default_ReturnsDefaultUnit()
        {
            Assert.Equal(default(Unit), Unit.Default);
        }

        [Fact]
        public static void Unit_Task_ReturnsCompletedTask()
        {
            Task<Unit> task = Unit.Task;

            Assert.True(task.IsCompletedSuccessfully);
            Assert.Equal(Unit.Default, task.Result);
        }

        [Fact]
        public static void Unit_Task_ReturnsSameInstance()
        {
            Task<Unit> task1 = Unit.Task;
            Task<Unit> task2 = Unit.Task;

            Assert.Same(task1, task2);
        }

        [Fact]
        public static void Unit_Equals_OtherUnit_ReturnsTrue()
        {
            Unit unit1 = new Unit();
            Unit unit2 = new Unit();

            Assert.True(unit1.Equals(unit2));
        }

        [Fact]
        public static void Unit_EqualsOperator_OtherUnit_ReturnsTrue()
        {
            Unit unit1 = new Unit();
            Unit unit2 = new Unit();

            Assert.True(unit1 == unit2);
        }

        [Fact]
        public static void Unit_NotEqualsOperator_OtherUnit_ReturnsFalse()
        {
            Unit unit1 = new Unit();
            Unit unit2 = new Unit();

            Assert.False(unit1 != unit2);
        }

        [Fact]
        public static void Unit_Equals_OtherUnitObject_ReturnsTrue()
        {
            Unit unit1 = new Unit();
            object unit2 = new Unit();

            Assert.True(unit1.Equals(unit2));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Unit_Equals_OtherTypeOfObject_ReturnsFalse(object value)
        {
            Unit unit = new Unit();

            Assert.False(unit.Equals(value));
        }

        [Fact]
        public static void Unit_GetHashCode_EqualsOtherUnitHashCode()
        {
            Unit unit1 = new Unit();
            Unit unit2 = new Unit();

            Assert.Equal(unit1.GetHashCode(), unit2.GetHashCode());
        }

        [Fact]
        public static void Unit_ToString_ReturnsConstantString()
        {
            const string expectedString = "()";

            Assert.Equal(expectedString, Unit.Default.ToString());
        }
    }
}
