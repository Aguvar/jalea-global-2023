using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour {
    public Transform Player;
    public float BaseMoveSpeed = 30;
    private float MoveSpeed = 0;
    public float AttackCooldown = 0.5f;
    public float AttackRange = 3f;
    public float AttackDamage = 10f;
    public float Health = 100f;
    public float ChanceToBlock = 0.5f;
    private bool isAttacking = false;
    private bool isBlocking = false;
    private bool isRetreating = false;
    public float retreatTime = 0.4f;
    public float BlockCooldown = 0.5f;
    public float DodgeWindow = 0.2f;
    public string Name = "Boss";
    public GameObject damageText;
    public bool isAggro = true;

    private Animator animator;

    public UnityEvent EnemyDied;

    [SerializeField]
    private ParticleSystem parti;

    public void TakeDamage(float damage) {
        TryBlock();
        if (isBlocking) {
            Debug.Log("Blocked half damage");
            damage = damage / 2;
        }
        Health -= damage;

        // Create a new instance of the DamageIndicator prefab
        DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText((int)damage);

        if (Health <= 0) {
            animator.SetBool("isDead", true);
            // Make the enemy gradually disappear through the floor
            StartCoroutine(Disappear());
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            EnemyDied.Invoke();
            //GameManager.Instance.Win();
            //Destroy(gameObject);
        }
    }

    IEnumerator Disappear() {
        float elapsedTime = 0f;
        while (elapsedTime < 1f) {
            transform.position += Vector3.down * Time.deltaTime*30;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    void DealDamage() {
        Player.GetComponent<PlayerController>().TakeDamage(AttackDamage * Random.Range(0.8f, 1.1f));
    }
    void TryBlock() {
        if (Random.value <= ChanceToBlock) {
            MoveSpeed = 0;
            GetComponent<Renderer>().material.color = Color.blue;
            isRetreating = false;
            isBlocking = true;
            StartCoroutine(ResetBlock());
        }
    }

    IEnumerator ResetBlock() {
        yield return new WaitForSeconds(BlockCooldown);
        isBlocking = false;
        GetComponent<Renderer>().material.color = Color.white;
        MoveSpeed = BaseMoveSpeed;
    }
    IEnumerator ResetAttack() {
        yield return new WaitForSeconds(AttackCooldown);
        isAttacking = false;
        GetComponent<Renderer>().material.color = Color.white;
        MoveSpeed = BaseMoveSpeed;

    }
    void Attack() {
        if (!isAttacking && !isBlocking) {
            parti.Play();
            isAttacking = true;
            MoveSpeed = 0;
            GetComponent<Renderer>().material.color = Color.red;
            Invoke("DealDamage", DodgeWindow);
            StartCoroutine(ResetAttack());
            StartCoroutine(Retreat());
        }
    }

    IEnumerator Retreat() {
        isRetreating = true;
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        float elapsedTime = 0f;
        while (elapsedTime < retreatTime && isRetreating) {
            transform.position += randomDirection * BaseMoveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRetreating = false;
        MoveSpeed = BaseMoveSpeed;
    }


    void Start() {
        MoveSpeed = BaseMoveSpeed;
        animator = GetComponentInChildren<Animator>();
        parti = GetComponentInChildren<ParticleSystem>();

        GetComponentInChildren<TextMeshPro>().text = Name;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(Player);

        animator.ResetTrigger("isAttacking");
        if (isAggro && Health > 0) {

            if (Vector3.Distance(transform.position, Player.position) < AttackRange) {
                Attack();
                animator.SetTrigger("isAttacking");
                animator.SetBool("isMoving", false);
            } else {
                
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
                animator.SetBool("isMoving", true);
            }


            if (transform.forward.x > 0) {
                transform.localScale.Set(transform.localScale.x, transform.localScale.y, -10);
            } else {
                transform.localScale.Set(transform.localScale.x, transform.localScale.y, -10);
            }
        }

        // If player tries to attack, try to block

    }
}
