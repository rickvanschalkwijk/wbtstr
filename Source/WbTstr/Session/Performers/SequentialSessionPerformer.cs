﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WbTstr.Commands.Interfaces;
using WbTstr.Configuration.WebDrivers.Interfaces;
using WbTstr.Session.Performers.Interfaces;
using WbTstr.Session.Trackers.Interfaces;
using WbTstr.WebDrivers;
using WbTstr.WebDrivers.Exceptions;

namespace WbTstr.Session.Performers
{
    public class SequentialSessionPerformer : ISessionPerformer
    {
        private readonly Queue<ICommand> _commands;
        private Lazy<IWebDriver> _webDriver;
        private ISessionTracker _tracker;
        private bool _initialized;

        public SequentialSessionPerformer()
        {
            _commands = new Queue<ICommand>();
        }

        public ISessionPerformer Initialize(Lazy<IWebDriverConfig> webDriverConfig, ISessionTracker tracker)
        {
            if (webDriverConfig == null) throw new ArgumentNullException(nameof(webDriverConfig));
            _tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));

            if (_initialized)
            {
                throw new InvalidOperationException($"{nameof(SequentialSessionPerformer)} can be initialized only once.");
            }

            _webDriver = new Lazy<IWebDriver>(() => WebDriverFactory.CreateFromConfig(webDriverConfig.Value));

            _initialized = true;
            return this;
        }

        /* Properties -------------------------------------------------------*/

        public bool DirectPlay { get; set; } = true;

        protected IWebDriver WebDriver => _webDriver?.Value;

        /* Methods ----------------------------------------------------------*/

        public void Perform(IActionCommand actionCommand)
        {
            if (actionCommand == null) throw new ArgumentNullException(nameof(actionCommand));

            if (!DirectPlay)
            {
                _commands.Enqueue(actionCommand);
                return;
            }

            ExecuteActionCommand(actionCommand);
        }

        public T PerformAndReturn<T>(IReturnCommand<T> returnCommand)
        {
            if (returnCommand == null) throw new ArgumentNullException(nameof(returnCommand));

            if (!DirectPlay)
            {
                _commands.Enqueue(returnCommand);
            }

            return ExecuteReturnCommand(returnCommand);
        }

        public void Play()
        {
            while (_commands.Count != 0)
            {
                var command = _commands.Dequeue();
                if (command is IActionCommand)
                {
                    ExecuteActionCommand(command as IActionCommand);
                }
            }
        }

        private void ExecuteActionCommand(IActionCommand actionCommand)
        {
            try
            {
                _tracker.MarkExecutionBegin(actionCommand);

                actionCommand.Execute(WebDriver);

                _tracker.MarkExecutionEnd(actionCommand);
            }
            catch (UnexpectedWebDriverStateException)
            {
                Dispose();
                throw;
            }
        }

        private T ExecuteReturnCommand<T>(IReturnCommand<T> command)
        {
            try
            {
                _tracker.MarkExecutionBegin(command);

                var result = command.Execute(WebDriver);

                _tracker.MarkExecutionEnd(command);

                return result;
            }
            catch (UnexpectedWebDriverStateException)
            {
                Dispose();
                throw;
            }
        }

        /* Finalizer --------------------------------------------------------*/

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || !_webDriver.IsValueCreated) return;

            WebDriver?.Quit();
            WebDriver?.Dispose();
        }
    }
}


