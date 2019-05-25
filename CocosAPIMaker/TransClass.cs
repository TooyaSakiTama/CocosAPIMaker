using System;
using System.Collections.Generic;

namespace CocosAPIMaker
{
    public class TransClass
    {
        public struct ClassStruct
        {
            public string _Class;
            public string _ParentModule;
            public string _InheritedClass;
            public List<FunctionStruct> _Functions;
        }
        public struct FunctionStruct
        {
            public string _Function;
            public string _Doc;
            public List<ParamStruct> _Params;
            public ReturnStruct _Return;
        }
        public struct ReturnStruct
        {
            public string _ReturnType;
            public string _ReturnDoc;
        }
        public struct ParamStruct
        {
            public string _Param;
            public string _Type;
            public string _Doc;
        }

        public static string[] splitKey = { "--------------------------------" };
        public static string classKey = "@module";
        public static string inheritedClassKey = "@extend";
        public static string parentClassKey = "@parent_module";
        public static string functionKey = "@function";
        public static string functionDocKey = "functionDoc";
        public static string paramKey = "@param";
        public static string paramDocKey = "param";
        public static string returnKey = "@return";
        public static string returnDocKey = "return";
        public static string[] enterKey = { "\n" };
        public static string[] spaseKey = { " " };
        public static string[] ignoreKeys = { "self" };
        public static ClassStruct _classStruct;
        /// <summary>
        /// 开始遍历所有的注释条目
        /// </summary>
        /// <param name="luaStr"></param>
        public void TransStrat(string luaStr)
        {
            string[] docs = SplitAllDoc(luaStr);
            _classStruct = new ClassStruct();
            foreach (var item in docs)
            {
                AnalyticalDoc(item);
            }

            TransDoc td = new TransDoc();
            td.Trans(_classStruct);

        }

        /// <summary>
        /// 格式化所有的注释条目
        /// </summary>
        /// <param name="luaStr"></param>
        /// <returns></returns>
        string[] SplitAllDoc(string luaStr)
        {
            string[] temp = luaStr.Split(splitKey, StringSplitOptions.None);
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i].Replace("\r", " ");
                temp[i] = temp[i].Replace("\n", " ");
            }

