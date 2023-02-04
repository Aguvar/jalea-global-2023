using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float dodgeSpeed;
    [SerializeField]
    private int attack = 1;
    [SerializeField]
    private float dodgeInternalCD;
    private float dodgeTimer;
    private Rigidbody _rigidbody;
    private PlayerInputs playerInputs;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;

    private Vector2 currentAim;
    private Vector2 currentMove;

    private bool canDodge = true;

    public float Health = 100f;
    public float AttackDamage = 10f;
    public void TakeDamage(float damage)
    {
        // -Implement Blocking first-
        // if (isBlocking)
        // {
        //     damage = damage / 2;
        // }
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Dead");
            //Destroy(gameObject);
        }
    }

    public void DealDamage(GameObject enemy)
    {
        if (enemy != null) enemy.GetComponent<EnemyAI>().TakeDamage(AttackDamage);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerInputs();
        animator = GetComponentInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If player pressed the space key (not in player inputs) attack()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
     
    }

    void FixedUpdate()
    {
        Aim();
        Move();
        if (playerInputs.Player.Dodge.inProgress && canDodge)
            //Dodge();
            StartCoroutine(DodgeCooldown());
    }

    List<Collider> GetObjectsInFront(Vector3 coneDirection)
    {
        float coneAngle = 180.0f;
        float coneDistance = 100.0f;
        Collider[] objectsInSphere = Physics.OverlapSphere(transform.position, coneDistance);
        List<Collider> objectsInCone = new List<Collider>();
        foreach (Collider col in objectsInSphere)
        {
            Vector3 directionToObject = (col.transform.position - transform.position).normalized;
            if (Vector3.Angle(coneDirection, directionToObject) <= coneAngle / 2)
            {
                objectsInCone.Add(col);
            }
        }
        return objectsInCone;
    }

  void Aim()
    {

        currentAim = playerInputs.Player.Aim.ReadValue<Vector2>();
        if (currentAim != Vector2.zero)
        {
            _rigidbody.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(currentAim.x, currentAim.y) * Mathf.Rad2Deg - 90, 0));
        }



    }
// public float AimSmoothing = 15f;
// Smoother version of aim, gotta fix the drifting
// void SmoothAim()
// {
//     currentAim = playerInputs.Player.Aim.ReadValue<Vector2>();
//     if (currentAim != Vector2.zero)
//     {
//         float targetAngle = Mathf.Atan2(currentAim.x, currentAim.y) * Mathf.Rad2Deg - 90;
//         float smoothedAngle = Mathf.LerpAngle(_rigidbody.rotation.eulerAngles.y, targetAngle, AimSmoothing * Time.deltaTime);
        
//         if (Mathf.Abs(smoothedAngle - targetAngle) < 0.1f)
//         {
//             _rigidbody.rotation = Quaternion.Euler(new Vector3(0, targetAngle, 0));
//         }
//         else
//         {
//             _rigidbody.rotation = Quaternion.Euler(new Vector3(0, smoothedAngle, 0));
//         }
//     }
// }

    void Move()
    {
        currentMove = playerInputs.Player.Move.ReadValue<Vector2>() * speed;

        _rigidbody.velocity = new Vector3(currentMove.x, 0, currentMove.y);


        animator.SetBool("isMoving", _rigidbody.velocity != Vector3.zero);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.flipX = _rigidbody.velocity.x > 0;
        }


    }

    void Attack()
    {
        Debug.Log("Attack");
        List<Collider> enemies = GetObjectsInFront(_rigidbody.transform.forward);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.tag == "Enemy"){
            DealDamage(enemy.gameObject);
            }
        }
    }

    void Block()
    {

    }

    void Parry()
    {


    }

    void Dodge()
    {
        _rigidbody.AddForce(new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed), ForceMode.Impulse);
        //_rigidbody.velocity = new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed);
        dodgeTimer = dodgeInternalCD;
        canDodge = false;
        StartCoroutine(DodgeCooldown());
        Debug.Log("doge");
    }



    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("doge");
        canDodge = false;
        _rigidbody.AddForce(new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed), ForceMode.Impulse);
        yield return new WaitForSeconds(dodgeInternalCD);
        canDodge = true;
        Debug.Log("Dodge Ready");
    }
}




