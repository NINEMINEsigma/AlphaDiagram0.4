using System.Collections;
using System.Collections.Generic;
using AD.UI;
using UnityEngine;

namespace AD.MainScene
{
    [RequireComponent(typeof(AudioSourceController))]
    public class SingleSoundPlayer : MonoBehaviour
    {
        private AudioSourceController _source;
        public AudioSourceController source
        {
            get
            {
                _source ??= GetComponent<AudioSourceController>();
                return _source;
            }
        }
    }
}
