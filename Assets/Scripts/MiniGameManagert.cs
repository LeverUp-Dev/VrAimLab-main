using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManagert : MonoBehaviour
{
    public GameObject[] StartObj;

    [SerializeField]private GameTimerXY gtXY;
    [SerializeField]private TargetSpawnXY tsXY;
    [SerializeField] private DeffencePlayer dfXY;
    [SerializeField] private GameObject failtext;
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
        gtXY.roundStart = false;
        tsXY.isLocked = false;
        dfXY.curHp = dfXY.maxHP;
        for(int i = 0; i < tsXY.targetCount.Length; i++)
        {
            tsXY.targetCount[i] = 1;
            tsXY.targetCountTemp[i] = 1;
        }
        failtext.SetActive(false);
        
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(e);
        }
    }
}
