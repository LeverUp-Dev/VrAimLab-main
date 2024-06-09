using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[Serializable]  // �ν����Ϳ� ���̴� ����ü�� ����
public struct Stats
{
    public int maxHP; // �ִ� ü��
    public int curHP; // ���� ü��

    public float attackDamage;      // ���ݷ�
    public float attackCoolTime;    // ���� ��Ÿ��
    public float attackRange;       // ���� �Ÿ�

    public float detectRadius;      // ���� ���� (������)
}
public class EnemyBaseXY : MonoBehaviour
{
    #region ��ġ��
    public Stats stats; // Stats ����ü����
    [SerializeField] protected float targetDistance; // Ÿ��(�÷��̾�)���� �Ÿ�
    [SerializeField] protected float stopDistance; // Ÿ��(�÷��̾�)���� ���ߴ� �Ÿ�
    protected float moveSpeedBackUp;    // �̵��ӵ� ��� ����
    protected float accelerationBackUp; // ���ӵ� ��� ����
    public float attackTime = 0f;// ���� ������
    
    #endregion

    #region ���°�
    protected bool isIdle;                  // ��� ���°� ( �̻�� )
    [SerializeField]public bool isAttack;   // ���� ���� ����
    [SerializeField]protected bool isWalk;  // �ȱ� �̵� ���� ����
    //protected bool isRun;                   // �޸��� �̵� ���� ���� ( �̻�� )
    [SerializeField]protected bool isChase; // �߰� ���� ����
    public bool isDie;   // ���� ���� ����
    protected bool isHit;

    public bool AttackTypeSC; // ��ũ��Ʈ ������, �ݶ��̴� ������ ����
    protected bool isDetected;
    protected bool isEnteredAttackRange;
    public bool isBattleCry;
    #endregion

    #region ��Ÿ
    [SerializeField]protected Collider[] colls; // ������ �ݶ��̴���
    [SerializeField]protected GameObject target; // Ÿ��(�÷��̾�) Ʈ������
    public Transform enemy; // �������� ���� ���� ����
    protected NavMeshAgent nav; // �׺���̼�
    protected Rigidbody rb;  // ������ٵ�
    [SerializeField]protected Animator anim;  // �ִϸ�����
    [SerializeField] private LayerMask targetLayer; // ������ ���̾��ũ
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
        colls = Physics.OverlapSphere(enemy.position, stats.detectRadius, (targetLayer | LayerMask.GetMask("Enemy")));   // Player ���̾� �ݶ��̴� ����
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
        print("������ " + stats.attackDamage + "�� ����");
        yield return new WaitForSeconds(attackTime); // �ִϸ��̼� �ð� + HasExitTime�� ����Ͽ� �Է����־���Ѵ�.
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
    protected IEnumerator Death() { // ���� �������� üũ
        while (!isDie)  // ���� ����
        {
            if (stats.curHP <= 0)
            {
                isDie = true;
                
                anim.SetTrigger("DoDeath");
                print("���� ����");

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
                yield break;    // �׾��� ��� �ڷ�ƾ ����
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
        
    protected void FreezeVelocity() { // ���� �� ���� ȿ�� ����
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    protected void StopMove() {
        nav.acceleration = 60;
        //nav.velocity = Vector3.zero;  // ���ӵ� �߰� or ���Ⱚ ���� �� �� �ϳ� ����
        nav.isStopped = true;
    }
    protected void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.position, stats.detectRadius); // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.position, stats.attackRange); // ���� ����
    }
}
