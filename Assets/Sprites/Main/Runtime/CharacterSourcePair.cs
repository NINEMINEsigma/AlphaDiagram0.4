using System;
using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;

namespace AD.MainScene
{
    [Serializable]
    [CreateAssetMenu(menuName = "AD/CharacterSourcePair")]
    public class CharacterSourcePair : ScriptableObject,ICanInitialize
    {
        [Header("Asset")]
        public string characterName = "";
        public int GUID = 0;
        public int state = 0;
        public SingleCharacter CharacterPrefab;
        public SingleChartBox ChartBoxPrefab;
        public SingleSoundPlayer SoundPlayerPrefab;
        [Header("Buffer")]
        public int current = 0;
        public Dictionary<string, int> buffer = new Dictionary<string, int>();
        public List<int> Appearance = new List<int>();

        public void Init()
        {
            buffer.Clear();
            current = 0;
        }
    }
}
