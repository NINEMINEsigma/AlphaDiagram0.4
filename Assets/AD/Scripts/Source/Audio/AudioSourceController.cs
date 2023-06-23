using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace AD.Utility
{
    [Serializable]
    public class SourcePair
    {
        public AudioClip Clip = null;
        public SourcePair ChangeClip(AudioClip newClip)
        {
            Clip = newClip;
            return this;
        }
        public string Name = "New Pair";
        public string CilpName = "New Clip";

        public bool IsLoaded
        {
            get
            {
                if (Clip != null)
                    return Clip.loadState == AudioDataLoadState.Loaded;
                else return false;
            }
        }
    }

    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioSourceController : MonoBehaviour, IAudioSourceController
    {
        #region Attribute

        public AudioSource Source { get; private set; }
        public List<SourcePair> SourcePairs = new List<SourcePair>();
        private int CurrentPairIndex = 0;

        public SourcePair CurrentSourcePair
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
        public AudioClip CurrentClip
        {
            get
            {
                if (SourcePairs.Count > 0) return SourcePairs[CurrentPairIndex].Clip;
                else return null;
            }
            set
            {
                if (SourcePairs.Count > 0) SourcePairs[CurrentPairIndex].Clip = value;
                else Debug.LogWarning("this AudioSource's list of Source is empty,but you try to change it");
            }
        }
        public int CurrentIndex
        {
            get { return CurrentPairIndex; }
        }
        public bool IsPlay
        {
            get { return Source.isPlaying; }
            set
            {
                if (SourcePairs.Count > 0)
                {
                    if (value)
                        Source.Play();
                    else
                        Source.Pause();
                }
            }
        }
        public float CurrentTime
        {
            get { return Source.time; }
            set { Source.time = value; }
        }

        [SerializeField] private AudioPostMixer _Mixer = null;
        public AudioPostMixer Mixer
        {
            get { return _Mixer; }
        }
        public bool IsHavePostMixer { get { return Mixer != null; } }
        public bool IsHaveMixer { get { return Mixer.Package.audioMixer != null; } }

        public bool LoopAtAll = true;

        #endregion 

        private void Start()
        {
            Source = GetComponent<AudioSource>();
            AD.SceneSingleAssets.audioSourceControllers.Add(this);
        }


        public AudioSourceController NextPair()
        {
            if (CurrentPairIndex < SourcePairs.Count) CurrentPairIndex++;
            else CurrentPairIndex = 0;
            if (Source.isPlaying) Source.clip = CurrentClip;
            return this;
        }
        public AudioSourceController PreviousPair()
        {
            if (CurrentPairIndex > 0) CurrentPairIndex--;
            else CurrentPairIndex = SourcePairs.Count;
            if (Source.isPlaying) Source.clip = CurrentClip;
            return this;
        }
        public AudioSourceController RandomPair()
        {
            CurrentPairIndex = UnityEngine.Random.Range(0, SourcePairs.Count);
            if (Source.isPlaying) Source.clip = CurrentClip;
            return this;
        }

        public AudioSourceController Play()
        {
            Source.Play();
            StartCoroutine(ClockWithCilp(CurrentClip, Source.time));
            return this;
        }
        public AudioSourceController Stop()
        {
            Source.Stop();
            StopCoroutine(nameof(ClockWithCilp));
            return this;
        }
        public AudioSourceController Pause()
        {
            Source.Pause();
            StopCoroutine(nameof(ClockWithCilp));
            return this;
        }

        public AudioSourceController IgnoreListenerPause()
        {
            Source.ignoreListenerPause = true;
            return this;
        }
        public AudioSourceController SubscribeListenerPause()
        {
            Source.ignoreListenerPause = false;
            return this;
        }

        public AudioSourceController IgnoreListenerVolume()
        {
            Source.ignoreListenerVolume = true;
            return this;
        }
        public AudioSourceController SubscribeListenerVolume()
        {
            Source.ignoreListenerVolume = false;
            return this;
        }

        public AudioSourceController SetLoop()
        {
            Source.loop = true;
            StopCoroutine(nameof(ClockWithCilp));
            return this;
        }
        public AudioSourceController UnLoop()
        {
            Source.loop = false;
            StartCoroutine(ClockWithCilp(CurrentClip, Source.time));
            return this;
        }

        public AudioSourceController SetLoopAtAll()
        {
            LoopAtAll = true;
            return this;
        }
        public AudioSourceController UnLoopAtAll()
        {
            LoopAtAll = false;
            return this;
        }

        public AudioSourceController SetMute()
        {
            Source.mute = true;
            return this;
        }
        public AudioSourceController CancelMute()
        {
            Source.mute = false;
            return this;
        }

        public AudioSourceController SetPitch(float pitch)
        {
            Source.pitch = pitch;
            return this;
        }

        public AudioSourceController SetSpeed(float speed)
        {
            if (_Mixer != null) _Mixer.SetSpeed(speed);
            else
            {
                Debug.LogWarning("you try to change an AudioSource's speed without AudioMixer, which will cause it to change its pitch");
                SetPitch(speed);
            }
            return this;
        }

        public AudioSourceController SetVolume(float volume)
        {
            Source.volume = volume;
            return this;
        }

        public AudioSourceController SetPriority(int priority)
        {
            Source.priority = priority;
            return this;
        }

        public AudioSourceController RandomPairs()
        {
            SourcePairs.Sort((T, P) => { if (UnityEngine.Random.Range(-1, 1) > 0) return 1; else return -1; });
            return this;
        }

        public void PrepareToOtherScene()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(ClockOnJump());
        }

        private IEnumerator ClockWithCilp(AudioClip clip, float current)
        {
            for (float duration = clip.length + 0.2f, clock = current; clock < duration; clock += UnityEngine.Time.deltaTime)
                yield return new WaitForEndOfFrame();
            Source.Stop();
            NextPair();
            if (LoopAtAll)
            {
                Source.Play();
                StartCoroutine(ClockWithCilp(CurrentClip, 0));
            }
        }

        private IEnumerator ClockOnJump()
        {
            for (float now = 0; now < 1; now += UnityEngine.Time.deltaTime)
            {
                this.SetVolume(1 - now);
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }
    }

    /*[CustomEditor(typeof(AudioSourceController))]
    public class ASCEditor : Editor
    {
        private AudioSourceController that = null;

        private void OnEnable()
        {
            that = target as AudioSourceController;
        }
    }*/


}