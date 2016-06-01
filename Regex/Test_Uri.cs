using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex
{
    public class Test_Uri : IRunner
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

        private static bool[] BuildCharSet()
        {
            var table = new bool[256];
            for(char c = 'a'; c <= 'z'; ++c)
                table[c] = true;
            for(char c = 'A'; c <= 'Z'; ++c)
                table[c] = true;
            for(char c = '0'; c <= '9'; ++c)
                table[c] = true;
            foreach(char c in "-._~!$&?'()*+,;=:")
                table[c] = true;
            return table;
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
            if(c >= 'a' && c <= 'z') goto _2;
            if(c >= 'A' && c <= 'Z') goto _2;
            begPos = idx;
            goto _1;

            _2:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _2;
            if(c >= 'A' && c <= 'Z') goto _2;
            if(c >= '0' && c <= '9') goto _2;
            if(c == ':') goto _3;
            begPos = idx;
            goto _1;

            _3:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c >= 'a' && c <= 'z')
            {
                begPos = idx - 1;
                goto _2;
            }
            if(c >= 'A' && c <= 'Z')
            {
                begPos = idx - 1;
                goto _2;
            }
            if(c == '/') goto _4;
            begPos = idx;
            goto _1;

            _4:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c >= 'a' && c <= 'z')
            {
                begPos = idx - 1;
                goto _2;
            }
            if(c >= 'A' && c <= 'Z')
            {
                begPos = idx - 1;
                goto _2;
            }
            if(c == '/') goto _5;
            begPos = idx;
            goto _1;

            _5:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c < 256 && charSet[c])
                goto _6;
            begPos = idx;
            goto _1;

            _6:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            c = str[idx++];
            if(c < 256 && charSet[c])
                goto _6;
            if(c == '/')
                goto _7;
            return new MatchResult
                {
                    start = begPos,
                    length = idx - 1 - begPos
                };

            _7:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            c = str[idx++];
            if(c < 256 && charSet[c])
                goto _7;
            return new MatchResult
                {
                    start = begPos,
                    length = idx - 1 - begPos
                };
        }

        private readonly System.Text.RegularExpressions.Regex regex_notCompiled = new System.Text.RegularExpressions.Regex(@"([a-zA-Z][a-zA-Z0-9]*):\/\/([a-zA-Z0-9-._~!$&?'()*+,;=:]+)(\/[a-zA-Z0-9-._~!$&'()*+?,;=:]*)?");
        private readonly System.Text.RegularExpressions.Regex regex_compiled = new System.Text.RegularExpressions.Regex(@"([a-zA-Z][a-zA-Z0-9]*):\/\/([a-zA-Z0-9-._~!$&?'()*+,;=:]+)(\/[a-zA-Z0-9-._~!$&'()*+?,;=:]*)?", RegexOptions.Compiled);

        private readonly SimpleAutomaton simpleAutomaton = new SimpleAutomaton();
        private readonly OptimizedAutomaton optimzedAutomaton = new OptimizedAutomaton();

        private static readonly bool[] charSet = BuildCharSet();

        public class SimpleAutomaton
        {
            public SimpleAutomaton()
            {
                var s1 = new Node();
                var s2 = new Node();
                var s3 = new Node();
                var s4 = new Node();
                var s5 = new Node();
                var s6 = new Node();
                var s7 = new Node();
                for(char c = 'a'; c <= 'z'; ++c)
                    s1.jumps.Add(c, new Jump(s2, -1));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s1.jumps.Add(c, new Jump(s2, -1));
                s1.rollbackTo = new Jump(s1, -1);

                for(char c = '0'; c <= '9'; ++c)
                    s2.jumps.Add(c, new Jump(s2, -1));
                for(char c = 'a'; c <= 'z'; ++c)
                    s2.jumps.Add(c, new Jump(s2, -1));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s2.jumps.Add(c, new Jump(s2, -1));
                s2.jumps.Add(':', new Jump(s3, -1));
                s2.rollbackTo = new Jump(s1, -1);

                for(char c = 'a'; c <= 'z'; ++c)
                    s3.jumps.Add(c, new Jump(s2, 0));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s3.jumps.Add(c, new Jump(s2, 0));
                s3.jumps.Add('/', new Jump(s4, -1));
                s3.rollbackTo = new Jump(s1, -1);

                for(char c = 'a'; c <= 'z'; ++c)
                    s4.jumps.Add(c, new Jump(s2, 0));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s4.jumps.Add(c, new Jump(s2, 0));
                s4.jumps.Add('/', new Jump(s5, -1));
                s4.rollbackTo = new Jump(s1, -1);

                for(char c = 'a'; c <= 'z'; ++c)
                    s5.jumps.Add(c, new Jump(s6, -1));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s5.jumps.Add(c, new Jump(s6, -1));
                for(char c = '0'; c <= '9'; ++c)
                    s5.jumps.Add(c, new Jump(s6, -1));
                foreach(char c in "-._~!$&?'()*+,;=:")
                    s5.jumps.Add(c, new Jump(s6, -1));
                s5.rollbackTo = new Jump(s1, -1);

                for(char c = 'a'; c <= 'z'; ++c)
                    s6.jumps.Add(c, new Jump(s6, -1));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s6.jumps.Add(c, new Jump(s6, -1));
                for(char c = '0'; c <= '9'; ++c)
                    s6.jumps.Add(c, new Jump(s6, -1));
                foreach(char c in "-._~!$&?'()*+,;=:")
                    s6.jumps.Add(c, new Jump(s6, -1));
                s6.jumps.Add('/', new Jump(s7, -1));
                s6.rollbackTo = new Jump(s1, -1);

                for(char c = 'a'; c <= 'z'; ++c)
                    s7.jumps.Add(c, new Jump(s7, -1));
                for(char c = 'A'; c <= 'Z'; ++c)
                    s7.jumps.Add(c, new Jump(s7, -1));
                for(char c = '0'; c <= '9'; ++c)
                    s7.jumps.Add(c, new Jump(s7, -1));
                foreach(char c in "-._~!$&'()*+?,;=:")
                    s7.jumps.Add(c, new Jump(s7, -1));
                s7.rollbackTo = new Jump(s1, -1);

                s6.accepted = true;
                s7.accepted = true;

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

        public class OptimizedAutomaton
        {
            public OptimizedAutomaton()
            {
                var t1 = new Node2_az();
                var t2 = new Node2_2();
                var t3 = new Node2_3();
                var t4 = new Node2_3();
                var t5 = new Node2_5();
                var t6 = new Node2_6();
                var t7 = new Node2_5();

                t1.child = new Jump2(t2, -1);
                t1.rollbackTo = new Jump2(t1, -1);

                t2.child_az09 = new Jump2(t2, -1);
                t2.child_semicolon = new Jump2(t3, -1);
                t2.rollbackTo = new Jump2(t1, -1);

                t3.child_slash = new Jump2(t4, -1);
                t3.child_az = new Jump2(t2, 0);
                t3.rollbackTo = new Jump2(t1, -1);

                t4.child_slash = new Jump2(t5, -1);
                t4.child_az = new Jump2(t2, 0);
                t4.rollbackTo = new Jump2(t1, -1);

                t5.child = new Jump2(t6, -1);
                t5.rollbackTo = new Jump2(t1, -1);

                t6.child = new Jump2(t6, -1);
                t6.child_slash = new Jump2(t7, -1);
                t6.rollbackTo = new Jump2(t1, -1);

                t7.child = new Jump2(t7, -1);
                t7.rollbackTo = new Jump2(t1, -1);

                t6.accepted = true;
                t7.accepted = true;

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

            private class Node2_az : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z') return child;
                    if(c >= 'A' && c <= 'Z') return child;
                    return null;
                }

                public Jump2 child;
            }

            private class Node2_2 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z')
                        return child_az09;
                    if(c >= 'A' && c <= 'Z')
                        return child_az09;
                    if(c >= '0' && c <= '9')
                        return child_az09;
                    if(c == ':')
                        return child_semicolon;
                    return null;
                }

                public Jump2 child_az09;
                public Jump2 child_semicolon;
            }

            private class Node2_3 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z')
                        return child_az;
                    if(c >= 'A' && c <= 'Z')
                        return child_az;
                    if(c == '/')
                        return child_slash;
                    return null;
                }

                public Jump2 child_slash;
                public Jump2 child_az;
            }

            private class Node2_5 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 256)
                        return null;
                    return table[c] ? child : null;
                }

                private static readonly bool[] table = BuildCharSet();

                public Jump2 child;
            }

            private class Node2_6 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c == '/') return child_slash;
                    if(c >= 256)
                        return null;
                    return table[c] ? child : null;
                }

                private static readonly bool[] table = BuildCharSet();

                public Jump2 child;
                public Jump2 child_slash;
            }
        }
    }
}