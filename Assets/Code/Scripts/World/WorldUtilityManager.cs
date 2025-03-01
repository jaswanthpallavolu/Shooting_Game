using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask characterLayer;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetObstacleLayer()
    {
        return obstacleLayer;
    }

    public LayerMask GetCharacterLayer()
    {
        return characterLayer;
    }
}
