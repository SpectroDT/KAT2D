using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform ObjectToFollow;
    public Vector3 Offset;

    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _mainCamera.WorldToScreenPoint(ObjectToFollow.position + Offset);
    }
}
