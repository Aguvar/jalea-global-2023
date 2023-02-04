using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform Player;
    public float BaseMoveSpeed = 60;
    private float MoveSpeed = 0;
    public float AttackCooldown = 0.5f;
    public float AttackRange = 70f;
    public float AttackDamage = 10f;
    public float Health = 100f;
    public float ChanceToBlock = 0.5f;
    private bool isAttacking = false;
    private bool isBlocking = false;
    private bool isRetreating = false;
    public float retreatTime = 0.4f;
    public float BlockCooldown = 0.5f;
    void TryBlock()
    {
        if (Random.value <= ChanceToBlock)
        {
            MoveSpeed = 0;
            GetComponent<Renderer>().material.color = Color.blue;
            isRetreating = false;
            isBlocking = true;
            StartCoroutine(ResetBlock());
        }
    }

    IEnumerator ResetBlock()
    {
        yield return new WaitForSeconds(BlockCooldown);
        isBlocking = false;
        GetComponent<Renderer>().material.color = Color.white;
        MoveSpeed = BaseMoveSpeed;
    }
    IEnumerator ResetAttack()
    {        
        yield return new WaitForSeconds(AttackCooldown);
        isAttacking = false;
        GetComponent<Renderer>().material.color = Color.white;
        MoveSpeed = BaseMoveSpeed;

    }
void Attack()
{
    if (!isAttacking && !isBlocking)
    {
        isAttacking = true;
        MoveSpeed = 0;
        GetComponent<Renderer>().material.color = Color.red;
        StartCoroutine(ResetAttack());
        StartCoroutine(Retreat());
    }
}

IEnumerator Retreat()
{
    isRetreating = true;
    Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    float elapsedTime = 0f;
    while (elapsedTime < retreatTime && isRetreating)
    {
        transform.position += randomDirection * BaseMoveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    isRetreating = false;
    MoveSpeed = BaseMoveSpeed;
}


    void Start()
    {
        MoveSpeed = BaseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
          transform.LookAt(Player);
        if (Vector3.Distance(transform.position, Player.position) < AttackRange)
        {
            Attack();
        }
        else
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
     // If player tries to attack, try to block
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryBlock();
        }
    }
}
