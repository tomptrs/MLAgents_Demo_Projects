using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 velocity;
    Vector3 acceleration;
    public float topSpeed = 5f;
    void Start()
    {
        velocity = new Vector3(Random.Range(-2f,2f), 0, Random.Range(-2f, 2f));
        acceleration = new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration;
        transform.Translate(velocity * Time.deltaTime);
        Vector3 screenPosition = Input.mousePosition;
        Vector3 tmp = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3 dir = screenPosition - transform.position;
        dir.y = 0;
        dir.Normalize();
        acceleration = dir;
       
       Debug.Log(tmp);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "z_wall")
        {

            velocity.z *= -1;
        }

        if (collision.gameObject.tag == "x_wall")
        {

            velocity.x *= -1;
        }
    }
}
