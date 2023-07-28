using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
#endif
using UnityEngine.Experiemntal.Rendering.Universal;

namespace AD.Experiemntal.Rendering.Universal
{
    public class AdditionPostProcessRendererFeature : ScriptableRendererFeature
    {
        public RenderPassEvent evt = RenderPassEvent.AfterRenderingTransparents;
        public AdditionalPostProcessData postData; 
        AdditionPostProcessPass postPass;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var cameraColorTarget = renderer.cameraColorTarget;
            var cameraDepth = renderer.cameraDepthTarget;
            var dest = RenderTargetHandle.CameraTarget;
            if (postData == null)
                return;
            postPass.Setup(evt, cameraColorTarget, cameraDepth, dest, postData);
            renderer.EnqueuePass(postPass);
        }

        public override void Create()
        {
            postPass = new AdditionPostProcessPass();
        }
    }
}
