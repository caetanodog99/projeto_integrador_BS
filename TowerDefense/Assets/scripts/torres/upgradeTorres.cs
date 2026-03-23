using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeTorres : MonoBehaviour
{
    [System.Serializable]
    class Nivel
    {
        public float area = 6f;
        public int dano = 10;
        public float cadencia = 1.1f;
        public int valor = 100;
    }

    [SerializeField]private Nivel[] niveis = new Nivel[3];
    [NonSerialized] public int nivelAtual = 0;
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
    }
}
