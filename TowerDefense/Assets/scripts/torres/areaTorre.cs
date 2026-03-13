using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class areaTorre : MonoBehaviour
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
            if (torre.primeiro)
            {
                float distanciaMin = Mathf.Infinity;
                int indexMax = 0;
                GameObject primeiroAlvo = null;

                foreach (GameObject alvo in alvos)
                {
                    int index = alvo.GetComponent<inimigo>().index;
                    float distancia = alvo.GetComponent<inimigo>().distancia;

                    if (index > indexMax || (index == indexMax && distancia < distanciaMin))
                    {
                        indexMax = index;
                        distanciaMin = distancia;
                        primeiroAlvo = alvo;
                    }
                }
                torre.alvo = primeiroAlvo;
            }
            else if (torre.ultimo)
            {
                float distanciaMax = -Mathf.Infinity;
                int indexMin = 0;
                GameObject ultimoAlvo = null;

                foreach (GameObject alvo in alvos)
                {
                    int index = alvo.GetComponent<inimigo>().index;
                    float distancia = alvo.GetComponent<inimigo>().distancia;

                    if (index < indexMin || (index == indexMin && distancia > distanciaMax))
                    {
                        indexMin = index;
                        distanciaMax = distancia;
                        ultimoAlvo = alvo;
                    }
                }
                torre.alvo = ultimoAlvo;
            }
            else if (torre.forte)
            {
                GameObject alvoForte = null;
                float vidaMax = 0f;

                foreach (GameObject alvo in alvos)
                {
                    float vida = alvo.GetComponent<inimigo>().vida;

                    if(vida < vidaMax)
                    {
                        vidaMax = vida;
                        alvoForte = alvo;
                    }
                }
                torre.alvo = alvoForte;
            }
            else
            {
                torre.alvo = alvos[0];
            }

        }
        else
        {
            torre.alvo = null;
        }
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

