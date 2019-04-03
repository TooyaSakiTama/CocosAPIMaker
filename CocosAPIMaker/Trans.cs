using System;
using System.Collections.Generic;

namespace CocosAPIMaker
{
    public class Trans
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
        public static string[] classKey = { "@module" };
        public static string[] inheritedClassKey = { "@extend" };
        public static string[] parentClassKey = { "@parent_module" };
        public static string[] functionKey = { "@function" };
        public static string[] functionDocKey = { "--" };
        public static string[] paramKey = { "@param" };
        public static string[] paramDocKey = { "param" };
        public static string[] returnKey = { "@return" };
        public static string[] returnDocKey = { "return" };
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

            Console.WriteLine(_classStruct);
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
        /// <summary>
        /// 解析一条注释
        /// </summary>
        /// <param name="_ClassStruct"></param>
        /// <param name="doc"></param>
        public void AnalyticalDoc(string doc)
        {
            string[] temp = doc.Split(Trans.enterKey, StringSplitOptions.None);
            foreach (var item in temp)
            {
                string[] itemTemp = item.Split(spaseKey, StringSplitOptions.None);
                Dictionary<string, string> itemDictionary = new Dictionary<string, string>();
                for (int i = 0; i < itemTemp.Length; i++)
                {
                    if (itemTemp[i].Contains("--"))
                    {
                        if (itemTemp[i+1].Contains("@"))
                        {
                            i = i + 1;
                            string stringTemp = "";

                            for (int j = i + 1; j < itemTemp.Length; j++)
                            {
                                if (itemTemp[j].Contains("--"))
                                {
                                    stringTemp = stringTemp.Trim(' ');
                                    itemDictionary.Add(itemTemp[i], stringTemp);
                                    i = j;
                                    break;
                                }
                                else
                                {
                                    stringTemp += itemTemp[j] + " ";
                                }
                            }
                        }
                        else if (itemTemp[i + 1].Contains("param"))
                        {
                            i = i + 1;

                        }
                        else
                        {
                            i = i + 1;
                        }
                    }
                }
                string s = "";
                ///这是一个类
                if (itemDictionary.TryGetValue(classKey[0], out s))
                {
                    AnalyticalClass(itemDictionary);
                }
                ///这是一个方法
                else if (itemDictionary.TryGetValue(functionKey[0], out s))
                {
                    AnalyticalFunction(itemDictionary);
                }

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
                if (item.Key == classKey[0])
                {
                    _classStruct._Class = item.Value;
                }
                else if (item.Key == inheritedClassKey[0])
                {
                    _classStruct._InheritedClass = item.Value;
                }
                else if (item.Key == parentClassKey[0])
                {
                    _classStruct._ParentModule = item.Value;
                }
            }
        }
        /// <summary>
        /// 解析一个方法的注释,并把方法信息装箱到传入的_ClassStruct
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        void AnalyticalFunction(Dictionary<string, string> docs)
        {
            FunctionStruct fs = new FunctionStruct();
            foreach (var item in docs)
            {
                ///这是一个方法
                if (item.Key == functionKey[0])
                {
                    string[] line = item.Value.Split(spaseKey,StringSplitOptions.None);
                    string functionName = line[1];
                    string functionClass = line[0].Split(new string[] { "=#" }, StringSplitOptions.None)[1].Replace("]","");
                    if (functionClass == _classStruct._Class)
                    {
                        fs._Function = functionName;
                    }
                }
                else if (item.Key == functionDocKey[0])
                {
                    //fs._Doc = "";
                }
                ///这是一个参数
                else if (item.Key == paramKey[0])
                {
                    if (!IsIgnoreKey(item.Value))
                    {
                        ParamStruct ps;
                        if (fs._Params == null)
                        {
                            fs._Params = new List<ParamStruct>();
                        }
                        if (fs._Params.Exists(x =>
                        x._Param == item.Value))
                        {
                            ps = fs._Params.Find(x => x._Param == item.Value);
                        }
                        else
                        {
                            ps = new ParamStruct();
                            fs._Params.Add(ps);
                        }
                        ps._Type = item.Value;
                        ps._Param = item.Value;
                    }
                }
                //这是一个参数的注释
                else if (item.Key == paramDocKey[0])
                {
                    ParamStruct ps;
                    if (fs._Params == null)
                    {
                        fs._Params = new List<ParamStruct>();
                    }
                    if (fs._Params.Exists(x =>
                    x._Param == item.Value))
                    {
                        ps = fs._Params.Find(x => x._Param == item.Value);
                    }
                    else
                    {
                        ps = new ParamStruct();
                        ps._Param = item.Value;
                        fs._Params.Add(ps);
                    }
                    ps._Doc += item.Value;
                }
                //这是一个返回值
                else if (item.Key == returnKey[0])
                {
                    fs._Return._ReturnType = item.Value;
                }
                else if (item.Key == returnDocKey[0])
                {

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
