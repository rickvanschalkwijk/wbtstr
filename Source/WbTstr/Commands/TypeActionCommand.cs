﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using WbTstr.Commands.Interfaces;

namespace WbTstr.Commands
{
    public class TypeActionCommand : IActionCommand
    {
        private readonly string _text;
        private readonly string _selector;
        private readonly bool _clear;

        public TypeActionCommand(string text)
        {
            _text = text;
        }

        public TypeActionCommand(string text, string selector, bool clear)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            
            _text = text;
            _selector = selector;
            _clear = clear;
        }

        /* Methods ----------------------------------------------------------*/

        public void Execute(object webDriverObj)
        {
            var webDriver = webDriverObj as IWebDriver;
            var webElement = webDriver?.FindElement(By.CssSelector(_selector ?? "body"));

            if (_clear) {
                webElement?.Clear();
            }

            webElement?.SendKeys(_text);
        }

        public override string ToString()
        {
            return $"Type '{_text}'";
        }
    }
}