using System.Collections.Generic;
using System.Linq;
using AD.UI;
using UnityEngine;
using AD.BASE;
using AD.ProjectTwilight.MainScene;

namespace AD.ProjectTwilight.MainScene
{
    public class SingleChartBox : MonoBehaviour
    {
        [Header("Asset")]
        public ViewController CharacterImage;

        [Header("Others")]
        public ReadOnlyBindProprety<List<int>> Appearance = new();
        public ReadOnlyBindProprety<List<CharacterSourcePair.DifferenceBuff>> PictureDifferences = new();
        public ReadOnlyBindProprety<List<CharacterMessage>> CharacterMessages = new();

        [HideInInspector]public SingleSoundPlayer soundPlayer;

        [SerializeField] private AD.UI.Button Next, Past, SoundReplay;
        [SerializeField] private AD.UI.Text ChartText;

        public void Init(int uid)
        {
            var character = MainApp.instance.GetSystem<MainGroupController>().characterGroup.characters[uid];
            this.PictureDifferences.TrackThisShared(character.PictureDifferences);
            this.Appearance.TrackThisShared(character.Appearance);
            this.CharacterMessages.MakeInit(MainApp.instance.GetSystem<MainGroupController>().SourcePairs.FirstOrDefault(T => T.GUID == uid).charts);
            this.soundPlayer = MainApp.instance.GetSystem<MainGroupController>().soundGroup.sounds[uid];

            Next.RemoveAllListener();
            Past.RemoveAllListener();
            SoundReplay.RemoveAllListener();

            Next.AddListener(NextPart);
            Past.AddListener(PastPart);
            SoundReplay.AddListener(PlaySound);

            Refresh(0);
        }

        private void UpdatePictureDifferences(int current)
        {
            if (PictureDifferences.Get().Count == 0) return;
            var target = PictureDifferences.Get().FirstOrDefault(T => T.Key > current);
            if (target.Equals(default)) return;
            else CharacterImage.SetPair(target.Value);
        }

        public void Refresh(int current)
        {
            //TestIsNeedAppearance
            gameObject.SetActive(TestIsNeedAppearance(current));
            //UpdatePictureDifferences
            UpdatePictureDifferences(current);
            //PlaySound
            soundPlayer.Refresh(current);
            //SetText
            SetText(current);
        }

        private bool TestIsNeedAppearance(int current)
        {
            bool currentState = true;
            for (int i = 0, e = Appearance.Get().Count; i < e; i++)
            {
                if (Appearance.Get()[i] >= current)
                {
                    return currentState;
                }
                currentState = !currentState;
            }
            return currentState;
        }

        public void NextPart()
        {
            MainApp.instance.GetSystem<MainGroupController>().AddCurrent(1);
        }

        public void PastPart()
        {
            MainApp.instance.GetSystem<MainGroupController>().AddCurrent(-1);
        }

        public void PlaySound()
        {
            soundPlayer.PlaySound();
        }

        public void SetText(int current)
        {
            if(current>= CharacterMessages.Get().Count||current<0)
            {
                ChartText.text = "null";
                return;
            }
            ChartText.text = CharacterMessages.Get()[current].message;
        }
    }
}
