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
            return luaStr.Split(splitKey, StringSplitOptions.None);
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
                foreach (var item1 in itemTemp)
                {
                    ///这是一个类
                    if (item1 == classKey[0])
                    {
                        AnalyticalClass( temp);
                        break;
                    }
                    ///这是一个方法
                    else if (item1 == functionKey[0])
                    {
                        AnalyticalFunction( temp);
                        break;
                    }
                }

            }
        }
        /// <summary>
        /// 解析一个类的注释,并把类信息装箱到传入的_ClassStruct
        /// </summary>
        /// <param name="_ClassStruct"></param>
        /// <param name="docs"></param>
        void AnalyticalClass( string[] docs)
        {
            foreach (var item in docs)
            {
                string[] itemTemp = item.Split(spaseKey, StringSplitOptions.None);
                for (int i = 0; i < itemTemp.Length; i++)
                {
                    if (itemTemp[i] == classKey[0])
                    {
                        _classStruct._Class = itemTemp[i + 1];
                        break;
                    }
                    else if (itemTemp[i] == inheritedClassKey[0])
                    {
                        _classStruct._InheritedClass = itemTemp[i + 1];
                        break;
                    }
                    else if (itemTemp[i] == parentClassKey[0])
                    {
                        _classStruct._ParentModule = itemTemp[i + 1];
                        break;
                    }

                }
            }
        }
        /// <summary>
        /// 解析一个方法的注释,并把方法信息装箱到传入的_ClassStruct
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        void AnalyticalFunction( string[] docs)
        {
            FunctionStruct fs = new FunctionStruct();
            foreach (var item in docs)
            {
                string[] itemTemp = item.Split(spaseKey, StringSplitOptions.None);
                for (int i = 0; i < itemTemp.Length; i++)
                {
                    ///这是一个方法
                    if (itemTemp[i] == functionKey[0])
                    {
                        fs._Function = itemTemp[3];
                        break;
                    }
                    else if (itemTemp[i] == functionDocKey[0])
                    {
                        //fs._Doc = "";
                    }
                    ///这是一个参数
                    else if (itemTemp[i] == paramKey[0])
                    {
                        if (!IsIgnoreKey(itemTemp[i+1]))
                        {
                            ParamStruct ps = new ParamStruct();
                            ps._Type = itemTemp[i + 1].Remove(0, 1);
                            ps._Param = itemTemp[i + 2];
                            if (fs._Params == null)
                            {
                                fs._Params = new List<ParamStruct>();
                            }
                            fs._Params.Add(ps);

                        }
                        break;
                    }
                    //这是一个参数的注释
                    else if (itemTemp[i] == paramDocKey[0])
                    {
                        ParamStruct ps;
                        if (fs._Params == null)
                        {
                            fs._Params = new List<ParamStruct>();
                        }
                        if (fs._Params.Exists(x => x._Param == itemTemp[i + 1]))
                        {
                            ps = fs._Params.Find(x => x._Param == itemTemp[i + 1]);
                        }
                        else
                        {
                            ps = new ParamStruct();
                            fs._Params.Add(ps);
                        }
                        ps._Doc = itemTemp[i + 2];
                        break;
                    }
                    //这是一个返回值
                    else if (itemTemp[i] == returnKey[0])
                    {
                        fs._Return._ReturnType = itemTemp[i + 1].Split(new string[] { "#" }, StringSplitOptions.None)[0];
                    }
                    else if (itemTemp[i] == returnDocKey[0])
                    {

                    }
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
