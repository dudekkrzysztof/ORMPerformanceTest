using System;
using System.Diagnostics;

namespace ORMPerformanceTest.Tests.Helpers
{
    public static class TestRunner
    {
        public static TestResult RunTest(Func<string,int> test, string connectionString, string label)
        {
            int count;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            count = test.Invoke(connectionString);
            watch.Stop();
            return new TestResult { Label = label, TimeInMilisecond = watch.ElapsedMilliseconds, AfectedRecord = count};
        }
    }
}