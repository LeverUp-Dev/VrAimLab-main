using UnityEngine;
using System.Collections;
using VRFPSKit;

public class AimLabSystem : MonoBehaviour
{
    [SerializeField]
    public GameObject TargetPrefab;
    public GameObject SpawnArea;
    public GameObject CountDownText;
    public GameObject TimerText;
    public GameObject ScoreText;
    public GameObject Button;
    public GameObject BulletSpawnPoint;

    public float LimitTime;
    public float NowTime;
    public int Score;
    public int NowMin;
    public int NowSec;

    public bool IsGameStarted;
    public bool IsGameReallyStarted;

    public GameObject[] Targets;

    private Coroutine GameCoroutine;
    void Start()
    {
        CountDownText.SetActive(false);
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i] = Instantiate(TargetPrefab);
            Targets[i].transform.parent = SpawnArea.transform;
            Targets[i].transform.position = SpawnArea.transform.position;
            Targets[i].SetActive(false);
            
        }
    }

    void Update()
    {

        if (IsGameReallyStarted == true)
        {


            NowTime -= Time.deltaTime;
            NowMin = (int)NowTime / 60;
            NowSec = (int)NowTime % 60;

            string TimerTextCont = NowMin.ToString() + ":" + NowSec.ToString("D2");
            string NowScoreText = "Score:" + Score.ToString();
            TimerText.GetComponent<CountDown>().TextSet(TimerTextCont);
            ScoreText.GetComponent<CountDown>().TextSet(NowScoreText);
            if (NowTime <= 0)
            {
                StopGameCoroutine();
                for(int i = 0;i < Targets.Length; i++)
                {
                    Targets[i].SetActive(false);
                }

                CountDownText.SetActive(true);
                CountDownText.GetComponent<CountDown>().TextSet("GameOver");
            }
            else
            {
                for (int i = 0; i < Targets.Length; i++)
                {
                    if (Targets[i].GetComponent<Damageable>().health <= 0)
                    {
                        Debug.Log("Hit!");
                        Score++;
                        Targets[i].transform.position = RandomMove();
                        if (Targets[i].GetComponentInChildren<TargetController>().overlap == true)
                        {
                            Debug.Log("OOF! OverLaped!");
                            Targets[i].transform.position = RandomMove();
                        }
                        Targets[i].GetComponent<Damageable>().ResetHealth();
                    }
                }
            }
        }
    }

    public void StartGameCoroutine()
    {
        if(IsGameStarted == false) {
            Debug.Log("GameStarting");
            IsGameStarted = true;
            GameCoroutine = StartCoroutine("GameStart");
        }
    }

    public void StopGameCoroutine()
    {
        if(IsGameStarted== true)
        {
            Button.SetActive(true);
            Debug.Log("GameEnding");
            StopCoroutine(GameCoroutine);
            IsGameStarted = false;
            IsGameReallyStarted = false;

        }
    }

    IEnumerator GameStart()
    {

        Debug.Log("GameStarted Succese");
        CountDownText.SetActive(true);
        CountDownText.GetComponent<CountDown>().TextSet("5");
        Debug.Log("5");
        yield return new WaitForSeconds(1f);
        CountDownText.GetComponent<CountDown>().TextSet("4");
        Debug.Log("4");
        yield return new WaitForSeconds(1f);
        CountDownText.GetComponent<CountDown>().TextSet("3");
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        CountDownText.GetComponent<CountDown>().TextSet("2");
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        CountDownText.GetComponent<CountDown>().TextSet("1");
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        CountDownText.SetActive(false);
        Button.SetActive(false);

        Score = 0;
        NowTime = LimitTime;
        IsGameReallyStarted = true;

        for (int i = 0; i < Targets.Length; i++)
        {
            Debug.Log("RandomChecking" + i);
            Targets[i].SetActive(true);
            do
            {
                Debug.Log("overlap is true! randomizing" + i);
                Targets[i].transform.position = RandomMove();
            }
            while (Targets[i].GetComponentInChildren<TargetController>().overlap == true);
        }



    }
    
    public void SetTime(int Time)
    {
        LimitTime = Time;
        int TimeSettingMin = Time / 60;
        int TimeSettingSec = Time % 60;

        TimerText.GetComponent<CountDown>().TextSet(TimeSettingMin.ToString() + ":" + TimeSettingSec.ToString("D2"));
    }

    void GameEnd()
    {
        for(int i = 0; i < 5; i++)
        {
            Targets[i].SetActive(false);
        }
    }

    Vector3 RandomMove()
    {
        Debug.Log("Random Moving");
        Vector3 SpawnAreaOriginPosition = SpawnArea.transform.position;
        float size_x = SpawnArea.GetComponent<BoxCollider>().bounds.size.x;
        float size_y = SpawnArea.GetComponent<BoxCollider>().bounds.size.y;
        float random_x = Random.Range((size_x/2) * -1 , size_x / 2);
        float random_y = Random.Range((size_y/2) * -1 , size_y / 2);
        Vector3 RandomPosition = new Vector3(random_x, random_y, 0f);
        Vector3 FinalVector = SpawnAreaOriginPosition + RandomPosition;
        return FinalVector;
    }
    
}
