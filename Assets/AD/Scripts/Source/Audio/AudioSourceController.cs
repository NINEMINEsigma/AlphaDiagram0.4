using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        public bool Sampling = true;

        #endregion

        #region Function

        private void Start()
        {
            Source = GetComponent<AudioSource>();
            AD.SceneSingleAssets.audioSourceControllers.Add(this);

            GetSampleCount();
        }

        private void OnValidate()
        {
            if (samples.Length != spectrumLength) samples = new float[spectrumLength];
            if (bands.Length != BandCount) bands = new float[BandCount];
            if (freqBands.Length != BandCount) freqBands = new float[BandCount];
            if (bandBuffers.Length != BandCount) bandBuffers = new float[BandCount];
            if (bufferDecrease.Length != BandCount) bufferDecrease = new float[BandCount];
            if (bandHighest.Length != BandCount) bandHighest = new float[BandCount];
            if (normalizedBands.Length != BandCount) normalizedBands = new float[BandCount];
            if (normalizedBandBuffers.Length != BandCount) normalizedBandBuffers = new float[BandCount];
            if (sampleCount.Length != BandCount) sampleCount = new int[BandCount];
        }

        private void Update()
        {
            if (Sampling)
            {
                GetSpectrums();
                GetFrequencyBands();
                GetNormalizedBands();
                GetBandBuffers(increasingType, decreasingType);
                BandNegativeCheck();
            }
        }

        public AudioSourceController NextPair()
        {
            if (SourcePairs.Count == 0) return this;
            if (CurrentPairIndex < SourcePairs.Count - 1) CurrentPairIndex++;
            else CurrentPairIndex = 0;
            if (Source.isPlaying) Source.clip = CurrentClip;
            return this;
        }
        public AudioSourceController PreviousPair()
        {
            if (SourcePairs.Count == 0) return this;
            if (CurrentPairIndex > 0) CurrentPairIndex--;
            else CurrentPairIndex = SourcePairs.Count - 1;
            if (Source.isPlaying) Source.clip = CurrentClip;
            return this;
        }
        public AudioSourceController RandomPair()
        {
            if (SourcePairs.Count == 0) return this;
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

        #endregion

        #region Inspector
        [Header("MusicSampler")]
        /// <summary>
        /// 这个参数用于设置进行采样的精度
        /// </summary>
        [Tooltip("采样精度")] public SpectrumLength SpectrumCount = SpectrumLength.Spectrum256;
        private int spectrumLength => (int)Mathf.Pow(2, ((int)SpectrumCount + 6));
        /// <summary>
        /// 这个属性返回采样得到的原始数据
        /// </summary>
        [Tooltip("原始数据")] public float[] samples = new float[64];
        private int[] sampleCount = new int[8];
        /// <summary>
        /// 这个参数用于设置将采样的结果分为几组进行讨论
        /// </summary>
        [Tooltip("拆分组数")] public uint BandCount = 8;
        /// <summary>
        /// 这个参数用于设置组别采样数值减小时使用的平滑策略
        /// </summary>
        [Tooltip("平衡采样值滑落的平滑策略")] public BufferDecreasingType decreasingType = BufferDecreasingType.Jump;
        /// <summary>
        /// 这个参数用于设置在Slide和Falling设置下，组别采样数值减小时每帧下降的大小。
        /// </summary>
        [Tooltip("Slide/Falling:采样值的滑落幅度")] public float decreasing = 0.003f;
        /// <summary>
        /// 这个参数用于设置在Falling设置下，组别采样数值减小时每帧下降时加速度的大小。
        /// </summary>
        [Tooltip("Falling:采样值滑落的加速度")] public float DecreaseAcceleration = 0.2f;
        /// <summary>
        /// 这个参数用于设置组别采样数值增大时使用的平滑策略
        /// </summary>
        [Tooltip("平衡采样值提升的平滑策略")] public BufferIncreasingType increasingType = BufferIncreasingType.Jump;
        /// <summary>
        /// 这个参数用于设置在Slide设置下，组别采样数值增大时每帧增加的大小。
        /// </summary>
        [Tooltip("Slide:采样值的提升幅度")] public float increasing = 0.003f;
        /// <summary>
        /// 这个属性返回经过平滑和平均的几组数据
        /// </summary>
        [Tooltip("经处理后的数据")] public float[] bands = new float[8];
        private float[] freqBands = new float[8];
        private float[] bandBuffers = new float[8];
        private float[] bufferDecrease = new float[8];
        /// <summary>
        /// 这个属性返回总平均采样结果
        /// </summary>
        public float average
        {
            get
            {
                float average = 0;
                for (int i = 0; i < BandCount; i++)
                {
                    average += normalizedBands[i];
                }
                average /= BandCount;
                return average;
            }
        }

        private float[] bandHighest = new float[8];
        /// <summary>
        /// 这个属性返回经过平滑、平均和归一化的几组数据
        /// </summary>
        [Tooltip("经过平滑、平均和归一化的几组数据")] public float[] normalizedBands = new float[8];
        private float[] normalizedBandBuffers = new float[8];

        #endregion  

        #region Programs

        private void GetSampleCount()
        {
            float acc = (((float)((int)SpectrumCount + 6)) / BandCount);
            int sum = 0;
            int last = 0;
            for (int i = 0; i < BandCount - 1; i++)
            {
                int pow = (int)Mathf.Pow(2, acc * (i));
                sampleCount[i] = pow - sum;
                if (sampleCount[i] < last) sampleCount[i] = last;
                sum += sampleCount[i];
                last = sampleCount[i];
            }
            sampleCount[BandCount - 1] = samples.Length - sum;
        }

        private void GetSpectrums()
        {
            Source.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        }

        private void GetFrequencyBands()
        {
            int counter = 0;
            for (int i = 0; i < BandCount; i++)
            {
                float average = 0;
                for (int j = 0; j < sampleCount[i]; j++)
                {
                    average += samples[counter] * (counter + 1);
                    counter++;
                }
                average /= sampleCount[i];
                freqBands[i] = average * 10;
            }
        }

        private void GetNormalizedBands()
        {
            for (int i = 0; i < BandCount; i++)
            {
                if (freqBands[i] > bandHighest[i])
                {
                    bandHighest[i] = freqBands[i];
                }
            }
        }

        private void GetBandBuffers(BufferIncreasingType increasingType, BufferDecreasingType decreasingType)
        {
            for (int i = 0; i < BandCount; i++)
            {
                if (freqBands[i] > bandBuffers[i])
                {
                    switch (increasingType)
                    {
                        case BufferIncreasingType.Jump:
                            bandBuffers[i] = freqBands[i];
                            bufferDecrease[i] = decreasing;
                            break;
                        case BufferIncreasingType.Slide:
                            bufferDecrease[i] = decreasing;
                            bandBuffers[i] += increasing;
                            break;
                    }
                    if (freqBands[i] < bandBuffers[i]) bandBuffers[i] = freqBands[i];
                }
                if (freqBands[i] < bandBuffers[i])
                {
                    switch (decreasingType)
                    {
                        case BufferDecreasingType.Jump:
                            bandBuffers[i] = freqBands[i];
                            break;
                        case BufferDecreasingType.Falling:
                            bandBuffers[i] -= decreasing;
                            break;
                        case BufferDecreasingType.Slide:
                            bandBuffers[i] -= bufferDecrease[i];
                            bufferDecrease[i] *= 1 + DecreaseAcceleration;
                            break;
                    }
                    if (freqBands[i] > bandBuffers[i]) bandBuffers[i] = freqBands[i]; ;
                }
                bands[i] = bandBuffers[i];
                if (bandHighest[i] == 0) continue;
                normalizedBands[i] = (freqBands[i] / bandHighest[i]);
                normalizedBandBuffers[i] = (bandBuffers[i] / bandHighest[i]);
                if (normalizedBands[i] > normalizedBandBuffers[i])
                {
                    switch (increasingType)
                    {
                        case BufferIncreasingType.Jump:
                            normalizedBandBuffers[i] = normalizedBands[i];
                            bufferDecrease[i] = decreasing;
                            break;
                        case BufferIncreasingType.Slide:
                            bufferDecrease[i] = decreasing;
                            normalizedBandBuffers[i] += increasing;
                            break;
                    }
                    if (normalizedBands[i] < normalizedBandBuffers[i]) normalizedBandBuffers[i] = normalizedBands[i];
                }
                if (normalizedBands[i] < normalizedBandBuffers[i])
                {
                    switch (decreasingType)
                    {
                        case BufferDecreasingType.Jump:
                            normalizedBandBuffers[i] = normalizedBands[i];
                            break;
                        case BufferDecreasingType.Falling:
                            normalizedBandBuffers[i] -= decreasing;
                            break;
                        case BufferDecreasingType.Slide:
                            normalizedBandBuffers[i] -= bufferDecrease[i];
                            bufferDecrease[i] *= 1 + DecreaseAcceleration;
                            break;
                    }
                    if (normalizedBands[i] > normalizedBandBuffers[i]) normalizedBandBuffers[i] = normalizedBands[i];
                }
                normalizedBands[i] = normalizedBandBuffers[i];
            }
        }

        private void BandNegativeCheck()
        {
            for (int i = 0; i < BandCount; i++)
            {
                if (bands[i] < 0)
                {
                    bands[i] = 0;
                }
                if (normalizedBands[i] < 0)
                {
                    normalizedBands[i] = 0;
                }
            }
        }

        #endregion

        /// <summary>
        /// 通过这个函数来生成一个AudioSource,并初始化其播放的片段为audioClip
        /// </summary>
        /// <param name="audioClip">播放的片段</param>
        /// <returns></returns>
        public static AudioSource CreateSampler(AudioClip audioClip)
        {
            GameObject go = new GameObject("New AudioSource");
            AudioSource asr = go.AddComponent<AudioSource>();
            asr.clip = audioClip;
            asr.loop = false;
            asr.Play();
            return asr;
        }

        /// <summary>
        /// 传入一个AudioClip 会将AudioClip上挂载的音频文件生成频谱到一张Texture2D上
        /// </summary>
        /// <param name="_clip"></param>
        /// <param name="resolution">这个值可以控制频谱的密度</param>
        /// <param name="width">这个是最后生成的Texture2D图片的宽度</param>
        /// <param name="height">这个是最后生成的Texture2D图片的高度</param>
        /// <returns></returns>
        public static Texture2D BakeAudioWaveform(AudioClip _clip, int resolution = 60, int width = 1920, int height = 200)
        {
            resolution = _clip.frequency / resolution;

            float[] samples = new float[_clip.samples * _clip.channels];
            _clip.GetData(samples, 0);

            float[] waveForm = new float[(samples.Length / resolution)];

            float min = 0;
            float max = 0;
            bool inited = false;

            for (int i = 0; i < waveForm.Length; i++)
            {
                waveForm[i] = 0;

                for (int j = 0; j < resolution; j++)
                {
                    waveForm[i] += Mathf.Abs(samples[(i * resolution) + j]);
                }

                if (!inited)
                {
                    min = waveForm[i];
                    max = waveForm[i];
                    inited = true;
                }
                else
                {
                    if (waveForm[i] < min)
                    {
                        min = waveForm[i];
                    }

                    if (waveForm[i] > max)
                    {
                        max = waveForm[i];
                    }
                }
                //waveForm[i] /= resolution;
            }


            Color backgroundColor = Color.black;
            Color waveformColor = Color.green;
            Color[] blank = new Color[width * height];
            Texture2D texture = new Texture2D(width, height);

            for (int i = 0; i < blank.Length; ++i)
            {
                blank[i] = backgroundColor;
            }

            texture.SetPixels(blank, 0);

            float xScale = (float)width / (float)waveForm.Length;

            int tMid = (int)(height / 2.0f);
            float yScale = 1;

            if (max > tMid)
            {
                yScale = tMid / max;
            }

            for (int i = 0; i < waveForm.Length; ++i)
            {
                int x = (int)(i * xScale);
                int yOffset = (int)(waveForm[i] * yScale);
                int startY = tMid - yOffset;
                int endY = tMid + yOffset;

                for (int y = startY; y <= endY; ++y)
                {
                    texture.SetPixel(x, y, waveformColor);
                }
            }

            texture.Apply();
            return texture;
        }

    }

    public enum SpectrumLength
    {
        Spectrum64, Spectrum128, Spectrum256, Spectrum512, Spectrum1024, Spectrum2048, Spectrum4096, Spectrum8192
    }

    public enum BufferDecreasingType
    {
        Jump, Slide, Falling
    }

    public enum BufferIncreasingType
    {
        Jump, Slide
    }

}