using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grip : MonoBehaviour
{
    public InputActionReference grip;
    public verstopperAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
        grip.action.performed += Action_performed1;

    }

    private void Action_performed1(InputAction.CallbackContext obj)
    {
        print("press grip");
        agent.CanSeek = true;
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
