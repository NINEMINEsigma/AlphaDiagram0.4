using System;
using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;


namespace AD.MainScene
{ 

    public class MainGroupController : MonoSystem
    {
        [Header("Asset")]
        public List<CharacterSourcePair> SourcePairs = new List<CharacterSourcePair>();

        [Header("Object")]
        public CharacterGroup characterGroup;
        public ChartBoxGroup  chartBoxGroup;
        public SoundGroup  soundGroup;

        private void Start()
        {
            MainApp.instance.RegisterSystem(this);
        }

        public override void Init()
        {
            RegisterController(characterGroup);
            RegisterController(chartBoxGroup);
            RegisterController(soundGroup);
        }
    }
}