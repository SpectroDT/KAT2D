using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerManager : MonoBehaviour
{
    [Header("General Infos")]
    public string Name = "Player";
    public int MaxHealth = 10;
    public bool IsInvincible = false;
    public GameObject ExplosionGO;
    private int _health;

    private PlayerMove _playerMove;
    private PlayerCanon _playerCanon;
    private PlayerUIManager _playerUIManager;

    private GameControl _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _health = MaxHealth;
        _playerMove = GetComponent<PlayerMove>();
        _playerCanon = GetComponent<PlayerCanon>();
        
        _playerUIManager = GetComponent<PlayerUIManager>();
        _playerUIManager.SetHealthMaxValue(MaxHealth);

        _gameManager = GameObject.FindGameObjectWithTag(TagsConst.GAME_MANAGER).GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsConst.BOTBULLET:
                if (IsInvincible == false)
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
            // taking a damage is called even with a healing so need to check if we are above the max
            // and if so re-set the value to the max
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
        }
        else
        {
            _health = 0;
            OnDead();
        }
    }

    private void OnDead()
    {
        // Disable any movement or shoots
        _playerMove.CanMove = false;
        _playerCanon.CanShoot = false;

        // instantiate explosion go
        Instantiate(ExplosionGO, transform.position, Quaternion.identity);

        // notify senpai
        _gameManager.OnPlayerDied();

        // destroy
        Destroy(gameObject);
    }

    private void UpdateUI()
    {
        _playerUIManager.SetHealthValue(_health);
    }
}
