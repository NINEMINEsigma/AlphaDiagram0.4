using System;
using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using AD.ProjectTwilight.Choose;
using AD.ProjectTwilight.Entry;
using AD.ProjectTwilight.MainScene;
using AD.ProjectTwilight.Source;

namespace AD.ProjectTwilight
{
    public class ProjectTwilightSubArchitecture : ISubPagesArchitecture
    {
        public class Enumerator : IEnumerator<IADArchitecture>
        {
            public Enumerator(Dictionary<Type, IADArchitecture>.Enumerator enumerator) 
            {
                this.enumerator = enumerator;
            }

            Dictionary<Type, IADArchitecture>.Enumerator enumerator;

            public IADArchitecture Current => enumerator.Current.Value;

            object IEnumerator.Current => enumerator.Current.Value;

            public void Dispose()
            {
                enumerator.Dispose();   
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Dispose();
            }
        }

        public IADArchitecture this[Type type]
        {
            get => SubArchitectures[type];
            set => SubArchitectures[type] = value;
        }

        public Dictionary<Type, IADArchitecture> SubArchitectures { get; internal set; } = new();

        public IEnumerator<IADArchitecture> GetEnumerator()
        {
            return new Enumerator(SubArchitectures.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(SubArchitectures.GetEnumerator());
        }
    }

    /// <summary>
    /// ProjectTwilight
    /// </summary>
    public class PTApp : TopArchitecture<PTApp, EntryApp,MainApp, EndApp, ProjectTwilightSubArchitecture>
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

            SubArchitectures.SubArchitectures[typeof(ChooseApp)] = ChooseApp.instance;

            RegisterModel<PlayerModel>();
        }

        public void LoadCharacterSourcePairsOnAB(string path)
        {

        }

        public void SetUpGameMainSceneSystem(CharacterSourcePairs assets)
        {
            MainArchitecture.GetSystem<MainGroupSystem>().SourceAsset = assets;
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

        public override EntryApp EntryArchitecture => EntryApp.instance;

        public override MainApp MainArchitecture => MainApp.instance;

        public override EndApp EndArchitecture => EndApp.instance;

        private ProjectTwilightSubArchitecture _m_SubArchitectures = new();
        public override ProjectTwilightSubArchitecture SubArchitectures => _m_SubArchitectures;
    }

    public class EndApp : ADArchitecture<EndApp>
    {
        public override bool FromMap(IBaseMap from)
        {
            throw new System.NotImplementedException();
        }

        public override IBaseMap ToMap()
        {
            throw new System.NotImplementedException();
        }
    }

    public class InitProjectTwilight : ADCommand
    {
        public override void OnExecute()
        {
            PTApp.instance.FromMap(null);
        }
    }
}
