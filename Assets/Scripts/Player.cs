using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    private Rigidbody2D rb;
    public GameManager gameManager;

    public float speed;
    private Vector2 input;
    public int coins;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        //gameManager.globalCoinsText.text = "Global coins: " + gameManager.globalCoins;
        //gameManager.coinsText.text = "Player coins: " + coins;

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Flip();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed / 100);
    }

    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins += 1;
            RpcGlobalCoinI();
        }
    }

    [ClientRpc]
    public void RpcGlobalCoinI()
    {
        //gameManager.globalCoins += 1;
    }
}
