using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use for displaying Bot Core related informations like health
/// </summary>
public class BotUICoreManager : MonoBehaviour
{
    public Slider HealthSlider;
    #region Private Methods

    #endregion

    #region Public Methods
    public void SetHealthVisibility(bool value)
    {
        HealthSlider.gameObject.SetActive(value);
    }
    public void SetHealthValue(float value)
    {
        HealthSlider.value = value;
    }
    public void SetHealthMaxValue(float value)
    {
        HealthSlider.maxValue = value;
    }
    public void SetHealthMinValue(float value)
    {
        HealthSlider.minValue = value;
    }
    #endregion
}
