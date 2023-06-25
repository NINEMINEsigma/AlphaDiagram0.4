using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AD.ADbase;
using AD.Utility;

namespace AD.Game
{ 
    public class GameManager : AD.SceneBaseController
    {
        private void Start()
        {
            GameApp.ADinstance.RegisterController(this);
        } 


        public override IADArchitecture ADinstance()
        {
            return GameApp.ADinstance;
        }
    }
}