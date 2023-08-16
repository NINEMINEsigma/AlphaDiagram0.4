using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AD.UI
{
    public interface IHaveValue
    {
        float value { get; }
        float Value { get; }
        float GetValue();
    }

    public class ModernUIFillBar : PropertyModule, IHaveValue
    {
        // Content
        [Range(0, 1)] public float currentPercent;
        public float minValue;
        public float maxValue = 100;

        public float value => (maxValue - minValue) * currentPercent + minValue;
        public float Value => (maxValue - minValue) * currentPercent + minValue;
        // Resources
        public Image loadingBar;
        public TextMeshProUGUI textPercent;
        public TextMeshProUGUI textValue;

        // Settings  
        public bool IsPercent = true;
        public bool IsInt = false;

        public void Update()
        {
            loadingBar.fillAmount = Mathf.Clamp(currentPercent, 0, 1);

            textPercent.text = currentPercent.ToString("F2") + (IsPercent ? "%" : "");
            textValue.text = GetValue().ToString("F2");
        }

        public float GetValue()
        {
            return IsInt ? (int)value : value;
        }
    }
}

