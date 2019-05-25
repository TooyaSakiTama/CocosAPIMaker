using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocosAPIMaker
{
    class TransDoc
    {
        private string newLine = "\n";
        public string Trans(TransClass.ClassStruct _classStruct)
        {
            string doc = "";
            CreateClassDoc(_classStruct);
            return doc;
        }
        private string CreateClassDoc(TransClass.ClassStruct _classStruct)
        {
            string s = "";
            StringBuilder _classDoc = new StringBuilder();
            _classDoc.Append($"{_classStruct._ParentModule} = {_classStruct._ParentModule} or {{}}");
            _classDoc.AppendLine($"---@class {_classStruct._Class} : {_classStruct._InheritedClass}");
            _classDoc.AppendLine($"local {_classStruct._Class} = {{}}");
            _classDoc.AppendLine($"{_classStruct._ParentModule}.{_classStruct._Class} = {_classStruct._Class}");
            _classDoc.AppendLine(StringBuilderEx.cursor);
            _classDoc.Append($"return {_classStruct._Class}");
            Console.WriteLine(_classDoc.ToString().GetLinesCount());
            Console.WriteLine(_classDoc);
            return _classDoc.ToString();
        }
    }
}
