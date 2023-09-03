using System;
using System.Reflection;
using AD.BASE;
using AD.Utility.Pipe;

namespace AD.Utility
{
    public static class ReflectionExtension
    {
        public static readonly BindingFlags DefaultBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        public static bool CreateInstance(this Assembly assembly, string fullName, out object obj)
        {
            obj = assembly.CreateInstance(fullName);
            return obj != null;
        }

        public static bool CreatePipeLineStep<T, P>(this object self, string methodName, out PipeFunc pipeFunc)
        {
            pipeFunc = null;
            MethodInfo method_info = self.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (method_info == null) return false;
            pipeFunc = new((obj) => method_info.Invoke(self, new object[] { obj }), typeof(T), typeof(P));
            return true;
        }

        public static bool CreatePipeLineStep<T, P>(this Assembly assembly, string fullName, out PipeFunc pipeFunc)
        {
            pipeFunc = null;
            string objName = fullName[..fullName.LastIndexOf('.')], methodName = fullName[fullName.LastIndexOf('.')..];
            var a = assembly.CreateInstance(objName);
            if (a == null) return false;
            return CreatePipeLineStep<T, P>(a, methodName, out pipeFunc);
        }

        public static bool Run(this Assembly assembly, string typeName, string detecter, string targetFuncName)
        {
            var objs = UnityEngine.Object.FindObjectsOfType(assembly.GetType(typeName));
            string objName = detecter[..detecter.LastIndexOf('.')], methodName = detecter[detecter.LastIndexOf('.')..];
            var a = assembly.CreateInstance(objName);
            if (a == null) return false;
            a.GetType().GetMethod("DetecterInit")?.Invoke(a, new object[] { });
            var detecterFunc = a.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (detecterFunc == null) return false;
            foreach (var obj in objs)
            {
                if (obj == null) continue;
                if ((bool)detecterFunc.Invoke(a, new object[] { obj }))
                {
                    var targetFunc = obj.GetType().GetMethod(targetFuncName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    if (targetFunc == null) return false;
                    targetFunc.Invoke(obj, new object[] { });
                    return true;
                }
            }
            return false;
        }

        public static bool Run(this Assembly assembly, string typeName, object detecter, string detecterFuncName, string targetFuncName)
        {
            var objs = UnityEngine.Object.FindObjectsOfType(assembly.GetType(typeName));
            if (detecter == null) return false;
            detecter.GetType().GetMethod("DetecterInit")?.Invoke(detecter, new object[] { });
            var detecterFunc = detecter.GetType().GetMethod(detecterFuncName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (detecterFunc == null) return false;
            foreach (var obj in objs)
            {
                if (obj == null) continue;
                if ((bool)detecterFunc.Invoke(detecter, new object[] { obj }))
                {
                    var targetFunc = obj.GetType().GetMethod(targetFuncName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    if (targetFunc == null) return false;
                    targetFunc.Invoke(obj, new object[] { });
                    return true;
                }
            }
            return false;
        }

        public class TypeResult
        {
            public Type type;
            public object target;
            public string CallingName;

            public void Init(Type type, object target, string CallingName = "@")
            {
                this.type = type;
                this.target = target;
                this.CallingName = CallingName;
            }
        }

        public class FullAutoRunResultInfo
        {
            public bool result = true;
            public Exception ex = null;
            public TypeResult[] typeResults = null;
        }

        public static FullAutoRunResultInfo FullAutoRun<T>(this T self, string callingStr)
        {
            string[] callingName = callingStr.Split("->");
            TypeResult[] currentStack = new TypeResult[callingName.Length + 1];
            for (int i = 0,e= callingName.Length + 1; i < e; i++)
            {
                currentStack[i] = new();
            }
            try
            { 
                currentStack[0].Init(self.GetType(), self); 
                for (int i = 0, e = callingName.Length; i < e; i++)
                {
                    TypeResult current = currentStack[i], next = currentStack[i + 1];
                    object currentTarget = callingName[i].Contains('(')
                        ? GetCurrentTargetWhenCallFunc(current.target, callingName[i], current)
                        : GetCurrentTargetWhenGetField(callingName[i], current);
                    next.Init(currentTarget?.GetType(), currentTarget, callingName[i]);
                }
                //TypeResult resultCammand = currentStack[^1];
            }
            catch (Exception ex)
            {
                return new() { result = false, ex = ex, typeResults = currentStack };
            }
            return new() { typeResults = currentStack };
        } 

        private static object[] GetCurrentArgsWhenNeedArgs<T>(T self, string currentCallingName)
        {
            object[] currentArgs;
            string[] currentArgsStrs = currentCallingName.Split(',');
            currentArgs = new object[currentArgsStrs.Length];
            for (int j = 0, e2 = currentArgsStrs.Length; j < e2; j++)
            {
                if (currentArgsStrs[j][0] == '\"' && currentArgsStrs[j][^1] == '\"')
                    currentArgs[j] = currentArgsStrs[j][1..^1];
                else if (currentArgsStrs[j][0] == '$')
                    currentArgs[j] = float.Parse(currentArgsStrs[j][1..]);
                else if (!self.FullAutoRun(out currentArgs[j], currentArgsStrs[j]).result)
                    throw new ADException("Parse Error : ResultValue");
            }
            return currentArgs;
        }

        private static object GetCurrentTargetWhenCallFunc<T>(T self, string currentCallingName, TypeResult current)
        {
            object currentTarget;
            object[] currentArgs = new object[0];
            int a_s = currentCallingName.IndexOf('(') + 1, b_s = currentCallingName.LastIndexOf(")");
            if (b_s - a_s > 1)
            {
                currentArgs = GetCurrentArgsWhenNeedArgs(self, currentCallingName[a_s..b_s]);
            }
            MethodBase method =
                current.target.GetType().GetMethod(currentCallingName[..(a_s - 1)], DefaultBindingFlags) 
                ?? throw new ADException("Parse Error : Method");
            currentTarget = method.Invoke(current.target, currentArgs);
            return currentTarget;
        }

        private static object GetCurrentTargetWhenGetField(string currentCallingName, TypeResult current)
        {
            object currentTarget;
            FieldInfo data =
                current.target.GetType().GetField(currentCallingName, DefaultBindingFlags) 
                ?? throw new ADException("Parse Error : Field");
            currentTarget = data.GetValue(current.target);
            return currentTarget;
        }

        public static FullAutoRunResultInfo FullAutoRun<T>(this T self, out object result, string callingStr)
        {
            string[] callingName = callingStr.Split("->");
            TypeResult[] currentStack = new TypeResult[callingName.Length + 1];
            for (int i = 0, e = callingName.Length + 1; i < e; i++)
            {
                currentStack[i] = new();
            }
            try
            {
                currentStack[0].Init(self.GetType(), self);
                for (int i = 0, e = callingName.Length; i < e; i++)
                {
                    TypeResult current = currentStack[i], next = currentStack[i + 1];
                    object currentTarget = callingName[i].Contains('(')
                        ? GetCurrentTargetWhenCallFunc(current.target, callingName[i], current)
                        : GetCurrentTargetWhenGetField(callingName[i], current);
                    next.Init(currentTarget.GetType(), currentTarget, callingName[i]);
                }
                TypeResult resultCammand = currentStack[^1];
                result = resultCammand.target;
            }
            catch (Exception ex)
            {
                result = null;
                return new() { result = false, ex = ex, typeResults = currentStack };
            }
            return new() { typeResults = currentStack };
        }

        public static Type ToType(this string self)
        {
            return Assembly.GetExecutingAssembly().GetType(self);
        }

        public static Type ToType(this string self, Assembly assembly)
        {
            return assembly.GetType(self);
        }

        public static Type Typen(string typeName, string singleTypeName = null)
        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(singleTypeName ?? typeName))
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }

        public static Type Typen(this Assembly self, string typeName, string singleTypeName = null)
        {
            Type type = self.GetType(typeName);
            if (type != null)
            {
                return type;
            }
            Type[] typeArray = self.GetTypes();
            int typeArrayLength = typeArray.Length;
            for (int j = 0; j < typeArrayLength; ++j)
            {
                if (typeArray[j].Name.Equals(singleTypeName ?? typeName))
                {
                    return typeArray[j];
                }
            }
            return type;
        }
    }
}
