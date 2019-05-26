using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class StringEx
    {
        public static int GetLinesCount(this string s)
        {
            int line = 0;
            if (string.IsNullOrEmpty(s))
            {
                return line;
            }
            line = s.Split(Environment.NewLine).Length;
            return line;
        }

        public static string[] Split(this string s,string septator)
        {
            return s.Split(new string[] { septator },StringSplitOptions.None);

        }
        public static StringBuilder NewLine(this string s)
        {
            return new StringBuilder(s).NewLine();
        }
        public static StringBuilder NewLine(this string s,string value)
        {
            return new StringBuilder(s).NewLine(value);
        }
        public static StringBuilder NewLine(this string s, int line)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.NewLine(line);
            return sb;
        }
        public static StringBuilder NewLine(this string s, int line,string value)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.NewLine(line, value);
            return sb;
        }
        public static StringBuilder Add(this string s,string value)
        {
            return new StringBuilder(s).Add(value);
        }
        
        public static StringBuilder Add(this string s, int line, string value)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Insert(s.FindLine(line), value);
            sb.Add(line, value);
            return sb;
        }
        
        
        private static int FindLine(this string s, int byIndex)
        {
            int index = 0;
            int count = 0;
            CharEnumerator sce = s.GetEnumerator();
            while (sce.MoveNext())
            {
                if (sce.Current.ToString() == Environment.NewLine)
                {
                    count++;
                    if (count == byIndex)
                    {
                        break;
                    }
                }
                index++;
            }
            return index;
        }
    }
    public static class StringBuilderEx
    {
        public static string cursor = "[cursor]";
        public static StringBuilder NewLine(this StringBuilder sb)
        {
            return sb.Remove(cursor).AppendLine(cursor);
        }
        public static StringBuilder RemoveCursor(this StringBuilder sb)
        {
            return sb.Remove(cursor);
        }
        public static StringBuilder AddCursor(this StringBuilder sb)
        {
            return sb.Append(cursor);
        }
        public static StringBuilder AddCursor(this StringBuilder sb,int line)
        {
            return sb.Insert(sb.FindLine(line + 1) - 1, cursor);
        }
        public static StringBuilder NewLine(this StringBuilder sb,string value)
        {

            return sb.RemoveCursor().AppendLine(value).AddCursor();
        }
        public static StringBuilder NewLine(this StringBuilder sb,int line)
        {
            return sb.RemoveCursor().Insert(sb.FindLine(line), Environment.NewLine).AddCursor(line);
        }
        public static StringBuilder NewLine(this StringBuilder sb, int line,string value)
        {
            sb.RemoveCursor().Insert(sb.FindLine(line), $"{Environment.NewLine}{value}").AddCursor(line);
            return sb;
        }
        public static StringBuilder Add(this StringBuilder sb, string value)
        {
            return sb.RemoveCursor().Append(value).AddCursor();
        }
        public static StringBuilder Add(this StringBuilder sb,int line, string value)
        {
            sb.RemoveCursor().Insert(sb.FindLine(line), value).AddCursor(line);
            return sb;
        }
        public static StringBuilder Remove(this StringBuilder sb,string value)
        {
            Console.WriteLine(sb.ToString().IndexOf(value));
            int startIndex = sb.ToString().IndexOf(value);
            if (startIndex == -1)
            {
                return sb;
            }
            return sb.Remove(startIndex, value.Length);
        }
        public static int FindLine(this StringBuilder sb,string s)
        {
            int line = 0;
            
            return line;
        }
        private static int FindLine(this StringBuilder sb, int byIndex)
        {
            int index = 0;
            int count = 0;
            CharEnumerator sce = sb.ToString().GetEnumerator();
            while (sce.MoveNext())
            {
                if (sce.Current.ToString() == Environment.NewLine)
                {
                    count++;
                    if (count == byIndex)
                    {
                        break;
                    }
                }
                index++;
            }
            return index;
        }
    }
}
