using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float Speed = 1.25f;

    //Fixed Update, т.к. движение игрока ограничевается физикой.
    void FixedUpdate()
    {
        MovingInput();
    }

    void MovingInput()
    {
        if (!GameManager.isGameStarted)
        {
            return;
        }
        Vector3 Direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            Direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Direction -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Direction -= Vector3.right;
        }

        if (Input.touchCount == 1)
        {
            Vector2 T = Input.GetTouch(0).position - new Vector2(Screen.width, Screen.height)/2f;
            T.Normalize();
            Direction = new Vector3(T.x, 0f, T.y);

        }

        if (Direction != Vector3.zero)
        {
            Direction.Normalize();

            transform.position += Direction * Speed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            GameManager.TakeCoin();
            Destroy(other.gameObject);
        }

        if (other.tag == "Enemy")
        {
            GameManager.LoseGame();
        }
    }

}
