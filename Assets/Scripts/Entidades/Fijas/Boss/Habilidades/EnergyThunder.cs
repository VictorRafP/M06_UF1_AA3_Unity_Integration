using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnergyThunder : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject EnergyBallPrefab;
    [SerializeField] private GameObject ThunderPrefab;
    [SerializeField] private Transform shootPoint;

    private Transform playerTarget;

    private void Awake()
    {
        var playerGo = GameObject.FindGameObjectWithTag("player");
        if (playerGo != null)
            playerTarget = playerGo.transform;
    }

    public void LaunchAttack()
    {
        GameObject EnergyBall = Instantiate(EnergyBallPrefab, new Vector3(-3.236f, 1.654f, 0), Quaternion.identity);
    }
}

