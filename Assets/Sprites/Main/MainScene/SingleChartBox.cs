using System.Collections.Generic;
using System.Linq;
using AD.UI;
using UnityEngine;
using AD.BASE;
using AD.ProjectTwilight.MainScene;
using static Cinemachine.CinemachinePathBase;

namespace AD.ProjectTwilight.MainScene
{
    public class SingleChartBox : MonoBehaviour
    {
        [Header("Asset")]
        public ViewController CharacterImage;

        [Header("Others")]
        public ReadOnlyBindProprety<List<CharacterSourcePair.DifferenceBuff>> PictureDifferences = new();
        public ReadOnlyBindProprety<List<CharacterMessage>> CharacterMessages = new();
        [SerializeField] private int RecordKeyIndex = 0, RecordStep = 0;

        [HideInInspector]public SingleSoundPlayer soundPlayer;

        [SerializeField] private AD.UI.Button Next, Past, SoundReplay;
        [SerializeField] private AD.UI.Text ChartText;

        public void Init(int uid)
        {
            var character = MainApp.instance.GetSystem<MainGroupSystem>().characterGroup.characters[uid];
            this.PictureDifferences.TrackThisShared(character.PictureDifferences);
            this.CharacterMessages.MakeInit(MainApp.instance.GetSystem<MainGroupSystem>().SourcePairs.FirstOrDefault(T => T.GUID == uid).charts);
            this.soundPlayer = MainApp.instance.GetSystem<MainGroupSystem>().soundGroup.sounds[uid];

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
            soundPlayer.Refresh(RecordKeyIndex);
            //SetText
            SetText(RecordKeyIndex);
        }

        public static readonly bool DefualtAppearance = false;

        private bool TestIsNeedAppearance(int current)
        {
            if (CharacterMessages.Get().Count == 0) return DefualtAppearance;
            if (RecordKeyIndex >= CharacterMessages.Get().Count || RecordKeyIndex < 0) return DefualtAppearance;
            while (RecordStep < current && CharacterMessages.Get()[RecordKeyIndex].Appearance < current)
            {
                RecordKeyIndex++;
                if (RecordKeyIndex >= CharacterMessages.Get().Count) return DefualtAppearance;
            }
            while (RecordStep > current && CharacterMessages.Get()[RecordKeyIndex].Appearance > current)
            {
                RecordKeyIndex--;
                if (RecordKeyIndex < 0) return DefualtAppearance;
            }
            RecordStep = current;
            return CharacterMessages.Get()[RecordKeyIndex].Appearance == current;
        }

        public void NextPart()
        {
            MainApp.instance.SendImmediatelyCommand<AddCurrentIndox>(new(1));
        }

        public void PastPart()
        {
            MainApp.instance.SendImmediatelyCommand<AddCurrentIndox>(new(-1));
        }

        public void PlaySound()
        {
            soundPlayer.PlaySound();
        }

        public void SetText(int key)
        {
            if (gameObject.activeInHierarchy)
                ChartText.text = CharacterMessages.Get()[key].message;
        }
    }
}
