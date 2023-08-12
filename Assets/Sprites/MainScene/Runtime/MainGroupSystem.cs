using System.Collections.Generic;
using AD.BASE;
using AD.ProjectTwilight.Source;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    public class ReadOnlyBindProprety<T> : AbstractBindProperty<T>, AD.BASE.IPropertyHasGet<T>
    {  
        public AbstractBindProperty<T> Property => this;

        public void MakeInit(T _init)
        {
            Init(_init);
        }
    }

    public class MainGroupSystem : MonoSystem
    {
        [Header("Asset")]
        public CharacterSourcePairs SourceAsset;
        [HideInInspector]public List<CharacterSourcePair> SourcePairs = new List<CharacterSourcePair>();

        [Header("Object")]
        public CharacterGroup characterGroup;
        public ChartBoxGroup  chartBoxGroup;
        public SoundGroup  soundGroup;

        [Header("State")]
        public int currentIndex = 0;

        private void Start()
        {
            MainApp.instance.RegisterSystem(this);
        }

        public void SourceAssetInit(out CurrentData CurrentData)
        {
            SourcePairs.Clear();
            CurrentData = Architecture.As<MainApp>().CurrentData;
            CurrentData.Load(); 
        }

        public override void Init()
        {
            SourceAssetInit(out CurrentData CurrentData);
            SourceAsset = CurrentData.AssetBundle.LoadAsset(CurrentData.current.Branch) as CharacterSourcePairs;
            foreach (var data in SourceAsset)
                SourcePairs.Add(data as CharacterSourcePair); 
            RegisterController(characterGroup);
            RegisterController(soundGroup);
            RegisterController(chartBoxGroup);
            currentIndex = CurrentData.current.step;
            Refresh();
        }

        public void SetCurrent(int newCurrent)
        {
            currentIndex = newCurrent;
            Refresh();
        }

        public void AddCurrent(int add)
        {
            currentIndex = (int)Mathf.Clamp(currentIndex + add, 0, Mathf.Infinity);
            Refresh();
        }

        public void Refresh()
        {
            characterGroup.Refresh(currentIndex);
            chartBoxGroup.Refresh(currentIndex);
        }
    }

    public class NeedAddCurrentIndexWhenAutoMode : ADCommand
    {
        public override string LogMessage()
        {
            return "Add Current Index";
        }

        public override void OnExecute()
        {
            MainApp.instance.GetSystem<MainGroupSystem>().AddCurrent(1);
        }
    }

    public class AddCurrentIndox : ADCommand
    {
        public AddCurrentIndox() { }

        public AddCurrentIndox(int addal)
        {
            this.addal = addal;
        }

        int addal;

        public override void OnExecute()
        {
            Architecture.GetSystem<MainGroupSystem>().AddCurrent(addal);
        }
    }

}