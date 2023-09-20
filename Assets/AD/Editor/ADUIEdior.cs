using AD.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AD.UI.Text)), CanEditMultipleObjects]
public class TextEdior : Editor
{
    private AD.UI.Text that = null;

    private void OnEnable()
    {
        that = target as AD.UI.Text;
    }

    public override void OnInspectorGUI()
    {
        string str = "";

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("Text");
        str = EditorGUILayout.TextField(gUIContent, that.text);
        if (EditorGUI.EndChangeCheck()) that.text = str;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(AD.UI.Slider)), CanEditMultipleObjects]
public class SliderEdior : Editor
{
    private AD.UI.Slider that = null;

    private SerializedProperty background = null;
     private SerializedProperty handle = null;
    private SerializedProperty fill = null;

    private void OnEnable()
    {
        that = target as AD.UI.Slider;

        background = serializedObject.FindProperty("background");
        handle = serializedObject.FindProperty("handle");
        fill = serializedObject.FindProperty("fill");
    }

    public override void OnInspectorGUI()
    {
        Object @object = null;
        Sprite sprite = null;

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        EditorGUILayout.PropertyField(background);
        EditorGUILayout.PropertyField(handle);
        EditorGUILayout.PropertyField(fill);

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("Background");
        sprite = EditorGUILayout.ObjectField(gUIContent, that.backgroundView as Object, typeof(Sprite), @object) as Sprite;
        if( EditorGUI.EndChangeCheck()) that.backgroundView = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent fUIContent = new GUIContent("Fill");
        sprite = EditorGUILayout.ObjectField(fUIContent, that.fillView as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.fillView = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent hUIContent = new GUIContent("Handle");
        sprite = EditorGUILayout.ObjectField(hUIContent, that.handleView as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.handleView = sprite;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(AD.UI.Toggle)), CanEditMultipleObjects]
public class ToggleEdior : Editor
{
    private AD.UI.Toggle that = null;

    private SerializedProperty background = null;
    private SerializedProperty tab = null;
    private SerializedProperty mark = null;
    private SerializedProperty title = null;

    private SerializedProperty _IsCheck = null; 

    private void OnEnable()
    {
        that = target as AD.UI.Toggle;

        background = serializedObject.FindProperty("background");
        tab = serializedObject.FindProperty("tab");
        mark = serializedObject.FindProperty("mark");
        title = serializedObject.FindProperty("title");
        _IsCheck = serializedObject.FindProperty("_IsCheck");
    }

    public override void OnInspectorGUI()
    {
        Sprite sprite = null;
        UnityEngine.Object @object = null;
        string str = "";

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        EditorGUILayout.PropertyField(background);
        EditorGUILayout.PropertyField(tab);
        EditorGUILayout.PropertyField(mark);
        EditorGUILayout.PropertyField(title);

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("Background");
        sprite = EditorGUILayout.ObjectField(gUIContent, that.background.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.background.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent tUIContent = new GUIContent("Tab");
        sprite = EditorGUILayout.ObjectField(tUIContent, that.tab.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.tab.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent mUIContent = new GUIContent("Mark");
        sprite = EditorGUILayout.ObjectField(mUIContent, that.mark.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.mark.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent tiUIContent = new GUIContent("Title");
        str = EditorGUILayout.TextField(tiUIContent, that.title.text);
        if (EditorGUI.EndChangeCheck()) that.title.text = str;

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.Toggle("IsCheck", that.IsCheck);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(AD.UI.Button)), CanEditMultipleObjects]
public class ButtonEdior : Editor
{
    private AD.UI.Button that = null;

    private void OnEnable()
    {
        that = target as AD.UI.Button;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
} 

[CustomEditor(typeof(AD.UI.RawImage)), CanEditMultipleObjects]
public class RawImageEdior : Editor
{
    private AD.UI.RawImage that = null;

    private void OnEnable()
    {
        that = target as AD.UI.RawImage;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
 
[CustomEditor(typeof(AD.UI.InputField)), CanEditMultipleObjects]
public class InputFieldEdior : Editor
{
    private AD.UI.InputField that = null;

    private void OnEnable()
    {
        that = target as AD.UI.InputField;
    }

    public override void OnInspectorGUI()
    {
        string str = "";

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("InputField");
        str = EditorGUILayout.TextField(gUIContent, that.text);
        if (EditorGUI.EndChangeCheck()) that.text = str;

        serializedObject.ApplyModifiedProperties();

    }
} 

[CustomEditor(typeof(AD.UI.ModernUIButton)),CanEditMultipleObjects]
public class ModernUIButtonEditor : Editor
{
    private AD.UI.ModernUIButton that;
    private int currentTab;

    SerializedProperty buttonText;
    SerializedProperty hoverSound;
    SerializedProperty clickSound;
    SerializedProperty clickEvent;
    SerializedProperty hoverEvent;
    SerializedProperty normalText;
    SerializedProperty highlightedText;
    SerializedProperty soundSource;
    SerializedProperty useCustomContent;
    SerializedProperty enableButtonSounds;
    SerializedProperty useHoverSound;
    SerializedProperty useClickSound;
    SerializedProperty rippleParent;
    SerializedProperty useRipple;
    SerializedProperty renderOnTop;
    SerializedProperty centered;
    SerializedProperty rippleShape;
    SerializedProperty speed;
    SerializedProperty maxSize;
    SerializedProperty startColor;
    SerializedProperty transitionColor;
    SerializedProperty animationSolution;
    SerializedProperty fadingMultiplier;
    SerializedProperty rippleUpdateMode;

    private void OnEnable()
    {
        that = target as AD.UI.ModernUIButton;

        buttonText = serializedObject.FindProperty("buttonText");
        hoverSound = serializedObject.FindProperty("hoverSound");
        clickSound = serializedObject.FindProperty("clickSound");
        clickEvent = serializedObject.FindProperty("clickEvent");
        hoverEvent = serializedObject.FindProperty("hoverEvent");
        normalText = serializedObject.FindProperty("normalText");
        highlightedText = serializedObject.FindProperty("highlightedText");
        soundSource = serializedObject.FindProperty("soundSource");
        useCustomContent = serializedObject.FindProperty("useCustomContent");
        enableButtonSounds = serializedObject.FindProperty("enableButtonSounds");
        useHoverSound = serializedObject.FindProperty("useHoverSound");
        useClickSound = serializedObject.FindProperty("useClickSound");
        rippleParent = serializedObject.FindProperty("rippleParent");
        useRipple = serializedObject.FindProperty("useRipple");
        renderOnTop = serializedObject.FindProperty("renderOnTop");
        centered = serializedObject.FindProperty("centered");
        rippleShape = serializedObject.FindProperty("rippleShape");
        speed = serializedObject.FindProperty("speed");
        maxSize = serializedObject.FindProperty("maxSize");
        startColor = serializedObject.FindProperty("startColor");
        transitionColor = serializedObject.FindProperty("transitionColor");
        animationSolution = serializedObject.FindProperty("animationSolution");
        fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
        rippleUpdateMode = serializedObject.FindProperty("rippleUpdateMode");
    }

    public override void OnInspectorGUI()
    {
        GUISkin customSkin;
        Color defaultColor = GUI.color;

        if (EditorGUIUtility.isProSkin == true)
            customSkin = (GUISkin)Resources.Load("Editor\\ADUI Skin Dark");
        else
            customSkin = (GUISkin)Resources.Load("Editor\\ADUI Skin Light");

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = defaultColor;

        GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Button Top Header"));

        GUILayout.EndHorizontal();
        GUILayout.Space(-42);

        GUIContent[] toolbarTabs = new GUIContent[3];
        toolbarTabs[0] = new GUIContent("Content");
        toolbarTabs[1] = new GUIContent("Resources");
        toolbarTabs[2] = new GUIContent("Settings");

        GUILayout.BeginHorizontal();
        GUILayout.Space(17);

        currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

        GUILayout.EndHorizontal();
        GUILayout.Space(-40);
        GUILayout.BeginHorizontal();
        GUILayout.Space(17);

        if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
            currentTab = 0;
        if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
            currentTab = 1;
        if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
            currentTab = 2;

        GUILayout.EndHorizontal();

        switch (currentTab)
        {
            case 0:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Button Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(buttonText, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (useCustomContent.boolValue == false && that.normalText != null)
                    {
                        that.normalText.text = buttonText.stringValue;
                        that.highlightedText.text = buttonText.stringValue;
                    }

                    else if (useCustomContent.boolValue == false && that.normalText == null)
                    {
                        GUILayout.Space(2);
                        EditorGUILayout.HelpBox("'Text Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                    }

                    if (enableButtonSounds.boolValue == true && useHoverSound.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Hover Sound"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(hoverSound, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (enableButtonSounds.boolValue == true && useClickSound.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Click Sound"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(clickSound, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(4);
                    EditorGUILayout.PropertyField(clickEvent, new GUIContent("On Click Event"), true);
                    EditorGUILayout.PropertyField(hoverEvent, new GUIContent("On Hover Event"), true);
                }
                break;

            case 1:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Normal Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(normalText, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Highlighted Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(highlightedText, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (enableButtonSounds.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Sound Source"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(soundSource, new GUIContent(""));

                        if (that.soundSource == null)
                        {
                            EditorGUILayout.HelpBox("'Sound Source' is not assigned. Go to Resources tab or click the button to create a new audio source.", MessageType.Warning);

                            if (GUILayout.Button("Create a new one", customSkin.button))
                            {
                                that.soundSource = that.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                                currentTab = 1;
                            }
                        }

                        GUILayout.EndHorizontal();
                    }

                    if (useRipple.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Ripple Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(rippleParent, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }
                }
                break;

            case 2:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Animation Solution"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(animationSolution, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Fading Multiplier"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(fadingMultiplier, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    useCustomContent.boolValue = GUILayout.Toggle(useCustomContent.boolValue, new GUIContent("Use Custom Content"), customSkin.FindStyle("Toggle"));
                    useCustomContent.boolValue = GUILayout.Toggle(useCustomContent.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableButtonSounds.boolValue = GUILayout.Toggle(enableButtonSounds.boolValue, new GUIContent("Enable Button Sounds"), customSkin.FindStyle("Toggle"));
                    enableButtonSounds.boolValue = GUILayout.Toggle(enableButtonSounds.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableButtonSounds.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Sound Source"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(soundSource, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        useHoverSound.boolValue = GUILayout.Toggle(useHoverSound.boolValue, new GUIContent("Enable Hover Sound"), customSkin.FindStyle("Toggle"));
                        useHoverSound.boolValue = GUILayout.Toggle(useHoverSound.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        useClickSound.boolValue = GUILayout.Toggle(useClickSound.boolValue, new GUIContent("Enable Click Sound"), customSkin.FindStyle("Toggle"));
                        useClickSound.boolValue = GUILayout.Toggle(useClickSound.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();

                        if (that.soundSource == null)
                        {
                            EditorGUILayout.HelpBox("'Sound Source' is not assigned. Go to Resources tab or click the button to create a new audio source.", MessageType.Warning);

                            if (GUILayout.Button("Create a new one", customSkin.button))
                            {
                                that.soundSource = that.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                                currentTab = 2;
                            }
                        }
                    }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-2);
                    GUILayout.BeginHorizontal();

                    useRipple.boolValue = GUILayout.Toggle(useRipple.boolValue, new GUIContent("Use Ripple"), customSkin.FindStyle("Toggle"));
                    useRipple.boolValue = GUILayout.Toggle(useRipple.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);

                    if (useRipple.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        renderOnTop.boolValue = GUILayout.Toggle(renderOnTop.boolValue, new GUIContent("Render On Top"), customSkin.FindStyle("Toggle"));
                        renderOnTop.boolValue = GUILayout.Toggle(renderOnTop.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        centered.boolValue = GUILayout.Toggle(centered.boolValue, new GUIContent("Centered"), customSkin.FindStyle("Toggle"));
                        centered.boolValue = GUILayout.Toggle(centered.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Update Mode"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(rippleUpdateMode, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Shape"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(rippleShape, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Speed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(speed, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Max Size"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(maxSize, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Start Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(startColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Transition Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(transitionColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(ModernUIFillBar))]
public class ProgressBarEditor : Editor
{
    private ModernUIFillBar pbTarget;
    private int currentTab; 
     
    SerializedProperty minValue;
    SerializedProperty maxValue;
    SerializedProperty loadingBar;
    SerializedProperty textPercent;
    SerializedProperty textValue;
    SerializedProperty IsPercent;
    SerializedProperty IsInt; 

    private void OnEnable()
    {
        pbTarget = (ModernUIFillBar)target;
         
        minValue = serializedObject.FindProperty("minValue");
        maxValue = serializedObject.FindProperty("maxValue");
        loadingBar = serializedObject.FindProperty("loadingBar");
        textPercent = serializedObject.FindProperty("textPercent");
        textValue = serializedObject.FindProperty("textValue");
        IsPercent = serializedObject.FindProperty("IsPercent");
        IsInt = serializedObject.FindProperty("IsInt");
    }

    public override void OnInspectorGUI()
    {
        GUISkin customSkin;
        Color defaultColor = GUI.color;

        if (EditorGUIUtility.isProSkin == true)
            customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Dark");
        else
            customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Light");

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = defaultColor;

        GUILayout.Box(new GUIContent(""), customSkin.FindStyle("PB Top Header"));

        GUILayout.EndHorizontal();
        GUILayout.Space(-42);

        GUIContent[] toolbarTabs = new GUIContent[3];
        toolbarTabs[0] = new GUIContent("Content");
        toolbarTabs[1] = new GUIContent("Resources");
        toolbarTabs[2] = new GUIContent("Settings");

        GUILayout.BeginHorizontal();
        GUILayout.Space(17);

        currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

        GUILayout.EndHorizontal();
        GUILayout.Space(-40);
        GUILayout.BeginHorizontal();
        GUILayout.Space(17);

        if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
            currentTab = 0;
        if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
            currentTab = 1;
        if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
            currentTab = 2;

        GUILayout.EndHorizontal();

        switch (currentTab)
        {
            case 0:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Percent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    pbTarget.currentPercent = EditorGUILayout.Slider(pbTarget.currentPercent, 0, 1);

                    GUILayout.EndHorizontal();

                    if (pbTarget.loadingBar != null && pbTarget.textPercent != null && pbTarget.textValue != null)
                    {
                        pbTarget.Update();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("Some resources are not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Min Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(minValue, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Max Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(maxValue, new GUIContent(""));

                    GUILayout.EndHorizontal();
                }
                break;

            case 1:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Loading Bar"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(loadingBar, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Text Indicator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(textPercent, new GUIContent(""));

                    GUILayout.EndHorizontal(); 
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Text Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(textValue, new GUIContent(""));

                    GUILayout.EndHorizontal();
                }
                break;

            case 2:
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    IsPercent.boolValue = GUILayout.Toggle(IsPercent.boolValue, new GUIContent("Is Percent"), customSkin.FindStyle("Toggle"));
                    IsPercent.boolValue = GUILayout.Toggle(IsPercent.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    IsInt.boolValue = GUILayout.Toggle(IsInt.boolValue, new GUIContent("Is Int"), customSkin.FindStyle("Toggle"));
                    IsInt.boolValue = GUILayout.Toggle(IsInt.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                } 
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

