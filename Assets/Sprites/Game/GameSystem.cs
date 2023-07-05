using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using UnityEngine;

namespace AD.Game
{
    public class GameSystem : IADSystem
    {
        public IADArchitecture ADinstance()
        {
            return GameApp.ADinstance;
        }

        public void Init()
        {

        }
    }
}