using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[Serializable]  // 인스펙터에 보이는 구조체로 정의
public struct Stats
{
    public int maxHP; // 최대 체력
    public int curHP; // 현재 체력

    public float attackDamage;      // 공격력
    public float attackCoolTime;    // 공격 쿨타임
    public float attackRange;       // 공격 거리

    public float detectRadius;      // 감지 범위 (반지름)
}
public class EnemyBaseXY : MonoBehaviour
{
    #region 수치값
    public Stats stats; // Stats 구조체변수
    [SerializeField] protected float targetDistance; // 타겟(플레이어)와의 거리
    [SerializeField] protected float stopDistance; // 타겟(플레이어)와의 멈추는 거리
    protected float moveSpeedBackUp;    // 이동속도 백업 변수
    protected float accelerationBackUp; // 가속도 백업 변수
    public float attackTime = 0f;// 공격 딜레이
    
    #endregion

    #region 상태값
    protected bool isIdle;                  // 대기 상태값 ( 미사용 )
    [SerializeField]public bool isAttack;   // 공격 상태 구분
    [SerializeField]protected bool isWalk;  // 걷기 이동 상태 구분
    //protected bool isRun;                   // 달리기 이동 상태 구분 ( 미사용 )
    [SerializeField]protected bool isChase; // 추격 상태 구분
    public bool isDie;   // 죽음 상태 구분
    protected bool isHit;

    public bool AttackTypeSC; // 스크립트 데미지, 콜라이더 데미지 구분
    protected bool isDetected;
    protected bool isEnteredAttackRange;
    public bool isBattleCry;
    #endregion

    #region 기타
    [SerializeField]protected Collider[] colls; // 감지할 콜라이더들
    [SerializeField]protected GameObject target; // 타겟(플레이어) 트랜스폼
    public Transform enemy; // 가독성을 위한 변수 선언
    protected NavMeshAgent nav; // 네비게이션
    protected Rigidbody rb;  // 리지드바디
    [SerializeField]protected Animator anim;  // 애니메이터
    [SerializeField] private LayerMask targetLayer; // 감지할 레이어마스크
    #endregion

    protected virtual void Awake() {
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        targetLayer = LayerMask.GetMask("Player");
        moveSpeedBackUp = nav.speed;
        accelerationBackUp = nav.acceleration;
    }
    private void OnEnable() {
        StartCoroutine(Death());
    }
    void Update() {
        if (!isDie)
        {
            DetectTarget();
            Chase();
        }
    }
    public void DetectTarget() {
        colls = Physics.OverlapSphere(enemy.position, stats.detectRadius, (targetLayer | LayerMask.GetMask("Enemy")));   // Player 레이어 콜라이더 감지
        foreach (Collider coll in colls)
        {
            if (coll.CompareTag("Player"))
            {
                isDetected = true;
                target = coll.gameObject;
                if (!isBattleCry)
                {
                    isBattleCry = true;
                    isChase = true;
                    isWalk = true;
                }
            }
            if(!isDetected)
            {
                //isChase = false;
                isBattleCry = false;
            }
                
            if (coll.CompareTag("Enemy") && isBattleCry)
            {
                EnemyBaseXY otherEnemy = coll.GetComponent<EnemyBaseXY>();
                otherEnemy.isBattleCry = true;
                otherEnemy.target = this.target;
                otherEnemy.isChase = true;
            }
        }
    }
    public virtual void Chase() {
        if (isChase)
        {
            targetDistance = Vector3.Distance(enemy.position, target.transform.position);
            FreezeVelocity();
            enemy.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));

            nav.SetDestination(target.transform.position);
            //nav.stoppingDistance = stopDistance;
            if (targetDistance < stopDistance || isAttack)
            {
                isWalk = false;
                StopMove();
            }
            else
            {
                isWalk = true;
                nav.isStopped = false;
                nav.acceleration = 8f;
            }
            if (targetDistance < stats.attackRange)
            {
                if (!isAttack && AttackTypeSC)
                {
                    StartCoroutine(AttackScript());
                }
                else if (!isAttack && !AttackTypeSC)
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }
    public virtual IEnumerator Attack() {
        isAttack = true;
        DeffencePlayer player = target.GetComponent<DeffencePlayer>();
        StopMove();
        yield return new WaitForSeconds(attackTime);
        nav.isStopped = false;
        isAttack = false;
    }
    public virtual IEnumerator AttackScript() {
        isAttack = true;
        //Player player = target.GetComponent<Player>();
        //weaponCol.enabled = true;
        anim.SetBool("IsAttack", true);
        target.GetComponent<DeffencePlayer>().curHp -= stats.attackDamage;
        print("데미지 " + stats.attackDamage + "를 입힘");
        yield return new WaitForSeconds(attackTime); // 애니메이션 시간 + HasExitTime을 고려하여 입력해주어야한다.
        anim.SetBool("IsAttack", false);
        //weaponCol.enabled = false;
        yield return new WaitForSeconds(stats.attackCoolTime);
        isAttack = false;
    }
    protected void GetBattleCry() {
        if (isBattleCry)
        {
            nav.destination = target.transform.position;
        }
    }
    protected IEnumerator Death() { // 죽은 상태인지 체크
        while (!isDie)  // 무한 루프
        {
            if (stats.curHP <= 0)
            {
                isDie = true;
                
                anim.SetTrigger("DoDeath");
                print("몬스터 죽음");

                Collider[] thiscols = GetComponentsInChildren<Collider>();
                foreach (Collider thiscol in thiscols)
                {
                    thiscol.enabled = false;
                }
                nav.enabled = false;
                this.gameObject.GetComponent<Collider>().enabled = false;
                yield return new WaitForSeconds(6f);
               
                //this.gameObject.SetActive(false);
                StopAllCoroutines();
                yield break;    // 죽었을 경우 코루틴 종료
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
        
    protected void FreezeVelocity() { // 추적 중 물리 효과 무시
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    protected void StopMove() {
        nav.acceleration = 60;
        //nav.velocity = Vector3.zero;  // 가속도 추가 or 방향값 제거 둘 중 하나 선택
        nav.isStopped = true;
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.position, stats.detectRadius); // 감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.position, stats.attackRange); // 공격 범위
    }
}
