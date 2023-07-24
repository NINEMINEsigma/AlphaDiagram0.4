using System;
using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;



namespace AD.MainScene
{
    public class ReadOnlyBindProprety<T> : AbstractBindProperty<T>, AD.BASE.IPropertyHasGet<T>
    {  
        public AbstractBindProperty<T> Property => this;

        public void MakeInit(T _init)
        {
            Init(_init);
        }
    }

    public class MainGroupController : MonoSystem
    {
        [Header("Asset")]
        public List<CharacterSourcePair> SourcePairs = new List<CharacterSourcePair>();

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

        public override void Init()
        {
            RegisterController(characterGroup);
            RegisterController(soundGroup);
            RegisterController(chartBoxGroup);
        }

        public void SetCurrent(int newCurrent)
        {
            currentIndex = newCurrent;
        }

        public void AddCurrent(int add)
        {
            currentIndex += add;
        }

        public void Refresh()
        {
            characterGroup.Refresh(currentIndex);
        }
    }
}