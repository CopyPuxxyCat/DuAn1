using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Vector3 moveInput;
    private Rigidbody2D _rigidbody2D;

    public GameObject PauseGame;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            PauseGame.SetActive(true);
        }
    }
    private void Move()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.position += moveInput * moveSpeed * Time.deltaTime;
        if (moveInput.x != 0)
        {
            if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
        }
    }

}
