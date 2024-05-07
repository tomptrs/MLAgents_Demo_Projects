using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class verstopperAgent : Agent
{
    Rigidbody m_AgentRb;
    public GameObject Block;
    public bool CanSeek = false;

    // Start is called before the first frame update
    void Start()
    {
        m_AgentRb = GetComponent<Rigidbody>();
    }

   
    public override void OnEpisodeBegin()
    {

        CanSeek = false;
        Vector3 r = GetRandomSpawnPos(0.25f);

        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(r.x, 0.5f, r.z);
        ResetBlock();
    }
    
    // Update is called once per frame
    public override void OnActionReceived(ActionBuffers actions)
    {
       // if (CanSeek)
        //{
            MoveAgent(actions.DiscreteActions);
            // Penalty given each step to encourage agent to finish task quickly.
            AddReward(-1f / MaxStep);
        //}
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 0.5f,
            ForceMode.VelocityChange);
    }

    void ResetBlock()
    {
        // Get a random position for the block.
        Block.transform.localPosition = GetRandomSpawnPos(0.5f);

        // Reset block velocity back to zero.
        Block.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Reset block angularVelocity back to zero.
        Block.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public Vector3 GetRandomSpawnPos(float y)
    {

        var randomPosX = Random.Range(-7, 7);

        var randomPosZ = Random.Range(-6, 6);
        Vector3 randomSpawnPos = new Vector3(randomPosX, y, randomPosZ);


        return randomSpawnPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "block")
        {
            AddReward(2f);
            EndEpisode();
        }
    }




    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActionsOut[0] = 2;
        }
    }
}
