using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Tilemaps;

public class ChasingAgent : Agent
{
    Rigidbody rBody;
    [SerializeField] public Transform Target;
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [SerializeField] float speed = 20f;
    [SerializeField] float MinHigh=11;
    [SerializeField] float MinWidth=7;
    [SerializeField] float firstPosX = 5.5f;
    [SerializeField] float firstPosY = -5.5f;
    [SerializeField] float MaxReward = 1;
    [SerializeField] float MinDis = 0.5f;
    public bool isChase = false;
        
    private float timeBetweenSteps;
    private bool makeStep=true;
    private bool getTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        timeBetweenSteps = 1 / speed;
    }
    IEnumerator waitStep()
    {
        makeStep = false;
        yield return new WaitForSeconds(timeBetweenSteps);
        makeStep = true;
    }

    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    public override void OnEpisodeBegin()
    {
        if(!getTarget)
        {
            getTarget = false;
            this.transform.localPosition = new Vector3(firstPosX, firstPosY, this.transform.localPosition.z);
        }
        if (!isChase)
        {
            while (true)
            {
                // Move the target to a new spot
                float randomX = Random.value * MinHigh - MinWidth;
                float randomY = Random.value * MinHigh - MinWidth;
                Target.localPosition = new Vector3(randomX, randomY, Target.localPosition.z);
                Vector3 newPosition = Target.position;

                TileBase tileOnNewPosition = TileOnPosition(newPosition);
                if (allowedTiles.Contain(tileOnNewPosition))
                {
                    break;
                }
            }
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
       
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        var moveAction = actionBuffers.ContinuousActions;

        // Move the agent towards the player - moveAction[0] = Horizontal, moveAction[1] = Vertical
        float moveX = moveAction[0];
        float moveY = moveAction[1];

        Vector3 movment = new Vector3(moveX, moveY, 0f);


        Vector3 newPosition = transform.position + movment;

        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        if (!allowedTiles.Contain(tileOnNewPosition))
        {
            if (!isChase)
            {
                getTarget = false;
                EndEpisode();
            }
        }

        else if (makeStep)
        {
            transform.position = newPosition;

            StartCoroutine(waitStep());
        }
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < MinDis)
        {
            getTarget = true;
            SetReward(MaxReward);
            if(!isChase)
                EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
