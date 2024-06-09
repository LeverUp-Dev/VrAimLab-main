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
        weaponCol = weapon.GetComponent<Collider>();  // 웨폰 콜라이더 받아오기
        base.Awake();  //부모 클래스 Awake 실행
    }
    private void Start() {
        isAttack = false;
        animState = UnityEngine.Random.Range(0, 3);
        print("ㅁㄴㅇ" + animState);
        switch (animState)
        {
            case 1: // 숙이기
                print("숙이기기"+gameObject.name);
                isRun = false;
                isCrouch = true;
                nav.speed = crouchSpeed;
                break;
            case 2: // 뛰기
                isRun = true;
                isCrouch = false;
                nav.speed = runSpeed;
                break;
            default: // 걷기
                isRun = false;
                isCrouch = false;
                nav.speed = walkSpeed;
                break;
        }
    }
    void Update() {
        if(!isDie)  // 죽지 않았다면
        {
            Anim();  // 상태값에 따라 애니메이션 실행
            base.DetectTarget();  // 적 감지
            Chase();              // 추격 실행 (오버라이드)
            StartCoroutine(Death()); // 죽음 감지
        }
    }
    public override void Chase() {  // EnemyWarrior 추적 오버라이드
        if (isChase)
        {
            weapon.GetComponent<EnemyWeaponXY>().targetObj = target;
            weapon.GetComponent<EnemyWeaponXY>().targetSc = target.GetComponent<DeffencePlayer>();
            targetDistance = Vector3.Distance(enemy.position, target.transform.position);   // Target(플레이어)와의 거리 계산
            base.FreezeVelocity();  // 추적 중 물리에 의한 방향 전환 고정
            enemy.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z)); // Target(플레이어) 시선 고정

            nav.SetDestination(target.transform.position);  // Target(플레이어)에 대한 이동 명령
            //nav.stoppingDistance = stopDistance;
            if (targetDistance < stopDistance || targetDistance < stats.attackRange)  // 정지 거리 및 상태값에 따른 정지 명령
            {
                isWalk = false;
                base.StopMove();    // 빠른 감속을 위한 가속도 증가 및 정지
            }
            else
            {
                isWalk = true;
                nav.isStopped = false;
                nav.acceleration = accelerationBackUp;  // 가속도 백업
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
        yield return new WaitForSeconds(attackTime); // 애니메이션 시간 + HasExitTime을 고려하여 입력해주어야한다.
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
        print("데미지 " + base.stats.attackDamage + "를 입힘");
        yield return new WaitForSeconds(attackTime); // 애니메이션 시간 + HasExitTime을 고려하여 입력해주어야한다.
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