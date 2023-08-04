using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AD.BASE;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;
using AD.UI;

namespace AD
{
    public class RegisterInfo
    {
        public RegisterInfo(List<ButtonControl> buttons, UnityAction action, PressType type)
        {
            this.buttons = buttons;
            this.action = action;
            this.type = type;
        }

        public void UnRegister()
        {
            ADGlobalSystem.RemoveListener(buttons, action, type);
        }

        public void TryRegister()
        {
            ADGlobalSystem.AddListener(buttons, action, type);
        }

        private bool _state = true;
        public bool state
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    if (value) TryRegister();
                    else UnRegister();
                }
                _state = value;
            }
        }

        private List<ButtonControl> buttons = new List<ButtonControl>();
        private UnityEngine.Events.UnityAction action;
        private PressType type;
    }

    public class MulHitControl : ButtonControl
    {
        public bool WasPressedThisFrame()
        {
            if (TargetButton.wasPressedThisFrame)
            {
                CurrentHitCount++;
                if (CurrentHitCount == TargetHitCount)
                {
                    CurrentTime = 0;
                    CurrentHitCount = 0;
                    return true;
                }
            }
            return false;
        }

        public MulHitControl(int targetHitCounter, ButtonControl targetButton)
        {
            TargetHitCount = targetHitCounter;
            TargetButton = targetButton;
        }

        private float CurrentTime = 0;
        private float CurrentHitCount = 0;
        public int TargetHitCount = 0;
        public ButtonControl TargetButton = null;

        public void Update()
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime > 0.5f)
            {
                CurrentHitCount = 0;
                CurrentTime = 0;
            }
        }

        public override string ToString()
        {
            return TargetButton.ToString() + "(MulHit" + TargetHitCount.ToString() + ")";
        }
    }

    public enum PressType
    {
        JustPressed,
        ThisFramePressed,
        ThisFrameReleased
    }

    public class ADGlobalSystem : MonoBehaviour
    {
        public static ADGlobalSystem _m_instance = null;
        public static ADGlobalSystem instance
        {
            get
            {
                if (_m_instance == null)
                {
                    _m_instance = new GameObject().AddComponent<ADGlobalSystem>();
                    _m_instance.name = "GlobalSystem"; 
                }
                return _m_instance;
            }
        }

        public ADUI _Toggle, _Slider, _Text, _Button,_RawImage,_InputField;
        public PropertyModule _VirtualJoystick;
        public ViewController _Image;
        public AudioSourceController _AudioSource;
        public CustomWindowElement _CustomWindowElement;

        public bool IsKeepObject = true;

        #region InputSystem

        public Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>> multipleInputController
            = new Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>>();

        private List<MulHitControl> mulHitControls = new List<MulHitControl>();

        private static void ReleaseThisFrameUpdate(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> key)
        {
            try
            {
                if (key.Key.All((P) => (!P.GetType().Equals(typeof(MulHitControl)) && P.wasReleasedThisFrame)))
                {
                    key.Value.TryGetValue(PressType.ThisFrameReleased, out var events);
                    events?.Invoke();
                }
            }
            catch (System.Exception ex)
            {
                AddError("ReleaseThisFrameUpdate(keys) keys=" + key.ToString() + " Exception:" + ex.StackTrace);
            }
        }
        private static void PressThisFrameUpdate(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> key)
        {
            try
            {
                if (key.Key.All((P) => (P.GetType().Equals(typeof(MulHitControl)) ? (P as MulHitControl).WasPressedThisFrame() : P.wasReleasedThisFrame)))
                {
                    key.Value.TryGetValue(PressType.ThisFramePressed, out var events);
                    events?.Invoke();
                }
            }
            catch (System.Exception ex)
            {
                AddError("PressThisFrameUpdate(key) key=" + key.ToString() + " Exception:" + ex.StackTrace);
            }
        }
        private static void PressButtonUpdate(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> key)
        {
            try
            {
                if (key.Key.All((P) => (!P.GetType().Equals(typeof(MulHitControl)) && P.isPressed)))
                {
                    key.Value.TryGetValue(PressType.JustPressed, out var events);
                    events?.Invoke();
                }
            }
            catch (System.Exception ex)
            {
                AddError("PressButtonUpdate(keys) keys=" + key.ToString() + " Exception:" + ex.StackTrace);
            }
        }

        public static RegisterInfo AddListener(ButtonControl key, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                List<ButtonControl> currentList = new List<ButtonControl> { key };
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                instance.multipleInputController.Add(currentList, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                AddMessage(key.ToString() + "-based event was established");

                instance._IsOnValidate = true;
                return new RegisterInfo(currentList, action, type);
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
                instance._IsOnValidate = true;
                return new RegisterInfo(pair.Key, action, type);
            }
        }
        public static RegisterInfo AddListener(List<ButtonControl> keys, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });

            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                if (keys.FindAll((P) => P == keys[0]).Count == keys.Count)
                {
                    List<ButtonControl> ckeys = new List<ButtonControl> { new MulHitControl(keys.Count, keys[0]) };
                    instance.mulHitControls.Add(ckeys[0] as MulHitControl);

                    instance.multipleInputController.Add(ckeys, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                    AddMessage(new MulHitControl(keys.Count, keys[0]).ToString() + "-based event was established");
                }
                else
                {
                    instance.multipleInputController.Add(keys, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                    AddMessage(keys.ToString() + "-based event was established");
                }

                instance._IsOnValidate = true;
                return new RegisterInfo(keys, action, type);
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
                instance._IsOnValidate = true;
                return new RegisterInfo(pair.Key, action, type);
            }

        }
        public static void RemoveListener(ButtonControl key, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveListener(action);
            }
            instance._IsOnValidate = true; 
        }
        public static void RemoveListener(List<ButtonControl> keys, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveListener(action);
            }
            if (keys.FindAll((P) => P == keys[0]).Count == keys.Count)
            {
                var temp = instance.mulHitControls.Find((P) => P.TargetButton == keys[0]);
                RemoveListener(temp, action, type);
                instance.mulHitControls.Remove(temp);
            }
            instance._IsOnValidate = true; 
        }
        public static void RemoveAllListeners(ButtonControl key, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveAllListeners();
            }
            instance._IsOnValidate = true; 
        }
        public static void RemoveAllListeners(List<ButtonControl> keys, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveAllListeners();
            }
            if (keys.FindAll((P) => P == keys[0]).Count == keys.Count)
            {
                var temp = instance.mulHitControls.Find((P) => P.TargetButton == keys[0]);
                RemoveAllListeners(temp, type);
                instance.mulHitControls.Remove(temp);
            }
            instance._IsOnValidate = true; 
        }
        public static void RemoveAllButtonListeners()
        {
            instance.multipleInputController = new Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>>(); 
        }

        #endregion

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/GlobalSystem", false, 10)]
        public static void ADD(MenuCommand menuCommand)
        {
            if (instance != null)
            {
                Selection.activeObject = instance.gameObject;
                return;
            }
            AD.ADGlobalSystem obj = new GameObject("GlobalSystem").AddComponent<AD.ADGlobalSystem>();
            _m_instance = obj;
            GameObjectUtility.SetParentAndAlign(obj.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj.gameObject, "Create " + obj.name);
            Selection.activeObject = obj.gameObject;
        }
