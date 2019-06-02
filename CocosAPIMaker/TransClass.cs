using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosAPIMaker
{
    public class TransClass
    {
        /// <summary>
        /// 类结构体
        /// </summary>
        public struct ClassStruct
        {
            public string _Class;
            public string _ParentModule;
            public string _InheritedClass;
            public List<FunctionStruct> _Functions;
        }
        /// <summary>
        /// 方法结构体
        /// </summary>
        public struct FunctionStruct
        {
            public string _Function;
            public string _Doc;
            public List<ParamStruct> _Params;
            public ReturnStruct _Return;
        }
        /// <summary>
        /// 返回值结构体
        /// </summary>
        public struct ReturnStruct
        {
            public string _ReturnType;
            public string _ReturnDoc;
        }
        /// <summary>
        /// 参数结构体
        /// </summary>
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
        public static string paramKey = "@param";
        public static string returnKey = "@return";
        public static string functionDocKey = "functionDoc";
        public static string paramDocKey = "param";
        public static string returnDocKey = "return";
        public static string[] enterKey = { "\n" };
        public static string[] spaseKey = { " " };
        public static string[] ignoreKeys = { "self" };

        public static ClassStruct _classStruct;
        /// <summary>
        /// 开始遍历所有的注释条目
        /// </summary>
        /// <param name="luaStr"></param>
        public ClassStruct Start(string luaStr)
        {
            luaStr = new StringBuilder(luaStr).Remove("return nil").GetString();
            string[] docs = SplitAllDoc(luaStr);
            _classStruct = new ClassStruct();
            foreach (var item in docs)
            {
                AnalyticalDoc(item);
            }
            return _classStruct;
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
        /// 解析数据并把数据添加到List中
        /// </summary>
        /// <param name="index">位置下标</param>
        /// <param name="data"></param>
        /// <param name="itemList"></param>
        /// <returns></returns>
        private int DismantlingData(int index, string[] data, List<Dictionary<string, string>> itemList)
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
            try
            {
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                tempDic.Add(stringTemp, data[index]);
                itemList.Add(tempDic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
            string[] itemTemp = item.Split(spaseKey, StringSplitOptions.RemoveEmptyEntries);
            List<Dictionary<string, string>> itemList = new List<Dictionary<string, string>>();
            for (int index = 0; index < itemTemp.Length; index++)
            {
                if (itemTemp[index].Contains("--"))
                {
                    if (itemTemp[index + 1].Contains("@"))
                    {
                        index = DismantlingData(index, itemTemp, itemList);
                    }
                    else if (itemTemp[index + 1].Contains(paramDocKey))
                    {
                        index = DismantlingData(index, itemTemp, itemList);
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
                        Dictionary<string, string> tempDic = new Dictionary<string, string>();
                        tempDic.Add(stringTemp, functionDocKey);
                        itemList.Add(tempDic);
                        index = j - 1;
                    }
                }
            }
            if (itemList.Count > 0)
            {
                // 这是一个类
                if (ListDicContainsValue(itemList, classKey))
                {
                    AnalyticalClass(itemList);
                }
                // 这是一个方法
                else if (ListDicContainsValue(itemList, functionKey))
                {
                    AnalyticalFunction(itemList);
                }
            }
            itemList.Clear();
        }
        bool ListDicContainsValue(List<Dictionary<string, string>> itemList, string value)
        {
            foreach (var item in itemList)
            {
                if (item.ContainsValue(value))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解析一个类的注释
        /// </summary>
        /// <param name="docs"></param>
        void AnalyticalClass(List<Dictionary<string, string>> docs)
        {
            foreach (var item in docs)
            {

                if (item.ContainsValue(classKey))
                {
                    _classStruct._Class = item.Keys.First();
                }
                else if (item.ContainsValue(inheritedClassKey))
                {
                    _classStruct._InheritedClass = item.Keys.First();
                }
                else if (item.ContainsValue(parentClassKey))
                {
                    _classStruct._ParentModule = item.Keys.First();
                }
            }
        }
        /// <summary>
        /// 解析一个方法的注释,并把方法信息装箱到_ClassStruct
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        void AnalyticalFunction(List<Dictionary<string, string>> docs)
        {
            FunctionStruct fs = new FunctionStruct();
            foreach (var item in docs)
            {
                //这是一个方法
                if (item.ContainsValue(functionKey))
                {
                    string[] line = item.Keys.First().Split(spaseKey, StringSplitOptions.None);
                    string functionName = line[1];
                    string functionClass = line[0].Split(new string[] { "=#" }, StringSplitOptions.None)[1].Replace("]", "");
                    if (functionClass == _classStruct._Class)
                    {
                        fs._Function = functionName;
                    }
                }
                else if (item.ContainsValue(functionDocKey))
                {
                    fs._Doc += item.Keys.First();
                }
                //这是一个参数
                else if (item.ContainsValue(paramKey))
                {
                    string key = item.Keys.First();
                    if (!IsIgnoreKey(key))
                    {
                        int index = key.IndexOf(" ");
                        if (index == -1)
                        {
                            index = key.Length;
                        }
                        string[] keyData = key.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < keyData.Length - 1; i++)
                        {
                            sb.Add(keyData[i]).Add(" ");
                        }

                        string paramType = sb.GetString().Trim(' ').Trim('#');
                        string paramName = keyData[keyData.Length - 1].Trim(' ').Trim('#'); //key.Substring(index, key.Length - index).Trim(' ');
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
                else if (item.ContainsValue(paramDocKey))
                {
                    string key = item.Keys.First();
                    int index = key.IndexOf(" ");
                    if (index == -1)
                    {
                        index = key.Length;
                    }
                    string paramName = key.Substring(0, index).Trim(' ');
                    string paramDoc = key.Substring(index, key.Length - index).Trim(' ');
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
                else if (item.ContainsValue(returnKey))
                {
                    fs._Return._ReturnType = item.Keys.First().Split(new string[] { "#" }, StringSplitOptions.None)[0];
                }
            }
            if (_classStruct._Functions == null)
            {
                _classStruct._Functions = new List<FunctionStruct>();
            }
            _classStruct._Functions.Add(fs);
        }
        /// <summary>
        /// 需要忽略的key值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
