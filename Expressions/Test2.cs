using System;
using System.Linq;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;

using GrobExp.Compiler;

namespace Expressions
{
    public class Test2
    {
        public Test2()
        {
            sharp = Sharp;
            Expression<Func<TestClassA, bool>> exp = o => o.ArrayB.Any(b => b.S == o.S);
            compile = exp.Compile();
            grobexpCompile = LambdaCompiler.Compile(exp, CompilerOptions.None);
            grobexpCompileWClosure = LambdaCompiler.Compile(exp, CompilerOptions.CreateDynamicClosure);
            a = new TestClassA {S = "zzz", ArrayB = new[] {new TestClassB {S = "zzz"},}};
        }

        private static bool Sharp(TestClassA a)
        {
            return a.ArrayB.Any(b => b.S == a.S);
        }

        [Benchmark(Baseline = true)]
        public bool Run_Sharp()
        {
            return sharp(a);
        }

        [Benchmark]
        public bool Compile()
        {
            return compile(a);
        }

        [Benchmark]
        public bool GrobExpCompile()
        {
            return grobexpCompile(a);
        }

        [Benchmark]
        public bool GrobExpCompileWClosure()
        {
            return grobexpCompileWClosure(a);
        }

        private readonly Func<TestClassA, bool> sharp;
        private readonly Func<TestClassA, bool> compile;
        private readonly Func<TestClassA, bool> grobexpCompile;
        private readonly Func<TestClassA, bool> grobexpCompileWClosure;

        private readonly TestClassA a;
    }
}