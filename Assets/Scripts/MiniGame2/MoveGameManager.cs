using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveGameManager : MonoBehaviour
{
    public GameObject targetPrefabLeft;
    public GameObject targetPrefabRight;
    public Transform[] spawnPoints;
    public GameObject gameEndUI;

    public int time;
    int currentTime;
    public Text timerTxt;
    public int score;
    public Text scoreTxt;

    bool isStart = false;

    private void Awake()
    {
        timerTxt.text = time / 60 + " : " + time % 60;
    }

    public void GameStart()
    {
        if (isStart == true)
            return;

        isStart = true;
        currentTime = time;
        timerTxt.text = time / 60 + " : " + time % 60;
        scoreTxt.text = "Á¡¼ö: " + 0;
        score = 0;
        gameEndUI.SetActive(false);

        StartCoroutine(StartTime());
        StartCoroutine(SpawnTarget());
    }

    IEnumerator StartTime()
    {
        yield return null;

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            timerTxt.text = currentTime < 60 ? 0 + " : " + currentTime%60 : currentTime/60 + " : " + currentTime % 60;
        }

        gameEndUI.SetActive(true);
        isStart = false;

        yield break;
    }

    IEnumerator SpawnTarget()
    {
        yield return null;

        while (currentTime > 0)
        {
            int ranDelay = Random.Range(1, 5);
            int ranNum = Random.Range(0, 6);

            if (ranNum <= 2)
            {
                Instantiate(targetPrefabLeft, spawnPoints[ranNum].position, Quaternion.identity);
            }
            else
            {
                Instantiate(targetPrefabRight, spawnPoints[ranNum].position, Quaternion.identity);
            }

            yield return new WaitForSeconds(ranDelay);
        }

        foreach (var t in FindObjectsOfType<TargetMove>())
        {
            Destroy(t.gameObject);
        }
        yield break;
    }
}
