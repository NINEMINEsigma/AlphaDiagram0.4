using AD.BASE;

namespace AD.ProjectTwilight.Entry
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
            RegisterSystem<EntrySystem>();
            RegisterCommand<EntrySceneOnEnd>();
        } 
    }

    public class SetTargetSceneName : ADCommand
    {
        public SetTargetSceneName() => throw new ADException("you cannt use this function to create command");

        public SetTargetSceneName(string target)
        {
            this.target = target;
        }

        string target;

        public override void OnExecute()
        {
            Architecture.GetController<EntryManager>().TargetSceneName = target;
        }
    }

    public class EntrySceneOnEnd : ADCommand
    {
        public override void OnExecute()
        {
            Architecture.GetController<EntryManager>().OnEnd();
        }
    }
}
