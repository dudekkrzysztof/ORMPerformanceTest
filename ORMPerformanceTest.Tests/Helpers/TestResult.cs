namespace ORMPerformanceTest.Tests.Helpers
{
    public class TestResult
    {
        public long TimeInMilisecond { get; set; }
        public string Label { get; set; }
        public int AfectedRecord { get; set; }
        public double RowPerSec
        {
            get { return (double )AfectedRecord / TimeInMilisecond * 1000; }
        }
    }
}