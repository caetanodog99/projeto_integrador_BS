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
            torre.alvo = alvos[0];
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
