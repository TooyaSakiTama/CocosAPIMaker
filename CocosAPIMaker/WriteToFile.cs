using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CocosAPIMaker
{
    class WriteToFile
    {
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
                            if (isEnd)
                            {
                                MessageBox.Show("全部转换已经完成");
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
