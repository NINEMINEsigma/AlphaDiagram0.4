using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.ConfigureHelper;
using UnityEngine;
using AD.ADbase;
using System.Linq;

namespace AD
{
    [Serializable]
    public class BaseElement: MonoBehaviour, IComparable<BaseElement>,IBase
    {
        public BaseElement()
        {
            Initialize("new element");
            Connectome = new Graph<BaseElement, HierarchyTab>.Edge(this, null);
        }

        public static int TotalSerialNumber = 0;
        public string AElementName = "null";
        public int SerialNumber = 0;

        public Graph<BaseElement, HierarchyTab>.Edge Connectome = null;
        public HierarchyTab ConnectedTab
        {
            get
            {
                return Connectome.Right;
            }
            set
            {
                Connectome.Right = value;
            }
        }

        public BaseElement_BM BindablePropertyBM = null;

        public BaseElement ParentElement = null;
        public List<BaseElement> ChildElements = null;

        public BaseElement Initialize(string name)
        {
            AElementName = name;
            //EditorManager.beatMapContainer.beatMapElementList.Add(this);
            SerialNumber = TotalSerialNumber++;
            return this;
        }

        /// <summary>
        /// 无法在不使用GenerateElement生成物体时自动初始化的部分，在SetUp中进行初始化
        /// </summary>
        public virtual BaseElement SetUp()
        {
            return this;
        }
        public virtual T SetUp<T>() where T : BaseElement
        {
            return this as T;
        }

        /// <summary>
        /// 被选中时执行此函数
        /// </summary> 
        public virtual BaseElement OnSelect()
        {
            return this;
        }
        public virtual T OnSelect<T>() where T : BaseElement
        {
            return this as T;
        }

        /// <summary>
        /// 生成Hierarchy Tab
        /// </summary>
        /// <param name="element">对应物体（本身）</param>
        /// <param name="parent">父物体</param>
        public static void GenerateTab(BaseElement element, BaseElement parent = null)
        {
            if (parent == null)
            {
                HierarchyManager.hierarchy.GenerateTab(element);
            }
            else
            {
                parent.ConnectedTab.childTabList.RemoveAll(s => s == null);
                HierarchyManager.hierarchy.GenerateTab(element, parent.ConnectedTab);
            }
        }

        public /*void*/BaseElement GenerateTab(BaseElement parent, bool isMainConnect = true)
        {
            if (parent == null)
            {
                HierarchyManager.hierarchy.GenerateTab(this);
            }
            else
            {
                parent.ConnectedTab.childTabList.RemoveAll(s => s == null);
                HierarchyManager.hierarchy.GenerateTab(this, parent.ConnectedTab, isMainConnect);
            }
            return this;
        }
        //ADD
        public T GenerateTab<T>(BaseElement parent, bool isMainConnect = true) where T : BaseElement, new()
        {
            if (parent == null)
            {
                HierarchyManager.hierarchy.GenerateTab(this);
            }
            else
            {
                parent.ConnectedTab.childTabList.RemoveAll(s => s == null);
                HierarchyManager.hierarchy.GenerateTab(this, parent.ConnectedTab, isMainConnect);
            }
            return this as T;
        }



        /// <summary>
        /// 设置父物体
        /// </summary>
        /// <param name="parentElement">父物体</param>
        public /*void*/BaseElement SetParent(BaseElement parentElement)
        {
            if (parentElement != null)
            {
                parentElement.ChildElements.Add(this);
                this.ParentElement = parentElement;
                transform.SetParent(parentElement.transform);
            }

            //ADD
            return this;
        }

        /// <summary>
        /// 生成属性窗口
        /// </summary>
        /*public virtual void GeneratePropertiesWindow()
        {
            PropertyModule baseInfo = GeneratePropertyModule("Base Info", 1);
            baseInfo.GenerateInputField("elementName").AddCustomListener((arg, str) =>
            {
                arg.connectedTab.tabButtonText.text = str;
            });
        }*/

        /// <summary>
        /// 生成属性模块
        /// </summary>
        /// <param name="moduleName">模块名</param>
        /// <param name="rows">行数</param>
        /// <param name="targetElement">目标物体</param>
        /// <returns></returns>
        /*public PropertyModule GeneratePropertyModule(string moduleName, int rows, BaseElement targetElement = null)
        {
            PropertyModule module;
            if (targetElement == null)
            {
                module = PropertiesManager.properties.GeneratePropertyModule(moduleName, this.GetType(), rows);
            }
            else
            {
                module = PropertiesManager.properties.GeneratePropertyModule(moduleName, targetElement.GetType(), rows);
            }

            return module;
        }*/

        public virtual void GenerateShortCutWindow()
        {


        }

        /// <summary>
        /// 刷新属性窗口
        /// </summary>
        public void RefreshPropertiesWindow()
        {
            /*PropertiesManager.properties.ClearPropertiesPanel();
            GeneratePropertiesWindow();*/
        }


