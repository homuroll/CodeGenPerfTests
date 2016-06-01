using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex
{
    public class Test_Number : IRunner
    {
        public void Run_RegexNotCompiled(string[] lines)
        {
            var regex = regex_notCompiled;
            foreach(var line in lines)
            {
                var match = regex.Match(line);
                while(match.Success)
                    match = match.NextMatch();
            }
        }

        public void Run_RegexCompiled(string[] lines)
        {
            var regex = regex_compiled;
            foreach(var line in lines)
            {
                var match = regex.Match(line);
                while(match.Success)
                    match = match.NextMatch();
            }
        }

        public void Run_Automaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = simpleAutomaton.Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        public void Run_OptimizedAutomaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = optimzedAutomaton.Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        public void Run_InlinedAutomaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        // This method can be generated automatically
        private static MatchResult Match(string str, int begPos)
        {
            int idx = begPos;
            int len = str.Length;
            char c;

            _1:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c == '+' || c == '-') goto _2;
            if(c >= '0' && c <= '9') goto _3;
            begPos = idx;
            goto _1;

            _2:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c >= '0' && c <= '9') goto _3;
            if(c == '+' || c == '-')
            {
                begPos = idx - 1;
                goto _2;
            }
            begPos = idx;
            goto _1;

            _3:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            c = str[idx++];
            if(c >= '0' && c <= '9') goto _3;
            if(c == '.') goto _4;
            return new MatchResult
                {
                    start = begPos,
                    length = idx - 1 - begPos
                };

            _4:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            c = str[idx++];
            if(c >= '0' && c <= '9') goto _4;
            return new MatchResult
                {
                    start = begPos,
                    length = idx - 1 - begPos
                };
        }

        private readonly System.Text.RegularExpressions.Regex regex_notCompiled = new System.Text.RegularExpressions.Regex(@"[\-+]?[0-9]+(\.[0-9]*)?");
        private readonly System.Text.RegularExpressions.Regex regex_compiled = new System.Text.RegularExpressions.Regex(@"[\-+]?[0-9]+(\.[0-9]*)?", RegexOptions.Compiled);

        private readonly SimpleAutomaton simpleAutomaton = new SimpleAutomaton();
        private readonly OptimizedAutomaton optimzedAutomaton = new OptimizedAutomaton();

        private class SimpleAutomaton
        {
            public SimpleAutomaton()
            {
                var s1 = new Node();
                var s2 = new Node();
                var s3 = new Node();
                var s4 = new Node();
                s1.jumps.Add('+', new Jump(s2, -1));
                s1.jumps.Add('-', new Jump(s2, -1));
                for(char c = '0'; c <= '9'; ++c)
                    s1.jumps.Add(c, new Jump(s3, -1));
                s1.rollbackTo = new Jump(s1, -1);
                for(char c = '0'; c <= '9'; ++c)
                    s2.jumps.Add(c, new Jump(s3, -1));
                s2.jumps.Add('+', new Jump(s2, 0));
                s2.jumps.Add('-', new Jump(s2, 0));
                s2.rollbackTo = new Jump(s1, -1);
                s3.jumps.Add('.', new Jump(s4, -1));
                for(char c = '0'; c <= '9'; ++c)
                    s3.jumps.Add(c, new Jump(s3, -1));
                s3.rollbackTo = new Jump(s1, -1);
                for(char c = '0'; c <= '9'; ++c)
                    s4.jumps.Add(c, new Jump(s4, -1));
                s4.rollbackTo = new Jump(s1, -1);
                s3.accepted = true;
                s4.accepted = true;

                start = s1;
            }

            public MatchResult Match(string str, int begPos)
            {
                var node = start;
                int endPos = 0;
                bool wasAccepted = false;
                for(int i = begPos; i < str.Length; ++i)
                {
                    if(node.accepted)
                        wasAccepted = true;
                    else if(wasAccepted)
                    {
                        endPos = i;
                        break;
                    }
                    Jump jump;
                    if(!node.jumps.TryGetValue(str[i], out jump))
                    {
                        if(wasAccepted)
                        {
                            endPos = i;
                            node = node.rollbackTo.node;
                            break;
                        }
                        jump = node.rollbackTo;
                        begPos = i + 1;
                    }
                    node = jump.node;
                    if(jump.diff >= 0)
                        begPos = i - jump.diff;
                }
                if(node.accepted)
                {
                    wasAccepted = true;
                    endPos = str.Length;
                }
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            length = endPos - begPos,
                            start = begPos
                        };
                }
                return new MatchResult();
            }

            private readonly Node start;

            private class Jump
            {
                public Jump(Node node, int diff)
                {
                    this.node = node;
                    this.diff = diff;
                }

                public readonly Node node;
                public readonly int diff;
            }

            private class Node
            {
                public readonly Dictionary<char, Jump> jumps = new Dictionary<char, Jump>();
                public Jump rollbackTo;
                public bool accepted;
            }
        }

        private class OptimizedAutomaton
        {
            public OptimizedAutomaton()
            {
                var t1 = new Node2_pm09();
                var t2 = new Node2_pm09();
                var t3 = new Node2_dot09();
                var t4 = new Node2_09();
                t1.child_pm = new Jump2(t2, -1);
                t1.child_09 = new Jump2(t3, -1);
                t1.rollbackTo = new Jump2(t1, -1);
                t2.child_pm = new Jump2(t2, 0);
                t2.child_09 = new Jump2(t3, -1);
                t2.rollbackTo = new Jump2(t1, -1);
                t3.child_dot = new Jump2(t4, -1);
                t3.child_09 = new Jump2(t3, -1);
                t3.rollbackTo = new Jump2(t1, -1);
                t4.child = new Jump2(t4, -1);
                t4.rollbackTo = new Jump2(t1, -1);
                t3.accepted = true;
                t4.accepted = true;

                start = t1;
            }

            public MatchResult Match(string str, int begPos)
            {
                var node = start;
                int endPos = 0;
                bool wasAccepted = false;
                for(int i = begPos; i < str.Length; ++i)
                {
                    if(node.accepted)
                        wasAccepted = true;
                    else if(wasAccepted)
                    {
                        endPos = i;
                        break;
                    }
                    var jump = node.Jump(str[i]);
                    if(jump == null)
                    {
                        if(wasAccepted)
                        {
                            endPos = i;
                            node = node.rollbackTo.node;
                            break;
                        }
                        jump = node.rollbackTo;
                        begPos = i + 1;
                    }
                    node = jump.node;
                    if(jump.diff >= 0)
                        begPos = i - jump.diff;
                }
                if(node.accepted)
                {
                    wasAccepted = true;
                    endPos = str.Length;
                }
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            length = endPos - begPos,
                            start = begPos
                        };
                }
                return new MatchResult();
            }

            private readonly Node2 start;

            private class Jump2
            {
                public Jump2(Node2 node, int diff)
                {
                    this.node = node;
                    this.diff = diff;
                }

                public readonly Node2 node;
                public readonly int diff;
            }

            private abstract class Node2
            {
                public abstract Jump2 Jump(char c);
                public Jump2 rollbackTo;
                public bool accepted;
            }

            private class Node2_09 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= '0' && c <= '9') return child;
                    return null;
                }

                public Jump2 child;
            }

            private class Node2_pm09 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c == '+' || c == '-')
                        return child_pm;
                    if(c >= '0' && c <= '9')
                        return child_09;
                    return null;
                }

                public Jump2 child_pm;
                public Jump2 child_09;
            }

            private class Node2_dot09 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c == '.')
                        return child_dot;
                    if(c >= '0' && c <= '9')
                        return child_09;
                    return null;
                }

                public Jump2 child_dot;
                public Jump2 child_09;
            }
        }
    }
}