using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using AD.UI;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    [RequireComponent(typeof(ViewController))]
    public class SingleCharacter : MonoBehaviour
    {
        private ViewController _view = null;
        public ViewController view
        {
            get
            {
                _view ??= GetComponent<ViewController>();
                return _view;
            }
        }

        public ReadOnlyBindProprety<List<CharacterSourcePair.DifferenceBuff>> PictureDifferences = new();
        public ReadOnlyBindProprety<List<int>> Appearance = new();
        [SerializeField]private int RecordKeyIndex = 0, RecordStep = 0;
        [SerializeField]private bool RecordActiveValue = false;

        public void Init(List<CharacterSourcePair.DifferenceBuff> PictureDifferences, List<int> Appearance)
        {
            this.PictureDifferences.MakeInit(PictureDifferences);
            this.Appearance.MakeInit(Appearance);
            Refresh(0);
        }

        private void UpdatePictureDifferences(int current)
        {
            if (PictureDifferences.Get().Count == 0) return;
            var target = PictureDifferences.Get().FirstOrDefault(T => T.Key > current);
            if (target.Equals(default)) return;
            else view.SetPair(target.Value);
        }

        public void Refresh(int current)
        {
            //TestIsNeedAppearance  
            gameObject.SetActive(TestIsNeedAppearance(current));
            //UpdatePictureDifferences
            UpdatePictureDifferences(current);
        }

        public static readonly bool DefualtAppearance = true;

        private bool TestIsNeedAppearance(int current)
        {
            if (Appearance.Get().Count == 0) return DefualtAppearance;
            if (RecordStep == current) return RecordActiveValue;
            if (RecordKeyIndex >= Appearance.Get().Count || RecordKeyIndex < 0) return DefualtAppearance;
            else if (RecordStep < current && Appearance.Get()[RecordKeyIndex] == current)
            {
                RecordActiveValue = !RecordActiveValue;
                RecordStep = current;
                RecordKeyIndex++;
                if (RecordKeyIndex >= Appearance.Get().Count) return DefualtAppearance;
            }
            else if (RecordStep > current && Appearance.Get()[RecordKeyIndex] == current)
            {
                RecordActiveValue = !RecordActiveValue;
                RecordStep = current;
                RecordKeyIndex--;
                if (RecordKeyIndex < 0) return DefualtAppearance;
            }
            return RecordActiveValue;
        }
    }
}
