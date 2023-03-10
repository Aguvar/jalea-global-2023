using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 30;
    [SerializeField]
    private float dodgeSpeed;
    [SerializeField]
    private float dodgeInternalCD;
    private float dodgeTimer;
    private Rigidbody _rigidbody;
    private PlayerInputs playerInputs;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;
    private float coneAngle = 180.0f;
    private float coneDistance = 20.0f;
    private Vector2 currentAim;
    private Vector2 currentMove;

    private bool canDodge = true;

    public float parryWindow = 0.2f;
    public float Health = 100f;
    public float AttackDamage = 10f;
    //base cooldown for reduction purposes when you get buffs
    public float AttackBaseCD;
    public float AttackInternalCD;

    private bool isParryable = false;
    private bool didParry = false;
    private bool isBlocking = false;
    private bool canAttack = true;
    public float AimSmoothing = 15f;
    private GameManager gameManager;
    private ControllerTimesTwo currentController;
    private bool isDead = false;

    [SerializeField]
    private ParticleSystem parti;

    //public bool damageDealtRumble;
    //public bool damageReceivedRumble;
    //public bool deathRumble;
    //public bool parryRumble;

    //public void ReceiveAttack(float damage) {
    //    if (!isBlocking) {
    //        isParryable = true;
    //        StartCoroutine(ResetParry(damage));
    //    }


    //}
    //IEnumerator ResetParry(float damage) {
    //    yield return new WaitForSeconds(parryWindow);
    //    GetComponent<Renderer>().material.color = Color.white;
    //    if (didParry) {
    //        Debug.Log("Parried");
    //        //parryRumble = true;
    //    } else {
    //        Debug.Log("Took Damage");
    //        TakeDamage(damage);
    //    }
    //    isParryable = false;
    //    didParry = false;
    //}
    public void TakeDamage(float damage) {
        
        //damageReceivedRumble = true;
        if (Health <= 0 && !isDead) {
            StartCoroutine(currentController.DeathRMCoroutine());
            playerInputs.Disable();
            speed = 0;
            GetComponent<Renderer>().material.color = Color.yellow;
            //deathRumble = true;
            Debug.Log("Dead");
            isDead = true;
            gameManager.lifePanel.UpdateLifePanel(100f);
            gameManager.OnDeath();
        } else {
            Health -= damage;
            StartCoroutine(currentController.DmgReceivedRMCoroutine());
        }
        gameManager.lifePanel.UpdateLifePanel(Health);
    }

    public void DealDamage(GameObject enemy) {
        if (enemy != null) enemy.GetComponent<EnemyAI>().TakeDamage(AttackDamage);
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerInputs();
        animator = GetComponentInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        currentController = GetComponentInChildren<ControllerTimesTwo>();
    }

    private void OnEnable() {
        playerInputs.Enable();
    }

    // Start is called before the first frame update
    void Start() {

        gameManager = GameManager.Instance;
        parti = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        Aim();
        Move();
        // If player pressed the space key (not in player inputs) attack()
        if (playerInputs.Player.Attack.triggered && canAttack)
            StartCoroutine(AtkCoroutine());

        Block();
    }

    void FixedUpdate() {
        if (playerInputs.Player.Dodge.inProgress && canDodge)
            //Dodge();
            StartCoroutine(DodgeCooldown());
    }

    List<Collider> GetObjectsInFront(Rigidbody body) {
        Collider[] objectsInSphere = Physics.OverlapSphere(transform.position, coneDistance);
        List<Collider> objectsInCone = new List<Collider>();

        float startAngle = body.rotation.eulerAngles.y - coneAngle / 2;
        float endAngle = body.rotation.eulerAngles.y + coneAngle / 2;

        for (float angle = startAngle; angle < endAngle; angle += 1) {
            Vector3 direction = Quaternion.Euler(0, angle + 90, 0) * Vector3.forward;
            Debug.DrawRay(transform.position, direction * coneDistance, Color.red, 0.5f);
        }

        foreach (Collider col in objectsInSphere) {

            if (Vector3.Angle(Quaternion.Euler(0, 90, 0) * body.transform.forward, col.transform.position - body.transform.position) <= coneAngle / 2) {
                objectsInCone.Add(col);
            }

        }
        return objectsInCone;
    }


    //Smoother version of aim, gotta fix the drifting
    void Aim() {
        currentAim = playerInputs.Player.Aim.ReadValue<Vector2>();
        if (currentAim != Vector2.zero) {
            float targetAngle = Mathf.Atan2(currentAim.x, currentAim.y) * Mathf.Rad2Deg - 90;
            float smoothedAngle = Mathf.LerpAngle(_rigidbody.rotation.eulerAngles.y, targetAngle, AimSmoothing * Time.deltaTime);

            if (Mathf.Abs(smoothedAngle - targetAngle) < 0.1f) {
                _rigidbody.rotation = Quaternion.Euler(new Vector3(0, targetAngle, 0));
            } else {
                _rigidbody.rotation = Quaternion.Euler(new Vector3(0, smoothedAngle, 0));
            }
        }
    }

    void Move() {
        currentMove = playerInputs.Player.Move.ReadValue<Vector2>() * speed;

        _rigidbody.velocity = new Vector3(currentMove.x, 0, currentMove.y);


        animator.SetBool("isMoving", _rigidbody.velocity != Vector3.zero);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            spriteRenderer.flipX = _rigidbody.velocity.x > 0;
        }
    }


    void Block() {
        playerInputs.Player.Block.performed += ctx => {
            Debug.Log("Blocking");
            isBlocking = true;
            // Turn blue
            gameManager.Player.GetComponent<Renderer>().material.color = Color.blue;
            if (isParryable) {
                Parry();
            }
        };
        playerInputs.Player.Block.canceled += ctx => {
            Debug.Log("Not Blocking");
            isBlocking = false;
            gameManager.Player.GetComponent<Renderer>().material.color = Color.white;
        };
    }

    void Parry() {
        didParry = true;
    }

    IEnumerator DodgeCooldown() {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("doge");
        canDodge = false;
        _rigidbody.AddForce(new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed), ForceMode.Impulse);
        yield return new WaitForSeconds(dodgeInternalCD);
        canDodge = true;
        Debug.Log("Dodge Ready");
    }

    IEnumerator AtkCoroutine() {
        if (!isBlocking) {
            parti.Play();
            Debug.Log("Attack");
            canAttack = false;
            //Habr??a que ajustar esto para que checkee que EnemyAI tiene cerca
            //esto me cacha como 4 rigidbodies. -ro
            animator.SetBool("isAttacking", true);
            List<Collider> enemies = GetObjectsInFront(_rigidbody);

            foreach (Collider enemy in enemies) {
                if (enemy.gameObject.tag == "Enemy") {
                    DealDamage(enemy.gameObject);
                    //no me siento bien haciendo esto
                    StartCoroutine(currentController.DmgDealtRMCoroutine());
                }
            }
            yield return new WaitForSeconds(AttackInternalCD);
            animator.SetBool("isAttacking", false);
            canAttack = true;
        }

    }
}



//deprecated

//void Dodge()
//{
//    _rigidbody.AddForce(new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed), ForceMode.Impulse);
//    //_rigidbody.velocity = new Vector3(currentMove.x * dodgeSpeed, 0, currentMove.y * dodgeSpeed);
//    dodgeTimer = dodgeInternalCD;
//    canDodge = false;
//    StartCoroutine(DodgeCooldown());
//    Debug.Log("doge");
//}

//void Attack()
//{
//    if (!isBlocking)
//    {
//    parti.Play();
//    Debug.Log("Attack");
//    List<Collider> enemies = GetObjectsInFront(_rigidbody);
//    foreach (Collider enemy in enemies)
//    {
//        if (enemy.gameObject.tag == "Enemy"){
//        DealDamage(enemy.gameObject);
//        }
//    }        }

//}