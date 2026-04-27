using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeTorres : NetworkBehaviour
{
    [System.Serializable]
    class Nivel
    {
        [Networked]public float area { get; set; } = 6f;
        [Networked] public int dano { get; set; } = 10;
        [Networked] public float cadencia { get; set; } = 1.1f;
        [Networked] public int valor { get; set; } = 100;
    }

    [SerializeField]private Nivel[] niveis = new Nivel[3];
    [Networked]
    public int nivelAtual { get; set; } = 0;
    [NonSerialized] public string valorAtual;

    private Torre torre;
    [SerializeField]private areaTorre areaTorre;

   

    void Awake()
    {
        torre = GetComponent<Torre>();
        valorAtual = niveis[0].valor.ToString();

    }

    
    public void Upgrade()
    {
        if (nivelAtual < niveis.Length && niveis[nivelAtual].valor < jogador.main.creditos)
        {
            torre.area = niveis[nivelAtual].area;
            torre.dano = niveis[nivelAtual].dano;
            torre.cadencia = niveis[nivelAtual].cadencia;
            areaTorre.AtualizarArea();

            jogador.main.creditos = niveis[nivelAtual].valor;

            nivelAtual++;

            if(nivelAtual >= niveis.Length)
            {
                valorAtual = "MAX";
            }
            else
            {
                valorAtual = niveis[nivelAtual].valor.ToString();
            }

            Debug.Log("Upou de nível!");
        }
        else
        {
            //StartCoroutine(SemDinheiro());
        }

    }
    //IEnumerator SemDinheiro()
    //{
    //    painelDinheiro.SetActive(true);
    //    yield return new WaitForSeconds(1.5f);
    //    painelDinheiro.SetActive(false);
    //}
}
