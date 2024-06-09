using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TargetSpawnXY : MonoBehaviour
{
    public GameObject UI;
    public GameObject normalTarget;
    public GameObject crouchedTarget;
    GameTimerXY gameTimer;
    public Transform[] targetSpawnPos;
    public enum TargetType { normalType = 0, crouchedType };
    //int EnumSize = Enum.GetValues(typeof(ZombieType)).Length; // ������ ���� �޾ƿ���. ���Ŀ� �迭 ���¸� ����Ʈ�� �ٲ� ��.
    public float[] SpawnInterval; // �� ��ȯ ����
    public int[] targetCount;
    public int[] targetCountTemp;
    public bool isLocked;
    public bool spawnTypeInterval; // �ѱ� : ���� �ð� �� ��ȯ ���� : ���� ���� �ð��� ��ȯ���� ���� ��ȯ
    void Awake() {
        gameTimer = UI.GetComponent<GameTimerXY>();
        targetCountTemp = new int[targetCount.Length]; //temp �迭 �ʱ�ȭ
        TempReset();
    }
    void Update() {
        if (gameTimer.roundStart == true)
        {
            switch (gameTimer.nowType)
            {
                case 0:
                    if (targetCountTemp[(int)TargetType.normalType] > 0 && !isLocked)
                    {
                        print("Test");
                        isLocked = true;  //������Ʈ���� ���� ���� ����
                        StartCoroutine(SpawnEnemy_Normal());
                        //isLocked = true;    //������Ʈ���� ���� ���� ����
                    }
                    //if (gameTimer.roundCount > 6)
                        //StartCoroutine(SpawnEnemy_Crouched());
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default: break;
            }
        }
    }
    IEnumerator SpawnEnemy_Normal() {
        if (targetCountTemp[(int)TargetType.normalType] > 0)
        {
            int spawnPoint = UnityEngine.Random.Range(0, 4);
            GameObject Target_Normal = Instantiate(normalTarget, targetSpawnPos[spawnPoint].position, targetSpawnPos[spawnPoint].rotation);
            UnityEngine.Debug.Log("��ȯ");
            if (spawnTypeInterval)
                yield return new WaitForSeconds(SpawnInterval[(int)TargetType.normalType]);
            else
                yield return new WaitForSeconds(gameTimer.normalTime / targetCount.Length);
            //print(gameTimer.normalTime / targetCount.Length);
            targetCountTemp[(int)TargetType.normalType]--;
            UnityEngine.Debug.Log("����");
            isLocked = false;
        }
    }
    IEnumerator SpawnEnemy_Crouched() {
        while (targetCountTemp[(int)TargetType.crouchedType] > 0)
        {
            int spawnPoint = UnityEngine.Random.Range(0, 4);
            //GameObject Target_Runner = Instantiate(crouchedTarget, targetSpawnPos[spawnPoint].position, targetSpawnPos[spawnPoint].rotation);
            yield return new WaitForSeconds(SpawnInterval[(int)TargetType.crouchedType]);
        }
    }
    void TempReset() {
        for (int i = 0; i < targetCount.Length; i++)
        {
            targetCountTemp[i] = targetCount[i];
        }
    }
    public void AddTargetCount() {
        targetCount[(int)TargetType.normalType]++;
        if (gameTimer.roundCount >= 3)
        {
            targetCount[(int)TargetType.normalType]++;
        }
        if (gameTimer.roundCount >= 8 && gameTimer.roundCount % 2 == 0)
        {
            //targetCount[(int)TargetType.crouchedType] += 1;
        }
        TempReset();
    }
}