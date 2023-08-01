//
//	ninemine
//
using AD.BASE;
using AD.Source;
namespace AD.Choose 
{ 
    //Register 
    public class ChooseApp : ADArchitecture<ChooseApp> 
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
            RegisterModel<PlayerModel>();

            RegisterSystem<ChooseSystem>(); 
        }

        public static string TargetSceneName
        {
            get
            {
                return instance.GetController<ChooseManager>().TargetSceneName;
            }
            set
            {
                instance.GetController<ChooseManager>().TargetSceneName = value;
            }
        }

        public static void OnEnd()
        {
            instance.GetController<ChooseManager>().OnEnd();
        }

    }
}
