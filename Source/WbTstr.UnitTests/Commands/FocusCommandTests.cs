﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using WbTstr.Commands;
using WbTstr.Proxies;
using WbTstr.Proxies.Interfaces;
using WbTstr.UnitTests._Auxiliaries;
using WbTstr.WebDrivers.Exceptions;

namespace WbTstr.UnitTests.Commands
{
    [TestFixture]
    public class FocusCommandTests
    {
        private const string DefaultSelector = "body";

        private FocusCommand _defaultCommand;

        [SetUp]
        public void SetUp()
        {
            _defaultCommand = new FocusCommand(DefaultSelector);
        }

        [TestCase]
        public void Constructor_SelectorNull_ThrowsArgumentNullException()
        {
            // Arrange
            string selector = null;

            // Act
            TestDelegate action = () => new FocusCommand(selector);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [TestCase]
        public void Constructor_ElementNull_ThrowsArgumentNullException()
        {
            // Arrange
            IElement selector = null;

            // Act
            TestDelegate action = () => new FocusCommand(selector);

            // Assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [TestCase]
        public void Constructor_SelectorInvalid_ThrowsArgumentException()
        {
            // Arrange

            // Act
            TestDelegate[] actions =
            {
                () => new FocusCommand(""),
                () => new FocusCommand(" "),
            };

            // Asset
            AssertMultiple.Throws<ArgumentException>(actions);
        }

        [TestCase]
        public void ToString_NoArgs_ReturnsString()
        {
            // Arrange

            // Act
            string stringRepresentation = _defaultCommand.ToString();

            // Assert
            AssertString.NotNullOrEmpty(stringRepresentation);
        }

        [TestCase]
        public void Execute_Selector_FindSingleElement()
        {
            // Arrange
            var webDriver = Substitute.For<IWebDriver>();

            // Act
            IgnoreExceptions.Run(() => _defaultCommand.Execute(webDriver));

            // Assert
            webDriver.ReceivedWithAnyArgs().FindElement(null);
        }

        [TestCase]
        public void Execute_NonExistingElement_ThrowsElementNotFoundException()
        {
            // Arrange
            var webDriver = Substitute.For<IWebDriver>();

            webDriver.FindElement(null).ReturnsForAnyArgs(w => throw new NoSuchElementException());

            // Act
            TestDelegate action = () => _defaultCommand.Execute(webDriver);

            // Assert
            Assert.Throws<ElementNotFoundException>(action);
        }
    }
}