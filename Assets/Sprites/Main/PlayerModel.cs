using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AD.BASE;
using UnityEngine;

namespace AD.Source
{
    [EaseSave3,Serializable]
    public class PlayerModel : AD.BASE.ADModel
    {
        public static string path => Path.Combine(Application.persistentDataPath, "data", "player") + ".model";
        public static string file => "player.model";

        public override void Init()
        {
            ADGlobalSystem.CreateDirectroryOfFile(path);
            Load(path);
        }

        public List<int> models = new List<int>();

        public PlayerModel Init(PlayerModel _Right)
        {
            this.models = _Right.models;
            return this;
        }

        public override IADModel Load(string path)
        {
            if (ADGlobalSystem.Input<PlayerModel>(path, out object target))
                Init(target as PlayerModel);
            else ADGlobalSystem.Output(path, this);
            return this;
        }

        public override void Save(string path)
        {
            ADGlobalSystem.Output(path, this);
        }
    }
}
