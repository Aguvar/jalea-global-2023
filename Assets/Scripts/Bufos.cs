using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bufos : MonoBehaviour
{
    public int bufftype;
    private PlayerController play;
    [SerializeField]
    private float value;

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        play = other.gameObject.GetComponent<PlayerController>();
        switch (bufftype)
        {
            case 0: buffATKDamage(play, value);
                break;
            case 1: buffHP(play, value);
                break;
            case 2: buffATKSpeed(play, value);
                break;
        }
    }
    

    public void buffATKDamage(PlayerController player, float flat)
    {
        player.AttackDamage += flat;
        Debug.Log("Attack Damage = " + player.AttackDamage);
        Destroy(gameObject);
    }

    public void buffATKSpeed(PlayerController player, float percent)
    {
        player.AttackInternalCD = player.AttackInternalCD - (player.AttackInternalCD * percent);
        Debug.Log("Attack Speed = " + player.AttackInternalCD);
        Destroy(gameObject);
    } 

    //buff player hp by x or so, maybe?
    public void buffHP(PlayerController player, float flat) 
    {
        player.Health += flat;
        Debug.Log("Total Health: " + player.Health);
        Destroy(gameObject);
    }

}
