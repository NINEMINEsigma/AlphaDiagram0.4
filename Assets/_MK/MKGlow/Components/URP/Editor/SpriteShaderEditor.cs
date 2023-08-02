//////////////////////////////////////////////////////
// MK Glow Sprite Shader Editor					    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2021 All rights reserved.            //
//////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using MK.Glow.Editor;

namespace MK.Glow.URP.Editor
{
    public class SpriteShaderEditor : MaterialEditor 
    {
        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            RenderQueueField();
            EnableInstancingField();
        }
    }
}

#endif