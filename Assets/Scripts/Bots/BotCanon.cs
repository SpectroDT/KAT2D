using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BotUICanonManager))]
public class BotCanon : MonoBehaviour
{
    public GameObject CanonEndGO;
    public GameObject CanonBulletPrefab;

    private bool _isInCameraSight = false;
    private bool _canShoot = true;
    private float _shootCooldown;
    private float _shootTimer;

    private bool _isReloading = false;
    private int _ammo;
    private int _maxAmmo;
    private float _reloadCooldown;
    private float _reloadTimer;

    private SpriteRenderer _spriteRenderer;

    private BotManager _botManager;
    private BotUICanonManager _botUIManager;
    #region Private Methodds
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Fire
        if (_canShoot && _shootTimer <= 0)
        {
            Shoot();
            _shootTimer = _shootCooldown;
        }

        // Reloading
        if (_ammo == 0)
        {
            _isReloading = true;
            Reloading();
        }

        UpdateUI();

        _shootTimer -= Time.deltaTime;
    }

    private void Reloading()
    {
        if (_reloadTimer <= 0)
        {
            _ammo = _maxAmmo;
            _reloadTimer = _reloadCooldown;
            _isReloading = false;
        }
        _reloadTimer -= Time.deltaTime;
    }
    private void InitializeMaxBullet(bool randomizedMaxBullet, int maxBullet)
    {
        if (randomizedMaxBullet)
        {
            switch (_botManager.Category)
            {
                case BotCategoriesEnum.ALPHA:
                    _maxAmmo = UnityEngine.Random.Range(AlphaBotConst.MIN_CANON_AMMO, AlphaBotConst.MAX_CANON_AMMO);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _maxAmmo = maxBullet;
        }
        _ammo = _maxAmmo;
    }
    private void InitializeShootCooldown(bool randomizedShootCooldown, float shootCooldown)
    {
        if (randomizedShootCooldown)
        {
            switch (_botManager.Category)
            {
                case BotCategoriesEnum.ALPHA:
                    _shootCooldown = UnityEngine.Random.Range(AlphaBotConst.MIN_SHOOT_COOLDOWN, AlphaBotConst.MAX_SHOOT_COOLDOWN);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _shootCooldown = shootCooldown;
        }
    }
    private void InitializeReloadCooldown(bool randomizedReloadCooldown, float reloadCooldown)
    {
        if (randomizedReloadCooldown)
        {
            switch (_botManager.Category)
            {
                case BotCategoriesEnum.ALPHA:
                    _reloadCooldown = UnityEngine.Random.Range(AlphaBotConst.MIN_RELOAD_COOLDOWN, AlphaBotConst.MAX_RELOAD_COOLDOWN);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _reloadCooldown = reloadCooldown;
        }
        _reloadTimer = _reloadCooldown;
    }
    private void UpdateUI()
    {
        if (_isReloading)
        {
            // show reload timer
            _botUIManager.SetReloadMaxValue(_reloadCooldown);
            _botUIManager.SetReloadValue(_reloadTimer);
        }
        else
        {
            // show bullet remaining
            _botUIManager.SetReloadMaxValue(_maxAmmo);
            _botUIManager.SetReloadValue(_ammo);
        }
        _botUIManager.SetReloadVisibility(_canShoot);
    }
    #endregion

    #region Public Methods
    public void ToggleCanonActive(bool value)
    {
        _canShoot = value;

        if (_canShoot)
        {
            _spriteRenderer.color = Color.white;
        }
        else
        {
            _spriteRenderer.color = Color.grey;
        }
    }
    public void SetInCameraSight(bool value)
    {
        _isInCameraSight = value;
    }
    public void Shoot()
    {
        if (_isInCameraSight && _canShoot && _isReloading == false)
        {
            GameObject bullet = Instantiate(CanonBulletPrefab, CanonEndGO.transform.position, Quaternion.identity);
            // get direction of the canon
            Vector3 direction = CanonEndGO.transform.position - CanonEndGO.transform.parent.transform.position;
            bullet.GetComponent<Bullet>().SetDirection(direction);
            _ammo--;
        }
    }

    public void Initialization(BotManager botParent,
                               bool randomizedMaxBullet,
                               int maxBullet,
                               bool randomizedShootCooldown,
                               float shootCooldown,
                               bool randomizedReloadCooldown,
                               float reloadCooldown)
    {
        _botManager = botParent;
        _botUIManager = GetComponent<BotUICanonManager>();
        InitializeMaxBullet(randomizedMaxBullet, maxBullet);
        InitializeShootCooldown(randomizedShootCooldown, shootCooldown);
        InitializeReloadCooldown(randomizedReloadCooldown, reloadCooldown);
    }
    #endregion
}
