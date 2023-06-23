using System;
using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using AD.Utility;
using UnityEngine;
using UnityEngine.UI;

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

    [RequireComponent(typeof(Image))]
    public sealed class ViewController : BaseController, IViewController
    {
        #region Attribute 
        public Image ViewImage { get; private set; }

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

        #endregion 

        private void Awake()
        {
            ViewImage = GetComponent<Image>();
            ViewImage.sprite = CurrentImage;
        }

        public override IADArchitecture ADinstance()
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
            if (CurrentPairIndex < SourcePairs.Count) CurrentPairIndex++;
            else CurrentPairIndex = 0;
            ViewImage.sprite = CurrentImage;
            return this;
        }
        public ViewController PreviousPair()
        {
            if (SourcePairs.Count == 0) return this;
            if (CurrentPairIndex > 0) CurrentPairIndex--;
            else CurrentPairIndex = SourcePairs.Count;
            ViewImage.sprite = CurrentImage;
            return this;
        }
        public ViewController RandomPair()
        {
            if (SourcePairs.Count == 0) return this;
            CurrentPairIndex = UnityEngine.Random.Range(0, SourcePairs.Count);
            ViewImage.sprite = CurrentImage;
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
    }
}