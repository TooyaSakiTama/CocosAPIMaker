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
            StringBuilder _classDoc = new StringBuilder();
            _classDoc.Add($"{_classStruct._ParentModule} = {_classStruct._ParentModule} or {{}}");
            _classDoc.NewLine($"---@class {_classStruct._Class} : {_classStruct._InheritedClass}");
            _classDoc.NewLine($"local {_classStruct._Class} = {{}}");
            _classDoc.NewLine($"{_classStruct._ParentModule}.{_classStruct._Class} = {_classStruct._Class}");
            _classDoc.NewLine();
            _classDoc.NewLine($"return {_classStruct._Class}");
            Console.WriteLine(_classDoc.ToString().GetLinesCount());
            Console.WriteLine(_classDoc);
            _classDoc.Add()
            Console.WriteLine()
            return _classDoc.ToString();
        }
    }
}
