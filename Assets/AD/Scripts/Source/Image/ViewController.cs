using System;
using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using AD.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Codice.Client.BaseCommands.Import.Commit;

namespace AD.UI
{
    public interface ICanGetArchitecture
    {
        IADArchitecture ADinstance();
    }

    public interface ICanPrepareToOtherScenee
    {
        void PrepareToOtherScenes(ViewController viewC);
    }

    public interface ICanTransformSprite
    {
        void TransformSprite(Sprite target, Image image);
    }

    [Serializable]
    public class ImagePair
    {
        public Sprite SpriteSource = null;
        public ImagePair ChangeSprite(Sprite newSprite)
        {
            SpriteSource = newSprite;
            return this;
        }
        public string Name = "New Pair";
        public string SpriteName = "New Sprite";
    }

    [Serializable]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("UI/AD/ViewController", 100)]
    public sealed class ViewController : ADUI, IViewController,IADController
    {
        #region Attribute 

        private Image _ViewImage;
        public Image ViewImage
        {
            get
            { 
                if (_ViewImage == null) _ViewImage = GetComponent<Image>();
                return _ViewImage;
            }
            private set
            {
                _ViewImage = value;
            }
        }

        public List<ImagePair> SourcePairs = new List<ImagePair>();
        private int CurrentPairIndex = 0;

        public ImagePair CurrentImagePair
        {
            get
            {
                if (SourcePairs.Count > 0) return SourcePairs[CurrentPairIndex];
                else return null;
            }
            set
            {
                SourcePairs[CurrentPairIndex] = value;
            }
        }
        public Sprite CurrentImage
        {
            get
            {
                if (SourcePairs.Count > 0) return SourcePairs[CurrentPairIndex].SpriteSource;
                else return null;
            }
            set
            {
                if (SourcePairs.Count > 0) SourcePairs[CurrentPairIndex].SpriteSource = value;
                else Debug.LogWarning("this Image's list of Source is empty,but you try to change it");
            }
        }
        public int CurrentIndex
        {
            get { return CurrentPairIndex; }
        }

        [HideInInspector] public ICanGetArchitecture architectureObtainer = null;
        [HideInInspector] public ICanPrepareToOtherScenee canPrepareToOtherScenee = null;
        [HideInInspector] public ICanTransformSprite canTransformSprite = null;

        #endregion

        #region Function

        public ViewController()
        {
            ElementArea = "Image"; 
        }

        private void Start()
        {
            AD.UI.ADUI.Initialize(this);
            AD.SceneSingleAssets.viewControllers.Add(this);

            ViewImage = GetComponent<Image>();
            ViewImage.sprite = CurrentImage; 
        }
        private void OnDestroy()
        { 
            AD.UI.ADUI.Destory(this);
            AD.SceneSingleAssets.viewControllers.Remove(this); 
        }

        [MenuItem("GameObject/AD/Image", false, 10)]
        private static void ADD(MenuCommand menuCommand)
        {
            AD.UI.ViewController obj = new GameObject("New Image").AddComponent<AD.UI.ViewController>();
            GameObjectUtility.SetParentAndAlign(obj.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj.gameObject, "Create " + obj.name);
            Selection.activeObject = obj.gameObject;
        }

        public static ViewController Generate(string name = "New Image", Transform parent = null, params System.Type[] components)
        {
            ViewController source = new GameObject(name, components).AddComponent<ViewController>();
            source.transform.parent = parent; 

            return source;
        }

        public IADArchitecture ADinstance()
        {
            if (architectureObtainer == null) return null;
            else return architectureObtainer.ADinstance();
        }

        public void PrepareToOtherScene()
        {
            canPrepareToOtherScenee.PrepareToOtherScenes(this);
        }

        public ViewController SetTransparentChannelCollisionThreshold(float value)
        {
            ViewImage.alphaHitTestMinimumThreshold = value;
            return this;
        }

        public ViewController SetMaterial(Material material)
        {
            ViewImage.material = material;
            return this;
        }

        public ViewController RandomPairs()
        {
            SourcePairs.Sort((T, P) => { if (UnityEngine.Random.Range(-1, 1) > 0) return 1; else return -1; });
            return this;
        }

        public ViewController NextPair()
        {
            if (SourcePairs.Count == 0) return this;
            if (CurrentPairIndex < SourcePairs.Count - 1) CurrentPairIndex++;
            else CurrentPairIndex = 0;
            if (canTransformSprite == null) ViewImage.sprite = CurrentImage;
            else canTransformSprite.TransformSprite(CurrentImage, ViewImage);
            return this;
        }
        public ViewController PreviousPair()
        {
            if (SourcePairs.Count == 0) return this;
            if (CurrentPairIndex > 0) CurrentPairIndex--;
            else CurrentPairIndex = SourcePairs.Count - 1;
            if (canTransformSprite == null) ViewImage.sprite = CurrentImage;
            else canTransformSprite.TransformSprite(CurrentImage, ViewImage);
            return this;
        }
        public ViewController RandomPair()
        {
            if (SourcePairs.Count == 0) return this;
            CurrentPairIndex = UnityEngine.Random.Range(0, SourcePairs.Count);
            if (canTransformSprite == null) ViewImage.sprite = CurrentImage;
            else canTransformSprite.TransformSprite(CurrentImage, ViewImage);
            return this;
        }

        public ViewController SetAlpha(float alpha)
        {
            ViewImage.color = new Color(ViewImage.color.r, ViewImage.color.g, ViewImage.color.b, alpha);
            return this;
        }
        public ViewController SetRed(float red)
        {
            ViewImage.color = new Color(red, ViewImage.color.g, ViewImage.color.b, ViewImage.color.a);
            return this;
        }
        public ViewController SetGreen(float green)
        {
            ViewImage.color = new Color(ViewImage.color.r, green, ViewImage.color.b, ViewImage.color.a);
            return this;
        }
        public ViewController SetBlue(float blue)
        {
            ViewImage.color = new Color(ViewImage.color.r, ViewImage.color.g, blue, ViewImage.color.a);
            return this;
        }

        public ViewController BakeAudioWaveformFormAudioCilp(AudioClip clip)
        {
            ViewImage.color = new Color();
            ViewImage.sprite = null;
            ViewImage.sprite = AudioSourceController.BakeAudioWaveform(clip).ToSprite();
            return this;
        }  

        #endregion

    }
}