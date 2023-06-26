using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.InputSystem;

namespace AD
{
    public class HierarchyManager : MonoBehaviour
    {
        public HierarchyTab selectingTab;
        public static HierarchyManager hierarchy;
        public RectTransform hierarchyListContainer;

        public GameObject hierarchyTab;

        public GameObject config, song, beatMap;

        public List<HierarchyTab> tabList;

        public List<HierarchyTab> tempSelectedTabList;

        private void Awake()
        {
            hierarchy = this;
            tabList = new List<HierarchyTab>();
            tempSelectedTabList = new List<HierarchyTab>();
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                /*if (UIManager.uiManager.CheckAnyArea(MousePointerArea.Hierarchy) && selectingTab == null)
                {
                    foreach (var element in EditorManager.editorManager.selectedElementList)
                    {
                        element.connectedTab.DisableHighlight();
                        if (element.GetComponent<GameElement>() != null)
                        {
                            (element as GameElement).Highlight(false);
                        }
                    }
                    HierarchyManager.hierarchy.tempSelectedTabList.Clear();
                    EditorManager.editorManager.selectedElementList.Clear();
                    PropertiesManager.properties.ClearPropertiesPanel();
                }*/
            }
        }

        public void GenerateTab(BaseElement targetElement, HierarchyTab parentTab = null, bool isMainConnect = true)
        {
            /*HierarchyTab tab = LeanPool.Spawn(hierarchyTab, hierarchyListContainer).GetComponent<HierarchyTab>();
            tab.SetTab(targetElement, parentTab, isMainConnect);
            tabList.Add(tab);*/
        }

        /// <summary>
        /// ���Ŀǰѡ�����������������Ƿ���ͬ����ͬ��Ŀǰ��������Properties����
        /// </summary>
        /// <returns></returns>
        public bool CheckAllEqual()
        {
            /*foreach (var element in EditorManager.editorManager.selectedElementList)
            {
                if (element.GetType() != EditorManager.editorManager.selectedElementList[0].GetType())
                {
                    return false;
                }
            }*/

            return true;
        }
    }
}
