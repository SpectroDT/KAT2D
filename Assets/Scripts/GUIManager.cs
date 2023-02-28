using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public GameObject PauseMenuGO;

    public TextMeshProUGUI WaveCountTxtPauseMenu;
    public TextMeshProUGUI WaveCountTxt;
    public TextMeshProUGUI MaxWaveCountTxtPauseMenu;
    public TextMeshProUGUI WaveInTxt;
    #region Private Methods
    #endregion
    #region Public Methods
    public void SetPauseMenuActive(bool value)
    {
        PauseMenuGO.SetActive(value);
    }

    public void SetWave(int value)
    {
        WaveCountTxtPauseMenu.text = value.ToString();
        WaveCountTxt.text = value.ToString();
    }
    public void SetMaxWave(int value)
    {
        MaxWaveCountTxtPauseMenu.text = value.ToString();
    }
    public void SetWaveIn(string value)
    {
        WaveInTxt.text = value;
    }
    #endregion
}
