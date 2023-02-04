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

    private Vector2 currentAim;
    private Vector2 currentMove;

    private bool canDodge = true;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        playerInputs = new PlayerInputs();
    }

    private void OnEnable() {
        playerInputs.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() {
        Aim();
        Move(); 
        if (playerInputs.Player.Dodge.inProgress && canDodge)
            //Dodge();
            StartCoroutine(DodgeCooldown());
    }

    void Aim() {
        currentAim = playerInputs.Player.Aim.ReadValue<Vector2>();

    }

    void Move() {
        currentMove = playerInputs.Player.Move.ReadValue<Vector2>() * speed;

        //print(currentMove);
        _rigidbody.velocity = new Vector3( currentMove.x, 0, currentMove.y  );
        //_rigidbody.position = _rigidbody.position + new Vector3();
    }

    void Attack() {

    }

    void Block() {

    }

    void Parry() {
        
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
