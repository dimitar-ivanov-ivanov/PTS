using ExelReader;
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

        [SetUp]
        public void Setup()
        {
            absoluteFrequencies = new Dictionary<int, int>();
            relativeFrequencies = new Dictionary<int, decimal>();
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_AllHaveSameFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 3 };

            //Act
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

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
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

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
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

            //Assert
            Assert.AreEqual(numbers.Count, absoluteFrequencies[1]);
        }

        [Test]
        public void Given_EmptyList_Then_FrequencyCalculatorGetAbsoluteFrequency_Should_ReturnNoFrequency()
        {
            //Arrange
            numbers = new List<int>() { };

            //Act
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

            //Assert
            Assert.AreEqual(0, absoluteFrequencies.Count);
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetRelativeFrequency_Should_AllHaveSameFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 3 };
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

            //Act
            relativeFrequencies = FrequencyCalculator.GetRelativeFrequencies(numbers);

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

            //Act
            relativeFrequencies = FrequencyCalculator.GetRelativeFrequencies(numbers);

            //Assert
            Assert.AreEqual(100, relativeFrequencies[1]);
        }

        [Test]
        public void Given_ListOfNumbers_Then_FrequencyCalculatorGetRelativeFrequency_Should_AllHaveDifferentFrequency()
        {
            //Arrange
            numbers = new List<int>() { 1, 2, 2, 3, 3, 3 };
            absoluteFrequencies = FrequencyCalculator.GetAbsoluteFrequencies(numbers);

            //Act
            relativeFrequencies = FrequencyCalculator.GetRelativeFrequencies(numbers);

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

            //Act
            relativeFrequencies = FrequencyCalculator.GetRelativeFrequencies(numbers);

            //Assert
            Assert.AreEqual(0, absoluteFrequencies.Count);
        }
    }
}