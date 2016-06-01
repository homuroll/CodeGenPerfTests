namespace Regex
{
    public interface IRunner
    {
        void Run_RegexNotCompiled(string[] lines);
        void Run_RegexCompiled(string[] lines);
        void Run_Automaton(string[] lines);
        void Run_OptimizedAutomaton(string[] lines);
        void Run_InlinedAutomaton(string[] lines);
    }
}