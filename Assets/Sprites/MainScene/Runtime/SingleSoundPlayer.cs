using System.Collections;
using System.Collections.Generic;
using AD.UI;
using UnityEngine;
using AD.BASE;
using System.Linq;

namespace AD.MainScene
{
    [RequireComponent(typeof(AudioSourceController))]
    public class SingleSoundPlayer : MonoBehaviour
    {
        [Header("Asset")]
        private AudioSourceController _source;
        public AudioSourceController source
        {
            get
            {
                _source ??= GetComponent<AudioSourceController>();
                return _source;
            }
        }

        public void Init(int uid)
        {
            source.SourcePairs = MainApp.instance.GetSystem<MainGroupController>().SourcePairs.FirstOrDefault(T => T.GUID == uid).SourcePairs;
            Refresh(0);
        }

        public void Refresh(int current)
        {
            //PlaySound
            PlaySound(current);
        }

        public void PlaySound(int current = -1)
        {
            if (current == -1)
            {
                source.Play();
                return;
            }
            source.SetPair(current);
            source.Play();
        }
    }
}
