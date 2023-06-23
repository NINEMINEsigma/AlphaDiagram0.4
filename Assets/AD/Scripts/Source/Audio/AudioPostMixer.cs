using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AD.Utility
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPostMixer : MonoBehaviour
    {
        private AudioSource SelfSource = null;
        public AudioSourcePackage Package = new AudioSourcePackage();

        void Start()
        {
            SelfSource = transform.GetComponent<AudioSource>();
            SelfSource.SetSpeed(Package);
        }

        public void SetSpeed(float speed)
        {
            Package.Speed = speed;
            SelfSource.SetSpeed(Package);
        }
    }
}