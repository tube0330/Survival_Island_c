using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    MFOV C_FOV;
    [Header("Component")]
    public NavMeshAgent agent2;
    public Transform Player;
    public Transform thisMonster;
    public Animator ani;
    public MonsterDamage damage;
    [Header("Vars")]
    public float attackDist = 3.0f;
    public float traceDist = 20.0f;

    void Start()
    {
        C_FOV = GetComponent<MFOV>();
        agent2 = this.gameObject.GetComponent<NavMeshAgent>();
        thisMonster = transform;
        Player = GameObject.FindWithTag("Player").transform;
        ani = GetComponent<Animator>();
        damage = GetComponent<MonsterDamage>();
    }

    void Update()
    {
        if (damage.isDie || Player.GetComponent<FpsDamage>().isPlayerDie)
            return;

        float distance = Vector3.Distance(thisMonster.position, Player.position);

        if (distance <= attackDist)
        {
            if (C_FOV.isViewPlayer())
            {
                //Debug.Log("공격");
                ani.SetBool("IsAttack", true);
                agent2.isStopped = true;
                Vector3 PlayerPos = Player.position - transform.position;
                PlayerPos = PlayerPos.normalized;
                Quaternion rot = Quaternion.LookRotation(PlayerPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3.0f);
            }

            else
            {
                ani.SetBool("IsAttack", false);
                ani.SetBool("IsTrace", true);
                agent2.isStopped = false;
                agent2.destination = Player.position;
            }
        }
        else if (C_FOV.isTracePlayer())
        {
            //Debug.Log("추적");
            ani.SetBool("IsAttack", false);
            ani.SetBool("IsTrace", true);
            agent2.isStopped = false;
            agent2.destination = Player.position;
        }
        else
        {   
            //Debug.Log("추적하지 않음");
            ani.SetBool("IsTrace", false);
            agent2.isStopped = true;
        }
    }
    // public void PlayerDeath()
    // {
    //     GetComponent<Animator>().SetTrigger("PlayerDie");
    // }

    void OnPlayerDie()
    {
        StopAllCoroutines();    //모든 코루틴 종료
        ani.SetTrigger("PlayerDie");
    }

    private void OnEnable() //오브젝트가 활성화될 때마다 호출
    {
        FpsDamage.OnPlayerDie += OnPlayerDie;  //damage class의 delegate. 이벤트 연결
    }
}