#endif

        #region MonoFunction

        private void Awake()
        {
            if (_m_instance != null) Destroy(this);
            else
            {
                _m_instance = this;
                AddMessage(gameObject.name + " is register ADGlobalSystem", "Build");
            }
        }

        private void Update()
        {
            foreach (var key in mulHitControls) key.Update();
            foreach (var key in multipleInputController)
            {
                PressButtonUpdate(key);
                PressThisFrameUpdate(key);
                ReleaseThisFrameUpdate(key);
            }
            if (record.Count > 10000)
            {
                SaveRecord();
            }
        }

        public bool _IsOnValidate = false;

        private void OnValidate()
        {
            _IsOnValidate = true;
        } 

        public void OnApplicationQuit()
        {
            if (!IsKeepObject) SaveRecord();
        }

        private void OnDestroy()
        {
            if(!IsKeepObject) _m_instance = null;
        }

        #endregion

        #region UtilityFunction

        public static void Output<T>(string filePath, T obj)
        {
            if (obj == null)
            {
                AddMessage("Failed Output " + filePath);
                return;
            }
            FileC.CreateDirectroryOfFile(filePath);
            if (typeof(T).Equals(typeof(string)))
            {
                File.WriteAllText(filePath, obj as string, Encoding.UTF8);
            }
            else if (obj.GetType().IsPrimitive)
            {
                File.WriteAllText(filePath, obj.ToString(), Encoding.UTF8);
            }
            else if (obj.GetType().GetAttribute<EaseSave3Attribute>() != null)
            {
                ES3.Save(filePath.Split('.')[^1], obj, filePath);
            }
            else if (obj.GetType().GetAttribute<SerializableAttribute>() != null)
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(obj), Encoding.UTF8);
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    byte[] bytes = ms.GetBuffer();
                    File.Create(filePath);
                    File.WriteAllBytes(filePath, bytes);
                }
            }
            AddMessage("Output " + filePath);
        }

        public static bool Input(string filePath, out string str)
        {
            if (FileC.GetDirectroryOfFile(filePath) == null)
            {
                str = "";
                return false;
            }
            else
            {
                try
                {
                    str = File.ReadAllText(filePath, Encoding.UTF8);
                    return true;
                }
                catch
                {
                    str = "";
                    return false;
                }
            }
        }

        public static bool Input<T>(string filePath, out object obj)
        {
            if (FileC.GetDirectroryOfFile(filePath) == null)
            {
                obj = default(T);
                return false;
            }
            else if (typeof(T).IsPrimitive)
            {
                try
                {
                    obj = typeof(T).GetMethod("Parse").Invoke(File.ReadAllText(filePath, Encoding.UTF8), null);
                    return true;
                }
                catch
                {
                    obj = default(T);
                    return false;
                }
            }
            else if (typeof(T).GetAttribute<EaseSave3Attribute>() != null)
            {
                try
                {
                    obj = ES3.Load(Path.GetFileNameWithoutExtension(filePath), filePath);
                    if (obj != null) return true;
                    else return false;
                }
                catch
                {
                    obj = default(T);
                    return false;
                }
            }
            else if (typeof(T).GetAttribute<SerializableAttribute>() != null)
            {
                try
                {
                    obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath, Encoding.UTF8));
                    if (obj != null) return true;
                    else return false;
                }
                catch
                {
                    obj = default(T);
                    return false;
                }
            }
            else
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        obj = formatter.Deserialize(ms);
                    }
                    if (obj != null) return true;
                    else return false;
                }
                catch
                {
                    obj = default(T);
                    return false;
                }
            }
        }

        public static void MoveFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建 
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception(" public static void MoveFolder(string sourcePath, string destPath),Target Directory fail to create" + ex.Message);
                        Debug.LogWarning("public static void MoveFolder(string sourcePath, string destPath),Target Directory fail to create" + ex.Message);
                        return;
                    }
                }
                //获得源文件下所有文件 
                List<string> files = new(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //覆盖模式 
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    File.Move(c, destFile);
                });
                //获得源文件下所有目录文件 
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));

                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //Directory.Move必须要在同一个根目录下移动才有效，不能在不同卷中移动。 
                    //Directory.Move(c, destDir); 

                    //采用递归的方法实现 
                    MoveFolder(c, destDir);
                });
            }
            else
            {
                //throw new Exception(" public static void MoveFolder(string sourcePath, string destPath),sourcePath cannt find");
                Debug.Log("public static void MoveFolder(string sourcePath, string destPath),sourcePath cannt find");
            }
        }

        public static void CopyFilefolder(string sourceFilePath, string targetFilePath)
        {
            //获取源文件夹中的所有非目录文件
            string[] files = Directory.GetFiles(sourceFilePath);
            string fileName;
            string destFile;
            //如果目标文件夹不存在，则新建目标文件夹
            if (!Directory.Exists(targetFilePath))
            {
                Directory.CreateDirectory(targetFilePath);
            }
            //将获取到的文件一个一个拷贝到目标文件夹中 
            foreach (string s in files)
            {
                fileName = Path.GetFileName(s);
                destFile = Path.Combine(targetFilePath, fileName);
                File.Copy(s, destFile, true);
            }
            //上面一段在MSDN上可以看到源码 

            //获取并存储源文件夹中的文件夹名
            string[] filefolders = Directory.GetFiles(sourceFilePath);
            //创建Directoryinfo实例 
            DirectoryInfo dirinfo = new DirectoryInfo(sourceFilePath);
            //获取得源文件夹下的所有子文件夹名
            DirectoryInfo[] subFileFolder = dirinfo.GetDirectories();
            for (int j = 0; j < subFileFolder.Length; j++)
            {
                //获取所有子文件夹名 
                string subSourcePath = sourceFilePath + "\\" + subFileFolder[j].ToString();
                string subTargetPath = targetFilePath + "\\" + subFileFolder[j].ToString();
                //把得到的子文件夹当成新的源文件夹，递归调用CopyFilefolder
                CopyFilefolder(subSourcePath, subTargetPath);
            }
        }

        public static void CopyFile(string sourceFile, string targetFilePath)
        {
            File.Copy(sourceFile, targetFilePath, true);
        }

        public static void DeleteFile(string sourceFile)
        {
            File.Delete(sourceFile);
        }

        #endregion

        #region UtilityRecord

        public List<UtilityPackage> record = new List<UtilityPackage>();

        public static void AddMessage(string message, string state = "")
        {
            if (instance.record.Count > 0 && instance.record[^1].message == message && instance.record[^1].state == state)
            {
                instance.record[^1].times++;
            }
            UtilityPackage cMessage = new UtilityPackage(message, state);
            instance.record.Add(cMessage);
            if (state != "") Debug.Log(cMessage.ObtainResult());
        }

        public static void AddWarning(string message, string state = "Warning")
        {
            if (instance.record.Count > 0 && instance.record[^1].message == message && instance.record[^1].state == state)
            {
                instance.record[^1].times++;
            }
            UtilityPackage cMessage = new UtilityPackage(message, state);
            instance.record.Add(cMessage);
            if (state == "Warning") Debug.LogWarning(cMessage.ObtainResult());
        }

        public static void AddError(string message, string state = "Error")
        {
            if (instance.record.Count > 0 && instance.record[^1].message == message && instance.record[^1].state == state)
            {
                instance.record[^1].times++;
            }
            UtilityPackage cMessage = new UtilityPackage(message, state);
            instance.record.Add(cMessage);
            if (state == "Error") Debug.LogError(cMessage.ObtainResult()); 
        }

        public string ObtainResultAndClean()
        {
            string result = "<Result>" + DateTime.Now.ToString() + "\n";
            foreach (var item in record) result += item.ObtainResult();
            record = new List<UtilityPackage>();
            Debug.Log("Record is clean");
            return result;
        }

        public void SaveRecord()
        {
            if (record.Count > 0)
            {
                string allMessage = ObtainResultAndClean();

                Output((RecordPath == "null") ? (Path.Combine(
                        Application.persistentDataPath,
                        "ADRecordlog",
                        DateTime.Now.Hour.ToString() + "H" +
                        DateTime.Now.Minute.ToString() + "M" +
                        DateTime.Now.Second.ToString() + "S" + ".AD.log")) : (RecordPath), allMessage);
            }
        }

        public static bool IsKeepException = true;

        public static T Error<T>(string message, Exception ex, T result) where T : class, new()
        {
            if (IsKeepException) throw new ADException(message, ex);
            AddError(message + "\nError: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            return result;
        }
        public static T Error<T>(string message, Exception ex = null) where T : class, new()
        {
            if (IsKeepException) throw new ADException(message, ex);
            AddError(message + "\nError: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            return default(T);
        }
        public static T Error<T>(string message) where T : class, new()
        {
            if (IsKeepException) throw new ADException(message);
            AddError(message);
            return default(T);
        }

        public static bool Error(string message, bool result = false, Exception ex = null)
        {
            if (IsKeepException) throw new ADException(message, ex);
            AddError(message);
            return result;
        }

        public static void TrackError(string message, System.Exception ex)
        {
            //utility.AddError("\nMessage: " + message + "\nError: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            Error<object>("\nMessage: " + message + "\nError: " + ex.Message + "\nStackTrace: " + ex.StackTrace, ex); 
        }

        public static T FinalCheck<T>(T result, string message = "you obtain a null object")
        {
            if (result == null) AddError(message);
            return result;
        }

        public static void FunctionalRecord<T>(T func)
        {
            AddMessage(func.ToString());
        }

        public string RecordPath { get; set; } = "null";

        #endregion
    }

    public static class MethodBaseExtension
    { 
        public static MethodBase TrackError(this MethodBase method, System.Exception ex)
        {
            var att = method.GetAttribute<ADAttribute>();
            if (att == null)
            {
                ADGlobalSystem.AddWarning(method.Name + " not has an Attribute(User)");
                ADGlobalSystem.AddError(method.Name + "\n" + ex.Message);
                return method;
            }
            ADGlobalSystem.TrackError((att.message == "") ? method.Name : att.message, ex);
            return method;
        }

        public static object SafeTrackError(this MethodBase method, System.Exception ex)
        {
            var att = method.GetAttribute<ADAttribute>();
            ADGlobalSystem.TrackError((att.message == "") ? method.Name : att.message, ex);
            return System.Activator.CreateInstance(att.type);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ADGlobalSystem))]
    public class GlobalSystemEditor : Editor
    {
        ADGlobalSystem that = null;

        List<string> buttons = new List<string>();

        private SerializedProperty _Toggle, _Slider, _Text, _Button, _RawImage, _InputField;
        private SerializedProperty _VirtualJoystick;
        private SerializedProperty _Image;
        private SerializedProperty _AudioSource;
        private SerializedProperty _CustomWindowElement; 

        private SerializedProperty _IsKeepObject;

        SerializedProperty record = null;

        private void OnEnable()
        {
            that = target as ADGlobalSystem;

            buttons = new List<string>();
            foreach (var key in that.multipleInputController)
            {
                foreach (var button in key.Key)
                {
                    buttons.Add(button.ToString() + "  ");
                }
            }
            that._IsOnValidate = false;

            _Toggle = serializedObject.FindProperty("_Toggle");
            _Slider = serializedObject.FindProperty("_Slider");
            _Text = serializedObject.FindProperty("_Text");
            _Button = serializedObject.FindProperty("_Button");
            _RawImage = serializedObject.FindProperty("_RawImage");
            _InputField = serializedObject.FindProperty("_InputField");
            _VirtualJoystick = serializedObject.FindProperty("_VirtualJoystick");
            _Image = serializedObject.FindProperty("_Image");
            _AudioSource = serializedObject.FindProperty("_AudioSource");

            _CustomWindowElement = serializedObject.FindProperty("_CustomWindowElement");

            record = serializedObject.FindProperty("record");

            _IsKeepObject = serializedObject.FindProperty("IsKeepObject");
        }

        public override void OnInspectorGUI()
        {
            if (that._IsOnValidate)
            {
                buttons = new List<string>();
                foreach (var key in that.multipleInputController)
                {
                    foreach (var button in key.Key)
                    {
                        buttons.Add(button.ToString() + "  ");
                    }
                }
                that._IsOnValidate = false;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(_IsKeepObject);

            if (Application.isEditor)
            { 
                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(_Toggle);
                EditorGUILayout.PropertyField(_Slider);
                EditorGUILayout.PropertyField(_Text);
                EditorGUILayout.PropertyField(_Button);
                EditorGUILayout.PropertyField(_RawImage);
                EditorGUILayout.PropertyField(_InputField);
                EditorGUILayout.PropertyField(_VirtualJoystick);
                EditorGUILayout.PropertyField(_Image);
                EditorGUILayout.PropertyField(_AudioSource);
                EditorGUILayout.PropertyField(_CustomWindowElement);
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.Space(25);

                EditorGUILayout.PropertyField(record);

                EditorGUILayout.Space(25);

                if (buttons.Count == 0) EditorGUILayout.TextArea("No Event was register");
                else foreach (var key in buttons) EditorGUILayout.TextArea(key);

                if (GUILayout.Button("SaveRecord"))
                {
                    that.SaveRecord();
                }
            }

            UnityEngine.Object @object = null;

            EditorGUI.BeginChangeCheck();
            ADGlobalSystem temp_cat = null;
            GUIContent gUIContent = new GUIContent("Instance");
            temp_cat = EditorGUILayout.ObjectField(gUIContent, ADGlobalSystem._m_instance as UnityEngine.Object, typeof(ADGlobalSystem), @object) as ADGlobalSystem;
            if (EditorGUI.EndChangeCheck()) ADGlobalSystem._m_instance = temp_cat;

            EditorGUI.BeginChangeCheck();
            GUIContent bUIContent = new GUIContent("IsKeepException");
            bool __IsKeepException = EditorGUILayout.Toggle(bUIContent, ADGlobalSystem.IsKeepException);
            if (EditorGUI.EndChangeCheck()) ADGlobalSystem.IsKeepException = __IsKeepException;

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Method)]
    public class ADAttribute : Attribute
    {
        public ADAttribute() { }
        public ADAttribute(string message) { this.message = message; }
        public ADAttribute(System.Type type) { this.type = type; }
        public ADAttribute(string message, System.Type type) { this.message = message; this.type = type; }

        public string message = "";
        public System.Type type = null;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EaseSave3Attribute : Attribute
    {
    }

    [Serializable]
    public class UtilityPackage
    {
        public string currentTime;
        public string message;
        public string state;
        public int times;

        public UtilityPackage(string message, string state = "")
        {
            currentTime = DateTime.Now.ToString();
            this.message = message;
            this.state = state;
            times = 1;
        }

        public string ObtainResult()
        {
            return ((times == 1) ? "" : ("#" + times.ToString() + "=>")) + "[ " + currentTime + "  " + state + " ] " + message + "\n";
        }
    }

}
