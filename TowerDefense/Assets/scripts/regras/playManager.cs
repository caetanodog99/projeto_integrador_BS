using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playManager : MonoBehaviour
{
    [SerializeField] public GameObject painelPause;
    [SerializeField] public GameObject painelLoja;

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

    public void AbrirLoja()
    {
        painelLoja.SetActive(true);
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
    }
}
