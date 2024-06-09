using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerXY : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI score;

    public int scoreCount = 0;

    enum TimeList { normal=0,boss,bonus,rest };
    [Header ("NowTimeSet")]
    public int roundCount = 0;
    public int nowType = 4;
    public float nowTime = 0;
    
    [Header ("GameTimeSet")]
    public float normalTime = 20.0f;
    public float bossTime = 60.0f;
    public float bonusTime = 60.0f;
    public float restTime = 20.0f;
    [Header ("Commons")]
    public bool roundStart;
    [SerializeField]bool rest = true;
    [SerializeField]bool cycle = false;
    [Header("Reference")]
    public GameObject gameSystem;
    TargetSpawnXY targetSpawn;

    void Awake() {
        nowTime = restTime;
        targetSpawn = gameSystem.GetComponent<TargetSpawnXY>();
        
    }
    void Start() {
        roundText.text = "현재 라운드 : " + roundCount.ToString();
    }
    void Update()
    {
        TimeCountDown();            
    }
    void TimeCountDown() {
        if (nowTime > 0 && rest == false)
        {
            //print("전투 시작");
            nowTime -= Time.deltaTime;
            timeText.text = "남은 전투 시간 : " + Mathf.Ceil(nowTime).ToString(); //mathf.Ceil = 소숫점 올림 처리
        }
        if (nowTime <= 0 && rest == false){
            roundStart = false;
            rest = true;
            nowTime = restTime;
            nowType = (int)TimeList.rest;
        }
        if (nowTime > 0 && rest == true)
        {
            nowTime -= Time.deltaTime;
            timeText.text = "남은 휴식 시간 : " + Mathf.Ceil(nowTime).ToString(); //mathf.Ceil = 소숫점 올림 처리
        }
        if (nowTime <= 0 && rest == true)
        {
            print("휴식 종료");
            roundStart = true;
            rest = false;
            cycle = true;
        }
        //==============================================
        if (cycle == true)
        {
            nowTime = normalTime;
            nowType = (int)TimeList.normal;
            roundCount++;
            roundText.text = "현재 라운드 : " + roundCount.ToString();
            targetSpawn.AddTargetCount();
            cycle = false;
        }
        
       /* if (nowTime <= 0 && cycle == true)
        {
            RoundCalculator();
        }*/

    }
    void RoundCalculator() {
        roundCount++;
        roundText.text = "현재 라운드 : "+roundCount.ToString();


        
        if(roundCount % 2 == 0|| roundCount % 3 == 0|| roundCount % 4 == 0|| roundCount % 6 == 0
           || roundCount % 8 == 0|| roundCount % 9 == 0){
            nowType = (int)TimeList.normal;
        }else if (roundCount % 5 == 0 || roundCount % 10 == 0) { 
            nowType = (int)TimeList.boss;
        }else if (roundCount % 7 == 0)
            nowType = (int)TimeList.bonus;
        switch (nowType)
        {
            case (int)TimeList.normal:
                Debug.Log("normal");
                nowTime = normalTime;
                targetSpawn.AddTargetCount();
                print("타겟추가");
                    break;
            case (int)TimeList.boss:
                Debug.Log("boss");
                nowTime = bossTime; break;
            case (int)TimeList.bonus:
                nowTime = bonusTime; break;
            default:
                nowTime = restTime; break;
        }
        cycle = false;

    }
}
