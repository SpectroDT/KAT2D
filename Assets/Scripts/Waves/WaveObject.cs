using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveObject
{
    public bool IsBossWave = false;
    public float TimeBeforeNextWave = 5f;
    public float TimeBetweenBotSpawn = 1f;

    public List<GameObject> Bosses = new List<GameObject>();
    public List<GameObject> AlphaBots = new List<GameObject>();

    public WaveObject()
    {

    }

    public WaveObject(bool isBossWave, float timeBeforeNextWave, float timeBetweenBotSpawn)
    {
        IsBossWave = isBossWave; 
        TimeBeforeNextWave = timeBeforeNextWave;
        TimeBetweenBotSpawn = timeBetweenBotSpawn;
    }

    public void ClearBots()
    {
        AlphaBots.Clear();
        Bosses.Clear();
    }
}
