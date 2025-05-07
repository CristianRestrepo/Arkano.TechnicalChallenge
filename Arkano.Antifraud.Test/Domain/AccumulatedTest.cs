using Arkano.Antifraud.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Test.Domain
{
    [TestClass]
    public sealed class AccumulatedTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void SetAccumulated_ValidValue_ShouldSetAccumulated()
        {
            // Arrange
            var accumulated = new Accumulated();
            var value = 100;
            // Act
            accumulated.SetAccumulated(value);
            // Assert
            Assert.AreEqual(value, accumulated.GetAccumulated());
        }

        [TestMethod]
        public void AddValueToAccumulated_ValidValue_ShouldSetAccumulated()
        {
            // Arrange
            var accumulated = new Accumulated();
            var value = 100;
            // Act
            accumulated.SetAccumulated(value);
            accumulated.AddValueToAccumulated(value);
            // Assert
            Assert.AreEqual(200, accumulated.GetAccumulated());
        }


        [TestMethod]
        public void ChangeAccumulatedDate_ValidValue_ShouldSetAccumulated()
        {
            // Arrange
            var accumulated = new Accumulated();
            var value = 100;
            // Act
            accumulated.SetAccumulated(value);
            accumulated.SetAccumulatedDate(DateTime.Today.AddDays(-1));

            accumulated.AddValueToAccumulated(value);
            // Assert
            Assert.AreEqual(100, accumulated.GetAccumulated());
            Assert.AreEqual(DateTime.Today, accumulated.GetAccumulatedDate());
        }
    }
}
