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
            return new StringBuilder(s).AppendLine();
        }
        public static StringBuilder NewLine(this string s,string value)
        {
            return new StringBuilder(s).AppendLine(value);
        }
        public static StringBuilder NewLine(this string s, int line)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Insert(s.FindLine(line), Environment.NewLine);
            return sb;
        }
        public static StringBuilder NewLine(this string s, int line,string value)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Insert(s.FindLine(line), $"{Environment.NewLine}{value}");
            return sb;
        }
        public static StringBuilder Add(this string s,string value)
        {
            return new StringBuilder(s).Append(value);
        }
        
        public static StringBuilder Add(this string s,string value,int line)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Insert(s.FindLine(line), value);
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
        public static StringBuilder NewLine(this StringBuilder sb,string value)
        {

            return sb.Remove(cursor).AppendLine(value).Append(cursor);
        }
        public static StringBuilder NewLine(this StringBuilder sb,int line)
        {
            return sb.Remove(cursor).Insert(sb.FindLine(line), $"{Environment.NewLine}{cursor}");
        }
        public static StringBuilder NewLine(this StringBuilder sb, int line,string value)
        {
            sb.Insert(sb.FindLine(line), $"{Environment.NewLine}{value}");
            return sb;
        }
        public static StringBuilder Add(this StringBuilder sb, string value)
        {
            return sb.Append(value);
        }
        public static StringBuilder Add(this StringBuilder sb,int line, string value)
        {
            sb.Insert(sb.FindLine(line), value);
            return sb;
        }
        public static StringBuilder Remove(this StringBuilder sb,string value)
        {
            return sb.Remove(sb.ToString().IndexOf(value),value.Length);
        }
        private static int FindLine(this StringBuilder s, int byIndex)
        {
            int index = 0;
            int count = 0;
            CharEnumerator sce = s.ToString().GetEnumerator();
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
