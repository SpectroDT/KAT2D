using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Slider HealthSlider;

    #region Private Methods

    #endregion

    #region Public Methods
    public void SetHealthVisibility(bool value)
    {
        HealthSlider.enabled = value;
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
