using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using AD.ADbase;
using AD.Entry;

namespace AD.Game
{
    public class GameApp : ADArchitecture<GameApp>
    {
        public override bool FromMap(IBaseMap from)
        {
            throw new System.NotImplementedException();
        }

        public override void Init()
        {
            this.RegisterSystem<GameSystem>();
        }

        public override IBaseMap ToMap()
        {
            throw new System.NotImplementedException();
        }
    }
}