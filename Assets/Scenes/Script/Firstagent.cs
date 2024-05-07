using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Firstagent : Agent
{

    public Transform Target;
    public float speedMultiplier = 0.1f;


    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {

            this.transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        }

������� // verplaats de target naar een nieuwe willekeurige locatie 
������� Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[1];
        controlSignal.z = actions.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);

        // Beloningen
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // target bereikt
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Van het�platform gevallen?
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }    
}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }
}
