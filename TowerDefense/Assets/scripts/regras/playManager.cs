using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playManager : MonoBehaviour
{
    [SerializeField] public GameObject painelPause;

    public void BotaoPause()
    {
        Time.timeScale = 0;
        painelPause.SetActive(true);
    }

    public void BotaoVoltar()
    {
        Time.timeScale = 1;
        painelPause.SetActive(false);
    }
}
