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
        [SerializeField] Image A;
        [SerializeField] Image B;

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
            for (float t = 0; t < 1; t+=UnityEngine.Time.deltaTime)
            {
                if (A != null) A.color=new Color(A.color.r, A.color.g, A.color.b,1 - t);
                if (B != null) B.color = new Color(B.color.r, B.color.g, B.color.b, 1 - t);
                yield return new WaitForEndOfFrame();
            }
            A.gameObject.SetActive(false);
            B.gameObject.SetActive(false) ;
        }
    } 
}