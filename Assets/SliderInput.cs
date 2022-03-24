using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderValueText;

    private void Awake()
    {
        var slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener((float value) => SetText(value));
        slider.onValueChanged.Invoke(slider.value);
    }
    public void SetText(float value)
    {
        sliderValueText.text = value.ToString("#0.##");
    }
}
