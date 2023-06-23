using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace AD.ADbase
{
    #region AD_I

    /*
     * ADinstance
     * Base
     * Low-level public implementation
     */

    public interface IBase
    {
        IBaseMap ToMap();
        bool FromMap(IBaseMap from);
    }

    public abstract class BaseClass : IBase
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

        public BaseClass() { }

        public BaseClass(List<string> group_name)
        {
            foreach (var name in group_name) CollectionMechanism.Collect(name, this);
            AD__InjoinedGroup = group_name;
            AD__IsCollected = true;
        }

        ~BaseClass()
        {
            IsCollected = false;
        }

        #endregion

        abstract public bool FromMap(IBaseMap from);

        abstract public IBaseMap ToMap();
    }

    public abstract class BaseMonoClass : MonoBehaviour, IBase
    {
        #region attribute

        private bool AD__IsCollected { get; set; } = false;
        private List<string> AD__InjoinedGroup { get; set; } = new List<string>();
        private Transform AD__Transform = null;

        protected bool AD_Transform
        {
            get { return AD__Transform; }
        }

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

        #region Monofunction

        protected void Awake()
        {
            AD__Transform = GetComponent<Transform>();
        }

        #endregion

        abstract public bool FromMap(IBaseMap from);

        abstract public IBaseMap ToMap();
    }

    public interface IBaseMap
    {
        IBaseMap ToObject();
        bool FromObject(IBase from);
        string Serialize();
        bool Deserialize(string source);
    }

    public abstract class BaseMap : IBaseMap
    {
        public abstract bool FromObject(IBase from);
        public abstract IBaseMap ToObject();
        public abstract string Serialize();
        public abstract bool Deserialize(string source);
    }

    static public class CollectionMechanism
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

        public IBaseMap ToObject()
        {
            throw new ADException("Can not use an ADException to create IBase object");
        }

        private DateTime AD__GeneratedTime;
    }

    public interface IADArchitecture
    {
        void Init();
        IADArchitecture AddMessage(string message);
        _Model GetModel<_Model>() where _Model : class, IADModel, new();
        _System GetSystem<_System>() where _System : class, IADSystem, new();
        _Controller GetController<_Controller>() where _Controller : class, IADSystem, new();
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
        IADArchitecture SentEvent<_Event>() where _Event : class, IADEvent, new();
        IADArchitecture SentCommand<_Command>() where _Command : class, IADCommand, new();
        IADArchitecture UnRegister<_T>() where _T : new();
    }

    public interface IADModel
    {
        void Init();
        void SetArchitecture(IADArchitecture target);
    }

    public interface IADSystem
    {
        IADArchitecture ADinstance();
    }

    public interface IADController
    {
        IADArchitecture ADinstance();
    }

    public abstract class ADController : MonoBehaviour, IADController
    {
        public abstract IADArchitecture ADinstance();
        protected abstract void Init();
    }

    public interface IADEvent
    {
        void Trigger();
        void SetArchitecture(IADArchitecture target);
    }

    public interface IADCommand
    {
        IADArchitecture Architecture { get; set; }
        void Execute();
        void SetArchitecture(IADArchitecture target);
        string LogMessage();
    }

    public abstract class ADCommand : IADCommand
    {
        public IADArchitecture Architecture { get; set; } = null;

        public void Execute()
        {
            if (Architecture == null) throw new ADException("Can not execute a command without setting architecture");
            Architecture.AddMessage(LogMessage());
            OnExecute();
        }

        public abstract string LogMessage();

        public abstract void OnExecute();

        public void SetArchitecture(IADArchitecture target)
        {
            Architecture = target;
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
        private IADArchitecture AD__instance = null;

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
            AD__instance = target;
        }

        public IADArchitecture ADinstance { get { return AD__instance; } }

        public int Count { get { return AD__messages.Count; } }

        public int MaxCount = 100;
    }

    public abstract class ADArchitecture<T> : IADArchitecture where T : ADArchitecture<T>, new()
    {
        #region attribute

        private Dictionary<Type, object> AD__Objects = new Dictionary<Type, object>();
        private ADMessageRecord AD__MessageRecord = null;
        private static IADArchitecture AD__Instance = null;

        protected ADMessageRecord AD_MessageRecord { get { return AD__MessageRecord; } }

        public static IADArchitecture ADinstance
        {
            get
            {
                if (AD__Instance == null) AD__Instance = new T();
                return AD__Instance;
            }
        }

        #endregion

        #region basefunction

        public ADArchitecture()
        {
            AD__Objects = new Dictionary<Type, object>();  
            AD__MessageRecord = new ADMessageRecord();
            AD__Instance = this;

            RegisterModel<ADMessageRecord>(AD_MessageRecord);
        }
        ~ADArchitecture()
        {
            ADMessageRecord Recorder = GetModel<ADMessageRecord>();
            if (Recorder != null)
            {
                string path = Path.Combine(Application.persistentDataPath, "ADinstance4", "LogRecord", DateTime.Now.Ticks.ToString());
                AD.ADbase.FileC.TryCreateDirectroryOfFile(path);
                StreamWriter writer = (new FileInfo(path)).CreateText();
                string message = Recorder.What();
                writer.WriteAsync(message);
            }
        }

        #endregion

        #region mFunction

        public static void Destory()
        {
            AD__Instance = null;
        }

        public abstract IBaseMap ToMap();

        public abstract bool FromMap(IBaseMap from);

        public abstract void Init();

        private IADArchitecture Register<_T>() where _T : new()
        {
            var key = typeof(T);

            if (AD__Objects.ContainsKey(key))
            {
                AD__Objects[key] = new _T();
            }
            else
            {
                AD__Objects.Add(key, new _T());
            }
            return ADinstance;
        }

        private IADArchitecture Register<_T>(_T _object) where _T : new()
        {
            var key = typeof(T);

            if (AD__Objects.ContainsKey(key))
            {
                AD__Objects[key] = _object;
            }
            else
            {
                AD__Objects.Add(key, _object);
            }
            return ADinstance;
        }

        private object Get<_T>()
        {
            AD__Objects.TryGetValue(typeof(T), out object _object);
            return _object;
        }

        public IADArchitecture UnRegister<_T>() where _T : new()
        {
            AD__Objects.Remove(typeof(_T));
            return ADinstance;
        }

        public _Model GetModel<_Model>() where _Model : class, IADModel, new()
        {
            return Get<_Model>() as _Model;
        }

        public _System GetSystem<_System>() where _System : class, IADSystem, new()
        {
            return Get<_System>() as _System;
        }

        public _Controller GetController<_Controller>() where _Controller : class, IADSystem, new()
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
            return ADinstance;
        }

        public IADArchitecture RegisterSystem<_System>(_System system) where _System : IADSystem, new()
        {
            Register<_System>(system);
            return ADinstance;
        }

        public IADArchitecture RegisterController<_Controller>(_Controller controller) where _Controller : IADController, new()
        {
            Register<_Controller>(controller);
            return ADinstance;
        }

        public IADArchitecture RegisterEvent<_Event>(_Event _event) where _Event : IADEvent, new()
        {
            Register<_Event>(_event);
            return ADinstance;
        }

        public IADArchitecture SentEvent<_Event>() where _Event : class, IADEvent, new()
        {
            (Get<_Event>() as _Event).Trigger();
            return ADinstance;
        }

        public IADArchitecture RegisterModel<_Model>() where _Model : IADModel, new()
        {
            Register<_Model>();
            return ADinstance;
        }

        public IADArchitecture RegisterSystem<_System>() where _System : IADSystem, new()
        {
            RegisterSystem(new _System());
            return ADinstance;
        }

        public IADArchitecture RegisterController<_Controller>() where _Controller : IADController, new()
        { 
            RegisterController(new _Controller());
            return ADinstance;
        }

        public IADArchitecture RegisterEvent<_Event>() where _Event : IADEvent, new()
        {
            RegisterEvent(new _Event());
            return ADinstance;
        }

        public IADArchitecture RegisterCommand<_Command>(_Command command) where _Command : IADCommand, new()
        {
            command.SetArchitecture(ADinstance);
            Register<_Command>(command);
            return ADinstance;
        }

        public IADArchitecture RegisterCommand<_Command>() where _Command : IADCommand, new()
        {
            RegisterCommand(new _Command());
            return ADinstance;
        }

        public virtual IADArchitecture AddMessage(string message)
        {
            AD__MessageRecord.Add(new ADMessage(message));
            return ADinstance;
        }

        public IADArchitecture SentCommand<_Command>() where _Command : class, IADCommand, new()
        {
            (Get<_Command>() as _Command).Execute();
            return ADinstance;
        }

        #endregion
    }

    #endregion

    #region Event from Unity

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
}
