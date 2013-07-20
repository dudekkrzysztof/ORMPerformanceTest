using System;
using ORMPerformanceTest.TestExecutor;

namespace PerformanceTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Executor executor = CreateExecutor(args);
            DateTime start = DateTime.Now;

            var result = executor.Run();
            foreach (var testResult in result)
            {
                Console.WriteLine("Test: {0} take: {1} millisecond affected {3} rows, {2} row/second", testResult.Label, testResult.TimeInMilisecond, testResult.RowPerSec, testResult.AfectedRecord);
            }

            Console.WriteLine("Total time: {0} millisecond", (DateTime.Now - start).TotalMilliseconds);
            Console.ReadLine();

        }

        private static Executor CreateExecutor(string[] args)
        {
            if (args.Length == 0)
                return new Executor();
            int counter = 0;
            int.TryParse(args[0], out counter);
            if (counter < 1)
                return new Executor();
            return new Executor(counter);
        }
    }
}
