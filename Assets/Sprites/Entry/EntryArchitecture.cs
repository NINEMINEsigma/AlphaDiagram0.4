using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AD.ADbase;

namespace AD.Entry
{
    //Register
    public class EntryApp : ADArchitecture<EntryApp>
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
            this.RegisterSystem<EntrySystem>(); 
        }
    }
}