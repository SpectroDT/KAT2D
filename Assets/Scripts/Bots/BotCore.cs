using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(BotUICoreManager))]
public class BotCore : MonoBehaviour
{
    public List<GameObject> CanonsGO = new List<GameObject>();
    public GameObject ExplosionGO;

    private BotManager _botManager;
    private BotUICoreManager _botUIManager;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private int _maxHealth = 1;
    private bool _isInvinsible = false;
    private int _health;

    #region Private Methods
    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }
    private void FixedUpdate()
    {
        SetSeeByCamera(WorldInfosConst.IsSeenByCamera(gameObject.transform.position));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsConst.BULLET:
                if (_isInvinsible == false)
                {
                    if (collision.gameObject.TryGetComponent(out Bullet bullet))
                    {
                        TakeDamage(bullet.Damage);
                        bullet.DestroyHandler();
                    }
                    else
                    {
                        Debug.LogWarning("Cannot find component type Bullet");
                    }
                }
                break;
            default:
                break;
        }
    }
    private void TakeDamage(int amount)
    {
        if (_health > 0)
        {
            _health -= amount;
        }

        if (_health <= 0)
        {
            _health = 0;
            _spriteRenderer.color = Color.grey;
            _collider.enabled = false;
            ToggleCanonsActive(false);
            Instantiate(ExplosionGO, transform.position, Quaternion.identity);

            _botManager.OnCoreDestroyed(gameObject);
        }
    }

    private void SetSeeByCamera(bool value)
    {
        foreach (GameObject canonGO in CanonsGO)
        {
            BotCanon canon = canonGO.GetComponent<BotCanon>();
            canon.SetInCameraSight(value);
        }
    }
    private void InitializeHealth(bool randomizedMinHealth,
                               int minHealth,
                               bool randomizedMaxHealth,
                               int maxHealth)
    {
        int calculatedMinHealth = minHealth;
        int calculatedMaxHealth = maxHealth;

        if (randomizedMinHealth)
        {
            switch (_botManager.Category)
            {
                case BotCategoriesEnum.ALPHA:
                    calculatedMinHealth = Random.Range(AlphaBotConst.MIN_MIN_HEALTH, AlphaBotConst.MAX_MIN_HEALTH);
                    break;
                default:
                    break;
            }
        }

        if (randomizedMaxHealth)
        {
            switch (_botManager.Category)
            {
                case BotCategoriesEnum.ALPHA:
                    calculatedMaxHealth = Random.Range(AlphaBotConst.MAX_MAX_HEALTH, AlphaBotConst.MAX_MAX_HEALTH);
                    break;
                default:
                    break;
            }
        }

        _maxHealth = Random.Range(calculatedMinHealth, calculatedMaxHealth);
        _health = _maxHealth;
    }
    private void InitializationGuns(bool randomizedMaxBullet,
                               int maxBullet,
                               bool randomizedShootCooldown,
                               float shootCooldown,
                               bool randomizedReloadCooldown,
                               float reloadCooldown)
    {
        foreach (GameObject canon in CanonsGO)
        {
            BotCanon botCanon = canon.GetComponent<BotCanon>();
            botCanon.Initialization(_botManager,
                                    randomizedMaxBullet,
                                    maxBullet,
                                    randomizedShootCooldown,
                                    shootCooldown,
                                    randomizedReloadCooldown,
                                    reloadCooldown);
        }
    }
    private void UpdateUI()
    {
        _botUIManager.SetHealthVisibility(_health > 0); // _health > 0 = is not break = display health
        _botUIManager.SetHealthValue(_health);
    }
    #endregion

    #region Public Methods
    public void Initialization(BotManager botParent,
                               bool isInvinsible,
                               bool randomizedMinHealth,
                               int minHealth,
                               bool randomizedMaxHealth,
                               int maxHealth,
                               bool randomizedMaxBullet,
                               int maxBullet,
                               bool randomizedShootCooldown,
                               float shootCooldown,
                               bool randomizedReloadCooldown,
                               float reloadCooldown)
    {
        _botManager = botParent;
        _botUIManager = GetComponent<BotUICoreManager>();
        _isInvinsible = isInvinsible;
        InitializeHealth(randomizedMinHealth,
                         minHealth,
                         randomizedMaxHealth,
                         maxHealth);

        InitializationGuns(randomizedMaxBullet,
                           maxBullet,
                           randomizedShootCooldown,
                           shootCooldown,
                           randomizedReloadCooldown,
                           reloadCooldown);

        _botUIManager.SetHealthMaxValue(_maxHealth);
    }

    public void ToggleCanonsActive(bool value)
    {
        foreach (GameObject canonGO in CanonsGO)
        {
            BotCanon canon = canonGO.GetComponent<BotCanon>();
            canon.ToggleCanonActive(value);
        }
    }
    #endregion
}
