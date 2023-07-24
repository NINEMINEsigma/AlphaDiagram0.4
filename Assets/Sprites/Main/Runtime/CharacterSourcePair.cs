using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using AD.UI;
using UnityEditor;
using UnityEngine;

namespace AD.MainScene
{
    [Serializable]
    public class CharacterMessage
    {
        public string keyName;
        public int GUID;
        public string message;
    }

    [Serializable]
    [CreateAssetMenu(menuName = "AD/CharacterSourcePair")]
    public class CharacterSourcePair : ScriptableObject,ICanInitialize
    {
        [Header("Asset")]
        //角色名
        public string characterName = "";
        //UID
        public int GUID = 0;
        //人物状态编号
        public int state = 0;
        //路径
        [TextArea]public string path = "";
        //预制体
        public SingleCharacter CharacterPrefab;
        public SingleChartBox ChartBoxPrefab;
        public SingleSoundPlayer SoundPlayerPrefab;
        [Header("Buffer")]
        //出现时机，反插
        public List<int> Appearance = new();
        //附加状态属性
        public Dictionary<string, int> buffer = new();
        //差分，区间堆
        public Dictionary<int, int> PictureDifferences = new();
        //音频
        public List<SourcePair> SourcePairs = new();
        //文本
        public List<CharacterMessage> charts = new();


        public void Init(CharacterSourcePair _Right)
        {
            this.characterName = _Right.characterName;
            GUID = _Right.GUID;
            this.state = _Right.state;
            this.path = _Right.path;
            CharacterPrefab = _Right.CharacterPrefab;
            ChartBoxPrefab = _Right.ChartBoxPrefab;
            SoundPlayerPrefab = _Right.SoundPlayerPrefab;
            Appearance = _Right.Appearance;
            this.buffer = _Right.buffer;
            PictureDifferences = _Right.PictureDifferences;
            SourcePairs = _Right.SourcePairs;
            this.charts = _Right.charts;
        }

        public void Init()
        {
            Appearance.Clear();
            PictureDifferences.Clear();
            buffer.Clear();
            SourcePairs.Clear();
            charts.Clear(); ;
        } 
    }

    internal class CharacterSourcePairTranslater
    {
        public CharacterSourcePairTranslater(string source)
        {
            CharacterSourcePairTranslater.source = source;
        }

        static string source;
        static List<CharacterSourcePair> results = null;

        public List<CharacterSourcePair> Get()
        {
            string[] sourcesLine = source.Split(';', '\n');
            for (int i = 0; i < sourcesLine.Length; i++)
            {
                string line = sourcesLine[i];
                string[] words = line.Split('\\', '/');
                if(words.Length!=4)
                {
                    AD.ADGlobalSystem.AddMessage("Line " + i.ToString() + " is error");
                    continue;
                } 
            }
            return null;
        }

    }

#if UNITY_EDITOR 

    [CustomEditor(typeof(CharacterSourcePair))]
    public class CharacterSourcePairEditor:Editor
    {
        private CharacterSourcePair that = null;
         
        SerializedProperty path; 

        private void OnEnable()
        {
            that = target as CharacterSourcePair;
             
            path = serializedObject.FindProperty("path"); 
        }

        public override void OnInspectorGUI()
        { 
            base.OnInspectorGUI(); 

            if (GUILayout.Button("..."))
            {
                path.stringValue =
                    EditorUtility.OpenFolderPanel("Select Folder", path.stringValue, "");
            }

            if (GUILayout.Button("Open Output Folder"))
            {
                EditorUtility.RevealInFinder(path.stringValue);
            }

            if (GUILayout.Button("Create"))
            {
                AD.ADGlobalSystem.Input(path.stringValue, out string source);
                var temp = new CharacterSourcePairTranslater(source).Get().FirstOrDefault(T => T.GUID == that.GUID);
                if (!temp.Equals(default))
                    that.Init(temp);
                else
                    Debug.LogWarning("No this character");
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
