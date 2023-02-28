using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;
    public bool CanMove = true;

    #region Private Methods
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (CanMove)
        {
            if (Input.GetAxisRaw(ActionsConst.HORIZONTAL) < 0
                && transform.position.x > PlayerConst.PLAYER_BORDER_LEFT)
            {
                transform.Translate(Speed * Time.deltaTime * Vector2.left);

            }
            else if (Input.GetAxisRaw(ActionsConst.HORIZONTAL) > 0
                && transform.position.x < PlayerConst.PLAYER_BORDER_RIGHT)
            {
                transform.Translate(Speed * Time.deltaTime * Vector2.right);
            }

            if (Input.GetAxisRaw(ActionsConst.VERTICAL) < 0
                && transform.position.y > PlayerConst.PLAYER_BORDER_DOWN)
            {
                transform.Translate(Speed * Time.deltaTime * Vector2.down);

            }
            else if (Input.GetAxisRaw(ActionsConst.VERTICAL) > 0
                && transform.position.y < PlayerConst.PLAYER_BORDER_UP)
            {
                transform.Translate(Speed * Time.deltaTime * Vector2.up);
            }
        }
    }
    #endregion
    #region Public Methods

    #endregion
}
