using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using AD.Entry;
using UnityEngine;

namespace AD.MainScene
{ 
    public class MainApp : ADArchitecture<MainApp>
    {
        public override bool FromMap(IBaseMap from)
        {
            throw new System.NotImplementedException();
        }

        public override IBaseMap ToMap()
        {
            throw new System.NotImplementedException();
        }

        public override void Init()
        {
            this
                .RegisterSystem<MainSceneSystem>();
        }
    }
}