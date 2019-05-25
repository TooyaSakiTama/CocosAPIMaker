using System.IO;

namespace CocosAPIMaker
{
    class WriteToFile
    {
        public bool Start(TransClass.ClassStruct classStruct)
        {

            using (FileStream file = File.Open(classStruct._Class + ".lua", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                //byte[] cstr =
                //file.WriteAsync()
            }

            return false;
        }
    }
}
