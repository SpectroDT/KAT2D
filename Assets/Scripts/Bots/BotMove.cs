using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    private BotMovePatternsEnum _botMoveType;
    private GameObject _startPointGO;
    private bool _canMove;
    private float _speed;
    private bool _loop;
    private bool _canRotate;
    private float _rotationSpeed;

    private BotManager _botManager;

    private List<GameObject> _movePointsGO = new List<GameObject>();
    private Vector3 _nextPos;

    #region Private Methods
    private void Start()
    {
        _movePointsGO = GameObject.FindGameObjectsWithTag(TagsConst.PATH).ToList();
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if (_canMove)
        {
            Vector3 curPos = transform.position;

            // if we are close enough to the target point
            if (Vector3.Distance(curPos, _startPointGO.transform.position) <= WorldInfosConst.VECTOR_DISTANCE_OFFSET
                || Vector3.Distance(curPos, _nextPos) <= WorldInfosConst.VECTOR_DISTANCE_OFFSET)
            {
                // get the next point to go
                GameObject nextPointGO = GetNextPointGO(curPos);

                // if we don't find it and we are looping
                if (nextPointGO is null && _loop)
                {
                    // go back to the start
                    nextPointGO = _startPointGO;
                    _botManager.OnBotGoingUpHandler();
                }
                else
                {
                    _botManager.OnBotGoingDownHandler();
                }

                _nextPos = nextPointGO.transform.position;
            }
            transform.position = Vector3.Lerp(curPos, _nextPos, _speed * Time.deltaTime);
        }
    }

    private GameObject GetNextPointGO(Vector3 curPos)
    {
        switch (_botMoveType)
        {
            case BotMovePatternsEnum.PATH_ONLY_RAND:
                return GetNextPointMoveRandomly();
            case BotMovePatternsEnum.PATH_ONLY_LINE:
                return GetNextPointMoveLine(curPos);
            case BotMovePatternsEnum.PATH_ONLY_STRAIGHT:
            default:
                return GetNextPointMoveStraight(curPos);
        }
    }

    /// <summary>
    /// Moving straight down 
    /// </summary>
    /// <param name="curPos">Current position of the bot</param>
    /// <example>
    /// Bot in Pos(0;0) will move at Pos(0,-1) then Pos(0,-2), etc
    /// </example>
    private GameObject GetNextPointMoveStraight(Vector3 curPos)
    {
        return _movePointsGO.FirstOrDefault(
                                point => point.transform.position.x == curPos.x
                                && point.transform.position.y < Mathf.Round(curPos.y));
    }

    /// <summary>
    /// Moving on the same line. Mainly use for boss
    /// </summary>
    /// <param name="curPos">Current bot position</param>
    /// <example>
    /// Bot in Pos(0;0) will move at Pos(-1,0) then Pos(4,0), etc
    /// </example>
    private GameObject GetNextPointMoveLine(Vector3 curPos)
    {
        // get player pos
        Vector3 playerPos = GameObject.FindGameObjectWithTag(TagsConst.PLAYER).transform.position;

        // if we are in the spawn point go down on the first line
        if (Mathf.Round(curPos.y) == _startPointGO.transform.position.y)
        {
            return _movePointsGO.FirstOrDefault(
                                point => point.transform.position.x == curPos.x
                                && point.transform.position.y < Mathf.Round(curPos.y)
                                && point.transform.position.y > Mathf.Round(playerPos.y));
        }
        else
        {
            // get a random point on this line
            List<GameObject> availablePoints = _movePointsGO.Where(point => point.transform.position.y == Mathf.Round(curPos.y)).ToList();
            return availablePoints[UnityEngine.Random.Range(0, availablePoints.Count)];
        }
    }

    /// <summary>
    /// Will take a random point in the waypoint list
    /// </summary>
    /// <param name="curPos"></param>
    private GameObject GetNextPointMoveRandomly()
    {
        // get player pos
        Vector3 playerPos = GameObject.FindGameObjectWithTag(TagsConst.PLAYER).transform.position;

        GameObject possiblePoints = _movePointsGO.FirstOrDefault(
                                point => point.transform.position.x == UnityEngine.Random.Range(0, _movePointsGO.Count)
                                && point.transform.position.y > Mathf.Round(playerPos.y));

        return possiblePoints;
    }
    private void Rotate()
    {
        if (_canRotate)
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    private void InitializeMovePattern(bool randomizedMovementPattern, BotMovePatternsEnum botMovePattern)
    {
        if (randomizedMovementPattern)
        {
            int r = UnityEngine.Random.Range(0, Enum.GetNames(typeof(BotMovePatternsEnum)).Length);
            _botMoveType = (BotMovePatternsEnum)r;
        }
        else
        {
            _botMoveType = botMovePattern;
        }
    }
    #endregion

    #region Public Methods
    public void Initialization(BotManager botParent,
                               bool randomizedMovementPattern,
                               BotMovePatternsEnum botMovePattern,
                               GameObject startPoint,
                               bool loop,
                               bool canMove,
                               float speed,
                               bool canRotate,
                               float rotationSpeed)
    {
        _botManager = botParent;
        _loop = loop;
        _startPointGO = startPoint;
        _canMove = canMove;
        _speed = speed;
        _canRotate = canRotate;
        _rotationSpeed = rotationSpeed;
        InitializeMovePattern(randomizedMovementPattern, botMovePattern);
    }
    #endregion
}
