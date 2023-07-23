using System.Collections.Generic;
using AD.BASE;
using UnityEngine;

namespace AD.MainScene
{
    public class CharacterGroup : ADController
    {
        public Dictionary<int,SingleCharacter> characters = new Dictionary<int,SingleCharacter>();

        public override void Init()
        {
            foreach (var item in GetSystem<MainGroupController>().SourcePairs)
            {
                var cat = GameObject.Instantiate(item.CharacterPrefab);
                cat.gameObject.SetActive(false);
                characters.Add(item.GUID, cat);
            }
        }

        public void Refresh(int current)
        {
            foreach (var item in characters)
            {
                item.Value.Refresh(current);
            }
        }

    }
}