            return temp;
        }
        private int DismantlingData(int index, string[] data, Dictionary<string, string> itemDictionary)
        {
            index = index + 1;
            string stringTemp = "";
            int j;
            for (j = index + 1; j < data.Length; j++)
            {
                if (data[j].Contains("--"))
                {
                    break;
                }
                else
                {
                    stringTemp += data[j] + " ";
                }
            }
            stringTemp = stringTemp.Trim(' ');
            itemDictionary.Add(stringTemp, data[index]);
            index = j - 1;
            return index;
        }
        /// <summary>
        /// 解析一条注释
        /// </summary>
        /// <param name="_ClassStruct"></param>
        /// <param name="doc"></param>
        public void AnalyticalDoc(string item)
        {
            string[] itemTemp = item.Split(spaseKey, StringSplitOptions.None);
            Dictionary<string, string> itemDictionary = new Dictionary<string, string>();
            for (int index = 0; index < itemTemp.Length; index++)
            {
                if (itemTemp[index].Contains("--"))
                {
                    if (itemTemp[index + 1].Contains("@"))
                    {
                        index = DismantlingData(index, itemTemp, itemDictionary);
                    }
                    else if (itemTemp[index + 1].Contains(paramDocKey))
                    {
                        index = DismantlingData(index, itemTemp, itemDictionary);
                    }
                    else if (!(itemTemp[index + 1] == ""))
                    {
                        index = index + 1;
                        string stringTemp = "";
                        int j;
                        for (j = index; j < itemTemp.Length; j++)
                        {
                            if (itemTemp[j].Contains("--"))
                            {
                                break;
                            }
                            else
                            {
                                stringTemp += itemTemp[j] + " ";
                            }

                        }
                        stringTemp = stringTemp.Trim(' ');
                        itemDictionary.Add(stringTemp, functionDocKey);
                        index = j - 1;
                    }
                }
            }
            ///这是一个类
            if (itemDictionary.ContainsValue(classKey))
            {
                AnalyticalClass(itemDictionary);
            }
            ///这是一个方法
            else if (itemDictionary.ContainsValue(functionKey))
            {
                AnalyticalFunction(itemDictionary);
            }

        }
        /// <summary>
        /// 解析一个类的注释
        /// </summary>
        /// <param name="docs"></param>
        void AnalyticalClass(Dictionary<string, string> docs)
        {
            foreach (var item in docs)
            {
                if (item.Value == classKey)
                {
                    _classStruct._Class = item.Key;
                }
                else if (item.Value == inheritedClassKey)
                {
                    _classStruct._InheritedClass = item.Key;
                }
                else if (item.Value == parentClassKey)
                {
                    _classStruct._ParentModule = item.Key;
                }
            }
        }
        /// <summary>
        /// 解析一个方法的注释,并把方法信息装箱到_ClassStruct
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        void AnalyticalFunction(Dictionary<string, string> docs)
        {
            FunctionStruct fs = new FunctionStruct();
            foreach (var item in docs)
            {
                //这是一个方法
                if (item.Value == functionKey)
                {
                    string[] line = item.Key.Split(spaseKey, StringSplitOptions.None);
                    string functionName = line[1];
                    string functionClass = line[0].Split(new string[] { "=#" }, StringSplitOptions.None)[1].Replace("]", "");
                    if (functionClass == _classStruct._Class)
                    {
                        fs._Function = functionName;
                    }
                }
                else if (item.Value == functionDocKey)
                {
                    fs._Doc = item.Key;
                }
                //这是一个参数
                else if (item.Value == paramKey)
                {
                    if (!IsIgnoreKey(item.Key))
                    {
                        int index = item.Key.IndexOf(" ");
                        string paramType = item.Key.Substring(0, index).Trim(' ').Trim('#');
                        string paramName = item.Key.Substring(index, item.Key.Length - index).Trim(' ');
                        ParamStruct ps;
                        if (fs._Params == null)
                        {
                            fs._Params = new List<ParamStruct>();
                        }
                        if (fs._Params.Exists(x =>
                        x._Param == paramName))
                        {
                            ps = fs._Params.Find(x => x._Param == paramName);
                            fs._Params.Remove(ps);
                        }
                        else
                        {
                            ps = new ParamStruct();
                            ps._Param = paramName;
                        }
                        ps._Type = paramType;
                        fs._Params.Add(ps);
                    }
                }
                //这是一个参数的注释
                else if (item.Value == paramDocKey)
                {
                    int index = item.Key.IndexOf(" ");
                    string paramName = item.Key.Substring(0, index).Trim(' ');
                    string paramDoc = item.Key.Substring(index, item.Key.Length - index).Trim(' ');
                    ParamStruct ps;
                    if (fs._Params == null)
                    {
                        fs._Params = new List<ParamStruct>();
                    }
                    if (fs._Params.Exists(x =>
                    x._Param == paramName))
                    {
                        ps = fs._Params.Find(x => x._Param == paramName);
                        fs._Params.Remove(ps);
                    }
                    else
                    {
                        ps = new ParamStruct();
                        ps._Param = paramName;
                    }
                    ps._Doc = paramDoc;
                    fs._Params.Add(ps);
                }
                //这是一个返回值
                else if (item.Value == returnKey)
                {
                    fs._Return._ReturnType = item.Key.Split(new string[] { "#" }, StringSplitOptions.None)[0];
                }
            }
            if (_classStruct._Functions == null)
            {
                _classStruct._Functions = new List<FunctionStruct>();
            }
            _classStruct._Functions.Add(fs);
        }
        bool IsIgnoreKey(string key)
        {
            foreach (var item in ignoreKeys)
            {
                if (item == key)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
