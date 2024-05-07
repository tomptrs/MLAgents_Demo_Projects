using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentScript : Agent
{
    public GameObject Ball;
    Rigidbody m_BallRb;
    public TextMeshPro scoreBoard;
    // Start is called before the first frame update
    void Start()
    {       
        m_BallRb = Ball.GetComponent<Rigidbody>();      
    }

    private void FixedUpdate()
    {
        //5000 steps + 0.1 reword = 500 award
        scoreBoard.text = GetCumulativeReward().ToString("f3");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.rotation.z);
        sensor.AddObservation(gameObject.transform.rotation.x);
        sensor.AddObservation(Ball.transform.position - gameObject.transform.position);
        sensor.AddObservation(m_BallRb.velocity);
    }

    public override void OnEpisodeBegin()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-2f, 2f));
        gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-2f, 2f));
        m_BallRb.velocity = new Vector3(0f, 0f, 0f);
        Ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 2f, Random.Range(-1.5f, 1.5f))
            + gameObject.transform.position;

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionZ = 1f * Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var actionX = 1f * Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

       
            gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
            gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
       

        if ( Ball.transform.position.y < 0 )
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(0.1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = -Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
       // Debug.Log(continuousActionsOut[0]);
    }
}
