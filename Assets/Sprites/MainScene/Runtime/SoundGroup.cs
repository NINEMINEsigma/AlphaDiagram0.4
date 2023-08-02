using System.Collections.Generic;
using AD.BASE;
using UnityEngine;
using UnityEngine.UI;

namespace AD.ProjectTwilight.MainScene
{
    public class SoundGroup : ADController
    {
        public Dictionary<int, SingleSoundPlayer> sounds = new Dictionary<int, SingleSoundPlayer>();

        public RectTransform rectTransform { get; private set; }
        public GridLayoutGroup layoutGroup { get; private set; }

        public override void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            layoutGroup = GetComponent<GridLayoutGroup>();
            foreach (var item in GetSystem<MainGroupController>().SourcePairs)
            {
                var cat = GameObject.Instantiate(item.SoundPlayerPrefab, transform);
                cat.Init(item.GUID);
                sounds.Add(item.GUID, cat);
            }
        }
    }
}
