using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SkeletonCtrl : MonoBehaviour
{
    [Header("Component")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform thisSkeleton;
    public Animator ani;
    public SkeletonDamage damage;
    public AudioSource audioSource;
    public AudioClip swordclip;
    [Header("Vars")]
    public float attackDist = 3.0f;
    public float traceDist = 20.0f;
    public string findTag = "Player";

    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        thisSkeleton = transform;
        player = GameObject.FindWithTag(findTag).transform;
        ani = GetComponent<Animator>();
        damage = GetComponent<SkeletonDamage>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (damage.isDie || player.GetComponent<FpsDamage>().isPlayerDie)
            return;
        float distance = Vector3.Distance(thisSkeleton.position, player.position);
        if (distance <= attackDist)
        {
            ani.SetBool("IsAttack", true);
            agent.isStopped = true;

            Vector3 playerPos = (player.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(playerPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (distance <= traceDist)
        {
            ani.SetBool("IsAttack", false);
            ani.SetBool("IsTrace", true);
            agent.isStopped = false;
            agent.destination = player.position;
        }
        else
        {
            ani.SetBool("IsTrace", false);
            agent.isStopped = true;
        }
        
    }
    public void SwordSfx()
    {
        audioSource.clip = swordclip;   //사운드 클립을 받아서 아래에서 1회 재생
        audioSource.PlayDelayed(0.1f);  //0.1초 딜레이 후 재생
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

    void OnEnable()
    {
        FpsDamage.OnPlayerDie += OnPlayerDie;  //damage class의 delegate. 이벤트 연결
    }
}
