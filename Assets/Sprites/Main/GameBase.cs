
using AD.BASE;
using AD.ProjectTwilight.Choose;
using AD.ProjectTwilight.Entry;
using AD.ProjectTwilight.MainScene;
using AD.ProjectTwilight.Source;

namespace AD.ProjectTwilight
{

    /// <summary>
    /// ProjectTwilight
    /// </summary>
    public class PTApp : ADArchitecture<PTApp>
    {
        public override bool FromMap(IBaseMap from)
        {
            return false;
        }

        public override IBaseMap ToMap()
        {
            return null;
        }

        public override void Init()
        {
            base.Init(); 

            RegisterModel<PlayerModel>();
        }

        public void LoadCharacterSourcePairsOnAB(string path)
        {

        }

        public void SetUpGameMainSceneSystem(CharacterSourcePairs assets)
        {
            GameMain.GetSystem<MainGroupSystem>().SourceAsset = assets;
        }

        public void SavePlayerModel()
        {
            GetModel<PlayerModel>().Save();
        }

        public SinglePlayerAsset CurrentPlayer
        {
            get
            {
                return GetModel<CurrentData>().current;
            }
        }

        public IADArchitecture Entry => EntryApp.instance;
        public IADArchitecture Choose => ChooseApp.instance;
        public IADArchitecture GameMain => MainApp.instance;

    }

    public class InitProjectTwilight : ADCommand
    {
        public override void OnExecute()
        {
            PTApp.instance.FromMap(null);
        }
    }
}
