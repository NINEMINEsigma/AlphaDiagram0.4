//
//	ninemine
//

namespace AD.ProjectTwilight.Choose
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

            Architecture.Init();
        }
         
    }
}
