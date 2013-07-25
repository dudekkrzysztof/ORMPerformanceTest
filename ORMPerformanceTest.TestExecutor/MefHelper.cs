using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using ORMPerformanceTest.Tests;

namespace ORMPerformanceTest.TestExecutor
{
    public class MefHelper
    {
        public static List<ITest> GetExport()
        {
            using (var container = CreateAppDomainContainer())
            {
                var exports = container.GetExports<ITest>().ToList();
                return exports.Select(i => i.Value).ToList();
            }
        }
        private static CompositionContainer CreateAppDomainContainer()
        {
            var aggregateCatalog = new AggregateCatalog();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(IsSimpleDataAssembly))
            {
                aggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }
            return new CompositionContainer(aggregateCatalog);
        }
        private static bool IsSimpleDataAssembly(Assembly assembly)
        {
            return GetFullName(assembly).StartsWith("ORMPerformanceTest.Tests", StringComparison.OrdinalIgnoreCase);
        }
        private static string GetFullName(Assembly assembly)
        {
            return assembly.FullName.Substring(0, assembly.FullName.IndexOf(",", StringComparison.OrdinalIgnoreCase));
        }
    }
}