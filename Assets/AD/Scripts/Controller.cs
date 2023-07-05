using System;
using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using AD.UI;
using AD.Utility;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AD
{
    [Serializable]
    public abstract class BaseController : AD.ADbase.ADController
    {
        public abstract override IADArchitecture ADinstance(); 

        public override void Init()
        {
            mCanvasInitializer.Initialize();
        }

        [Header("BaseController")]
        [SerializeField] private AD.UI.CanvasInitializer mCanvasInitializer = new AD.UI.CanvasInitializer();
    }
    [Serializable]
    public abstract class BaseController<T> : AD.ADbase.ADController
    {
        public abstract override IADArchitecture ADinstance();

        public override void Init()
        {
            mCanvasInitializer.Initialize();
        }

        [Header("BaseController")]
        [SerializeField] private AD.UI.CanvasInitializer mCanvasInitializer = new AD.UI.CanvasInitializer();
        public ADEvent<T> OnEvent = new ADEvent<T>();
    }
    [Serializable]
    public abstract class BaseController<T1, T2> : AD.ADbase.ADController
    {
        public abstract override IADArchitecture ADinstance();

        public override void Init()
        {
            mCanvasInitializer.Initialize();
        }

        [Header("BaseController")]
        [SerializeField] private AD.UI.CanvasInitializer mCanvasInitializer = new AD.UI.CanvasInitializer();
        public ADEvent<T1, T2> OnEvent = new ADEvent<T1, T2>();
    }
    [Serializable]
    public abstract class BaseController<T1, T2, T3> : AD.ADbase.ADController
    {
        public abstract override IADArchitecture ADinstance();

        public override void Init()
        {
            mCanvasInitializer.Initialize();
        }

        [Header("BaseController")]
        [SerializeField] private AD.UI.CanvasInitializer mCanvasInitializer = new AD.UI.CanvasInitializer();
        public ADEvent<T1, T2, T3> OnEvent = new ADEvent<T1, T2, T3>();
    }
    [Serializable]
    public abstract class BaseController<T1, T2, T3, T4> : AD.ADbase.ADController
    {
        public abstract override IADArchitecture ADinstance();

        public override void Init()
        {
            mCanvasInitializer.Initialize();
        }

        [Header("BaseController")]
        [SerializeField] private AD.UI.CanvasInitializer mCanvasInitializer = new AD.UI.CanvasInitializer();
        public ADEvent<T1, T2, T3, T4> OnEvent = new ADEvent<T1, T2, T3, T4>();
    }

    public interface ISceneSingleController
    {

    }
    public interface IAudioSourceController : IADUI
    {
        void PrepareToOtherScene();
    }
    public interface IViewController : IADUI
    {
        void PrepareToOtherScene();
    }
    public interface IADInputSystem
    {

    }

    [Serializable]
    public abstract class SceneBaseController : BaseController, ISceneSingleController
    {
        [Header("SceneBaseController")]
        public string TargetSceneName = "";

        protected virtual void Awake()
        {
            SceneSingleAssets.Init();
            base.Init();

            InitFormLastSceneInfo();

            if (TargetSceneName == "") TargetSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            SceneSingleAssets.instence = this;
            OnSceneStart.Invoke();
        }

        protected virtual void InitFormLastSceneInfo(object info)
        {

        }

        public void InitFormLastSceneInfo()
        {
            InitFormLastSceneInfo(SceneSingleAssets.infomation);
        }

        public virtual void OnEnd()
        {
            foreach (var source in SceneSingleAssets.audioSourceControllers) source.PrepareToOtherScene();

            OnSceneEnd.Invoke();


            HowToLoadScene();
        }

        protected virtual void HowToLoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(TargetSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public ADEvent OnSceneStart = new ADEvent(), OnSceneEnd = new ADEvent();
    }

    public class CoroutineWorkerMono:MonoBehaviour
    {
        
    }

    public static class SceneSingleAssets
    {
        public static ISceneSingleController instence = null;
        public static List<IAudioSourceController> audioSourceControllers = new List<IAudioSourceController>();
        public static List<IViewController> viewControllers = new List<IViewController>();
        public static List<IADInputSystem> inputSystems = new List<IADInputSystem>();
        public static object infomation = null;
        public static MonoBehaviour CoroutineWorker = null;

        public static void Init()
        {
            instence = null;
            audioSourceControllers = new List<IAudioSourceController>();
            viewControllers = new List<IViewController>();
            inputSystems = new List<IADInputSystem>();
            if (CoroutineWorker == null && instence == null)
            {
                CoroutineWorker = new GameObject().AddComponent<CoroutineWorkerMono>();
                CoroutineWorker.name = "CoroutineWorker(SingleAssets)";
            }
            else
            {
                CoroutineWorker = instence as MonoBehaviour;
            }
        }
    }



}
