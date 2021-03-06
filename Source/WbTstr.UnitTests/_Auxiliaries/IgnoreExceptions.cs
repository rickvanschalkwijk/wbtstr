﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbTstr.UnitTests._Auxiliaries
{
    public static class IgnoreExceptions
    {
        public static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                // This is not the exception you are looking for..
            }
        }

        public static void Run<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                // This is not the exception you are looking for..
            }
        }
    }
}
