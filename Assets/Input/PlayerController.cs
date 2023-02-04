using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody _rigidbody;
    private PlayerInputs playerInputs;

    private Vector2 currentAim;
    private Vector2 currentMove;

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
    }

    void Aim() {
        currentAim = playerInputs.Player.Aim.ReadValue<Vector2>();
        print(currentAim);
    }

    void Move() {
        currentMove = playerInputs.Player.Move.ReadValue<Vector2>();

        _rigidbody.velocity = currentMove * speed;
    }
        
}
