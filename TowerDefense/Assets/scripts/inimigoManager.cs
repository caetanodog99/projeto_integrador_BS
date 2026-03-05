using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoManager : MonoBehaviour
{
    public static inimigoManager main;

    public Transform[] checkpoints;

    void Awake()
    {
        main = this;
    }
}
