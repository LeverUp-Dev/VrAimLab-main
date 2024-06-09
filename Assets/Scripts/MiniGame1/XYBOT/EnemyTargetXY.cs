using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTargetXY : EnemyBaseXY
{
    public GameObject weapon;
    public Collider weaponCol;
    public int animState;
    public bool isCrouch;
    public bool isRun;
    public float walkSpeed;
    public float crouchSpeed;
    public float runSpeed;
    override protected void Awake() {
        weaponCol = weapon.GetComponent<Collider>();  // ���� �ݶ��̴� �޾ƿ���
        base.Awake();  //�θ� Ŭ���� Awake ����
    }
    private void Start() {
        isAttack = false;
        animState = UnityEngine.Random.Range(0, 3);
        print("������" + animState);
        switch (animState)
        {
            case 1: // ���̱�
                print("���̱��"+gameObject.name);
                isRun = false;
                isCrouch = true;
                nav.speed = crouchSpeed;
                break;
            case 2: // �ٱ�
                isRun = true;
                isCrouch = false;
                nav.speed = runSpeed;
                break;
            default: // �ȱ�
                isRun = false;
                isCrouch = false;
                nav.speed = walkSpeed;
                break;
        }
    }
    void Update() {
        if(!isDie)  // ���� �ʾҴٸ�
        {
            Anim();  // ���°��� ���� �ִϸ��̼� ����
            base.DetectTarget();  // �� ����
            Chase();              // �߰� ���� (�������̵�)
            StartCoroutine(Death()); // ���� ����
        }
    }
    public override void Chase() {  // EnemyWarrior ���� �������̵�
        if (isChase)
        {
            weapon.GetComponent<EnemyWeaponXY>().targetObj = target;
            weapon.GetComponent<EnemyWeaponXY>().targetSc = target.GetComponent<DeffencePlayer>();
            targetDistance = Vector3.Distance(enemy.position, target.transform.position);   // Target(�÷��̾�)���� �Ÿ� ���
            base.FreezeVelocity();  // ���� �� ������ ���� ���� ��ȯ ����
            enemy.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z)); // Target(�÷��̾�) �ü� ����

            nav.SetDestination(target.transform.position);  // Target(�÷��̾�)�� ���� �̵� ���
            //nav.stoppingDistance = stopDistance;
            if (targetDistance < stopDistance || targetDistance < stats.attackRange)  // ���� �Ÿ� �� ���°��� ���� ���� ���
            {
                isWalk = false;
                base.StopMove();    // ���� ������ ���� ���ӵ� ���� �� ����
            }
            else
            {
                isWalk = true;
                nav.isStopped = false;
                nav.acceleration = accelerationBackUp;  // ���ӵ� ���
            }
            if (targetDistance < stats.attackRange)
            {
                anim.SetBool("IsAttackIdle", true);
                if (!isAttack&&base.AttackTypeSC)
                {
                    StartCoroutine(AttackScript());
                }
                else if (!isAttack && !base.AttackTypeSC)
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }
    public override IEnumerator Attack() {
        isAttack = true;
        //Player player = target.GetComponent<Player>();
        weaponCol.enabled = true;
        anim.SetBool("IsAttack", true);
        yield return new WaitForSeconds(attackTime); // �ִϸ��̼� �ð� + HasExitTime�� ����Ͽ� �Է����־���Ѵ�.
        anim.SetBool("IsAttack", false);
        weaponCol.enabled = false;
        yield return new WaitForSeconds(stats.attackCoolTime); 
        isAttack = false;
    }
    public override IEnumerator AttackScript() {
        isAttack = true;
        //Player player = target.GetComponent<Player>();
        //weaponCol.enabled = true;
        anim.SetBool("IsAttack", true);
        target.GetComponent<DeffencePlayer>().curHp -= base.stats.attackDamage;
        print("������ " + base.stats.attackDamage + "�� ����");
        yield return new WaitForSeconds(attackTime); // �ִϸ��̼� �ð� + HasExitTime�� ����Ͽ� �Է����־���Ѵ�.
        anim.SetBool("IsAttack", false);
        //weaponCol.enabled = false;
        yield return new WaitForSeconds(stats.attackCoolTime);
        isAttack = false;
    }
    public void Anim() {
        if (isWalk && isChase && !isDie && !isAttack &&!nav.isStopped)
        {
            if (isCrouch)
                anim.SetBool("IsCrouch", true);
            else if (isRun)
                anim.SetBool("IsRun", true);
            else
                anim.SetBool("IsWalk", true);
        }
        else
        {
            if (isCrouch)
                anim.SetBool("IsCrouch", false);
            else if (isRun)
                anim.SetBool("IsRun", false);
            else
                anim.SetBool("IsWalk", false);
        }
            
    }
}