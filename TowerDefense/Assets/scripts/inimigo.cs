using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class inimigo : MonoBehaviour
{
    [SerializeField] private float movespeed = 2f;

    private Rigidbody2D rb;

    private Transform checkpoint;

    private int index = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        checkpoint = inimigoManager.main.checkpoints[index]; 
    }

    void Update()
    {
        checkpoint = inimigoManager.main.checkpoints[index];

        if (Vector2.Distance(checkpoint.transform.position, transform.position) <= 0.1f)
        {
            index++;
            if (index >= inimigoManager.main.checkpoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }


    void FixedUpdate()
    {
        Vector2 direction = (checkpoint.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, angle);

        float rotationSpeed = movespeed * 3f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);

        rb.velocity = direction * movespeed;
    }
}
