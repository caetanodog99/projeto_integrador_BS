using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] public GameObject painelTutorial;

    public void FecharTutorial()
    {
        painelTutorial.SetActive(false);
    }

    public void AbrirTutorial()
    {
        painelTutorial.SetActive(true);
    }
}
