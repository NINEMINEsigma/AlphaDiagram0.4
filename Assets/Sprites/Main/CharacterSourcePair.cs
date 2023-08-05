using System;
using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using AD.Experimental.EditorAsset.Cache;
using AD.UI;
using UnityEditor;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    [Serializable]
    public class CharacterMessage
    {
        public string keyName;
        public int GUID;
        public int MUID;
        public string message;
    }

    [Serializable]
    [CreateAssetMenu(menuName = "AD/CharacterSourcePair")]
    public class CharacterSourcePair : AbstractScriptableObject, ICanInitialize
    {
        public static List<CharacterSourcePair> CharacterSourcePairsBuffer = new List<CharacterSourcePair>();

        [Header("Asset")]
        //角色名
        public string characterName = "";
        //UID
        public int GUID = 0;
        //人物分支编号
        public int branch = 0;
        //路径
        [TextArea] public string path = "";
        //预制体
        public SingleCharacter CharacterPrefab;
        public SingleChartBox ChartBoxPrefab;
        public SingleSoundPlayer SoundPlayerPrefab;
        [Header("Buffer")]
        //出现时机，反插
        public List<int> Appearance = new();
        //附加状态属性
        public List<StateBuff> buffer = new();
        //差分，区间堆
        public List<DifferenceBuff> PictureDifferences = new();
        //音频
        public List<UI.SourcePair> SourcePairs = new();
        //文本
        public List<CharacterMessage> charts = new();

        [Serializable]
        public class StateBuff
        {
            public string Key;
            public int Value;
        }

        [Serializable]
        public class DifferenceBuff
        {
            public int Key;
            public int Value;
        }

        public void Init(CharacterSourcePair _Right)
        {
            this.characterName = _Right.characterName;
            GUID = _Right.GUID;
            this.branch = _Right.branch;
            this.path = _Right.path;
            CharacterPrefab ??= _Right.CharacterPrefab;
            ChartBoxPrefab ??= _Right.ChartBoxPrefab;
            SoundPlayerPrefab ??= _Right.SoundPlayerPrefab;
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

        public List<CharacterSourcePair> Get()
        {
            string[] sourcesLine = source.Split(';', '\n');
            CharacterSourcePair.CharacterSourcePairsBuffer = new List<CharacterSourcePair>(); 
            for (int i = 0; i < sourcesLine.Length; i++)
            {
                string line = sourcesLine[i];
                string[] words = line.Split('\\', '/');
                if (words.Length > 4)
                {
                    Debug.Log("Line " + i.ToString() + " is error");
                    continue;
                }
                int a = int.Parse(words[0]), b = int.Parse(words[1]), c = int.Parse(words[2]);
                Debug.Log(words[0] + "/" + words[1] + "/" + words[2] + "/" + words[3]);
                var cat = CharacterSourcePair.CharacterSourcePairsBuffer.FirstOrDefault(T => T.GUID == b && T.branch == a);
                if (MakePair(cat, a, b, c, words[3], out cat))
                {
                    CharacterSourcePair.CharacterSourcePairsBuffer.Add(cat);
                }
            } 
            return CharacterSourcePair.CharacterSourcePairsBuffer;
        }

        private static bool MakePair(CharacterSourcePair target, int arg0, int arg1, int arg2, string arg3, out CharacterSourcePair pair)
        {
            bool check = target == default;
            pair = (check) ? new CharacterSourcePair() : target;
            switch (arg2)
            {
                case 0:
                    {
                        pair.branch = arg0;
                        pair.GUID = arg1;
                        pair.charts.Add(new CharacterMessage() { GUID = arg1, keyName = arg1.ToString(), MUID = arg0, message = arg3 });
                    }
                    break;
                default:
                    break;
            }
            return check;
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

            if (GUILayout.Button("Open Input Path"))
            {
                //path.stringValue =
                //    EditorUtility.OpenFolderPanel("Select Folder", path.stringValue, "Select");
                path.stringValue = EditorUtility.OpenFilePanel("Select File", path.stringValue, "");//RevealInFinder(path.stringValue);
            }

            if (GUILayout.Button("Create"))
            {
                if (path.stringValue != "Error" && AD.ADGlobalSystem.Input(path.stringValue, out string source))
                {
                    var temp = new CharacterSourcePairTranslater(source).Get().FirstOrDefault(T => T.GUID == that.GUID && T.branch == that.branch);
                    if (temp != default)
                    {
                        Debug.Log("Init " + temp.GUID.ToString());
                        that.Init(temp);
                    }
                    else
                        Debug.LogWarning("No this character");
                }
                else path.stringValue = "Error";
            }

            if (GUILayout.Button("Find With GUID"))
            {
                var temp = CharacterSourcePair.CharacterSourcePairsBuffer.FirstOrDefault(T => T.GUID == that.GUID && T.branch == that.branch);
                if (temp!=default)
                    that.Init(temp);
                else
                    Debug.Log("Not Find GUID");
            } 

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
