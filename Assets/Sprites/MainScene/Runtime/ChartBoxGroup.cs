using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;
using UnityEngine.UI;

namespace AD.ProjectTwilight.MainScene
{
    public class ChartBoxGroup : ADController
    {
        public Dictionary<int, SingleChartBox> boxs = new Dictionary<int, SingleChartBox>();

        public RectTransform rectTransform { get; private set; }
        public GridLayoutGroup layoutGroup { get; private set; }

        public override void Init()
        {
            rectTransform = GetComponent<RectTransform>();
            layoutGroup = GetComponent<GridLayoutGroup>();
            foreach (var item in GetSystem<MainGroupSystem>().SourcePairs)
            {
                var cat = GameObject.Instantiate(item.ChartBoxPrefab, transform);
                cat.Init(item.GUID);
                boxs.Add(item.GUID, cat);
            }
        }

        public void Refresh(int current)
        {
            foreach (var item in boxs)
            {
                item.Value.Refresh(current);
            }
        }
    }
}