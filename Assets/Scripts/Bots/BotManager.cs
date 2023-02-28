using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [Header("General Infos")]
    public string Name = "Bot";
    public bool IsBoss = false;
    public BotCategoriesEnum Category = BotCategoriesEnum.ALPHA;

    [Header("Movements")]
    public bool RandomizedMovementPattern = false;
    public BotMovePatternsEnum MovementPattern = BotMovePatternsEnum.PATH_ONLY_STRAIGHT;
    public bool LoopMoveUntilDead = false;
    public bool CanMove = true;
    public float Speed;
    public bool CanRotate = false;
    public float RotationSpeed;
    private BotMove _botMove;

    [Header("Cores")]
    public List<GameObject> CoresGO = new List<GameObject>();
    public bool RandomizedMinHealth = false;
    public int MinHealthPerCore;
    public bool RandomizedMaxHealth = false;
    public int MaxHealthPerCore;
    public bool IsInvinsible = false;
    public GameObject ExplosionGO;

    [Header("Guns")]
    public bool RandomizedMaxBullet = true;
    public int MaxBullet;
    public bool RandomizedShootRate = false;
    public float ShootCooldown;
    public bool RandomizedReloadRate = false;
    public float ReloadCooldown;

    [Header("Loot")]
    public bool RandomizedLootAmount = true;
    public int LootAmount = 5;
    private LootManager _lootManager;

    #region Private Methods
    private void InitializeLoot()
    {
        _lootManager = GetComponent<LootManager>();

        _lootManager.Initialisation(RandomizedLootAmount, LootAmount);
    }
    private void InitializeBotCore()
    {
        foreach (GameObject coreGO in CoresGO)
        {
            BotCore core = coreGO.GetComponent<BotCore>();
            core.Initialization(this,
                                IsInvinsible,
                                RandomizedMinHealth,
                                MinHealthPerCore,
                                RandomizedMaxHealth,
                                MaxHealthPerCore,
                                RandomizedMaxBullet,
                                MaxBullet,
                                RandomizedShootRate,
                                ShootCooldown,
                                RandomizedReloadRate,
                                ReloadCooldown);
        }
    }

    private void InitializeBotMove(GameObject spawnPoint)
    {
        _botMove = GetComponentInChildren<BotMove>();
        _botMove.Initialization(this,
                                RandomizedMovementPattern,
                                MovementPattern,
                                spawnPoint,
                                LoopMoveUntilDead,
                                CanMove,
                                Speed,
                                CanRotate,
                                RotationSpeed);
    }
    #endregion

    #region Public Methods
    public void InitializeBot(GameObject spawnPoint)
    {
        InitializeLoot();
        InitializeBotCore();
        InitializeBotMove(spawnPoint);
    }

    /// <summary>
    /// Occured when the bot is looping and going up
    /// Bot can't shoot
    /// </summary>
    public void OnBotGoingUpHandler()
    {
        foreach (GameObject core in CoresGO)
        {
            BotCore botCore = core.GetComponent<BotCore>();
            botCore.ToggleCanonsActive(false);
        }
    }

    /// <summary>
    /// Occured when the bot is going down
    /// Bot can shoot
    /// </summary>
    public void OnBotGoingDownHandler()
    {
        foreach (GameObject core in CoresGO)
        {
            BotCore botCore = core.GetComponent<BotCore>();
            botCore.ToggleCanonsActive(true);
        }
    }

    /// <summary>
    /// Occured when a Core is destroyed
    /// Remove the core from the Cores, spawn explosion, looting, notifying
    /// </summary>
    /// <param name="core">Destroyed core</param>
    public void OnCoreDestroyed(GameObject core)
    {
        CoresGO.Remove(core);
        if (CoresGO.Count == 0)
        {
            Instantiate(ExplosionGO, transform.position, Quaternion.identity);
            if (IsBoss)
            {
                // loot health
                _lootManager.Loot();

                // notice senpai
                GameControl gameManager = GameObject.FindGameObjectWithTag(TagsConst.GAME_MANAGER).GetComponent<GameControl>();
                gameManager.OnBossKilledHandler();
            }
            Destroy(gameObject); 
        }
    }
    #endregion
}
