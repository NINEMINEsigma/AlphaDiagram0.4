using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.Entry
{
    public class BackgroundView : AD.BASE.ADController
    {
        public Animator animator;
        public Material material;
        public Canvas target;

        private void Awake()
        {
            EntryApp.instance.RegisterController(this);
        }

        public override void Init()
        {
            SetLevel(1);
            SetMaterial_Two(0);
        }

        public void SetLevel(int level)
        {
            if (level == 1 || level == 2) animator.SetInteger("Level", level);
            else Debug.LogError("Level Error");
        }

        public void SetMaterial_Contrast(float value)
        {
            material.SetFloat("_Contrast", value);
        }

        public void SetMaterial_GaussianBlurLevel(float value)
        {
            material.SetFloat("_Distance", value);
        }

        public void SetMaterial_Two(float value)
        {
            SetMaterial_Contrast(value * 10);
            SetMaterial_GaussianBlurLevel((1 - value) * 0.15f);
        }

    }
}