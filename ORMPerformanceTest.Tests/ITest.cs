using ORMPerformanceTest.Tests.Helpers;

namespace ORMPerformanceTest.Tests
{
    public interface ITest
    {
        string TestKind { get; }
        TestResult Count(string connectionString);
        TestResult Insert(string connectionString);
        TestResult SelectAll(string connectionString);
        TestResult SelectPart(string connectionString);
        TestResult SelectJoin(string connectionString);
        TestResult Update100(string connectionString);
        TestResult Delete100(string connectionString);
        void InitDataBase(string connectionString);
        int Priority { get; }
    }
}
