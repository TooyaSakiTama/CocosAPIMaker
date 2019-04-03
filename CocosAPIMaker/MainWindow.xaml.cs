using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

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

        private void SelectDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fbw = new FolderBrowserDialog();
            var result = fbw.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            var path = fbw.SelectedPath.Trim();
            Log.Text = "当前选择路径:" + path + "\n";
            //this.SearchAllFile(path);
            this.TransAsync("C:/Users/Tooya/Desktop/api/AbstractCheckButton.lua");
        }
        private void SearchAllFile(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (var item in fileInfos)
            {
                Log.Text += item.FullName+"\n";
                Console.WriteLine(item.FullName);
            }
        }
        private async Task<bool> TransAsync(string filePath)
        {
            byte[] stream;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                stream = new byte[fileStream.Length];
                await fileStream.ReadAsync(stream,0,(int)fileStream.Length);
            };
            string luaStr = Encoding.ASCII.GetString(stream);
            Trans trans = new Trans();
            trans.TransStrat(luaStr);
            return true;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
