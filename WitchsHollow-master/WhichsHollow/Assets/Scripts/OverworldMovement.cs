using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;
    public Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        player.MovePosition(player.position + moveVelocity * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Equals("Enemy"))
        {
            SceneManager.LoadScene("Combat");
        }
    }
}
