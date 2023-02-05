using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerTimesTwo : MonoBehaviour
{
    [SerializeField]
    private Gamepad currentPad = Gamepad.current;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator DmgDealtRMCoroutine()
    {
        currentPad.SetMotorSpeeds(0.50f, 0);
        yield return new WaitForSeconds(0.2f);
        currentPad.SetMotorSpeeds(0.25f, 1f);
        yield return new WaitForSeconds(0.1f);
        currentPad.SetMotorSpeeds(0, 0.5f);
        yield return new WaitForSeconds(0.25f);
        currentPad.SetMotorSpeeds(0, 0);
        //yield return new WaitForSeconds(0.1f);
        //currentPad.SetMotorSpeeds(0f, 0f);
    }

    public IEnumerator DmgReceivedRMCoroutine()
    {
        currentPad.SetMotorSpeeds(1, 1);
        yield return new WaitForSeconds(0.5f);
        currentPad.SetMotorSpeeds(0, 0);
    }
    public IEnumerator DeathRMCoroutine()
    {
        currentPad.SetMotorSpeeds(1, 1);
        yield return new WaitForSeconds(2f);
        currentPad.SetMotorSpeeds(0.5f, 0.5f);
        yield return new WaitForSeconds(1f);
        currentPad.SetMotorSpeeds(0.25f, 0.25f);
        yield return new WaitForSeconds(1f);
        currentPad.SetMotorSpeeds(0, 0);
    }

}
