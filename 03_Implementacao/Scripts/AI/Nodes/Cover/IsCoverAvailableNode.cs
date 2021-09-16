using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{
    private readonly Location[] availableCovers;
    private readonly Transform target;
    private readonly EnemyAI ai;

    public IsCoverAvailableNode(Location[] availableCovers, Transform target, EnemyAI ai)
    {
        this.availableCovers = availableCovers;
        this.target = target;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform bestSpot = FindBestCoverSpot();
        ai.bestCoverSpot = bestSpot;
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private Transform FindBestCoverSpot()
    {
        if (ai.bestCoverSpot != null)
            if (CheckIfSpotIsValid(ai.bestCoverSpot))
                return ai.bestCoverSpot;
        
        float minAngle = 90;
        Transform bestSpot = null;
        foreach (Location cover in availableCovers)
        {
            Transform bestSpotInCover = FindBestSpotInCover(cover, ref minAngle);
            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
                break;
            }
        }

        return bestSpot;
    }

    private Transform FindBestSpotInCover(Location location, ref float minAngle)
    {
        Transform[] availableSpots = location.GetLocationSpots();
        Transform bestSpot = null;
 
        foreach (Transform spot in availableSpots)
        {
            Vector3 direction = target.position - spot.position;
            if (CheckIfSpotIsValid(spot))
            {
                float angle = Vector3.Angle(spot.forward, direction);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = spot;
                }
            }
        }

        return bestSpot;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        Vector3 direction = target.position - spot.position;
        if (Physics.Raycast(spot.position, direction, out RaycastHit hit))
            if (hit.collider.transform != target)
                return true;
        return false;
    }
}