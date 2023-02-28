using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 1;
    public GameObject ExplosionGO;
    public float Speed = 10f;

    private Vector2 _direction;
    #region Private Methods
    private void FixedUpdate()
    {
        transform.Translate(Time.deltaTime * Speed * _direction);
    }
    #endregion

    #region Public Methods
    public void DestroyHandler()
    {
        Instantiate(ExplosionGO, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    #endregion
}
