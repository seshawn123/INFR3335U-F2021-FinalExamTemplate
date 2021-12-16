using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab; //*

    public float minX, maxX;
    public float minZ, maxZ;

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 1f, Random.Range(minZ, maxZ));
        //PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

        GameObject temp = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity); //*
        if (temp.GetComponent<PhotonView>().IsMine)
            temp.GetComponent<Movement>().SetJoysticks(Instantiate(cameraPrefab, randomPosition, Quaternion.identity)); //*
    }


}
