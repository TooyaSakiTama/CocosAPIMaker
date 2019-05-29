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
            return sb.Insert(sb.FindLineIndex(line + 1), cursor);
        }
        public static StringBuilder AddCursorIndex(this StringBuilder sb,int index)
        {
            return sb.Insert(index, cursor);
        }
        public static StringBuilder MoveCursor(this StringBuilder sb,int line)
        {
            return sb.RemoveCursor().AddCursor(line);
        }
        private static int FindCursorIndex(this StringBuilder sb)
        {
            int index = sb.ToString().IndexOf(cursor);
            if (index == -1)
            {
                index = 0;
            }
            return index;
        }
        private static int FindCursorLine(this StringBuilder sb)
        {
            return sb.FindIndexLine(sb.FindCursorIndex());
        }
        public static int GetLineLength(this StringBuilder sb, int line)
        {
            int lineIndex = sb.FindLineIndex(line);
            int afterLineIndex = sb.FindLineIndex(line + 1);
            int length = afterLineIndex - lineIndex;
            return length;
        }
        public static int GetLinesCount(this StringBuilder sb)
        {
            int line = 0;
            if (string.IsNullOrEmpty(sb.ToString()))
            {
                return line;
            }
            line = sb.ToString().Split(Environment.NewLine).Length - 1;
            return line;
        }
        public static StringBuilder MoveCursorToLineStart(this StringBuilder sb)
        {
            int cursorLine = sb.FindIndexLine(sb.FindCursorIndex());
            int index = sb.FindLineIndex(cursorLine);
            sb.RemoveCursor().AddCursorIndex(index);
            return sb;
        }
        public static StringBuilder MoveCursorToLineEnd(this StringBuilder sb)
        {
            int cursorLine = sb.FindCursorLine();
            return sb.RemoveCursor().AddCursor(cursorLine);
        }
        /// <summary>
        /// 获取StringBuilder的字符串,这个方法可以避免装箱操作,但是光标会被删除,如果不想删除光标可以直接使用ToString方法
        /// </summary>
        /// <returns></returns>
        public static string GetString(this StringBuilder sb)
        {
            sb.RemoveCursor();
            return sb.ToString();
        }

        public static StringBuilder NewLine(this StringBuilder sb)
        {
            int line = sb.FindCursorLine();
            int index = sb.FindCursorIndex();
            return sb.RemoveCursor().Insert(index, Environment.NewLine).AddCursor(line + 1);
        }
        public static StringBuilder NewLine(this StringBuilder sb, int line)
        {
            return sb.RemoveCursor().Insert(sb.FindLineIndex(line), Environment.NewLine).AddCursor(line + 1);
        }
        public static StringBuilder NewLine(this StringBuilder sb,string value)
        {
            int line = sb.FindCursorLine();
            int index = sb.FindCursorIndex();
            return sb.RemoveCursor().Insert(index, $"{Environment.NewLine}{value}").AddCursor(line + 1);
        }
        public static StringBuilder NewLine(this StringBuilder sb, int line,string value)
        {
            sb.RemoveCursor().Insert(sb.FindLineIndex(line), $"{Environment.NewLine}{value}").AddCursor(line + 1);
            return sb;
        }
        public static StringBuilder Add(this StringBuilder sb, string value)
        {
            int line = sb.FindCursorLine();
            int index = sb.FindCursorIndex();
            return sb.RemoveCursor().Insert(index,value).AddCursor(line + 1);
        }
        public static StringBuilder Add(this StringBuilder sb,int line, string value)
        {
            int index = sb.FindCursorIndex();
            sb.RemoveCursor().Insert(index, value).AddCursor(line);
            return sb;
        }
        public static StringBuilder Remove(this StringBuilder sb,string value)
        {
            int startIndex = sb.ToString().IndexOf(value);
            if (startIndex == -1)
            {
                return sb;
            }
            return sb.Remove(startIndex, value.Length);
        }
        public static int FindLine(this StringBuilder sb,string s)
        {
            return sb.FindIndexLine(sb.ToString().IndexOf(s));
        }
        private static int FindIndexLine(this StringBuilder sb, int index)
        {
            int line = 0;
            int count = 0;
            CharEnumerator sce = sb.ToString().GetEnumerator();
            while (sce.MoveNext())
            {
                if (count == index)
                {
                    break;
                }
                if (sce.Current.ToString() == "\r")
                {
                    line++;
                }
                count++;
            }
            return line;
        }
        private static int FindLineIndex(this StringBuilder sb, int line)
        {
            int index = 0;
            int count = 0;
            CharEnumerator sce = sb.ToString().GetEnumerator();
            while (sce.MoveNext())
            {
                if (sce.Current.ToString() == "\r")
                {
                    count++;
                    if (count == line)
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
