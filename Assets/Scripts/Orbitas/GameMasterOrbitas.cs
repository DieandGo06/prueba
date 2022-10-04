using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterOrbitas : MonoBehaviour
{
    public static GameMasterOrbitas instance;
    public GameObject[] orbitas = new GameObject[3];
    public GameObject[] satelites = new GameObject[3];
    public GameObject[] posiblePosicionNave = new GameObject[3];

    private void Awake()
    {
        instance = this;
    }
}
