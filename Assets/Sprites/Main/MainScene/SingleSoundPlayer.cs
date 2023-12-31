using AD.UI;
using UnityEngine;
using System.Linq;

namespace AD.ProjectTwilight.MainScene
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
            source.SourcePairs = MainApp.instance.GetSystem<MainGroupSystem>().SourcePairs.FirstOrDefault(T => T.GUID == uid).SourcePairs;
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
