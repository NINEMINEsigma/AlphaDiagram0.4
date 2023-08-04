//
//	ninemine
//

using System.Linq;
using AD.BASE;
using AD.ProjectTwilight.Source;
namespace AD.ProjectTwilight.Choose
{
    public class ChooseSystem : ADSystem
    {
        public override void Init()
        {
            model = PTApp.instance.GetModel<PlayerModel>();
            LoadModel();
        }

        public void LoadModel()
        {
            model.Load();
        }

        public void ConfirmModelWithListViewIndex(int index)
        {
            if (index < 0 || index >= model.models.Count)
            {
                ADGlobalSystem.AddError("error index with listview");
                return;
            }
            model.ConfirmModel(new CurrentData() { current = model.models[index] });
            Architecture.GetController<ChooseManager>().OnEnd();
        }        public void ConfirmModel(int index)
        {
            var cat = model.models.FirstOrDefault(T => T.index == index);
            if (cat == null) ADGlobalSystem.AddError("you try to refer a null model");
            else
            {
                model.ConfirmModel(new CurrentData() { current = cat });
                Architecture.GetController<ChooseManager>().OnEnd();
            }
        }        public PlayerModel model;
    }

}
