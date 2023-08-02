using System;
using System.Collections.Generic;
using System.IO;
using AD.BASE;
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
    }

    [EaseSave3, Serializable]
    public class PlayerModel : AD.BASE.ADModel
    {
        public static string DataPath => Path.Combine(Application.persistentDataPath, "data", "player") + "/";

        public static string GetPath(string fileName) => DataPath + fileName + ".model";

        public override void Init()
        {
            models = new();
            ADGlobalSystem.CreateDirectroryOfFile(DataPath + "file.model");
        }

        public List<SinglePlayerAsset> models = new();
        public SinglePlayerAsset current = null;

        public PlayerModel Init(PlayerModel _Right)
        {
            this.models = _Right.models;
            this.current = null;
            return this;
        } 

        public override IADModel Load(string fileName)
        {
            FileC.LoadFiles(nameof(PlayerModel), DataPath, T => Path.GetExtension(T) == "model");
            if (ADGlobalSystem.Input<PlayerModel>(GetPath(fileName), out object target))
                Init(target as PlayerModel);
            else ADGlobalSystem.Output(GetPath(fileName), this);
            return this;
        }

        public override void Save(string fileName)
        {
            ADGlobalSystem.Output(GetPath(fileName), this);
        }
    }
}
