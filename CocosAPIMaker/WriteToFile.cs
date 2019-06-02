using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CocosAPIMaker
{
    class WriteToFile
    {
        public delegate void Log(string str);
        public Log log;
        /// <summary>
        /// 把转换出来的注解字符串写入到文件内
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <param name="name">文件名</param>
        /// <param name="emmyLuaDoc">EmmyLua使用的注解字符串</param>
        /// <param name="isEnd">是否是最后一个写入任务</param>
        /// <returns></returns>
        public async Task<bool> StartAsync(string path,string name,string emmyLuaDoc, bool isEnd = false)
        {
            using (FileStream file = File.Open(path + Path.DirectorySeparatorChar + name + ".lua", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                byte[] buffer = new UTF8Encoding(true).GetBytes(emmyLuaDoc);
                Task t = file.WriteAsync(buffer, 0, buffer.Length);
                return await Task.Run(() =>
                {
                    while (true)
                    {
                        if (t.IsCompleted)
                        {
                            file.Close();
                            Console.WriteLine(name + "写入完成");
                            log(name + "写入完成\n");
                            if (isEnd)
                            {
                                MessageBox.Show("全部转换已经完成");
                                MainWindow.transing = false;
                            }
                            break;
                        };
                    }
                    return true;
                });
            }
        }
    }
}
