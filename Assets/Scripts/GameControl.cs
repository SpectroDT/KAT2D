using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// GameManager seems to be reserved name so GameControl will do the job
public class GameControl : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerSpawnGO;

    private WaveManager _waveManager;
    private GUIManager _guiManager;

    private bool _gameIsRunning = true;
    private bool _playerIsDead = false;
    #region Private Methods
    // Start is called before the first frame update
    private void Start()
    {
        _waveManager = GetComponent<WaveManager>();
        _guiManager = GetComponent<GUIManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(ActionsConst.CANCEL) && _playerIsDead == false)
        {
            SwitchGameRunningMode();
        }

        if (_gameIsRunning == false 
            && Input.GetButton(ActionsConst.RETRY))
        {
            RestartGame();
        }

        if (_gameIsRunning == false
            && Input.GetButton(ActionsConst.ACTION))
        {
            OnBtnExitClicked();
        }
    }

    private void SwitchGameRunningMode(bool? forcedValue = null)
    {
        if (forcedValue == null)
        {
            _gameIsRunning = !_gameIsRunning;
        }
        else
        {
            _gameIsRunning = forcedValue.Value;
        }

        if (_gameIsRunning)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        TogglePauseMenu();
    }

    private void TogglePauseMenu()
    {
        // if game is in pause (_gameIsRunning = false) we show the menu else hide it
        _guiManager.SetPauseMenuActive(!_gameIsRunning);
    }
    private void RestartGame()
    {
        // Destroy all remaining Bots, BotBullets, Player, PlayerBullets
        List<string> tags = new List<string>() { TagsConst.BOT, TagsConst.BOTBULLET, TagsConst.PLAYER, TagsConst.BULLET };
        DestroyObjectsWithTag(tags);

        // Spawn player
        Instantiate(PlayerPrefab, PlayerSpawnGO.transform.position, Quaternion.identity);
        _playerIsDead = false;

        // Restart wave system
        _waveManager.Restart();

        // Resume game and hidding pause menu
        SwitchGameRunningMode(forcedValue: true);
    }
    private void DestroyObjectsWithTag(List<string> tags)
    {
        foreach (string tag in tags)
        {
            List<GameObject> gameObjects = GameObject.FindGameObjectsWithTag(tag).ToList();

            foreach (GameObject gameObject in gameObjects)
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion
    #region Public Methods
    public void OnBtnExitClicked()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
    public void OnBtnPlayClicked()
    {
        RestartGame();
    }
    public void OnPlayerDied()
    {
        _playerIsDead = true;
        SwitchGameRunningMode(false);
    }
    public void OnBossKilledHandler()
    {
        _waveManager.OnBossKilledHandler();
    }
    #endregion
}
