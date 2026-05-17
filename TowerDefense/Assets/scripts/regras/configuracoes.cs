using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class configuracoes : MonoBehaviour
{
    [SerializeField] public GameObject painelPause;
    [SerializeField] public GameObject painelControles;
    [SerializeField] public GameObject botaoPlay;
    [SerializeField] public GameObject botaoPause;

    public void BotaoPause()
    {
        painelPause.SetActive(true);
        botaoPlay.SetActive(false);
        botaoPause.SetActive(false);
    }

    public void BotaoVoltar()
    {
        painelPause.SetActive(false);
        botaoPlay.SetActive(true);
        botaoPause.SetActive(true);
    }
    public void AbrirControles()
    {
        painelControles.SetActive(true);
    }

    public void FecharControles()
    {
        painelControles.SetActive(false);
    }
}
