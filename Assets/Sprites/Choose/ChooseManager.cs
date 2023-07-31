//
//	ninemine
//

using System.Collections;
using UnityEngine;
using AD.UI;
using UnityEngine.Rendering;

namespace AD.Choose 
{ 
    //Main Controller 
    public class ChooseManager : AD.SceneBaseController 
    { 
        private void Start() 
        { 
            ChooseApp.instance.RegisterController(this); 
        } 
        public override void Init() 
        { 
            base.Init(); 
        }
         
    }
}
