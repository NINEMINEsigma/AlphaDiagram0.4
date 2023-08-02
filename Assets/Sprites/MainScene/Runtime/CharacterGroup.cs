using System.Collections.Generic;
using AD.BASE;
using UnityEngine;
using UnityEngine.UI;

namespace AD.ProjectTwilight.MainScene
{
    public class CharacterGroup : ADController
    {
        public Dictionary<int, SingleCharacter> characters = new Dictionary<int, SingleCharacter>();

        public RectTransform rectTransform { get; private set; }
        public GridLayoutGroup layoutGroup { get; private set; }

        public override void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            layoutGroup = GetComponent<GridLayoutGroup>();
            foreach (var item in GetSystem<MainGroupController>().SourcePairs)
            {
                var cat = GameObject.Instantiate(item.CharacterPrefab, transform);
                cat.Init(item.PictureDifferences, item.Appearance);
                characters.Add(item.GUID, cat);
            }
            Refresh(0);
        }

        public void Refresh(int current)
        {
            int activeItemCount = 0;
            foreach (var item in characters)
            {
                item.Value.Refresh(current);
                if (item.Value.gameObject.activeInHierarchy) activeItemCount++;
            }
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
              -transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x * 0.5f +
              activeItemCount * layoutGroup.cellSize.x + Mathf.Clamp(activeItemCount - 1, 0, Mathf.Infinity) * layoutGroup.spacing.x,
                0);
        }

    }
}
