using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //получить команды от пользователя
        float rotateInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        float angle = rb.rotation / 180 * Mathf.PI;

        rb.angularVelocity = -rotateInput * 40; //повернуть игрока
        rb.velocity = new Vector2(-moveInput * Mathf.Sin(angle), moveInput * Mathf.Cos(angle)); //двигать игрока вдоль направления взгляда
    }
}
