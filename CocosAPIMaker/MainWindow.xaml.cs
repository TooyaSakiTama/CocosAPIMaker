using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static CocosAPIMaker.TransClass;
using MessageBox = System.Windows.MessageBox;

namespace CocosAPIMaker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private List<FileInfo> files = new List<FileInfo>();
        private string inputDir = string.Empty;
        private string outputDir = string.Empty;
        private readonly TaskScheduler _syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        public static bool transing = false;
        private void SelectInputDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fbw = new FolderBrowserDialog();
            var result = fbw.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            var path = fbw.SelectedPath.Trim();
            SetLogAsync("当前选择输入路径:" + path + "\n");
            inputDir = path;
            SearchAllFile();
        }
        public void SetLog(string log)
        {
            Task.Factory.StartNew(() => {
                Log.Text += log;
                LogScrollView.ScrollToBottom();
            }, new CancellationTokenSource().Token, TaskCreationOptions.None, _syncContextTaskScheduler).Wait();
        }
        private void SetLogAsync(string log)
        {
            Task.Factory.StartNew(() => {
                Log.Text += log;
                LogScrollView.ScrollToBottom();
            }, new CancellationTokenSource().Token, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).Wait();
        }

        private void SelectOutPutDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fbw = new FolderBrowserDialog();
            var result = fbw.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            var path = fbw.SelectedPath.Trim();
            SetLogAsync("当前选择输出路径:" + path + "\n");
            outputDir = path;
        }
        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (inputDir == string.Empty)
            {
                MessageBox.Show("请选择输入目录");
                return;
            }
            if (outputDir == string.Empty)
            {
                MessageBox.Show("请选择输出目录");
                return;
            }
            if (transing)
            {
                MessageBox.Show("转换已经开始");
                return;
            }
            transing = true;
            foreach (var item in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(item.Name);
                if (!fileName.Contains("auto_api"))
                {

                    string emmyLuaDoc = await TransAsync(item.FullName);
                    WriteToFile wtf = new WriteToFile();
                    wtf.log = SetLog;
                    if (files.IndexOf(item) == (files.Count - 1))
                    {
                        await wtf.StartAsync(outputDir, Path.GetFileNameWithoutExtension(item.Name), emmyLuaDoc, true);
                    }
                    else
                    {
                        await wtf.StartAsync(outputDir, Path.GetFileNameWithoutExtension(item.Name), emmyLuaDoc);
                    }

                }
            }
        }
        private void SearchAllFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(inputDir);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            SetLogAsync("当前扫描到的文件");
            foreach (var item in fileInfos)
            {
                SetLogAsync(item.FullName + "\n");
                files.Add(item);
                Console.WriteLine(item.FullName);
            }
        }
        private async Task<string> TransAsync(string filePath)
        {

            byte[] stream;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                stream = new byte[fileStream.Length];
                await fileStream.ReadAsync(stream, 0, (int)fileStream.Length);
            };
            string luaStr = Encoding.ASCII.GetString(stream);
            ClassStruct cs = new TransClass().Start(luaStr);
            string emmyLuaDoc = new TransDoc().Start(cs);
            return emmyLuaDoc;
        }
    }
}
