using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    [SerializeField]
    private GameObject player;

    void Awake()
    {
        instance = this;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

}
