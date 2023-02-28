using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float time = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);   
    }
}
