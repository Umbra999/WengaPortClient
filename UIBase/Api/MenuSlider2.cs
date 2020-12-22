using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WengaPort.Modules;

namespace WengaPort.Api
{
    class MenuSlider2
    {
        public MenuSlider2(Transform parent, string name, float x, float y, Action<float> evt, float defaultValue = 0f, float MaxValue = 1f, float MinValue = 0f, bool DisplayState = true)
        {
            Slider = UnityEngine.Object.Instantiate(Utils.VRCUiManager.menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeGameWorld"), parent).gameObject;
            UnityEngine.Object.Destroy(Slider.GetComponent<UiSettingConfig>());
            Slider.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            Slider.name = "Menu_Slider_2_" + name;
            labelObj = Slider.transform.Find("Label").gameObject;
            label = labelObj.GetComponent<Text>();
            SliderLabel = Slider.transform.Find("SliderLabel").GetComponent<Text>();
            SetPos(x, y);
            Slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            Slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            Slider.GetComponentInChildren<Slider>().onValueChanged = new Slider.SliderEvent();
            Slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(evt);
            if (DisplayState)
            {
                Slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(new Action<float>((value) =>
                {
                    SetSliderLabelText(Convert.ToInt32(Slider.GetComponentInChildren<Slider>().value).ToString());
                }));
            }
            SetMaxValue(MaxValue);
            SetMinValue(MinValue);
            SetValue(defaultValue);
            SetTextLabel(name);
        }
        public MenuSlider2(Transform parent, float x, float y, Action<float> evt, float defaultValue = 0f, float MaxValue = 1f, float MinValue = 0f, bool DisplayState = true)
        {
            Slider = UnityEngine.Object.Instantiate(Utils.VRCUiManager.menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeGameWorld"), parent).gameObject;
            UnityEngine.Object.Destroy(Slider.GetComponent<UiSettingConfig>());
            Slider.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            Slider.name = $"Menu_Slider_2_{x}_{y}";
            labelObj = Slider.transform.Find("Label").gameObject;
            label = labelObj.GetComponent<Text>();
            SliderLabel = Slider.transform.Find("SliderLabel").GetComponent<Text>();
            SetPos(x, y);
            Slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0f);
            Slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0f);
            Slider.GetComponentInChildren<Slider>().onValueChanged = new Slider.SliderEvent();
            Slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(evt);
            if (DisplayState)
            {
                Slider.GetComponentInChildren<Slider>().onValueChanged.AddListener(new Action<float>((value) =>
                {
                    SetSliderLabelText(Slider.GetComponentInChildren<Slider>().value.ToString());
                }));
            }
            SetMaxValue(MaxValue);
            SetMinValue(MinValue);
            SetValue(defaultValue);
        }

        public void SetMaxValue(float MaxValue)
        {
            Slider.GetComponentInChildren<Slider>().maxValue = MaxValue;
        }
        public void SetMinValue(float MinValue)
        {
            Slider.GetComponentInChildren<Slider>().minValue = MinValue;
        }
        public void SetPos(float x, float y)
        {
            Slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
        public void SetValue(float Value)
        {
            Slider.GetComponentInChildren<Slider>().value = Value;
        }
        public void SetTextLabel(string text)
        {
            label.text = text;
        }
        public void SetSliderLabelText(string Text)
        {
            SliderLabel.text = Text;
        }
        public void SetLocalLabelPosition(float x, float y)
        {
            labelObj.transform.position = new Vector3(x, y);
        }
        public GameObject Slider;
        public Text label;
        public Text SliderLabel;
        private GameObject labelObj;
    }
}
