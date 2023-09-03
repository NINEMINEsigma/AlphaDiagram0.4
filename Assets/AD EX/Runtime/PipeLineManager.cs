using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AD.BASE;
using AD.UI;
using Newtonsoft.Json;
using UnityEngine;
using static AD.Experimental.Runtime.PipeEx.PipeLineArchitecture;

namespace AD.Experimental.Runtime.PipeEx
{
    public class PipeLineArchitecture : ADArchitecture<PipeLineArchitecture>,IBase<PipeLineArchitecture_BM>
    {
        public override bool FromMap(IBaseMap from)
        {
            if (from == null) return false; 
            if (!from.As(out PipeLineArchitecture_BM bm)) throw new ADException("Type Error,Target Type:" + nameof(PipeLineArchitecture_BM));
            return FromMap(bm);
        }

        public override IBaseMap ToMap()
        {
            ToMap(out PipeLineArchitecture_BM bm);
            return bm;
        }

        public void ToMap(out IBaseMap BM)
        {
            ToMap(out PipeLineArchitecture_BM bm);
            BM = bm;
        }

        //*****************************************

        public override void Init()
        {
            RegisterModel<CurrentChoosingLeftItem>();
        }

        public void ToMap(out PipeLineArchitecture_BM BM)
        {
            throw new System.NotImplementedException();
        }

        public bool FromMap(PipeLineArchitecture_BM from)
        {
            throw new System.NotImplementedException();
        }

        //转向委托TODO
        public List<LeftItem> AllLeftItem = new();
        public class RefreshAllLeftItem : ADCommand
        {
            public override void OnExecute()
            {
                LeftItem current = GetModel<CurrentChoosingLeftItem>().current;
                foreach (var item in instance.AllLeftItem)
                {
                    if (item == current) continue;
                    item.OnPointerClickOther();
                }
            }
        }

    }

    [Serializable]
    public class PipeLineArchitecture_BM : IBaseMap<PipeLineArchitecture>
    {
        [Obsolete]
        public bool Deserialize(string source)
        {
            return false;
        }

        public void ToObject(out IBase obj)
        {
            ToObject(out PipeLineArchitecture _obj);
            obj = _obj;
        }

        public bool FromObject(IBase from)
        {
            if (from == null) return false;
            if(!from.As(out PipeLineArchitecture pla)) throw new ADException("Type Error,Target Type:" + nameof(PipeLineArchitecture));
            return FromObject(pla);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void ToObject(out PipeLineArchitecture obj)
        {
            PipeLineArchitecture.instance.FromMap(this);
            obj = null;
        }

        public bool FromObject(PipeLineArchitecture from = null)
        {
            try
            {
                __FromObject();
            }
            catch (Exception ex)
            {
                string a = "Fail to create a " + nameof(PipeLineArchitecture_BM);
                PipeLineArchitecture.instance.AddMessage(a);
                ADGlobalSystem.TrackError(a, ex);
                return false;
            }
            return true;
        }

        //***************************************************************

        public PipeLineArchitecture_BM() { }

        private void __FromObject()
        {
            PipeLineArchitecture target = PipeLineArchitecture.instance;

        }

    }

    public class PipeLineManager : ADController
    {
        [SerializeField] ListView LeftItemListView;

        public override void Init()
        {
            PipeLineArchitecture.instance.RegisterController(this);
        }

        public void GenerateLeftItem(string title)
        {
            LeftItemListView.
        }
    }

    public class CurrentChoosingLeftItem : AD.BASE.ADModel
    {
        private LeftItem CurrentChoosingLeftItemTarget;

        public LeftItem current
        {
            get => GetCirrent();
            set => SetCirrent(value);
        }

        private LeftItem GetCirrent()
        {
            return CurrentChoosingLeftItemTarget;
        }

        private void SetCirrent(LeftItem value)
        { 
            CurrentChoosingLeftItemTarget = value;
            Architecture.SendImmediatelyCommand<RefreshAllLeftItem>();
        }

        public override void Init()
        {
            current = null;
        }

        public override IADModel Load(string path)
        {
            throw new NotImplementedException();
        }

        public override void Save(string path)
        {
            throw new NotImplementedException();
        }
    }
}
