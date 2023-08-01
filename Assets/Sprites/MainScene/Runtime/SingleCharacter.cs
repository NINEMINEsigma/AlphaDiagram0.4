using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using AD.UI;
using UnityEngine; 

namespace AD.MainScene
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
    }
}
