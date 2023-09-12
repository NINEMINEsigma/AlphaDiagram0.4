using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AD.BASE
{
    #region AD_I

    /*
     * ADInstance
     * Base
     * Low-level public implementation
     */

    public interface IBase
    {
        void ToMap(out IBaseMap BM);
        bool FromMap(IBaseMap from);
    }

    public interface IBase<T> : IBase where T : class, IBaseMap, new()
    {
        void ToMap(out T BM);
        bool FromMap(T from);
    }

    public abstract class BaseMonoClass : MonoBehaviour, IBase
    {
        #region attribute

        protected bool AD__IsCollected { get; set; } = false;
        private List<string> AD__InjoinedGroup { get; set; } = new List<string>(); 

        public bool IsCollected
        {
            get { return AD__IsCollected; }
            protected set
            {
                if (value != AD__IsCollected) foreach (var name in AD__InjoinedGroup)
                        if (value) CollectionMechanism.Collect(name, this);
                        else CollectionMechanism.Erase(name, this);
                AD__IsCollected = value;
            }
        }

        #endregion

        #region Basefunction

        public BaseMonoClass() { }

        public BaseMonoClass(List<string> group_name)
        {
            foreach (var name in group_name) CollectionMechanism.Collect(name, this);
            AD__InjoinedGroup = group_name;
            AD__IsCollected = true;
        }

        ~BaseMonoClass()
        {
            IsCollected = false;
        }

        #endregion 

        public abstract void ToMap(out IBaseMap BM);
        public abstract bool FromMap(IBaseMap from);

    }

    public abstract class BaseMonoClass<T> : MonoBehaviour, IBase<T> where T : class, IBaseMap, new()
    {
        #region attribute

        private bool AD__IsCollected { get; set; } = false;
        private List<string> AD__InjoinedGroup { get; set; } = new List<string>(); 

        public bool IsCollected
        {
            get { return AD__IsCollected; }
            protected set
            {
                if (value != AD__IsCollected) foreach (var name in AD__InjoinedGroup)
                        if (value) CollectionMechanism.Collect(name, this);
                        else CollectionMechanism.Erase(name, this);
                AD__IsCollected = value;
            }
        }

        #endregion

        #region Basefunction

        public BaseMonoClass() { }

        public BaseMonoClass(List<string> group_name)
        {
            foreach (var name in group_name) CollectionMechanism.Collect(name, this);
            AD__InjoinedGroup = group_name;
            AD__IsCollected = true;
        }

        ~BaseMonoClass()
        {
            IsCollected = false;
        }

        #endregion

        public abstract void ToMap(out T BM);
        public abstract bool FromMap(T from);
        public virtual void ToMap(out IBaseMap BM)
        {
            ToMap(out T bm);
            BM = bm;
        }
        public abstract bool FromMap(IBaseMap from);
    }

    public interface IBaseMap
    {
        void ToObject(out IBase obj);
        bool FromObject(IBase from);
        string Serialize();
        bool Deserialize(string source);
    }

    public interface IBaseMap<T> : IBaseMap where T : class, IBase, new()
    {
        void ToObject(out T obj);
        bool FromObject(T from); 
    }

    public static class CollectionMechanism
    {
        static public Dictionary<string, List<IBase>> ADCollectTable { get; set; }

        static public List<IBase> GetList(string key)
        {
            ADCollectTable.TryGetValue(key, out var cat);
            return cat;
        }

        static public void Collect(string key, IBase target)
        {
            ADCollectTable.TryGetValue(key, out var value_list);
            if (value_list != null) value_list.Add(target);
            else ADCollectTable[key].Add(target);
        }

        static public void Erase(string key, IBase target)
        {
            ADCollectTable.TryGetValue(key, out var value_list);
            value_list?.Remove(target);
        }


    }

    #endregion

    #region AD_S

    [Serializable]
    public class ADException : Exception, IBaseMap
    {
        public ADException() { AD__GeneratedTime = DateTime.Now; }
        public ADException(string message) : base(message) { AD__GeneratedTime = DateTime.Now; }
        public ADException(string message, Exception inner) : base(message, inner) { AD__GeneratedTime = DateTime.Now; }
        public ADException(Exception inner) : base("Unknow error", inner) { AD__GeneratedTime = DateTime.Now; }
        protected ADException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { AD__GeneratedTime = DateTime.Now; }

        public bool Deserialize(string source)
        {
            throw new ADException("Can not deserialize to an ADException");
        }

        public bool FromObject(IBase from)
        {
            throw new ADException("Can not create an ADException from IBase object");
        }

        public string Serialize()
        {
            return "[" + AD__GeneratedTime.ToString() + "]:" + Message;
        }
        public string SerializeStackTrace()
        {
            return "[" + AD__GeneratedTime.ToString() + "]:" + StackTrace;
        }
        public string SerializeSource()
        {
            return "[" + AD__GeneratedTime.ToString() + "]:" + Source;
        }
        public string SerializeMessage()
        {
            return "[" + AD__GeneratedTime.ToString() + "]:" + Message;
        }
        public string SerializeHelpLink()
        {
            return "[" + AD__GeneratedTime.ToString() + "]:" + HelpLink;
        }

        public IBase ToObject()
        {
            throw new ADException("Can not use an ADException to create IBase object");
        }

        public void ToObject(out IBase obj)
        {
            throw new ADException("Can not use an ADException to create IBase object");
        }

        private DateTime AD__GeneratedTime;
    }

    public interface IAnyArchitecture
    {

    }
    public interface ICanInitialize: IAnyArchitecture
    {
        void Init();
    }
    public interface ICanGetArchitecture: IAnyArchitecture
    {
        IADArchitecture ADInstance();
        void SetArchitecture(IADArchitecture target);
    }
    public interface ICanUnRegister : ICanGetArchitecture
    {
        void UnRegist<T>() where T : new();
    }
    public interface ICanGetSystem : ICanGetArchitecture
    {
        T GetSystem<T>() where T : class, IADSystem, new();
    }
    public interface ICanGetModel : ICanGetArchitecture
    {
        T GetModel<T>() where T : class, IADModel, new();
    }
    public interface ICanGetController : ICanGetArchitecture
    {
        T GetController<T>() where T : class, IADController, new();
    }
    public interface ICanSendCommand : ICanGetArchitecture
    {
        void SendCommand<T>() where T : class, IADCommand, new();
    }
    public interface ICanGetEvent : ICanGetArchitecture
    {
        T GetEvent<T>() where T : class, IADEvent, new();
    }
    public interface ICanSendEvent : ICanGetEvent
    {
        void SendEvent<T>() where T : class, IADEvent, new();
    }
    public interface ICanSimulateFunction
    {
        void Execute();
        void Execute(params object[] args);
    }

    public interface IADArchitecture
    {
        static IADArchitecture instence { get; }
        void Init();
        IADArchitecture AddMessage(string message);
        _Model GetModel<_Model>() where _Model : class, IADModel, new();
        _System GetSystem<_System>() where _System : class, IADSystem, new();
        _Controller GetController<_Controller>() where _Controller : class, IADController, new();
        _Event GetEvent<_Event>() where _Event : class, IADEvent, new();
        IADArchitecture RegisterModel<_Model>(_Model model) where _Model : IADModel, new();
        IADArchitecture RegisterSystem<_System>(_System system) where _System : IADSystem, new();
        IADArchitecture RegisterController<_Controller>(_Controller controller) where _Controller : IADController, new();
        IADArchitecture RegisterEvent<_Event>(_Event _event) where _Event : IADEvent, new();
        IADArchitecture RegisterCommand<_Command>(_Command command) where _Command : IADCommand, new();
        IADArchitecture RegisterModel<_Model>() where _Model : IADModel, new();
        IADArchitecture RegisterSystem<_System>() where _System : IADSystem, new();
        IADArchitecture RegisterController<_Controller>() where _Controller : IADController, new();
        IADArchitecture RegisterCommand<_Command>() where _Command : IADCommand, new();
        IADArchitecture RegisterEvent<_Event>() where _Event : IADEvent, new();
        IADArchitecture SendEvent<_Event>() where _Event : class, IADEvent, new();
        IADArchitecture SendCommand<_Command>() where _Command : class, IADCommand, new();
        IADArchitecture UnRegister<_T>() where _T : new();
        bool Contains<_Type>();
        public IADArchitecture SendImmediatelyCommand<_Command>() where _Command : class, IADCommand, new();
        public IADArchitecture SendImmediatelyCommand<_Command>(_Command command) where _Command : class, IADCommand, new();
    }

    public interface IADModel : ICanInitialize, ICanGetArchitecture
    {
        abstract void Save(string path);
        abstract IADModel Load(string path);
    }

    public abstract class ADModel : IADModel
    {
        public IADArchitecture Architecture { get; protected set; } = null;

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public abstract void Init();
        public abstract IADModel Load(string path);
        public abstract void Save(string path);

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        protected virtual void Output(string str)
        {

        }
    }

    public interface IADSystem : ICanInitialize, ICanSendCommand, ICanGetArchitecture, ICanGetController
    {
        void RegisterCommand<T>() where T : class, IADCommand, new();
        void UnRegisterCommand<T>() where T : class, IADCommand, new();
    }

    public abstract class ADSystem : IADSystem
    {
        public IADArchitecture Architecture { get; protected set; } = null;

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public abstract void Init();

        public void RegisterCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.RegisterCommand<T>();
        }
        public void UnRegisterCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.UnRegister<T>();
        }
        public void SendCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.SendCommand<T>();
        }

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public T GetController<T>() where T : class, IADController, new()
        {
            return Architecture.GetController<T>();
        }
    }

    public abstract class MonoSystem : MonoBehaviour, IADSystem
    {
        public IADArchitecture Architecture { get; protected set; } = null;

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public abstract void Init();

        public void RegisterCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.RegisterCommand<T>();
        }
        public void UnRegisterCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.UnRegister<T>();
        }
        public void SendCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.SendCommand<T>();
        }

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public T GetController<T>() where T : class, IADController, new()
        {
            return Architecture.GetController<T>();
        }

        public MonoSystem RegisterController<_Controller>(_Controller controller) where _Controller : IADController, new()
        {
            Architecture.RegisterController(controller);
            return this;
        }

    }

    public interface IADController : ICanInitialize, ICanGetArchitecture, ICanSendCommand, ICanSendEvent, ICanGetSystem, ICanGetModel
    {
        void RegisterCommand<T>() where T : class, IADCommand, new();
        void RegisterModel<T>() where T : class, IADModel, new();
        void RegisterCommand<T>(T _Command) where T : class, IADCommand, new();
        void RegisterModel<T>(T _Model) where T : class, IADModel, new();
    }

    public abstract class ADController : MonoBehaviour, IADController
    {
        public IADArchitecture Architecture { get; set; } = null;
        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public abstract void Init();

        public virtual void RegisterCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.RegisterCommand<T>();
        }

        public virtual void SendCommand<T>() where T : class, IADCommand, new()
        {
            Architecture.SendCommand<T>();
        }

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public T GetEvent<T>() where T : class, IADEvent, new()
        {
            return Architecture.GetEvent<T>();
        }

        public void SendEvent<T>() where T : class, IADEvent, new()
        {
            Architecture.SendEvent<T>();
        }

        public T GetSystem<T>() where T : class, IADSystem, new()
        {
            return Architecture.GetSystem<T>();
        }

        public void RegisterModel<T>() where T : class, IADModel, new()
        {
            Architecture.RegisterModel<T>();
        }

        public T GetModel<T>() where T : class, IADModel, new()
        {
            return Architecture.GetModel<T>();
        }

        public void RegisterCommand<T>(T _Command) where T : class, IADCommand, new()
        {
            Architecture.RegisterCommand<T>(_Command);
        }

        public void RegisterModel<T>(T _Model) where T : class, IADModel, new()
        {
            Architecture.RegisterModel<T>(_Model);
        }
    }

    public interface IADEvent : ICanGetArchitecture
    {
        void Trigger();
    }

    public interface IADCommand : ICanGetArchitecture, ICanGetModel,ICanGetController
    {
        void Execute();
        string LogMessage();
    }

    public abstract class ADCommand : IADCommand
    {
        public IADArchitecture Architecture { get; protected set; } = null;

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public void Execute()
        {
            if (Architecture == null) throw new ADException("Can not execute a command without setting architecture");
            Architecture.AddMessage(LogMessage());
            OnExecute();
        }

        public virtual string LogMessage()
        {
            return this.GetType().FullName;
        }

        public abstract void OnExecute();

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public T GetModel<T>() where T : class, IADModel, new()
        {
            return Architecture.GetModel<T>();
        }

        public T GetController<T>() where T : class, IADController, new()
        {
            return Architecture.GetController<T>();
        }
    }

    public class SimulatedFunction : IADCommand, ICanSimulateFunction
    {
        public IADArchitecture Architecture { get; protected set; } = null;
        private ADBaseInvokableCall OnExecute = null;
        protected string logMessage = "";

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public SimulatedFunction(ADBaseInvokableCall action, string message)
        {
            OnExecute = action;
            logMessage = message;
        }

        public SimulatedFunction(UnityAction action, string message)
        {
            OnExecute = new ADInvokableCall(action);
            logMessage = message;
        }

        public SimulatedFunction(UnityAction<object> action, string message)
        {
            OnExecute = new ADInvokableCall<object>(action);
            logMessage = message;
        }

        public SimulatedFunction(UnityAction<object, object> action, string message)
        {
            OnExecute = new ADInvokableCall<object, object>(action);
            logMessage = message;
        }

        public SimulatedFunction(UnityAction<object, object, object> action, string message)
        {
            OnExecute = new ADInvokableCall<object, object, object>(action);
            logMessage = message;
        }

        public SimulatedFunction(UnityAction<object, object, object, object> action, string message)
        {
            OnExecute = new ADInvokableCall<object, object, object, object>(action);
            logMessage = message;
        }

        public void Execute()
        {
            if (Architecture == null) throw new ADException("Can not execute a command without setting architecture");
            HowLog();
            OnExecute?.Invoke(new object[0]);
        }

        public void Execute(params object[] args)
        {
            if (Architecture == null) throw new ADException("Can not execute a command without setting architecture");
            HowLog();
            OnExecute?.Invoke(args);
        }

        protected virtual void HowLog()
        {
            Architecture.AddMessage(LogMessage());
        }

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public T GetModel<T>() where T : class, IADModel, new()
        {
            return Architecture.GetModel<T>();
        }

        public string LogMessage()
        {
            return logMessage;
        }

        public T GetController<T>() where T : class, IADController, new()
        {
            return Architecture.GetController<T>();
        }
    }

    public interface IADMessage
    {
        string What();
    }

    public class ADMessage : IADMessage
    {
        public string AD__Message = "null";

        public ADMessage() { }
        public ADMessage(string message) { AD__Message = "[" + DateTime.Now.ToString() + "] " + message; }

        public string What()
        {
            return AD__Message;
        }
    }

    public class ADMessageRecord : IADModel
    {
        private IADArchitecture Architecture = null;

        private List<IADMessage> AD__messages = new List<IADMessage>();

        public void Init()
        {
            AD__messages.Add(new ADMessage("Already generated"));
        }

        public string What()
        {
            string cat = "";
            foreach (var message in AD__messages) cat += message.What() + "\n";
            return cat;
        }

        public void Add(IADMessage message)
        {
            if (Count > MaxCount) AD__messages.RemoveAt(0);
            AD__messages.Add(message);
        }

        public void Remove(IADMessage message)
        {
            AD__messages.Remove(message);
        }

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
        }

        public virtual void Save(string path)
        {
            AD.BASE.FileC.TryCreateDirectroryOfFile(path); 
            string message = What();
            File.WriteAllText(path, message, Encoding.UTF8);
        }

        public virtual IADModel Load(string path)
        {
            throw new NotImplementedException();
        }

        public IADArchitecture ADInstance()
        {
            return Architecture;
        }

        public int Count { get { return AD__messages.Count; } }

        public int MaxCount = 100;
    }

    public abstract class ADArchitecture<T> : IADArchitecture where T : ADArchitecture<T>, new()
    {
        #region attribute

        private HashSet<object> AD__Objects = new HashSet<object>() { new ADMessageRecord() };
        private ADMessageRecord _m_AD__MessageRecord = null;
        private ADMessageRecord AD__MessageRecord
        {
            get
            {
                _m_AD__MessageRecord ??= GetModel<ADMessageRecord>();
                return _m_AD__MessageRecord;
            }
        }
        private static IADArchitecture __ADinstance = null;
        public static T instance
        {
            get
            {
                if (__ADinstance == null)
                {
                    __ADinstance = new T();
                    __ADinstance.Init();
                }
                return __ADinstance as T;
            }
        }

        protected ADMessageRecord MessageRecord { get { return AD__MessageRecord; } }

        #endregion

        #region basefunction 
        ~ADArchitecture()
        {
            Debug.Log(typeof(T).FullName + " is distory");
        }
        #endregion

        #region mFunction

        public static void Destory()
        {
            __ADinstance = null;
        }

        public virtual void SaveRecord()
        {
            AD__MessageRecord?.Save(Path.Combine(Application.persistentDataPath, "AD", this.GetType().Name, DateTime.Now.Ticks.ToString()) + ".AD.log");
        }

        public abstract IBaseMap ToMap();

        public abstract bool FromMap(IBaseMap from);

        public virtual void Init()
        {

        }

        private IADArchitecture Register<_T>(_T _object) where _T : new()
        {
            var key = typeof(T);
            AD__Objects.RemoveWhere(T => T == null || T.GetType().Equals(typeof(_T)));
            AD__Objects.Add(_object);
            AddMessage(_object.GetType().FullName + " is register");
            return instance;
        }

        private object _tempObject_Get = null;
        private object Get<_T>()
        {
            if (_tempObject_Get == null || !_tempObject_Get.GetType().Equals(typeof(_T)))
                _tempObject_Get = AD__Objects.FirstOrDefault(P => P.GetType().Equals(typeof(_T)));
            if (_tempObject_Get == null) Debug.LogWarning("Obtain null<" + typeof(_T).FullName + ">");
            return _tempObject_Get;
        }

        public IADArchitecture UnRegister<_T>() where _T : new()
        {
            AD__Objects.Remove(AD__Objects.FirstOrDefault(T => T.GetType().Equals(typeof(_T))));
            return instance;
        }

        public bool Contains<_Type>()
        {
            if (_tempObject_Get == null || !_tempObject_Get.GetType().Equals(typeof(_Type)))
                _tempObject_Get = AD__Objects.FirstOrDefault(P => P.GetType().Equals(typeof(_Type)));
            if (_tempObject_Get == null) return false;
            return true;
        }

        public _Model GetModel<_Model>() where _Model : class, IADModel, new()
        {
            return Get<_Model>() as _Model;
        }

        public _System GetSystem<_System>() where _System : class, IADSystem, new()
        {
            return Get<_System>() as _System;
        }

        public _Controller GetController<_Controller>() where _Controller : class, IADController, new()
        {
            return Get<_Controller>() as _Controller;
        }

        public _Event GetEvent<_Event>() where _Event : class, IADEvent, new()
        {
            return Get<_Event>() as _Event;
        }

        public IADArchitecture RegisterModel<_Model>(_Model model) where _Model : IADModel, new()
        {
            Register<_Model>(model);
            model.SetArchitecture(instance);
            model.Init();
            return instance;
        }

        public IADArchitecture RegisterSystem<_System>(_System system) where _System : IADSystem, new()
        {
            Register<_System>(system);
            system.SetArchitecture(instance);
            system.Init();
            return instance;
        }

        public IADArchitecture RegisterController<_Controller>(_Controller controller) where _Controller : IADController, new()
        {
            Register<_Controller>(controller);
            controller.SetArchitecture(instance);
            controller.Init();
            return instance;
        }

        public IADArchitecture RegisterEvent<_Event>(_Event _event) where _Event : IADEvent, new()
        {
            Register<_Event>(_event);
            _event.SetArchitecture(instance);
            return instance;
        }

        public IADArchitecture SendEvent<_Event>() where _Event : class, IADEvent, new()
        {
            (Get<_Event>() as _Event).Trigger();
            return instance;
        }

        public IADArchitecture RegisterModel<_Model>() where _Model : IADModel, new()
        {
            RegisterModel(new _Model());
            return instance;
        }

        public IADArchitecture RegisterSystem<_System>() where _System : IADSystem, new()
        {
            RegisterSystem(new _System());
            return instance;
        }

        public IADArchitecture RegisterController<_Controller>() where _Controller : IADController, new()
        {
            RegisterController(new _Controller());
            return instance;
        }

        public IADArchitecture RegisterEvent<_Event>() where _Event : IADEvent, new()
        {
            RegisterEvent(new _Event());
            return instance;
        }

        public IADArchitecture RegisterCommand<_Command>(_Command command) where _Command : IADCommand, new()
        {
            Register(command);
            command.SetArchitecture(instance);
            return instance;
        }

        public IADArchitecture RegisterCommand<_Command>() where _Command : IADCommand, new()
        {
            RegisterCommand(new _Command());
            return instance;
        }

        public virtual IADArchitecture AddMessage(string message)
        {
            AD__MessageRecord?.Add(new ADMessage(message));
            Debug.Log(message);
            return instance;
        }

        public IADArchitecture SendCommand<_Command>() where _Command : class, IADCommand, new()
        {
            try
            {
                (Get<_Command>() as _Command).Execute();
            }
            catch (Exception ex)
            {
                AddMessage("SendCommand\nErrer: " + ex.Message + "\nStackTrace:" + ex.StackTrace);
                Debug.LogError(ex);
            }
            return instance;
        }

        /// <summary>
        /// _Command use this function cannot depend on this Architecture
        /// <para> (because not SetArchitecture) </para>
        /// </summary>
        /// <typeparam name="_Command"></typeparam>
        /// <returns></returns>
        public IADArchitecture SendImmediatelyCommand<_Command>() where _Command : class, IADCommand, new()
        {
            SendImmediatelyCommand(new _Command());
            return instance;
        }

        /// <summary>
        /// _Command use this function cannot depend on this Architecture
        /// <para> (because not SetArchitecture) </para>
        /// </summary>
        /// <typeparam name="_Command"></typeparam>
        /// <returns></returns>
        public IADArchitecture SendImmediatelyCommand<_Command>(_Command command) where _Command : class, IADCommand, new()
        {
            RegisterCommand(command);
            command.Execute();
            return instance;
        }
        #endregion
    }

    public interface ISubPagesArchitecture : IEnumerable<IADArchitecture>
    {
        Dictionary<Type, IADArchitecture> SubArchitectures { get; }
        IADArchitecture this[Type type] { get; }
    }

    public abstract class TopArchitecture<T,_Entry,_MainPage,_EndPage,_SubPages> 
        : ADArchitecture<T> 
        where T : TopArchitecture<T, _Entry, _MainPage, _EndPage, _SubPages>, new()
        where _Entry : IADArchitecture
        where _MainPage : IADArchitecture
        where _EndPage : IADArchitecture
        where _SubPages : ISubPagesArchitecture
    {
        public abstract _Entry EntryArchitecture { get; }
        public abstract _MainPage MainArchitecture { get; }
        public abstract _EndPage EndArchitecture { get; }
        public abstract _SubPages SubArchitectures { get; }

        public IADArchitecture this[Type type]
        {
            get
            {
                if (type == typeof(_Entry)) return EntryArchitecture;
                else if (type == typeof(_MainPage)) return MainArchitecture;
                else if (type == typeof(_EndPage)) return EndArchitecture;
                else return SubArchitectures[type]; 
            }
        }
    }

    #endregion

    #region Event from Unity & ExtAD

    /*
    [Serializable]
    [UsedByNativeCode]
    public abstract class UnityEventBase : ISerializationCallbackReceiver
    {
        private InvokableCallList m_Calls;

        [SerializeField]
        [FormerlySerializedAs("m_PersistentListeners")]
        private PersistentCallGroup m_PersistentCalls;

        private bool m_CallsDirty = true;

        protected UnityEventBase()
        {
            m_Calls = new InvokableCallList();
            m_PersistentCalls = new PersistentCallGroup();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            DirtyPersistentCalls();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            DirtyPersistentCalls();
        }

        protected MethodInfo FindMethod_Impl(string name, object targetObj)
        {
            return FindMethod_Impl(name, targetObj.GetType());
        }

        protected abstract MethodInfo FindMethod_Impl(string name, Type targetObjType);

        internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);

        internal MethodInfo FindMethod(PersistentCall call)
        {
            Type argumentType = typeof(Object);
            if (!string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName))
            {
                argumentType = Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, throwOnError: false) ?? typeof(Object);
            }

            Type listenerType = ((call.target != null) ? call.target.GetType() : Type.GetType(call.targetAssemblyTypeName, throwOnError: false));
            return FindMethod(call.methodName, listenerType, call.mode, argumentType);
        }

        internal MethodInfo FindMethod(string name, Type listenerType, PersistentListenerMode mode, Type argumentType)
        {
            return mode switch
            {
                PersistentListenerMode.EventDefined => FindMethod_Impl(name, listenerType),
                PersistentListenerMode.Void => GetValidMethodInfo(listenerType, name, new Type[0]),
                PersistentListenerMode.Float => GetValidMethodInfo(listenerType, name, new Type[1] { typeof(float) }),
                PersistentListenerMode.Int => GetValidMethodInfo(listenerType, name, new Type[1] { typeof(int) }),
                PersistentListenerMode.Bool => GetValidMethodInfo(listenerType, name, new Type[1] { typeof(bool) }),
                PersistentListenerMode.String => GetValidMethodInfo(listenerType, name, new Type[1] { typeof(string) }),
                PersistentListenerMode.Object => GetValidMethodInfo(listenerType, name, new Type[1] { argumentType ?? typeof(Object) }),
                _ => null,
            };
        }

        //
        // 摘要:
        //     Get the number of registered persistent listeners.
        public int GetPersistentEventCount()
        {
            return m_PersistentCalls.Count;
        }

        //
        // 摘要:
        //     Get the target component of the listener at index index.
        //
        // 参数:
        //   index:
        //     Index of the listener to query.
        public Object GetPersistentTarget(int index)
        {
            return m_PersistentCalls.GetListener(index)?.target;
        }

        //
        // 摘要:
        //     Get the target method name of the listener at index index.
        //
        // 参数:
        //   index:
        //     Index of the listener to query.
        public string GetPersistentMethodName(int index)
        {
            PersistentCall listener = m_PersistentCalls.GetListener(index);
            return (listener != null) ? listener.methodName : string.Empty;
        }

        private void DirtyPersistentCalls()
        {
            m_Calls.ClearPersistent();
            m_CallsDirty = true;
        }

        private void RebuildPersistentCallsIfNeeded()
        {
            if (m_CallsDirty)
            {
                m_PersistentCalls.Initialize(m_Calls, this);
                m_CallsDirty = false;
            }
        }

        //
        // 摘要:
        //     Modify the execution state of a persistent listener.
        //
        // 参数:
        //   index:
        //     Index of the listener to query.
        //
        //   state:
        //     State to set.
        public void SetPersistentListenerState(int index, UnityEventCallState state)
        {
            PersistentCall listener = m_PersistentCalls.GetListener(index);
            if (listener != null)
            {
                listener.callState = state;
            }

            DirtyPersistentCalls();
        }

        //
        // 摘要:
        //     Returns the execution state of a persistent listener.
        //
        // 参数:
        //   index:
        //     Index of the listener to query.
        //
        // 返回结果:
        //     Execution state of the persistent listener.
        public UnityEventCallState GetPersistentListenerState(int index)
        {
            if (index < 0 || index > m_PersistentCalls.Count)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of range of the {GetPersistentEventCount()} persistent listeners.");
            }

            return m_PersistentCalls.GetListener(index).callState;
        }

        protected void AddListener(object targetObj, MethodInfo method)
        {
            m_Calls.AddListener(GetDelegate(targetObj, method));
        }

        internal void AddCall(BaseInvokableCall call)
        {
            m_Calls.AddListener(call);
        }

        protected void RemoveListener(object targetObj, MethodInfo method)
        {
            m_Calls.RemoveListener(targetObj, method);
        }

        //
        // 摘要:
        //     Remove all non-persisent (ie created from script) listeners from the event.
        public void RemoveAllListeners()
        {
            m_Calls.Clear();
        }

        internal List<BaseInvokableCall> PrepareInvoke()
        {
            RebuildPersistentCallsIfNeeded();
            return m_Calls.PrepareInvoke();
        }

        protected void Invoke(object[] parameters)
        {
            List<BaseInvokableCall> list = PrepareInvoke();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Invoke(parameters);
            }
        }

        public override string ToString()
        {
            return BASE.ToString() + " " + GetType().FullName;
        }

        //
        // 摘要:
        //     Given an object, function name, and a list of argument types; find the method
        //     that matches.
        //
        // 参数:
        //   obj:
        //     Object to search for the method.
        //
        //   functionName:
        //     Function name to search for.
        //
        //   argumentTypes:
        //     Argument types for the function.
        public static MethodInfo GetValidMethodInfo(object obj, string functionName, Type[] argumentTypes)
        {
            return GetValidMethodInfo(obj.GetType(), functionName, argumentTypes);
        }

        //
        // 摘要:
        //     Given an object type, function name, and a list of argument types; find the method
        //     that matches.
        //
        // 参数:
        //   objectType:
        //     Object type to search for the method.
        //
        //   functionName:
        //     Function name to search for.
        //
        //   argumentTypes:
        //     Argument types for the function.
        public static MethodInfo GetValidMethodInfo(Type objectType, string functionName, Type[] argumentTypes)
        {
            while ((object)objectType != typeof(object) && (object)objectType != null)
            {
                MethodInfo method = objectType.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, argumentTypes, null);
                if ((object)method != null)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    bool flag = true;
                    int num = 0;
                    ParameterInfo[] array = parameters;
                    foreach (ParameterInfo parameterInfo in array)
                    {
                        Type type = argumentTypes[num];
                        Type parameterType = parameterInfo.ParameterType;
                        flag = type.IsPrimitive == parameterType.IsPrimitive;
                        if (!flag)
                        {
                            break;
                        }

                        num++;
                    }

                    if (flag)
                    {
                        return method;
                    }
                }

                objectType = objectType.BaseType;
            }

            return null;
        }

        protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode)
        {
            return ValidateRegistration(method, targetObj, mode, typeof(Object));
        }

        protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode, Type argumentType)
        {
            if ((object)method == null)
            {
                throw new ArgumentNullException("method", UnityString.Format("Can not register null method on {0} for callback!", targetObj));
            }

            if ((object)method.DeclaringType == null)
            {
                throw new NullReferenceException(UnityString.Format("Method '{0}' declaring type is null, global methods are not supported", method.Name));
            }

            Type type;
            if (!method.IsStatic)
            {
                Object @object = targetObj as Object;
                if (@object == null || @object.GetInstanceID() == 0)
                {
                    throw new ArgumentException(UnityString.Format("Could not register callback {0} on {1}. The class {2} does not derive from UnityEngine.Object", method.Name, targetObj, (targetObj == null) ? "null" : targetObj.GetType().ToString()));
                }

                type = @object.GetType();
                if (!method.DeclaringType.IsAssignableFrom(type))
                {
                    throw new ArgumentException(UnityString.Format("Method '{0}' declaring type '{1}' is not assignable from object type '{2}'", method.Name, method.DeclaringType.Name, @object.GetType().Name));
                }
            }
            else
            {
                type = method.DeclaringType;
            }

            if ((object)FindMethod(method.Name, type, mode, argumentType) == null)
            {
                Debug.LogWarning(UnityString.Format("Could not register listener {0}.{1} on {2} the method could not be found.", targetObj, method, GetType()));
                return false;
            }

            return true;
        }

        internal void AddPersistentListener()
        {
            m_PersistentCalls.AddListener();
        }

        protected void RegisterPersistentListener(int index, object targetObj, MethodInfo method)
        {
            RegisterPersistentListener(index, targetObj, targetObj.GetType(), method);
        }

        protected void RegisterPersistentListener(int index, object targetObj, Type targetObjType, MethodInfo method)
        {
            if (ValidateRegistration(method, targetObj, PersistentListenerMode.EventDefined))
            {
                m_PersistentCalls.RegisterEventPersistentListener(index, targetObj as Object, targetObjType, method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void RemovePersistentListener(Object target, MethodInfo method)
        {
            if ((object)method != null)
            {
                m_PersistentCalls.RemoveListeners(target, method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void RemovePersistentListener(int index)
        {
            m_PersistentCalls.RemoveListener(index);
            DirtyPersistentCalls();
        }

        internal void UnregisterPersistentListener(int index)
        {
            m_PersistentCalls.UnregisterPersistentListener(index);
            DirtyPersistentCalls();
        }

        internal void AddVoidPersistentListener(UnityAction call)
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterVoidPersistentListener(persistentEventCount, call);
        }

        internal void RegisterVoidPersistentListener(int index, UnityAction call)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Void))
            {
                m_PersistentCalls.RegisterVoidPersistentListener(index, call.Target as Object, call.Method.DeclaringType, call.Method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void RegisterVoidPersistentListenerWithoutValidation(int index, Object target, string methodName)
        {
            RegisterVoidPersistentListenerWithoutValidation(index, target, target.GetType(), methodName);
        }

        internal void RegisterVoidPersistentListenerWithoutValidation(int index, Object target, Type targetType, string methodName)
        {
            m_PersistentCalls.RegisterVoidPersistentListener(index, target, targetType, methodName);
            DirtyPersistentCalls();
        }

        internal void AddIntPersistentListener(UnityAction<int> call, int argument)
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterIntPersistentListener(persistentEventCount, call, argument);
        }

        internal void RegisterIntPersistentListener(int index, UnityAction<int> call, int argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Int))
            {
                m_PersistentCalls.RegisterIntPersistentListener(index, call.Target as Object, call.Method.DeclaringType, argument, call.Method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void AddFloatPersistentListener(UnityAction<float> call, float argument)
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterFloatPersistentListener(persistentEventCount, call, argument);
        }

        internal void RegisterFloatPersistentListener(int index, UnityAction<float> call, float argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Float))
            {
                m_PersistentCalls.RegisterFloatPersistentListener(index, call.Target as Object, call.Method.DeclaringType, argument, call.Method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void AddBoolPersistentListener(UnityAction<bool> call, bool argument)
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterBoolPersistentListener(persistentEventCount, call, argument);
        }

        internal void RegisterBoolPersistentListener(int index, UnityAction<bool> call, bool argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Bool))
            {
                m_PersistentCalls.RegisterBoolPersistentListener(index, call.Target as Object, call.Method.DeclaringType, argument, call.Method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void AddStringPersistentListener(UnityAction<string> call, string argument)
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterStringPersistentListener(persistentEventCount, call, argument);
        }

        internal void RegisterStringPersistentListener(int index, UnityAction<string> call, string argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.String))
            {
                m_PersistentCalls.RegisterStringPersistentListener(index, call.Target as Object, call.Method.DeclaringType, argument, call.Method.Name);
                DirtyPersistentCalls();
            }
        }

        internal void AddObjectPersistentListener<T>(UnityAction<T> call, T argument) where T : Object
        {
            int persistentEventCount = GetPersistentEventCount();
            AddPersistentListener();
            RegisterObjectPersistentListener(persistentEventCount, call, argument);
        }

        internal void RegisterObjectPersistentListener<T>(int index, UnityAction<T> call, T argument) where T : Object
        {
            if (call == null)
            {
                throw new ArgumentNullException("call", "Registering a Listener requires a non null call");
            }

            if (ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Object, ((Object)argument == (Object)null) ? typeof(Object) : argument.GetType()))
            {
                m_PersistentCalls.RegisterObjectPersistentListener(index, call.Target as Object, call.Method.DeclaringType, argument, call.Method.Name);
                DirtyPersistentCalls();
            }
        }
    }
    */

    public abstract class ADBaseInvokableCall
    {
        protected ADBaseInvokableCall()
        {
        }

        protected ADBaseInvokableCall(object target, MethodInfo function)
        {
            if (function is null)
            {
                throw new ArgumentNullException("function");
            }

            if (function.IsStatic)
            {
                if (target != null)
                {
                    throw new ArgumentException("target must be null");
                }
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target");
            }
        }

        public abstract void Invoke(object[] args);

        protected static void ThrowOnInvalidArg<T>(object arg)
        {
            if (arg != null && arg is not T)
            {
                throw new ArgumentException(string.Format("Passed argument 'args[0]' is of the wrong type. Type:{0} Expected:{1}", arg.GetType(), typeof(T)));
            }
        }

        protected static bool AllowInvoke(Delegate @delegate)
        {
            object target = @delegate.Target;
            if (target == null)
            {
                return true;
            }

            if (target is object @object)
            {
                return @object != null;
            }

            return true;
        }

        public abstract bool Find(object targetObj, MethodInfo method);
    }

    public class ADInvokableCall : ADBaseInvokableCall
    {
        protected event UnityAction Delegate;

        public ADInvokableCall(object target, MethodInfo theFunction)
            : base(target, theFunction)
        {
            this.Delegate = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, theFunction);
        }

        public ADInvokableCall(UnityAction action)
        {
            Delegate += action;
        }

        public override void Invoke(object[] args)
        {
            if (args.Length != 0)
            {
                throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
            }

            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate();
            }
        }

        public void Invoke()
        {
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate();
            }
        }

        public override bool Find(object targetObj, MethodInfo method)
        {
            return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
        }
    }

    public class ADInvokableCall<T1> : ADBaseInvokableCall
    {
        protected event UnityAction<T1> Delegate;

        public ADInvokableCall(object target, MethodInfo theFunction)
            : base(target, theFunction)
        {
            this.Delegate = (UnityAction<T1>)System.Delegate.CreateDelegate(typeof(UnityAction<T1>), target, theFunction);
        }

        public ADInvokableCall(UnityAction<T1> action)
        {
            Delegate += action;
        }

        public override void Invoke(object[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
            }

            ADBaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate((T1)args[0]);
            }
        }

        public void Invoke(T1 args)
        {
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate(args);
            }
        }

        public override bool Find(object targetObj, MethodInfo method)
        {
            return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
        }
    }

    public class ADInvokableCall<T1, T2> : ADBaseInvokableCall
    {
        protected event UnityAction<T1, T2> Delegate;

        public ADInvokableCall(object target, MethodInfo theFunction)
            : base(target, theFunction)
        {
            this.Delegate = (UnityAction<T1, T2>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2>), target, theFunction);
        }

        public ADInvokableCall(UnityAction<T1, T2> action)
        {
            Delegate += action;
        }

        public override void Invoke(object[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
            }

            ADBaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate((T1)args[0], (T2)args[1]);
            }
        }

        public void Invoke(T1 args0, T2 args1)
        {
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate(args0, args1);
            }
        }

        public override bool Find(object targetObj, MethodInfo method)
        {
            return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
        }
    }

    public class ADInvokableCall<T1, T2, T3> : ADBaseInvokableCall
    {
        protected event UnityAction<T1, T2, T3> Delegate;

        public ADInvokableCall(object target, MethodInfo theFunction)
            : base(target, theFunction)
        {
            this.Delegate = (UnityAction<T1, T2, T3>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3>), target, theFunction);
        }

        public ADInvokableCall(UnityAction<T1, T2, T3> action)
        {
            Delegate += action;
        }

        public override void Invoke(object[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
            }

            ADBaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate((T1)args[0], (T2)args[1], (T3)args[2]);
            }
        }

        public void Invoke(T1 args0, T2 args1, T3 args2)
        {
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate(args0, args1, args2);
            }
        }

        public override bool Find(object targetObj, MethodInfo method)
        {
            return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
        }
    }

    public class ADInvokableCall<T1, T2, T3, T4> : ADBaseInvokableCall
    {
        protected event UnityAction<T1, T2, T3, T4> Delegate;

        public ADInvokableCall(object target, MethodInfo theFunction)
            : base(target, theFunction)
        {
            this.Delegate = (UnityAction<T1, T2, T3, T4>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4>), target, theFunction);
        }

        public ADInvokableCall(UnityAction<T1, T2, T3, T4> action)
        {
            Delegate += action;
        }

        public override void Invoke(object[] args)
        {
            if (args.Length != 4)
            {
                throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
            }

            ADBaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
            ADBaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
            }
        }

        public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3)
        {
            if (ADBaseInvokableCall.AllowInvoke(this.Delegate))
            {
                this.Delegate(args0, args1, args2, args3);
            }
        }

        public override bool Find(object targetObj, MethodInfo method)
        {
            return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
        }
    }

    public abstract class ADBaseOrderlyEvent
    {
        public abstract ADBaseInvokableCall[] GetAllListener();
        public abstract void RemoveAllListeners();
        protected abstract MethodInfo FindMethod_Impl(string name, Type targetObjType);
        public abstract void Invoke(params object[] args);
    }

    [Serializable]
    public class ADOrderlyEvent : ADBaseOrderlyEvent
    {
        public List<int> InvokeArray = null;
        public int Count => (_m_Delegates == null) ? 0 : _m_Delegates.Count;
        private List<ADInvokableCall> _m_Delegates = null;

        public void AddListener(UnityAction call)
        {
            InvokeArray ??= new List<int>();
            _m_Delegates ??= new List<ADInvokableCall>();
            InvokeArray.Add(_m_Delegates.Count);
            _m_Delegates.Add(GetDelegate(call));
        }

        public void RemoveListener(UnityAction call)
        {
            if (_m_Delegates == null) return;
            var cat = _m_Delegates.Find(T => T.Find(call.Target, call.Method));
            if (cat != null)
            {
                int cat_index = _m_Delegates.FindIndex(T => T == cat);
                InvokeArray.RemoveAll(T => T == cat_index);
                _m_Delegates.Remove(cat);
            }
        }

        public override void RemoveAllListeners()
        {
            InvokeArray = null;
            _m_Delegates = null;
        }

        protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
        {
            return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[0]
            {
            });
        }

        public ADInvokableCall GetDelegate(object target, MethodInfo theFunction)
        {
            return new ADInvokableCall(target, theFunction);
        }

        private static ADInvokableCall GetDelegate(UnityAction action)
        {
            return new ADInvokableCall(action);
        }

        public void Invoke()
        {
            foreach (var index in InvokeArray)
            {
                if (index > 0 && index < _m_Delegates.Count && _m_Delegates[index] == null)
                {
                    Debug.LogWarning("you like to try invoke a error action");
                    continue;
                }
                _m_Delegates[index].Invoke();
            }
        }

        public override ADBaseInvokableCall[] GetAllListener()
        {
            return _m_Delegates.ToArray();
        }

        public override void Invoke(params object[] args)
        {
            if (args.Length > 0) Debug.LogWarning("you try to input some error args");
            Invoke();
        }
    }

    [Serializable]
    public class ADOrderlyEvent<T0> : ADBaseOrderlyEvent
    {
        public List<int> InvokeArray = null;
        public int Count => (_m_Delegates == null) ? 0 : _m_Delegates.Count;
        private List<ADInvokableCall<T0>> _m_Delegates = null;

        public void AddListener(UnityAction<T0> call)
        {
            InvokeArray ??= new List<int>();
            _m_Delegates ??= new List<ADInvokableCall<T0>>();
            InvokeArray.Add(_m_Delegates.Count);
            _m_Delegates.Add(GetDelegate(call));
        }

        public void RemoveListener(UnityAction<T0> call)
        {
            if (_m_Delegates == null) return;
            var cat = _m_Delegates.Find(T => T.Find(call.Target, call.Method));
            if (cat != null)
            {
                int cat_index = _m_Delegates.FindIndex(T => T == cat);
                InvokeArray.RemoveAll(T => T == cat_index);
                _m_Delegates.Remove(cat);
            }
        }

        public override void RemoveAllListeners()
        {
            InvokeArray = null;
            _m_Delegates = null;
        }

        protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
        {
            return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[1]
            {
                typeof(T0)
            });
        }

        public ADInvokableCall<T0> GetDelegate(object target, MethodInfo theFunction)
        {
            return new ADInvokableCall<T0>(target, theFunction);
        }

        private static ADInvokableCall<T0> GetDelegate(UnityAction<T0> action)
        {
            return new ADInvokableCall<T0>(action);
        }

        public void Invoke(T0 arg0)
        {
            if (InvokeArray != null)
                foreach (var index in InvokeArray)
                {
                    if (index > 0 && index < _m_Delegates.Count && _m_Delegates[index] == null)
                    {
                        Debug.LogWarning("you like to try invoke a error action");
                        continue;
                    }
                    _m_Delegates[index].Invoke(arg0);
                }
        }

        public override ADBaseInvokableCall[] GetAllListener()
        {
            return _m_Delegates.ToArray();
        }

        public override void Invoke(params object[] args)
        {
            if (args.Length != 1) Debug.LogWarning("you try to input some error args");
            T0 a0 = (args[0] is T0 t0) ? t0 : default;
            Invoke(a0);
        }
    }

    [Serializable]
    public class ADOrderlyEvent<T0, T1> : ADBaseOrderlyEvent
    {
        public List<int> InvokeArray = null;
        public int Count => (_m_Delegates == null) ? 0 : _m_Delegates.Count;
        private List<ADInvokableCall<T0, T1>> _m_Delegates = null;

        public void AddListener(UnityAction<T0, T1> call)
        {
            InvokeArray ??= new List<int>();
            _m_Delegates ??= new List<ADInvokableCall<T0, T1>>();
            InvokeArray.Add(_m_Delegates.Count);
            _m_Delegates.Add(GetDelegate(call));
        }

        public void RemoveListener(UnityAction<T0, T1> call)
        {
            if (_m_Delegates == null) return;
            var cat = _m_Delegates.Find(T => T.Find(call.Target, call.Method));
            if (cat != null)
            {
                int cat_index = _m_Delegates.FindIndex(T => T == cat);
                InvokeArray.RemoveAll(T => T == cat_index);
                _m_Delegates.Remove(cat);
            }
        }

        public override void RemoveAllListeners()
        {
            InvokeArray = null;
            _m_Delegates = null;
        }

        protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
        {
            return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[2]
            {
                typeof(T0),
                typeof(T1)
            });
        }

        public ADInvokableCall<T0, T1> GetDelegate(object target, MethodInfo theFunction)
        {
            return new ADInvokableCall<T0, T1>(target, theFunction);
        }

        private static ADInvokableCall<T0, T1> GetDelegate(UnityAction<T0, T1> action)
        {
            return new ADInvokableCall<T0, T1>(action);
        }

        public void Invoke(T0 arg0, T1 arg1)
        {
            if (InvokeArray != null)
                foreach (var index in InvokeArray)
                {
                    if (index > 0 && index < _m_Delegates.Count && _m_Delegates[index] == null)
                    {
                        Debug.LogWarning("you like to try invoke a error action");
                        continue;
                    }
                    _m_Delegates[index].Invoke(arg0, arg1);
                }
        }

        public override ADBaseInvokableCall[] GetAllListener()
        {
            return _m_Delegates.ToArray();
        }

        public override void Invoke(params object[] args)
        {
            if (args.Length != 2) Debug.LogWarning("you try to input some error args");
            T0 a0 = (args[0] is T0 t0) ? t0 : default;
            T1 a1 = (args[1] is T1 t1) ? t1 : default;
            Invoke(a0, a1);
        }
    }

    [Serializable]
    public class ADOrderlyEvent<T0, T1, T2> : ADBaseOrderlyEvent
    {
        public List<int> InvokeArray = null;
        public int Count => (_m_Delegates == null) ? 0 : _m_Delegates.Count;
        private List<ADInvokableCall<T0, T1, T2>> _m_Delegates = null;

        public void AddListener(UnityAction<T0, T1, T2> call)
        {
            InvokeArray ??= new List<int>();
            _m_Delegates ??= new List<ADInvokableCall<T0, T1, T2>>();
            InvokeArray.Add(_m_Delegates.Count);
            _m_Delegates.Add(GetDelegate(call));
        }

        public void RemoveListener(UnityAction<T0, T1, T2> call)
        {
            if (_m_Delegates == null) return;
            var cat = _m_Delegates.Find(T => T.Find(call.Target, call.Method));
            if (cat != null)
            {
                int cat_index = _m_Delegates.FindIndex(T => T == cat);
                InvokeArray.RemoveAll(T => T == cat_index);
                _m_Delegates.Remove(cat);
            }
        }

        public override void RemoveAllListeners()
        {
            InvokeArray = null;
            _m_Delegates = null;
        }

        protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
        {
            return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[3]
            {
                typeof(T0),
                typeof(T1),
                typeof(T2)
            });
        }

        public ADInvokableCall<T0, T1, T2> GetDelegate(object target, MethodInfo theFunction)
        {
            return new ADInvokableCall<T0, T1, T2>(target, theFunction);
        }

        private static ADInvokableCall<T0, T1, T2> GetDelegate(UnityAction<T0, T1, T2> action)
        {
            return new ADInvokableCall<T0, T1, T2>(action);
        }

        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            if (InvokeArray != null)
                foreach (var index in InvokeArray)
                {
                    if (index > 0 && index < _m_Delegates.Count && _m_Delegates[index] == null)
                    {
                        Debug.LogWarning("you like to try invoke a error action");
                        continue;
                    }
                    _m_Delegates[index].Invoke(arg0, arg1, arg2);
                }
        }

        public override ADBaseInvokableCall[] GetAllListener()
        {
            return _m_Delegates.ToArray();
        }

        public override void Invoke(params object[] args)
        {
            if (args.Length != 3) Debug.LogWarning("you try to input some error args");
            T0 a0 = (args[0] is T0 t0) ? t0 : default;
            T1 a1 = (args[1] is T1 t1) ? t1 : default;
            T2 a2 = (args[2] is T2 t2) ? t2 : default;
            Invoke(a0, a1, a2);
        }
    }

    [Serializable]
    public class ADOrderlyEvent<T0, T1, T2, T3> : ADBaseOrderlyEvent
    {
        public List<int> InvokeArray = null;
        public int Count => (_m_Delegates == null) ? 0 : _m_Delegates.Count;
        private List<ADInvokableCall<T0, T1, T2, T3>> _m_Delegates = null;

        public void AddListener(UnityAction<T0, T1, T2, T3> call)
        {
            InvokeArray ??= new List<int>();
            _m_Delegates ??= new List<ADInvokableCall<T0, T1, T2, T3>>();
            InvokeArray.Add(_m_Delegates.Count);
            _m_Delegates.Add(GetDelegate(call));
        }

        public void RemoveListener(UnityAction<T0, T1, T2, T3> call)
        {
            if (_m_Delegates == null) return;
            var cat = _m_Delegates.Find(T => T.Find(call.Target, call.Method));
            if (cat != null)
            {
                int cat_index = _m_Delegates.FindIndex(T => T == cat);
                InvokeArray.RemoveAll(T => T == cat_index);
                _m_Delegates.Remove(cat);
            }
        }

        public override void RemoveAllListeners()
        {
            InvokeArray = null;
            _m_Delegates = null;
        }

        protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
        {
            return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[4]
            {
                typeof(T0),
                typeof(T1),
                typeof(T2),
                typeof(T3)
            });
        }

        public ADInvokableCall<T0, T1, T2, T3> GetDelegate(object target, MethodInfo theFunction)
        {
            return new ADInvokableCall<T0, T1, T2, T3>(target, theFunction);
        }

        private static ADInvokableCall<T0, T1, T2, T3> GetDelegate(UnityAction<T0, T1, T2, T3> action)
        {
            return new ADInvokableCall<T0, T1, T2, T3>(action);
        }

        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            if (InvokeArray != null)
                foreach (var index in InvokeArray)
                {
                    if (index > 0 && index < _m_Delegates.Count && _m_Delegates[index] == null)
                    {
                        Debug.LogWarning("you like to try invoke a error action");
                        continue;
                    }
                    _m_Delegates[index].Invoke(arg0, arg1, arg2, arg3);
                }
        }

        public override ADBaseInvokableCall[] GetAllListener()
        {
            return _m_Delegates.ToArray();
        }

        public override void Invoke(params object[] args)
        {
            if (args.Length != 4) Debug.LogWarning("you try to input some error args");
            T0 a0 = (args[0] is T0 t0) ? t0 : default;
            T1 a1 = (args[1] is T1 t1) ? t1 : default;
            T2 a2 = (args[2] is T2 t2) ? t2 : default;
            T3 a3 = (args[3] is T3 t3) ? t3 : default;
            Invoke(a0, a1, a2, a3);
        }
    }

    [Serializable]
    public class ADEvent : UnityEvent
    {

    }
    [Serializable]
    public class ADEvent<T1> : UnityEvent<T1>
    {

    }
    [Serializable]
    public class ADEvent<T1, T2> : UnityEvent<T1, T2>
    {

    }
    [Serializable]
    public class ADEvent<T1, T2, T3> : UnityEvent<T1, T2, T3>
    {

    }
    [Serializable]
    public class ADEvent<T1, T2, T3, T4> : UnityEvent<T1, T2, T3, T4>
    {

    }

    #endregion

    #region Property 

    public interface IPropertyHasGet<T>
    {
        AbstractBindProperty<T> Property { get; }
    }
    public interface IPropertyHasSet<T>
    {
        AbstractBindProperty<T> Property { get; }
    }

    public class Property<T>
    {
        public class PropertyAsset
        {
            public PropertyAsset()
            {
            }
            public PropertyAsset(T from)
            {
                value = from;
            }

            public ADOrderlyEvent<T> OnDestory = null;
            public virtual T value { get; set; } = default;
        }

        internal ADOrderlyEvent _m_get = null;
        internal ADOrderlyEvent<T> _m_set = null;
        internal ADOrderlyEvent<T> _m_set_same = null;
        internal PropertyAsset _m_data = null;
        public bool IsHaveValue => _m_data != null;

        public Property(ADOrderlyEvent<T> set, T data)
        {
            _m_set = set ?? throw new ArgumentNullException(nameof(set));
            _m_data = new PropertyAsset(data);
        }
        public Property(T data)
        {
            _m_data = new PropertyAsset(data);
        }
        public Property()
        {
        }

        public T Set(T _Right = default)
        {
            if (_Right.Equals(default))
            {
                if (_m_data == null)
                    _m_set_same?.Invoke(_Right);
                else
                {
                    _m_data.OnDestory?.Invoke(_Right);
                    _m_set?.Invoke(_Right);
                }
                _m_data = null;
                return _Right;
            }
            else
            {
                if (_m_data != null)
                    if (_m_data.value.Equals(_Right))
                        _m_set_same?.Invoke(_Right);
                    else
                    {
                        _m_data = new PropertyAsset();
                        _m_set?.Invoke(_Right);
                    }
                _m_data.value = _Right;
            }
            return _Right;
        }

        public T Get()
        {
            return (IsHaveValue) ? _m_data.value : default;
        }

        public void Init()
        {
            _m_get = null;
            _m_set = null;
            _m_set_same = null;
            _m_data = null;
        }

        public void AddListenerOnSet(UnityAction<T> action)
        {
            _m_set ??= new ADOrderlyEvent<T>();
            _m_set.AddListener(action);
        }
        public void AddListenerOnSetSame(UnityAction<T> action)
        {
            _m_set_same ??= new ADOrderlyEvent<T>();
            _m_set_same.AddListener(action);
        }
        public void AddListenerOnGet(UnityAction action)
        {
            _m_get ??= new ADOrderlyEvent();
            _m_get.AddListener(action);
        }

        public void RemoveListenerOnSet(UnityAction<T> action)
        {
            _m_set?.RemoveListener(action);
        }
        public void RemoveListenerOnSetSame(UnityAction<T> action)
        {
            _m_set_same?.RemoveListener(action);
        }
        public void RemoveListenerOnGet(UnityAction action)
        {
            _m_get?.RemoveListener(action);
        }

        public void RemoveListenerOnSet()
        {
            _m_set = null;
        }
        public void RemoveListenerOnSetSame()
        {
            _m_set_same = null;
        }
        public void RemoveListenerOnGet()
        {
            _m_get = null;
        }

        public void SortOnSet(IComparer<int> comparer)
        {
            _m_set?.InvokeArray.Sort(comparer);
        }
        public void SortOnSetSame(IComparer<int> comparer)
        {
            _m_set_same?.InvokeArray.Sort(comparer);
        }
        public void SortOnGet(IComparer<int> comparer)
        {
            _m_get.InvokeArray.Sort(comparer);
        }

        public List<int> IndexArrayOnSet()
        {
            return _m_set?.InvokeArray;
        }
        public List<int> IndexArrayOnSetSame()
        {
            return _m_set_same?.InvokeArray;
        }
        public List<int> IndexArrayOnGet()
        {
            return _m_get?.InvokeArray;
        }

        public bool Equals(Property<T> _Right)
        {
            return Get().Equals(_Right.Get());
        }

        public override int GetHashCode()
        {
            return Get().GetHashCode();
        }

        public override string ToString()
        {
            return Get().ToString();
        }

        public int GetThisHashCode()
        {
            return base.GetHashCode();
        }

        public string ToThisString()
        {
            return base.ToString();
        }
    }

    public abstract class AbstractBindProperty<T>
    {
        internal Property<T> _m_value = new Property<T>();
        internal T value
        {
            get
            {
                return this._m_value.Get();
            }
            set
            {
                this._m_value.Set(value);
            }
        }

        protected void SetPropertyAsset(Property<T>.PropertyAsset asset)
        {
            _m_value._m_data = asset;
        }

        #region Func

        public AbstractBindProperty<T> Init()
        {
            _m_value.Init();
            return this;
        }

        public void TrackThisShared(AbstractBindProperty<T> OtherProperty)
        {
            this._m_value = OtherProperty._m_value;
        }

        protected void Init(T _init)
        {
            _m_value = new Property<T>(_init);
        }

        internal AbstractBindProperty<T> AddListenerOnSet(UnityAction<T> action)
        {
            _m_value.AddListenerOnSet(action);
            return this;
        }
        internal AbstractBindProperty<T> AddListenerOnSetSame(UnityAction<T> action)
        {
            _m_value.AddListenerOnSetSame(action);
            return this;
        }
        internal AbstractBindProperty<T> AddListenerOnGet(UnityAction action)
        {
            _m_value.AddListenerOnGet(action);
            return this;
        }

        internal AbstractBindProperty<T> RemoveListenerOnSet(UnityAction<T> action)
        {
            _m_value.RemoveListenerOnSet(action);
            return this;
        }
        internal AbstractBindProperty<T> RemoveListenerOnSetSame(UnityAction<T> action)
        {
            _m_value.RemoveListenerOnSetSame(action);
            return this;
        }
        internal AbstractBindProperty<T> RemoveListenerOnGet(UnityAction action)
        {
            _m_value.RemoveListenerOnGet(action);
            return this;
        }

        internal AbstractBindProperty<T> RemoveListenerOnSet()
        {
            _m_value.RemoveListenerOnSet();
            return this;
        }
        internal AbstractBindProperty<T> RemoveListenerOnSetSame()
        {
            _m_value.RemoveListenerOnSetSame();
            return this;
        }
        internal AbstractBindProperty<T> RemoveListenerOnGet()
        {
            _m_value.RemoveListenerOnGet();
            return this;
        }

        internal AbstractBindProperty<T> SortOnSet(IComparer<int> comparer)
        {
            _m_value.SortOnSet(comparer);
            return this;
        }
        internal AbstractBindProperty<T> SortOnSetSame(IComparer<int> comparer)
        {
            _m_value.SortOnSetSame(comparer);
            return this;
        }
        internal AbstractBindProperty<T> SortOnGet(IComparer<int> comparer)
        {
            _m_value.SortOnGet(comparer);
            return this;
        }

        internal List<int> IndexArrayOnSet()
        {
            return _m_value.IndexArrayOnSet();
        }
        internal List<int> IndexArrayOnSetSame()
        {
            return _m_value.IndexArrayOnSetSame();
        }
        internal List<int> IndexArrayOnGet()
        {
            return _m_value.IndexArrayOnGet();
        }

        #endregion

        public bool Equals(AbstractBindProperty<T> _Right)
        {
            if (!(_Right._m_value.IsHaveValue || this._m_value.IsHaveValue)) return true;
            else if (_Right._m_value.IsHaveValue != this._m_value.IsHaveValue) return false;
            else if (_Right._m_value.Get().Equals(this._m_value.Get())) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return _m_value.Get().ToString();
        }

        public int GetPropertyHashCode()
        {
            return _m_value.GetHashCode();
        }

        public int GetValueHashCode()
        {
            return (_m_value.IsHaveValue) ? _m_value.Get().GetHashCode() : -1;
        }

        public string ToThisString()
        {
            return base.ToString();
        }

        public string ToPropertyString()
        {
            return _m_value.ToString();
        }
    }

    public class BindProperty<T> : AbstractBindProperty<T>, IPropertyHasGet<T>, IPropertyHasSet<T>
    {
        public AbstractBindProperty<T> Property => this;

        public BindPropertyJustGet<T> BindJustGet()
        {
            BindPropertyJustGet<T> get = new BindPropertyJustGet<T>();
            get.TrackThisShared(this);
            return get;
        }

        public BindPropertyJustSet<T> BindJustSet()
        {
            BindPropertyJustSet<T> get = new BindPropertyJustSet<T>();
            get.TrackThisShared(this);
            return get;
        }
    }

    public class BindPropertyJustGet<T> : AbstractBindProperty<T>, IPropertyHasGet<T>
    {
        public AbstractBindProperty<T> Property => this;
    }

    public class BindPropertyJustSet<T> : AbstractBindProperty<T>, IPropertyHasSet<T>
    {
        public AbstractBindProperty<T> Property => this;
    }

    public static class PropertyExtension
    {
        public static T GetOriginal<T>(this IPropertyHasGet<T> self)
        {
            return self.Property._m_value._m_data.value;
        }

        public static T SetOriginal<T>(this IPropertyHasSet<T> self, T value)
        {
            self.Property._m_value._m_data.value = value;
            return self.Property._m_value._m_data.value;
        }

        public static IPropertyHasSet<T> AddListenerOnSet<T>(this IPropertyHasSet<T> self, UnityAction<T> action)
        {
            self.Property.AddListenerOnSet(action);
            return self;
        }

        public static IPropertyHasSet<T> AddListenerOnSetSame<T>(this IPropertyHasSet<T> self, UnityAction<T> action)
        {
            self.Property.AddListenerOnSetSame(action);
            return self;
        }

        public static IPropertyHasGet<T> AddListenerOnGet<T>(this IPropertyHasGet<T> self, UnityAction action)
        {
            self.Property.AddListenerOnGet(action);
            return self;
        }

        public static IPropertyHasSet<T> RemoveListenerOnSet<T>(this IPropertyHasSet<T> self, UnityAction<T> action)
        {
            self.Property.RemoveListenerOnSet(action);
            return self;
        }

        public static IPropertyHasSet<T> RemoveListenerOnSetSame<T>(this IPropertyHasSet<T> self, UnityAction<T> action)
        {
            self.Property.RemoveListenerOnSetSame(action);
            return self;
        }

        public static IPropertyHasGet<T> RemoveListenerOnGet<T>(this IPropertyHasGet<T> self, UnityAction action)
        {
            self.Property.RemoveListenerOnGet(action);
            return self;
        }

        public static IPropertyHasSet<T> RemoveListenerOnSet<T>(this IPropertyHasSet<T> self)
        {
            self.RemoveListenerOnSet();
            return self;
        }

        public static IPropertyHasSet<T> RemoveListenerOnSetSame<T>(this IPropertyHasSet<T> self)
        {
            self.RemoveListenerOnSetSame();
            return self;
        }

        public static IPropertyHasGet<T> RemoveListenerOnGet<T>(this IPropertyHasGet<T> self)
        {
            self.RemoveListenerOnGet();
            return self;
        }

        public static IPropertyHasSet<T> SortOnSet<T>(this IPropertyHasSet<T> self, IComparer<int> comparer)
        {
            self.SortOnSet(comparer);
            return self;
        }

        public static IPropertyHasSet<T> SortOnSetSame<T>(this IPropertyHasSet<T> self, IComparer<int> comparer)
        {
            self.SortOnSetSame(comparer);
            return self;
        }

        public static IPropertyHasGet<T> SortOnGet<T>(this IPropertyHasGet<T> self, IComparer<int> comparer)
        {
            self.SortOnGet(comparer);
            return self;
        }

        public static List<int> IndexArrayOnSet<T>(this IPropertyHasSet<T> self)
        {
            return self.IndexArrayOnSet();
        }

        public static List<int> IndexArrayOnSetSame<T>(this IPropertyHasSet<T> self)
        {
            return self.IndexArrayOnSetSame();
        }

        public static List<int> IndexArrayOnGet<T>(this IPropertyHasGet<T> self)
        {
            return self.IndexArrayOnGet();
        }

        public static T Get<T>(this IPropertyHasGet<T> self)
        {
            return self.Property._m_value.Get();
        }

        public static T Set<T>(this IPropertyHasSet<T> self, T value)
        {
            return self.Property._m_value.Set(value);
        }


    }

    #endregion

    #region Extension

    public static class ObjectExtension
    {
        public static T As<T>(this object self) where T : class
        {
            return self as T;
        }

        public static bool As<T>(this object self,out T result) where T : class
        {
            if (self != null)
            {
                result = self as T;
                return result != null;
            }
            else
            {
                result = null;
                return false;
            } 
        }

        public static bool Convertible<T>(this object self) where T : class
        {
            if (self != null)
            {
                return self as T != null;
            }
            else return false;
        }

        public static bool Is<T>(this object self,out T result) where T : class
        {
            result = null;
            if (self is T r)
            {
                result = r;
                return true;
            }
            else return false; 
        }

        public static bool IsAssignableFromOrSubClass(this Type self,Type target)
        {
            return self.IsAssignableFrom(target)||self.IsSubclassOf(target);
        }

    }

    #endregion

}
