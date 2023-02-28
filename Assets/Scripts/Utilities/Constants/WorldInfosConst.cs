using UnityEngine;

public static class WorldInfosConst
{
    public static Vector3 SCREEN_START = new Vector3(-8.5f, 4.5f, 0);
    public static Vector3 SCREEN_END = new Vector3(8.5f, -4.5f, 0);

    public static float VECTOR_DISTANCE_OFFSET = 0.1f;

    public static bool IsSeenByCamera(Vector3 pos)
    {
        if ((pos.x > SCREEN_START.x && pos.x < SCREEN_END.x)
            && (pos.y < SCREEN_START.y && pos.y > SCREEN_END.y))
        {
            return true;
        }
        return false;
    }
}
