using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CocosAPIMaker
{
    class Win32Native
    {
        [DllImport("dbhelp", SetLastError = true)]
        public static extern IntPtr ImageRvatoVa(IntPtr NtHeaders, IntPtr Base, uint Rva, int LastRavSection);
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile, int lpFileMappingAttributes, int flProtect, uint dwMaximumSizeHeigh, uint dwMaxImumSizeLow, string lpName);
         
    }
}
