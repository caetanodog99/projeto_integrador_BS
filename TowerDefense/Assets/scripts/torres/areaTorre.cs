using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class areaTorre : NetworkBehaviour
{
    [SerializeField] private Torre torre;
    private List<GameObject> alvos = new List<GameObject>();
    void Start()
    {
        AtualizarArea();
    }


    void Update()
    {
        if (alvos.Count > 0)
        {
            if (this.torre.primeiro)
            {
                float distanciaMin = Mathf.Infinity;
                int indexMax = -1; 
                GameObject primeiroAlvo = null;

                foreach (GameObject alvo in alvos)
                {
                    var scriptInimigo = alvo.GetComponent<inimigo>();
                    int index = scriptInimigo.index;
                    float distancia = scriptInimigo.distancia;

                    
                    if (index > indexMax || (index == indexMax && distancia < distanciaMin))
                    {
                        indexMax = index;
                        distanciaMin = distancia;
                        primeiroAlvo = alvo;
                    }
                }
                this.torre.alvo = primeiroAlvo;
            }
            else if (this.torre.ultimo)
            {
                float distanciaMax = -1f;
                int indexMin = int.MaxValue; 
                GameObject ultimoAlvo = null;

                foreach (GameObject alvo in alvos)
                {
                    var scriptInimigo = alvo.GetComponent<inimigo>();
                    int index = scriptInimigo.index;
                    float distancia = scriptInimigo.distancia;

                    
                    if (index < indexMin || (index == indexMin && distancia > distanciaMax))
                    {
                        indexMin = index;
                        distanciaMax = distancia;
                        ultimoAlvo = alvo;
                    }
                }
                this.torre.alvo = ultimoAlvo;
            }
            else if (this.torre.forte)
            {
                GameObject alvoForte = null;
                float vidaMax = -1f;

                foreach (GameObject alvo in alvos)
                {
                    float vida = alvo.GetComponent<inimigo>().vida;

                    
                    if (vida > vidaMax)
                    {
                        vidaMax = vida;
                        alvoForte = alvo;
                    }
                }
                this.torre.alvo = alvoForte;
            }
            else
            {
                this.torre.alvo = alvos[0];
            }

        }
        else
        {
            this.torre.alvo = null;
        }
       // Debug.Log("alvo da torre: " + this.torre +" / "+ this.torre.alvo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "inimigo")
        {
            alvos.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "inimigo")
        {
            alvos.Remove(collision.gameObject);
        }
    }

    public void AtualizarArea()
    {
        transform.localScale = new Vector3(torre.area, torre.area, torre.area);
    }
}

