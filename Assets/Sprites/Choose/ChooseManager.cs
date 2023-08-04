//
//	ninemine
//

using AD.ProjectTwilight.Source;
using UnityEngine;

namespace AD.ProjectTwilight.Choose
{
    //Main Controller 
    public class ChooseManager : AD.SceneBaseController 
    { 
        private void Start() 
        { 
            ChooseApp.instance.RegisterController(this); 
        } 
        public override void Init() 
        { 
            base.Init();

            for (int i = 0,e= GetSystem<ChooseSystem>().model.models.Count; i < e; i++)
            {
                SinglePlayerAsset model = GetSystem<ChooseSystem>().model.models[i];
                InitModelListViewItem(ListView.GenerateItem(), model, i);
            }
        }
        [SerializeField] AD.UI.ListView ListView;
         
        private void InitModelListViewItem(AD.UI.ListViewItem item,SinglePlayerAsset model,int index)
        {
            (item as RecordItem).GenerateItemWithTitle(model.PlayerName);
            (item as RecordItem).GenerateItemWithTitle(model.Chapter);
            (item as RecordItem).GenerateItemWithTitle(model.Branch);
            (item as RecordItem).GenerateItemWithTitle("Index" + model.index.ToString());
            (item as RecordItem).GenerateItemWithTitle("Step" + model.step.ToString());
            (item as RecordItem).myModelIndex = index;
        }

    }
}
