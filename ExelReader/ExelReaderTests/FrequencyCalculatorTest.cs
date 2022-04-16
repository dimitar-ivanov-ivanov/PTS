using ExelReader;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExelReaderTests
{
    public class FrequencyCalculatorTest
    {
        private List<int> numbers;
        private Dictionary<int, int> absoluteFrequencies;
        private Dictionary<int, decimal> relativeFrequencies;
        private Mock<FrequencyCalculator> mockedCalcultor;


        [SetUp]
        public void Setup()
        {
            absoluteFrequencies = new Dictionary<int, int>();
            relativeFrequencies = new Dictionary<int, decimal>();
            mockedCalcultor = new Mock<FrequencyCalculator>() { CallBase = true };
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_AllHaveSameFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 3 };

            //Act
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            for (int i = 0; i < numbers.Count; i++)
            {
                //Assert
                Assert.AreEqual(1, absoluteFrequencies[numbers[i]]);
            }
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_AllHaveDifferentFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 2, 3, 3, 3 };

            //Act
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            //Assert
            Assert.AreEqual(1, absoluteFrequencies[1]);
            Assert.AreEqual(2, absoluteFrequencies[2]);
            Assert.AreEqual(3, absoluteFrequencies[3]);
        }

        [Test]
        public void Given_ListOfSameNumber_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_HaveSameFrequencyAsListCount()
        {
            //Arrange
            numbers = new List<int>() { 1, 1, 1 };

            //Act
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            //Assert
            Assert.AreEqual(numbers.Count, absoluteFrequencies[1]);
        }

        [Test]
        public void Given_EmptyList_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_ReturnNoFrequency()
        {
            //Arrange
            numbers = new List<int>() { };

            //Act
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            //Assert
            Assert.AreEqual(0, absoluteFrequencies.Count);
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetRelativeFrequency_Should_AllHaveSameFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 3 };
            mockedCalcultor.Setup(x => x.GetAbsoluteFrequencies(It.IsAny<List<int>>())).Returns(new Dictionary<int, int>()
            {
                { 1, 1 },
                { 2, 1 },
                { 3, 1 }
            });
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            //Act
            relativeFrequencies = mockedCalcultor.Object.GetRelativeFrequencies(numbers);

            for (int i = 0; i < numbers.Count; i++)
            {
                //Assert
                decimal expectedResult = (Decimal)(absoluteFrequencies[numbers[i]] * 100) / numbers.Count;
                Assert.AreEqual(Decimal.Round(expectedResult, 1, MidpointRounding.AwayFromZero), relativeFrequencies[numbers[i]]);
            }
        }

        [Test]
        public void Given_ListOfSameNumber_Then_FrequencyCalculatorGetRelativeFrequency_Should_Have100PercentFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 1, 1 };
            mockedCalcultor.Setup(x => x.GetAbsoluteFrequencies(It.IsAny<List<int>>())).Returns(new Dictionary<int, int>()
            {
                { 1, 3 },
            });

            //Act
            relativeFrequencies = mockedCalcultor.Object.GetRelativeFrequencies(numbers);

            //Assert
            Assert.AreEqual(100, relativeFrequencies[1]);
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetRelativeFrequency_Should_AllHaveDifferentFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 2, 3, 3, 3 };
            mockedCalcultor.Setup(x => x.GetAbsoluteFrequencies(It.IsAny<List<int>>())).Returns(new Dictionary<int, int>()
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 }
            });
            absoluteFrequencies = mockedCalcultor.Object.GetAbsoluteFrequencies(numbers);

            //Act
            relativeFrequencies = mockedCalcultor.Object.GetRelativeFrequencies(numbers);

            for (int i = 0; i < numbers.Count; i++)
            {
                //Assert
                decimal expectedResult = (Decimal)(absoluteFrequencies[numbers[i]] * 100) / numbers.Count;
                Assert.AreEqual(Decimal.Round(expectedResult, 1, MidpointRounding.AwayFromZero), relativeFrequencies[numbers[i]]);
            }
        }

        [Test]
        public void Given_EmptyList_Then_FrequencyCalculatorGetRelativeFrequency_Should_ReturnNoFrequency()
        {
            //Arrange
            numbers = new List<int>() { };
            mockedCalcultor.Setup(x => x.GetAbsoluteFrequencies(It.IsAny<List<int>>())).Returns(new Dictionary<int, int>() { });

            //Act
            relativeFrequencies = mockedCalcultor.Object.GetRelativeFrequencies(numbers);

            //Assert
            Assert.AreEqual(0, absoluteFrequencies.Count);
        }
    }
}