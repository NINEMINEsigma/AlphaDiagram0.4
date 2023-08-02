using AD.BASE;

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
            this
                .RegisterSystem<MainSceneSystem>();
        }
    }
}