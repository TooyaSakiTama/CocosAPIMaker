using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        private void SelectInputDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fbw = new FolderBrowserDialog();
            var result = fbw.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            var path = fbw.SelectedPath.Trim();
            Log.Text = "当前选择路径:" + path + "\n";
            inputDir = path;
            SearchAllFile();
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
            Log.Text = "当前选择路径:" + path + "\n";
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
            List<Task> taskList = new List<Task>();
            foreach (var item in files)
            {
                //Action action = new Action(async () =>
                //{

                //});
                //Task task = new Task(action);
                //taskList.Add(task);
                string emmyLuaDoc = await TransAsync(item.FullName);
                await new WriteToFile().StartAsync(outputDir, Path.GetFileNameWithoutExtension(item.Name), emmyLuaDoc);
            }
            await Task.Run(() =>
             {
                 int index = 0;
                 Task task = taskList[index];
                 while (true)
                 {
                     if (index == 0 && task.Status == TaskStatus.Created)
                     {
                         task.Start();
                     }
                     else if (task.Status == TaskStatus.RanToCompletion)
                     {
                         index++;
                         task = taskList[index];
                         task.Start();
                     }
                 }
             });
        }
        private void SearchAllFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(inputDir);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (var item in fileInfos)
            {
                Log.Text += item.FullName + "\n";
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
