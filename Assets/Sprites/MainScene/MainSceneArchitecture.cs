using AD.BASE;
using AD.ProjectTwilight.Source;

namespace AD.ProjectTwilight.MainScene
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
            RegisterSystem<MainSceneSystem>(); 
        }

        public float AudoDelayTime = 3.5f;

        public CurrentData CurrentData=>PTApp.instance.GetModel<CurrentData>();

    }
}
