using System;
using System.Reflection;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Xsd
{
    public class EntryPoint
    {
        private static byte[] ReadResource(string name)
        {
            string resourceName = "Xsd.Data." + name;
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if(resourceStream == null)
                throw new InvalidOperationException($"Resource '{resourceName}' is not found");
            var content = new byte[resourceStream.Length];
            if(resourceStream.Read(content, 0, content.Length) != content.Length)
                throw new InvalidOperationException($"Unable to read data from resource '{resourceName}'");
            return content;
        }

        public static void Main(string[] args)
        {
            //new EntryPoint().Test();
            BenchmarkRunner.Run<EntryPoint>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono)
                );
        }

        public void Test()
        {
            var xsd = xsdRunner.Run();
            var inlinedAutomaton = inlinedAutomatonRunner.Run();
            Console.WriteLine("xsd: {0}\r\ninlined automaton: {1}", xsd, inlinedAutomaton);
        }

        // Время работы этого метода нужно вычесть из каждого бенчмарка - это некий сетап, не являющийся частью алгоритма
        [Benchmark(Baseline = true)]
        public int Scan()
        {
            return scanRunner.Run();
        }

        [Benchmark]
        public int Xsd()
        {
            return xsdRunner.Run();
        }

        [Benchmark]
        public int InlinedAutomaton()
        {
            return inlinedAutomatonRunner.Run();
        }

        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private static byte[] xsd1 = ReadResource("110201.xsd");
        private static byte[] xml1 = ReadResource("110201.xml");
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        public static readonly Encoding encoding = Encoding.GetEncoding(1251);

        private readonly _110210_AutomatonRunner inlinedAutomatonRunner = new _110210_AutomatonRunner(xml1);
        private readonly XsdRunner xsdRunner = new XsdRunner(xml1, xsd1, true);
        private readonly XsdRunner scanRunner = new XsdRunner(xml1, xsd1, false);


    }
}