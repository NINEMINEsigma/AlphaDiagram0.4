using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using UnityEngine;

namespace AD.MainScene
{
    public class MainSceneSystem : IADSystem
    {
        public IADArchitecture ADinstance()
        {
            return MainApp.ADinstance;
        }
    }
}