using System;
using System.Reflection;
using AD.BASE;
using AD.Utility.Pipe;

namespace AD.Utility
{
    public static class ReflectionExtension
    {
        public static bool CreateInstance<T>(this Assembly assembly, string fullName, out T obj)
        {
            obj = (T)assembly.CreateInstance(fullName);
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
            string[] callingName = callingStr.Split('.');
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
                        ? GetCurrentTargetWhenCallFunc(self, i, callingName[i], current)
                        : GetCurrentTargetWhenGetField(self, callingName[i], current);
                    next.Init(currentTarget.GetType(), currentTarget, callingName[i]);
                }
                //TypeResult resultCammand = currentStack[^1];
            }
            catch (Exception ex)
            {
                return new() { result = false, ex = ex, typeResults = currentStack };
            }
            return new() { typeResults = currentStack };
        } 

        private static object[] GetCurrentArgsWhenNeedArgs<T>(T self, int i, string currentCallingName, int a_s, int b_s)
        {
            object[] currentArgs;
            string[] currentArgsStrs = currentCallingName[a_s..b_s].Split(',');
            currentArgs = new object[currentArgsStrs.Length];
            for (int j = 0, e2 = currentArgsStrs.Length; j < e2; j++)
            {
                if (!self.FullAutoRun(out currentArgs[i], currentCallingName[a_s..b_s]).result)
                    throw new ADException("Parse Error : ResultValue");
            }
            return currentArgs;
        }

        private static object GetCurrentTargetWhenCallFunc<T>(T self, int i, string currentCallingName, TypeResult current)
        {
            object currentTarget;
            object[] currentArgs = new object[0];
            int a_s = currentCallingName.IndexOf('(') + 1, b_s = currentCallingName.LastIndexOf(")");
            if (b_s - a_s > 1)
            {
                currentArgs = GetCurrentArgsWhenNeedArgs(self, i, currentCallingName, a_s, b_s);
            }
            string ccn = currentCallingName[..(a_s - 1)];
            MethodBase method =
                current.target.GetType().GetMethod(ccn, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static) 
                ?? throw new ADException("Parse Error : Method");
            currentTarget = method.Invoke(current.target, currentArgs);
            return currentTarget;
        }

        private static object GetCurrentTargetWhenGetField<T>(T self, string currentCallingName, TypeResult current)
        {
            object currentTarget;
            FieldInfo data =
                current.GetType().GetField(currentCallingName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static) 
                ?? throw new ADException("Parse Error : Field");
            currentTarget = data.GetValue(self);
            return currentTarget;
        }

        public static FullAutoRunResultInfo FullAutoRun<T>(this T self, out object result, string callingStr)
        {
            string[] callingName = callingStr.Split('.');
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
                        ? GetCurrentTargetWhenCallFunc(self, i, callingName[i], current)
                        : GetCurrentTargetWhenGetField(self, callingName[i], current);
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
    }
}
