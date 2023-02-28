using UnityEngine;

public class BorderBulletDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsConst.BULLET:
            case TagsConst.BOTBULLET:
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
