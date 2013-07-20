using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Massive;
using ORMPerformanceTest.Tests;
using ORMPerformanceTest.Tests.Ado;
using ORMPerformanceTest.Tests.Dapper;
using ORMPerformanceTest.Tests.EF;
using ORMPerformanceTest.Tests.Helpers;
using ORMPerformanceTest.Tests.Peta;
using ORMPerformanceTest.Tests.Simple.Data;

namespace ORMPerformanceTest.TestExecutor
{
    public class Executor
    {
        private readonly List<ITest> _tests;
        private readonly int _runCount;
        private List<TestResult> _insertResult;
        private List<TestResult> _countResult;
        private List<TestResult> _selectAllResult;
        private List<TestResult> _selectPartResult;
        private List<TestResult> _selectJoinResult;
        private List<TestResult> _updateResult;
        private List<TestResult> _deleteResult;
        public Executor(int runCount = 5)
        {
            _tests = new List<ITest>
            {
                new TestEF(),
                new TestAdo(),
                new TestDapper(),
                new TestMassive(),
                new TestPeta(),
                new TestSimpleData()
            };
            _runCount = runCount;
        }

        public List<TestResult> Run()
        {
            string connection = ConfigurationManager.ConnectionStrings["ORMTest"].ConnectionString;
            List<TestResult> averageResults = new List<TestResult>(_tests.Count * 7);

            foreach (var test in _tests)
            {
                _insertResult = new List<TestResult>(7 * _runCount);
                _countResult = new List<TestResult>(7 * _runCount);
                _selectAllResult = new List<TestResult>(7 * _runCount);
                _selectPartResult = new List<TestResult>(7 * _runCount);
                _selectJoinResult = new List<TestResult>(7 * _runCount);
                _updateResult = new List<TestResult>(7 * _runCount);
                _deleteResult = new List<TestResult>(7 * _runCount);
                test.InitDataBase(connection);
                for (int i = 0; i < _runCount; i++)
                {
                    _insertResult.Add(test.Insert(connection));
                    _countResult.Add(test.Count(connection));
                    _selectAllResult.Add(test.SelectAll(connection));
                    _selectPartResult.Add(test.SelectPart(connection));
                    _selectJoinResult.Add(test.SelectJoin(connection));
                    _updateResult.Add(test.Update100(connection));
                    _deleteResult.Add(test.Delete100(connection));
                }
                averageResults.Add(GetAverage(_insertResult));
                averageResults.Add(GetAverage(_countResult));
                averageResults.Add(GetAverage(_selectAllResult));
                averageResults.Add(GetAverage(_selectPartResult));
                averageResults.Add(GetAverage(_selectJoinResult));
                averageResults.Add(GetAverage(_updateResult));
                averageResults.Add(GetAverage(_deleteResult));
            }

            return averageResults;
        }

        private static TestResult GetAverage(List<TestResult> insertResult)
        {
            return new TestResult
            {
                AfectedRecord = (int)insertResult.Average(result => result.AfectedRecord),
                Label = insertResult.First().Label,
                TimeInMilisecond = (int)insertResult.Average(result => result.TimeInMilisecond)
            };
        }
    }
}
