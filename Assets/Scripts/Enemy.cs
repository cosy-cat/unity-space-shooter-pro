using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = InitRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // move down 4 meters/sec
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if bottom screen respawn at random x position
        if (transform.position.y < -7f)
        {
            transform.position = InitRandomPosition();
            // transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
        }
    }

    private Vector3 InitRandomPosition()
    {
        return new Vector3(Random.Range(-9f, 9f), 7f, 0);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("hit by " + other.name);

        if (other.tag == "Player")
        {
            // By tuto ... transform is the root of the object ... humf
            // other.transform.GetComponent<Player>().Damage();
            // access other object method :
            // other.GetComponent<Player>().Damage();
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            // Debug.Log("Destroying Laser");
            Destroy(this.gameObject);
        }

    }
}
