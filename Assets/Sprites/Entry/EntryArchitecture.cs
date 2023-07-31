using AD.BASE;

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
            this
                .RegisterSystem<EntrySystem>();
        }

        public static string TargetSceneName
        {
            get
            {
                return instance.GetController<EntryManager>().TargetSceneName;
            }
            set
            {
                instance.GetController<EntryManager>().TargetSceneName = value;
            }
        }

        public static void OnEnd()
        {
            instance.GetController<EntryManager>().OnEnd();
        }
    }
}