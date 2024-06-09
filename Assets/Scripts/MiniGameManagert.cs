using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManagert : MonoBehaviour
{
    public GameObject[] StartObj;

    public GameTimerXY gtXY;

    public void GameStart()
    {
        foreach (GameObject obj in StartObj)
        {
            obj.SetActive(true);
        }
    }

    public void GameReset()
    {
        foreach (GameObject obj in StartObj)
        {
            obj.SetActive(false);
        }

        gtXY.scoreCount = 0;
        gtXY.roundCount = 0;
        gtXY.nowTime = 0;

        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(e);
        }
    }
}
