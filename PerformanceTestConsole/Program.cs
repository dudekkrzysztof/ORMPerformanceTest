using System;
using System.Configuration;
using Massive;
using ORMPerformanceTest.Tests.Ado;
using ORMPerformanceTest.Tests.Bulk;
using ORMPerformanceTest.Tests.EF;
using ORMPerformanceTest.Tests.Helpers;
using ORMPerformanceTest.Tests.Peta;

namespace PerformanceTestConsole
{
    class Program
    {
        static void Main()
        {
            string connection = ConfigurationManager.ConnectionStrings["ORMTest"].ConnectionString;
            TestAdo ado = new TestAdo();
            TestEF ef = new TestEF();
            TestPeta peta = new TestPeta();
            TestMassive massive = new TestMassive();
            TestResult result;

            ef.InitDataBase(connection);
            result = ef.Insert(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.Count(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.SelectAll(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.SelectPart(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.SelectJoin(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.Update100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ef.Delete100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);

            massive.InitDataBase(connection);
            result = massive.Insert(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.Count(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.SelectAll(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.SelectPart(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.SelectJoin(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.Update100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = massive.Delete100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);

            peta.InitDataBase(connection);
            result = peta.Insert(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.Count(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.SelectAll(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.SelectPart(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.SelectJoin(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.Update100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = peta.Delete100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);

            ado.InitDataBase(connection);
            result = ado.Insert(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.Count(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.SelectAll(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.SelectPart(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.SelectJoin(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.Update100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);
            result = ado.Delete100(connection);
            Console.WriteLine("{1} {2} {3} Test: {0} trwal: {1} milisec co daje na sekunde {2}", result.Label, result.TimeInMilisecond, result.RowPerSec, result.AfectedRecord);

            Console.ReadLine();

        }
    }
}
