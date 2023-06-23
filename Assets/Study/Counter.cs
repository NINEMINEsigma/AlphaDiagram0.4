using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEditor.VersionControl;
using QFramework.Example;

namespace Study.Example.Count
{
    //IController ���ؽӿ�
    //MVC Model-View-Commandģʽ

    //SOLIDԭ�� ����
    //��һְ��ԭ��SRP����ֹ�������
    //���ŷ��ԭ��OCP) ʵ��Ӧ�ö���չ�ǿ��ŵģ����޸��Ƿ�յġ�������չ(extension)�������޸�(modification)��
    //�����滻ԭ��LSP���κ����඼�����ڸ�����ֵĵط����滻
    //�ӿڸ���ԭ��ISP���Ǳ���ӿڲ���Ҫǿ��ʵ��
    //��������ԭ��DIP������Ӧ��������ϸ�ڣ�ϸ��Ӧ�������ڳ��󡣻���֮��Ҫ��Գ��󣨽ӿڣ���̣����������ʵ��ϸ�ڱ�̡�

    //Register ע�� RegisterWithInitValue ͬʱ����һ��
    //Model Utility System ����Ҫע��

    //IArchitecture ��ʽ���صķ���ֵ �����ĵ�App
    //Model ģ�� ���ڼ�¼���� GetModel
    //IUtility ���ú���
    //AbstractSystem ϵͳ��
    //AbstractQuery ��ѯ��������
    //AbstractCommand ָ��Լ��޸ģ�д����



    public class Counter : MonoBehaviour, IController
    {
        private void OnDisable()
        {
            Debug.Log(this.GetModel<IMessageModel>().Log());
        }

        #region Model

        ICountModel count;

        private void Start()
        {
            count = this.GetModel<ICountModel>();

            //inter
            add.onClick.AddListener(() =>
            {
                this.SendCommand<IncreaseCountCommand>();
            });
            decline.onClick.AddListener(() =>
            {
                this.SendCommand<DecreaseCountCommand>();
            });

            //view
            count.count.RegisterWithInitValue(_count =>
            {
                UpdateCount();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

        }


        void UpdateCount()
        {
            count_text.text = count.count.ToString();
        }

        public IArchitecture GetArchitecture()
        {
            return CounterApp.Interface;
        }

        #endregion

        #region View

        [SerializeField] Button add, decline;
        [SerializeField] Text count_text;

        #endregion

        #region Controller

        #endregion
    }

    public interface ICountModel:IModel
    {
        BindableProperty<int> count { get; }
    }

    public class CounterModel : AbstractModel,ICountModel
    {
        public BindableProperty<int> count { get; } = new(0);

        protected override void OnInit()
        {
            IIntSourceUtility storage = this.GetUtility<IIntSourceUtility>();
            count.Value = storage.LoadInt(nameof(count), 0);

            count.Register(_count =>
            {
                storage.SaveInt(nameof(count), count);
            });
        }
    }

    public class CounterApp : Architecture<CounterApp>
    {
        protected override void Init()
        {
            this.RegisterModel<ICountModel>(new CounterModel());
            this.RegisterModel<IMessageModel>(new CounterMessage());

            this.RegisterUtility<IIntSourceUtility>(new Storage());

            this.RegisterSystem<ICounterSystem>(new CounterSystem());
        }
    }

    public interface IMessageModel : IModel
    {
        List<string> messages { get; set; }
        public void AddMessage(string message);
        public string Log();
    }

    public class CounterMessage : AbstractModel, IMessageModel 
    {
        public List<string> messages { get; set; }

        public void AddMessage(string message)
        {
            messages.Add(System.DateTime.Now.ToString() + " : " + message);
            if (messages.Count > 50) messages.RemoveAt(0);
        }

        public string Log()
        {
            string _log = "";
            foreach (var message in messages) { _log += message.ToString() + "\n"; }
            return _log;
        }

        protected override void OnInit()
        {
            messages = new();
        }
    }

    public abstract class CounterCommand : AbstractCommand
    {
        protected abstract string CommandLog();

        protected override void OnExecute()
        {
            this.GetModel<IMessageModel>().AddMessage(CommandLog());
            HowExecute();
        }

        protected abstract void HowExecute();
    }

    public class IncreaseCountCommand : CounterCommand
    {
        protected override string CommandLog()
        {
            return "count++";
        }

        protected override void HowExecute()
        {
            this.GetModel<ICountModel>().count.Value++;
        }
    }

    public class DecreaseCountCommand : CounterCommand
    {
        protected override string CommandLog()
        {
            return "count--";
        }

        protected override void HowExecute()
        //protected override void OnExecute()
        {
            this.GetModel<ICountModel>().count.Value--;
        }
    }

    public interface IIntSourceUtility : IUtility
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue);
    }

    public class Storage : IIntSourceUtility
    {
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int LoadInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
    }

    public interface ICounterSystem:ISystem
    {

    }

    public class CounterSystem : AbstractSystem, ICounterSystem
    {
        protected override void OnInit()
        {
            var model = this.GetModel<ICountModel>();

            model.count.Register(count =>
            {
                if (count == 10)
                {
                    this.GetModel<IMessageModel>().AddMessage("count == 10");
                }
            });
        }
    }

    public interface IIntQuery:IQuery<int>
    {

    }

    public class CounterQuery : AbstractQuery<int>, IIntQuery
    {
        protected override int OnDo()
        {
            return 0;//
        }
    }
}
