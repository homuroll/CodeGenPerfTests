namespace Regex
{
    public class MatchResult
    {
        public override string ToString()
        {
            return length <= 0 ? "No match" : $"Start: {start}, length: {length}";
        }

        public int start;
        public int length;
    }
}