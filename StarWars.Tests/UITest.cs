using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarWars.UI;

namespace StarWars.Tests
{
    [TestClass]
    public class UITest
    {
        [TestMethod]
        public void VerifyIfAnEmptyInputIsValid()
        {
            var input = "";
            Assert.IsFalse(Program.InputIsValid(input, out long distanceInMGLT));
        }

        [TestMethod]
        public void VerifyIfAnAlphanumericInputIsValid()
        {
            var input = "Marcus";
            Assert.IsFalse(Program.InputIsValid(input, out long distanceInMGLT));
        }

        [TestMethod]
        public void VerifyIfANumberBiggerThanLongIsValid()
        {
            // The largest possible value to a long variable is 9223372036854775807
            // 9223372036854775808 is an error
            var input = "9223372036854775808";
            Assert.IsFalse(Program.InputIsValid(input, out long distanceInMGLT));
        }

        [TestMethod]
        public void VerifyIfANegativeValueIsValid()
        {
            var input = "-100000";
            Assert.IsFalse(Program.InputIsValid(input, out long distanceInMGLT));
        }

        [TestMethod]
        public void VerifyIfLongValuesAreValid()
        {
            var input = "100000";
            Assert.IsTrue(Program.InputIsValid(input, out long distanceInMGLT));
            Assert.IsTrue(distanceInMGLT > 0);

            input = "9223372036854775807";
            Assert.IsTrue(Program.InputIsValid(input, out distanceInMGLT));
            Assert.IsTrue(distanceInMGLT > 0);
        }
    }
}
