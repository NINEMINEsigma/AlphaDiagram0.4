using System;
using System.Collections.Generic;
using System.IO;
using AD.BASE;
using AD.Utility;
using UnityEngine;

namespace AD.ProjectTwilight.Source
{
    [EaseSave3, Serializable]
    public class SinglePlayerAsset
    {
        public string PlayerName;
        public string Chapter;
        public string Branch;
        public int step;
        public int index;
        public string FileName;
    }

    public class CurrentData : AD.BASE.ADModel
    {
        private const string ABPath = "twilight.ab";
        public SinglePlayerAsset current = null;

        ~CurrentData()
        {
            AssetBundle.Unload(true);
        }

        public AssetBundle AssetBundle;

        public override void Init()
        {

        }

        public override IADModel Load(string path = "")
        {
#if UNITY_EDITOR
            AssetBundle = (Path.Combine(Application.streamingAssetsPath, "AB") + "/" + ABPath).LoadAssetBundle();
#elif UNITY_WINDOW
            AssetBundle = (Path.Combine(Application.streamingAssetsPath, "AB") + "/" + ABPath).LoadAssetBundle();
#elif UNITY_ANDROID
        //安卓平台
#elif UNITY_IOS
        //苹果平台
#endif
            return this;
        }

        public override void Save(string path)
        {
            throw new NotImplementedException();
        }
    }

    [EaseSave3, Serializable]
    public class PlayerModel : AD.BASE.ADModel
    {
        public static string DataPath => Path.Combine(Application.persistentDataPath, "data", "player") + "/";

        public static string GetPath(string fileName) => DataPath + fileName + ".adm";

        public override void Init()
        {
            models = new();
            FileC.CreateDirectroryOfFile(DataPath + "file.model");
        }

        public List<SinglePlayerAsset> models = new();

        public void ConfirmModel(CurrentData currentData)
        {
            PTApp.instance.RegisterModel(currentData);
        }

        public override IADModel Load(string fileName = "")
        {
            Architecture.UnRegister<CurrentData>();
            FileC.LoadFiles(nameof(PlayerModel), DataPath, T => Path.GetExtension(T) == "model");
            //if (ADGlobalSystem.Input<PlayerModel>(GetPath(fileName), out object target))
            //    Init(target as PlayerModel); 
            if (FileC.TryGetFiles(nameof(PlayerModel), out var infos))
            {
                foreach (var info in infos)
                {
                    if (ADGlobalSystem.Input<SinglePlayerAsset>(GetPath(info.FullName), out object target))
                    {
                        models.Add(target as SinglePlayerAsset);
                    }
                    else ADGlobalSystem.AddWarning(info.FullName + " is cannt load");
                }
                models.Sort((T, P) => T.index.CompareTo(P.index));
            }
            else
            {
                CurrentData current = new CurrentData() { current = GenerateDefualt() };
                ConfirmModel(current);
                models.Add(current.current);
                ADGlobalSystem.Output(GetPath("defualt"), current.current);
            }
            return this;
        }

        public static SinglePlayerAsset GenerateDefualt()
        {
            return new SinglePlayerAsset()
            {
                PlayerName = "观者",
                Chapter = "New Start",
                Branch = "New Start",
                step = 0,
                index = 0,
                FileName = "defualt"
            };
        }

        public override void Save(string fileName = "")
        {
            for (int i = 0; i < models.Count; i++)
            {
                SinglePlayerAsset model = models[i];
                model.index = i;
                if (model.FileName != "defualt")
                    model.FileName = i.ToString();
                ADGlobalSystem.Output<SinglePlayerAsset>(GetPath(model.FileName), model);
            }
        }
    }
}