        /// <summary>
        /// 生成Timeline物体
        /// </summary>
        public void GenerateTimeLineListItem()
        {
            List<Type> legalTypes = new List<Type>();
            List<List<BaseElement>> elementLists = new List<List<BaseElement>>();
            for (int i = 0; i < ChildElements.Count; i++)
            {
                Type type = ChildElements[i].GetType();
                if (!legalTypes.Contains(type) && type.GetInterface("IHaveTimerModule") != null)
                {
                    legalTypes.Add(type);
                    elementLists.Add(new List<BaseElement>());
                }

                if (elementLists.Count > 0)
                {
                    elementLists[legalTypes.IndexOf(type)].Add(ChildElements[i]);
                }
            }


            /*for (var i = 0; i < elementLists.Count; i++)
            {
                var elementList = elementLists[i];
                TimeLineItem item = TimeLineManager.timeLine.timeLineList.CreateItem(legalTypes[i].ToString());
                for (int j = 0; j < elementList.Count; j++)
                {
                    (elementList[j] as IHaveTimerModule).GenerateTimeLineInfo(item);
                }
            }*/
        }

        public void Delete()
        {
            for (int i = 0; i < ChildElements.Count; i++)
            {
                ChildElements[i].Delete(); //删除子GameElement、
            }

            AffiliatedDelete();
        }

        public void Copy()
        {
            /*for (int index = 0; index < EditorManager.editorManager.copyingObjectList.Count; index++)
            {
                Destroy(EditorManager.editorManager.copyingObjectContainer.transform.GetChild(index).gameObject);
            }
            EditorManager.editorManager.copyingObjectList.Clear();
            GameObject g = Instantiate(gameObject, EditorManager.editorManager.copyingObjectContainer.transform);
            EditorManager.editorManager.copyingObjectList.Add(g);*/
        }

        public virtual void AffiliatedDelete()
        {
            /*if (this.GetComponent<GameElement>() != null)
            {
                EditorManager.beatMapContainer.beatMapElementList.Remove(this as GameElement); //从保存列表中剔除
            }*/
            this.ConnectedTab.parentTab.childTabList.RemoveAll(s => s == null);
            Destroy(this.ConnectedTab.gameObject);
            HierarchyManager.hierarchy.tabList.RemoveAll(s => s == null); //删除Tab
            HierarchyManager.hierarchy.tabList.RemoveAll(s => s.connectedElement == null);
            Destroy(gameObject); //销毁
        }

        public void Paste(BaseElement parent)
        {
            /*foreach (var obj in EditorManager.editorManager.copyingObjectList)
            {
                BaseElement now = Instantiate(obj, parent.transform).GetComponent<BaseElement>();
                (now as GameElement).Initialize(now.elementName);
                now.SetUp();
                now.SetParent(parent);
                now.GenerateTab(parent);

                for (int i = 0; i < now.ChildElements.Count; i++)
                {
                    now.ChildElements[i].AffiliatedPaste(now);
                }
            }*/
        }

        public void AffiliatedPaste(BaseElement parent)
        {
            //(this as GameElement).Initialize(this.elementName);
            this.SetUp();
            this.GenerateTab(parent);

            for (int i = 0; i < ChildElements.Count; i++)
            {
                ChildElements[i].AffiliatedPaste(this);
            }
        }

        public int CompareTo(BaseElement other)
        {
            return SerialNumber.CompareTo(other.SerialNumber);
        }

        public virtual IBaseMap ToMap()
        {
            throw new AD.ADbase.ADException("Can not use BaseElement's ToMap");
        }

        public virtual bool FromMap(IBaseMap from)
        {
            throw new AD.ADbase.ADException("Can not use BaseElement's FormMap");
        }
    }

    public abstract class BaseElement_BM
    {
        [System.NonSerialized]
        public static Dictionary<string, BaseElement_BM> identifier = new Dictionary<string, BaseElement_BM>();

        public string elementName;
        public string id;

        public string attachedElementId;

        //[System.NonSerialized] public GameElement correspondingGameElement;

        public BaseElement_BM()
        {

        }

        public BaseElement_BM(string elementName)
        {
            this.elementName = elementName;
            this.attachedElementId = "null";
            //this.id = BeatMapSaver.GetBeatMapElementID();
            identifier.Add(this.id, this);
        }

        public BaseElement_BM(string elementName, BaseElement_BM attachedElement)
        {
            this.elementName = elementName;
            this.attachedElementId = attachedElement.id;
            //this.id = BeatMapSaver.GetBeatMapElementID();
            identifier.Add(this.id, this);
        }

        public static BaseElement_BM GetElement(string ID)
        {
            return identifier[ID];
        }

        public static string GetID(BaseElement_BM instruction)
        {
            return identifier.FirstOrDefault(x => x.Value == instruction).Key;
        }

        public abstract void ExecuteBM();
    }
}