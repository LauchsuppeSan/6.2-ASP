using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int speed = 6;
    private Vector3 movement;
    private Vector3 input;

    public static int PlayerHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Death();
    }
    private void Movement()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        movement = new Vector3(input.x, 0, input.y);

        movement = Vector3.ClampMagnitude(movement, 1);

        transform.Translate(movement * speed * Time.deltaTime);
    }
    private void Death()
    {
        if (PlayerHealth <= 0) Debug.Log("U dead"); 
    }
}
