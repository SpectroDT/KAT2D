using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use for displaying Bot Canon related informations like ammo, reloading
/// </summary>
public class BotUICanonManager : MonoBehaviour
{
    public Slider ReloadSlider;
    #region Private Methods

    #endregion

    #region Public Methods
    public void SetReloadVisibility(bool value)
    {
        ReloadSlider.gameObject.SetActive(value);
    }
    public void SetReloadValue(float value)
    {
        ReloadSlider.value = value;
    }
    public void SetReloadMaxValue(float value)
    {
        ReloadSlider.maxValue = value;
    }
    public void SetReloadMinValue(float value)
    {
        ReloadSlider.minValue = value;
    }
    #endregion
}
