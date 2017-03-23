﻿using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using WbTstr.Commands;
using WbTstr.UnitTests._Auxiliaries;
using WbTstr.WebDrivers.Constants;

namespace WbTstr.UnitTests.Commands
{
    [TestFixture]
    public class AssertStateExpCommandTests
    {
        private const string DefaultPropertyValue = "WbTstr 4 Life";

        [TestCase]
        public void Constructor_TypicalPropertyKeyValue_DoesNotThrow()
        {
            // Arrange

            // Act
            TestDelegate action = () => new AssertStateExpCommand(x => x.Title == DefaultPropertyValue);

            // Assert
            Assert.DoesNotThrow(action);
        }

        [TestCase]
        public void Constructor_UninitializedPropertyValue_DoesNotThrow()
        {
            // Arrange
            string propertyValue = null;

            // Act
            TestDelegate action = () => new AssertStateExpCommand(x => x.Title == propertyValue);

            // Assert
            Assert.DoesNotThrow(action);
        }

        [TestCase]
        public void Constructor_EmptyPropertyValue_DoesNotThrow()
        {
            // Arrange
            string propertyValue = string.Empty;

            // Act
            TestDelegate action = () => new AssertStateExpCommand(x => x.Title == propertyValue);

            // Assert
            Assert.DoesNotThrow(action);
        }

        [TestCase]
        public void Execute_UninitializedWebDriver_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new AssertStateExpCommand(x => x.Title == DefaultPropertyValue);
            object webDriverObj = null;

            // Act
            TestDelegate action = () => command.Execute(webDriverObj);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [TestCase]
        public void Execute_DifferentThanWebDriverType_ThrowsArgumentException()
        {
            // Arrange
            var command = new AssertStateExpCommand(x => x.Title == DefaultPropertyValue);
            object webDriverObj = "Not a WebDriver object";

            // Act
            TestDelegate action = () => command.Execute(webDriverObj);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        [TestCase]
        public void Execute_AssertTitleProperty_UseRightStateProperty()
        {
            // Arrange
            var webDriver = Substitute.For<IWebDriver>();
            var command = new AssertStateExpCommand(x => x.Title == DefaultPropertyValue);

            // Act
            IgnoreExpections.Run(() => command.Execute(webDriver));

            // Assert
            string title = webDriver.Received().Title;
        }

        [TestCase]
        public void Execute_AssertUrlProperty_UseRightStateProperty()
        {
            // Arrange
            var webDriver = Substitute.For<IWebDriver>();
            var command = new AssertStateExpCommand(x => x.Url == DefaultPropertyValue);

            // Act
            IgnoreExpections.Run(() => command.Execute(webDriver));

            // Assert
            string url = webDriver.Received().Url;
        }
    }
}