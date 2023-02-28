using UnityEngine;

public class PlayerCanon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject CanonGO;
    public bool CanShoot = true;

    private const float _fireCooldown = 0.25f;
    private float _fireTimer;

    // Update is called once per frame
    void Update()
    {
        if (CanShoot
            && Input.GetButton(ActionsConst.FIRE)
            && _fireTimer <= 0)
        {
            GameObject bullet = Instantiate(BulletPrefab, CanonGO.transform.position, Quaternion.identity);
            // get direction of the canon
            Vector3 direction = CanonGO.transform.position - CanonGO.transform.parent.transform.position;
            bullet.GetComponent<Bullet>().SetDirection(direction);
            _fireTimer = _fireCooldown;
        }
        _fireTimer -= Time.deltaTime;
    }
}
