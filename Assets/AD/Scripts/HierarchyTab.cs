using System.Collections;
using System.Collections.Generic;
using Codice.CM.ConfigureHelper;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AD
{
    public class HierarchyTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BaseElement connectedElement;
        public HierarchyTab parentTab;
        public List<HierarchyTab> childTabList;

        public int tabLayer;
        public bool isSelected;
        public bool isExpanded;

        public Button tabButton;
        public Text tabButtonText;
        public Button expandButton;
        public Image highlightImage, expandButtonImage;

        public void SetTab(BaseElement targetElement, HierarchyTab parentTab, bool isMainConnect = true)
        {
            tabButtonText.text = targetElement.AElementName;
            this.isExpanded = false;
            if (parentTab == null)
            {
                this.tabLayer = 0;
            }
            else
            {
                this.parentTab = parentTab;
            }

            this.connectedElement = targetElement;

            if (isMainConnect)
            {
                connectedElement.ConnectedTab = this;
            }

            this.childTabList = new List<HierarchyTab>();
            if (parentTab != null)
            {
                this.parentTab.childTabList.Add(this);
                this.tabLayer = parentTab.tabLayer + 1;

                this.transform.SetSiblingIndex(this.parentTab.transform.GetSiblingIndex() +
                                               GetAllChildrenCount(this.parentTab));

                if (!parentTab.isExpanded)
                {
                    this.isExpanded = false;
                    ExpandOrFold(false);
                }
            }
            else
            {
                this.transform.SetAsLastSibling();
            }

            tabButton.onClick.AddListener(() =>
            {
                SelectElement();
                ExpandOrFold(true);
            });
            expandButton.onClick.AddListener(ExpandOrFold);
            tabButton.GetComponent<RectTransform>().sizeDelta -= new Vector2(tabLayer * 15f, 0);
            tabButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(tabLayer * 7.5f, 0);
            expandButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-110 + (15f * tabLayer), 0);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isSelected = true;
            HierarchyManager.hierarchy.selectingTab = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isSelected = false;
            HierarchyManager.hierarchy.selectingTab = null;
        }

        private void Update()
        {
            if (isSelected)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame) //右键快捷菜单
                {
                    bool matched = false;

                    /*foreach (var element in EditorManager.editorManager.selectedElementList)
                    {
                        if (element.connectedTab == this)
                        {
                            matched = true;
                            break;
                        }
                    }*/

                    if (!matched)
                    {
                        ReplaceSelect();
                    }

                    CreateShortCutWindow(Mouse.current.position.ReadValue());
                }
            }
        }

        public void SelectElement()
        {
            /*if (Keyboard.current.leftCtrlKey.isPressed)
            {
                AddSelect(); //多选
            }
            else
            {
                ReplaceSelect(); //单选
            }

            TimeLineManager.timeLine.timeLineList.ClearTimeLineList();

            foreach (var element in EditorManager.editorManager.selectedElementList)
            {
                element.OnSelect();
            }

            if (EditorManager.editorManager.selectedElementList.Count == 1)
            {
                EditorManager.editorManager.selectedElementList[0].GenerateTimeLineListItem();
            }*/
        }

        public void SetHighlight()
        {
            foreach (var tab in HierarchyManager.hierarchy.tempSelectedTabList)
            {
                /*bool same = false;
                foreach (var element in EditorManager.editorManager.selectedElementList)
                {
                    if (tab == element.connectedTab)
                    {
                        same = true;
                        break;
                    }
                }

                if (!same)
                {
                    if (tab.connectedElement.GetComponent<GameElement>() != null)
                    {
                        (tab.connectedElement as GameElement).Highlight(false);
                    }
                    tab.tabButtonText.color = Color.white;
                    tab.highlightImage.color = Color.clear;
                }*/
            }

            HierarchyManager.hierarchy.tempSelectedTabList.RemoveAll(tab => tab.highlightImage.color == Color.clear);

            /*foreach (var element in EditorManager.editorManager.selectedElementList)
            {
                if (element.GetComponent<GameElement>() != null)
                {
                    (element as GameElement).Highlight(true);
                }
                element.connectedTab.tabButtonText.color = Color.black;
                element.connectedTab.highlightImage.color = new Color(1, 1, 1, 0.5f);
            }*/
        }

        void ReplaceSelect()
        {
            /*EditorManager.editorManager.selectedElementList.Clear();
            EditorManager.editorManager.selectedElementList.Add(connectedElement);
            PropertiesManager.properties.ClearPropertiesPanel();*/

            if (HierarchyManager.hierarchy.CheckAllEqual())
            {
               // connectedElement.GeneratePropertiesWindow();
            }


            SetHighlight();
            HierarchyManager.hierarchy.tempSelectedTabList.Clear();
            HierarchyManager.hierarchy.tempSelectedTabList.Add(this);
        }

        void AddSelect()
        {
            /*EditorManager.editorManager.selectedElementList.Add(connectedElement);
            PropertiesManager.properties.ClearPropertiesPanel();*/

            if (HierarchyManager.hierarchy.CheckAllEqual())
            {
               //connectedElement.GeneratePropertiesWindow();
            }

            SetHighlight();
            HierarchyManager.hierarchy.tempSelectedTabList.Add(this);
        }

        public void DisableHighlight()
        {
            tabButtonText.color = Color.white;
            highlightImage.color = Color.clear;
        }

        void CreateShortCutWindow(Vector2 mousePosition)
        {
            //ShortCutWindowManager.shortCutWindow.SetShortCutWindowPosition(UITools.GetUIPosition(mousePosition));
            if (HierarchyManager.hierarchy.CheckAllEqual())
            {
                connectedElement.GenerateShortCutWindow();
            }
        }

        private static int GetAllChildrenCount(HierarchyTab tab)
        {
            int c = tab.childTabList.Count;

            for (int i = 0; i < tab.childTabList.Count; i++)
            {
                c += GetAllChildrenCount(tab.childTabList[i]);
            }

            return c;
        }

        private void ExpandOrFold()
        {
            this.childTabList.RemoveAll(s => s == null);
            bool op = !isExpanded;

            for (int i = 0; i < childTabList.Count; i++)
            {
                childTabList[i].ExpandOrFold(op);
            }

            isExpanded = op;

            expandButtonImage.GetComponent<RectTransform>().localRotation =
                Quaternion.Euler(0, 0, isExpanded ? 90 : 180);
        }

        private void ExpandOrFold(bool op)
        {
            if (!(op == true && isExpanded == false))
            {
                for (int i = 0; i < childTabList.Count; i++)
                {
                    childTabList[i].ExpandOrFold(op);
                }
            }

            if (op)
            {
                Expand();
            }
            else
            {
                Fold();
            }
        }

        private void Expand()
        {
            gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            gameObject.GetComponent<LayoutElement>().ignoreLayout = false;
        }

        private void Fold()
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
            gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
        }
    }
}