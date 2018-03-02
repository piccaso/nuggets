using System;

namespace TryHarder
{
    public static class TryHarder
    {
        public delegate void ExceptionHandlerDelegate(Exception exception, int howHard, int failCount, bool timeToThrow);

        public static T Try<T>(Func<T> creFunc) => Try(1, creFunc, null);
        public static T Try<T>(int howHard, Func<T> createFunc, ExceptionHandlerDelegate exceptionHandler)
        {
            var failCount = 0;
            while (true)
            {
                try
                {
                    return createFunc();
                }
                catch (Exception e)
                {
                    failCount++;
                    var timeToThrow = failCount >= howHard;
                    exceptionHandler?.Invoke(e, failCount, howHard, timeToThrow);
                    if (timeToThrow) throw;
                }
            }
        }

        public static T Wrap<T>(string exceptionMessage, Func<T> createFunc) => Wrap(1, exceptionMessage, createFunc);
        public static T Wrap<T>(int howHard, string exceptionMessage, Func<T> createFunc)
        {
            return Try(howHard, createFunc, (innerException, hh, fc, timeToThrow) =>
            {
                if (timeToThrow) throw new Exception(exceptionMessage, innerException);
            });
        }

        public static void Try(Action createFunc) => Try(1, createFunc, null);
        public static void Try(int howHard, Action createAction, ExceptionHandlerDelegate exceptionHandler)
        {
            var failCount = 0;
            while (true)
            {
                try
                {
                    createAction();
                    return;
                }
                catch (Exception e)
                {
                    failCount++;
                    var timeToThrow = failCount >= howHard;
                    exceptionHandler?.Invoke(e, failCount, howHard, timeToThrow);
                    if (timeToThrow) throw;
                }
            }
        }

        public static void Wrap(string exceptionMessage, Action createAction) => Wrap(1, exceptionMessage, createAction);
        public static void Wrap(int howHard, string exceptionMessage, Action crateAction)
        {
            Try(howHard, crateAction, (innerException, hh, fc, timeToThrow) =>
            {
                if (timeToThrow) throw new Exception(exceptionMessage, innerException);
            });
        }
    }
}
