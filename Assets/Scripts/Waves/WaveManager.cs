using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public bool IsActive = false;

    public GameObject AlphaBotPrefab;
    public GameObject AlphaBossBotPrefab;

    private List<GameObject> _spawnPointsGO = new List<GameObject>();

    private GUIManager _guiManager;

    private int _waveScore;
    private int _maxWaveScore;

    private WaveObject _wave = new WaveObject();
    private Queue<GameObject> _botsToSpawnQueue = new Queue<GameObject>();

    private float _spawnTimer;
    private float _waveTimer;
    #region Private Methods
    // Start is called before the first frame update
    void Start()
    {
        _spawnPointsGO = GameObject.FindGameObjectsWithTag(TagsConst.SPAWN_POINT).ToList();
        _guiManager = GetComponent<GUIManager>();
        _maxWaveScore = _waveScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            SpawnWave();
            SpawnBot();

            UpdateUI();
        }
    }

    private void SpawnWave()
    {
        if (_wave.IsBossWave == false)
        {
            if (_waveTimer <= 0)
            {
                _waveTimer = 0;

                // update wave score
                UpdateWaveScore();
                // generate new wave
                if (_waveScore <= WavesConst.TUTORIAL_WAVE_COUNT)
                {
                    GenerateTutorialWaves();
                }
                else if (_waveScore % WavesConst.BOSS_WAVE_EVERY_X_WAVES == 0)
                {
                    GenerateBossWave();
                }
                else
                {
                    GenerateRandomWave();
                }

                UpdateBotsSpawnQueue();

                // set next spawn time
                _waveTimer = _wave.TimeBeforeNextWave;
            }
            _waveTimer -= Time.deltaTime;
        }
    }

    private void SpawnBot()
    {
        if (_spawnTimer <= 0 && _botsToSpawnQueue.Count > 0)
        {
            _spawnTimer = 0;

            // Get which spawn point will be used
            GameObject spawnPoint = _spawnPointsGO[UnityEngine.Random.Range(0, _spawnPointsGO.Count - 1)];

            // Spawn Bot and initialize it
            GameObject bot = Instantiate(_botsToSpawnQueue.Dequeue(), spawnPoint.transform.position, Quaternion.identity);
            bot.GetComponent<BotManager>().InitializeBot(spawnPoint);

            // set next spawn time
            _spawnTimer = _wave.TimeBetweenBotSpawn;
        }
        _spawnTimer -= Time.deltaTime;
    }

    private void UpdateWaveScore()
    {
        _waveScore++;

        if (_waveScore > _maxWaveScore)
        {
            _maxWaveScore = _waveScore;
        }

        // Update GUI
        SetWaveGUI();
    }

    private void SetWaveGUI()
    {
        _guiManager.SetWave(_waveScore);
        _guiManager.SetMaxWave(_maxWaveScore);
    }

    private void GenerateTutorialWaves()
    {
        _wave.ClearBots();

        _wave.IsBossWave = false;
        _wave.TimeBetweenBotSpawn = WavesConst.MIN_WAVE_TIME_BETWEEN_BOT_SPAWN;
        _wave.TimeBeforeNextWave = WavesConst.MIN_TIME_BEFORE_NEXT_WAVE;

        GameObject customAlphaBotPrefab = AlphaBotPrefab;
        BotManager customAlphaBotPrefabBotComp = customAlphaBotPrefab.GetComponent<BotManager>();
        customAlphaBotPrefabBotComp.RandomizedMaxBullet = false;
        customAlphaBotPrefabBotComp.RandomizedReloadRate = false;
        customAlphaBotPrefabBotComp.RandomizedShootRate = false;
        customAlphaBotPrefabBotComp.RandomizedMinHealth = false;
        customAlphaBotPrefabBotComp.RandomizedMaxHealth = false;
        customAlphaBotPrefabBotComp.RandomizedMovementPattern = false;
        customAlphaBotPrefabBotComp.MaxHealthPerCore = 3;
        customAlphaBotPrefabBotComp.MinHealthPerCore = 3;
        customAlphaBotPrefabBotComp.MaxBullet = 5;
        customAlphaBotPrefabBotComp.MaxBullet = 5;
        customAlphaBotPrefabBotComp.ShootCooldown = 1f;
        customAlphaBotPrefabBotComp.ReloadCooldown = 3f;
        customAlphaBotPrefabBotComp.MovementPattern = BotMovePatternsEnum.PATH_ONLY_STRAIGHT;

        if (_waveScore == 1)
        {
            _wave.AlphaBots.Add(customAlphaBotPrefab);
        }
        else if (_waveScore == 2)
        {
            _wave.AlphaBots.Add(customAlphaBotPrefab);
            _wave.AlphaBots.Add(customAlphaBotPrefab);
        }
        else
        {
            _wave.AlphaBots.Add(customAlphaBotPrefab);
            _wave.AlphaBots.Add(customAlphaBotPrefab);
            _wave.AlphaBots.Add(customAlphaBotPrefab);
        }
    }
    private void GenerateRandomWave()
    {
        _wave.ClearBots();

        _wave.IsBossWave = false;
        _wave.TimeBetweenBotSpawn = UnityEngine.Random.Range(
            WavesConst.MIN_WAVE_TIME_BETWEEN_BOT_SPAWN,
            WavesConst.MAX_WAVE_TIME_BETWEEN_BOT_SPAWN);

        // Use _wave.TimeBetweenBotSpawn as minim because we want to spawn all bots before spawning new wave
        _wave.TimeBeforeNextWave = UnityEngine.Random.Range(
            _wave.TimeBetweenBotSpawn,
            WavesConst.MAX_TIME_BEFORE_NEXT_WAVE);

        // TODO put 0 instead of 1 as minimum when more bot types will be available 
        // reason: allow a wave to have just a type of bot instead of at least 1 of each
        // carefull of no bot in wave
        int rdmAlphaBotCount = UnityEngine.Random.Range(1, WavesConst.MAX_ALPHA_BOTS_PER_WAVE);

        GameObject customAlphaBotPrefab = AlphaBotPrefab;
        BotManager customAlphaBotPrefabBotComp = customAlphaBotPrefab.GetComponent<BotManager>();
        customAlphaBotPrefabBotComp.RandomizedMaxBullet = true;
        customAlphaBotPrefabBotComp.RandomizedReloadRate = true;
        customAlphaBotPrefabBotComp.RandomizedShootRate = true;
        customAlphaBotPrefabBotComp.RandomizedMinHealth = true;
        customAlphaBotPrefabBotComp.RandomizedMaxHealth = true;
        customAlphaBotPrefabBotComp.RandomizedMovementPattern = true;

        for (int i = 0; i < rdmAlphaBotCount; i++)
        {
            _wave.AlphaBots.Add(customAlphaBotPrefab);
        }
    }
    private void GenerateBossWave()
    {
        _wave.ClearBots();

        _wave.IsBossWave = true;
        _wave.TimeBeforeNextWave = WavesConst.TIME_BEFORE_NEXT_WAVE_AFTER_BOSS;

        GameObject customBot = AlphaBossBotPrefab;
        BotManager customBotManager = customBot.GetComponent<BotManager>();
        customBotManager.RandomizedMaxBullet = true;
        customBotManager.RandomizedReloadRate = true;
        customBotManager.RandomizedShootRate = true;
        customBotManager.RandomizedMinHealth = true;
        customBotManager.RandomizedMaxHealth = true;
        customBotManager.RandomizedMovementPattern = false;
        customBotManager.MovementPattern = BotMovePatternsEnum.PATH_ONLY_LINE;

        _wave.Bosses.Add(customBot);
    }
    private void UpdateBotsSpawnQueue()
    {
        // TODO Found a proper way to shuffle the queue
        foreach (GameObject bot in _wave.AlphaBots)
        {
            _botsToSpawnQueue.Enqueue(bot);
        }
        foreach (GameObject bot in _wave.Bosses)
        {
            _botsToSpawnQueue.Enqueue(bot);
        }
    }

    private void UpdateUI()
    {
        if (_wave.IsBossWave)
        {
            _guiManager.SetWaveIn(WavesConst.BOSS_WAVE_IN_CAPTION);
        }
        else
        {
            _guiManager.SetWaveIn(Mathf.Round(_waveTimer).ToString());
        }
    }
    #endregion
    #region Public Methods
    public void Restart()
    {
        IsActive = true;
        _waveScore = 0;
        _botsToSpawnQueue.Clear();
        _spawnTimer = 0;
        _waveTimer = 0;
    }

    public void OnBossKilledHandler()
    {
        _wave.IsBossWave = false;
    }
    #endregion
}
