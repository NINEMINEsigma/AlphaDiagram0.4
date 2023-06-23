using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using AD.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace AD.MainScene
{
    public class MainSceneManager : AD.SceneBaseController
    {
        [Header("Assets")]
        [SerializeField] List<Image> Images = new List<Image>();

        private void Start()
        { 
            MainApp.ADinstance.RegisterController(this);

            SceneSingleAssets.CoroutineWorker.StartCoroutine(BGInit());
        }

        public override IADArchitecture ADinstance()
        { 
            return MainApp.ADinstance;
        }

        private IEnumerator BGInit()
        {
            foreach (var image in Images) image.gameObject.SetActive(true); 
            for (float t = 0; t < 1; t+=UnityEngine.Time.deltaTime)
            { 
                foreach (var image in Images) image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - t);
                yield return new WaitForEndOfFrame();
            }
            foreach (var image in Images) image.gameObject.SetActive(false);
        }
    } 
}