using System;
using System.Text;

namespace CocosAPIMaker
{
    class TransDoc
    {
        private string className = string.Empty;
        public string Start(TransClass.ClassStruct _classStruct)
        {
            className = _classStruct._Class;
            StringBuilder sb = new StringBuilder(CreateClassDoc(_classStruct));
            foreach (var function in _classStruct._Functions)
            {
                sb.Add(CreateFunctionDoc(function));
                sb.MoveCursor(sb.GetLinesCount() - 1);
            }
            return sb.GetString();
        }
        private string CreateFunctionDoc(TransClass.FunctionStruct _functionStruct)
        {
            StringBuilder _functionDoc = new StringBuilder();
            if (_functionStruct._Doc != null)
            {
                _functionDoc.NewLine($"---{_functionStruct._Doc}");
            }
            StringBuilder paramsStr = new StringBuilder(string.Empty);
            if (_functionStruct._Params != null)
            {
                foreach (var param in _functionStruct._Params)
                {
                    _functionDoc.NewLine(CreateParamDoc(param));
                    
                    if (string.IsNullOrEmpty(paramsStr.ToString()))
                    {
                        paramsStr.Append(param._Param);
                    }
                    else
                    {
                        paramsStr.Append(",").Append(param._Param);
                    }
                }

            }
            _functionDoc.NewLine(CreateReturnDoc(_functionStruct._Return));
            _functionDoc.NewLine($"function {className}:{_functionStruct._Function}({paramsStr}) end");
            _functionDoc.NewLine();
            return _functionDoc.GetString();
        }
        private string CreateParamDoc(TransClass.ParamStruct _paramStruct)
        {
            StringBuilder _paramDoc = new StringBuilder();
            _paramDoc.Add($"---@param {_paramStruct._Param} {_paramStruct._Type} {_paramStruct._Doc}");
            return _paramDoc.GetString();
        }
        private string CreateReturnDoc(TransClass.ReturnStruct _returnStruct)
        {
            StringBuilder _returnDoc = new StringBuilder();
            _returnDoc.Add($"---@return {_returnStruct._ReturnType} {_returnStruct._ReturnDoc}");
            return _returnDoc.GetString();
        }
        private string CreateClassDoc(TransClass.ClassStruct _classStruct)
        {
            StringBuilder _classDoc = new StringBuilder();
            _classDoc.Add($"{_classStruct._ParentModule} = {_classStruct._ParentModule} or {{}}");
            _classDoc.NewLine($"---@class {_classStruct._Class} : {_classStruct._InheritedClass}");
            _classDoc.NewLine($"local {_classStruct._Class} = {{}}");
            _classDoc.NewLine($"{_classStruct._ParentModule}.{_classStruct._Class} = {_classStruct._Class}");
            _classDoc.NewLine($"return {_classStruct._Class}");
            int line = _classDoc.FindLine($"{_classStruct._ParentModule}.{_classStruct._Class} = {_classStruct._Class}");
            _classDoc.MoveCursor(line);
            _classDoc.NewLine();
            return _classDoc.ToString();
        }
    }
}
