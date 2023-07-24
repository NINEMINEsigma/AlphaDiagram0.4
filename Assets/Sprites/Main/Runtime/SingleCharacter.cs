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

        public ReadOnlyBindProprety<Dictionary<int, int>> PictureDifferences = new();
        public ReadOnlyBindProprety<List<int>> Appearance = new();

        public void Init(Dictionary<int, int> PictureDifferences, List<int> Appearance)
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
            int currentState = 0;
            currentState = TestIsNeedAppearance(current, currentState);
            if (currentState % 2 == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            else gameObject.SetActive(true);
            //UpdatePictureDifferences
            UpdatePictureDifferences(current);
        }

        private int TestIsNeedAppearance(int current, int currentState)
        {
            for (int i = 0, e = Appearance.Get().Count; i < e; i++)
            {
                currentState++;
                if (Appearance.Get()[i] >= current)
                {
                    break;
                }
            }

            return currentState;
        }
    }
}
