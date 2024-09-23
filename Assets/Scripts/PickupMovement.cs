using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickupMovement : MonoBehaviour
{
    private string bigPickupTag = "Pickup"; //tag for tracking behavior upon being hit by a bullet
    private string littlePickupTag = "LilPickup"; //tag for tracking behavior upon being hit by a bullet
    [SerializeField] private float speed; //Movement speed of pickup
    private Rigidbody2D rb; 
    private Vector2 direction; //Vector to apply direction to pickups rigidbody
    private Vector2 lastDirection; //Vector to track last movement direction
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Set direction to a random vector within the unit circle (normalized)
        direction = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Save current direction to lastDirection
        lastDirection = direction;
        //Set pickup's velocity to the direction vector * speed
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        //Reflects the direction vector based on lastDirection vector (bounces when it hits walls or other pickups)
        direction = Vector2.Reflect(lastDirection, -other.GetContact(0).normal);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        //Destroy the trigger gameObject
        Destroy(other.gameObject);
        //If this is a bigPickup (the default at the start of the game)
        if (gameObject.tag == bigPickupTag)
        {
            //instantiate 2 child pickups with decreased size, and a "littlePickupTag" to differntiate them from the big pickup
            GameObject childPickup_1 = Instantiate(gameObject, transform.position, Quaternion.identity);
            childPickup_1.transform.tag = littlePickupTag;
            childPickup_1.transform.localScale = new Vector3(0.5f,0.5f,1);
            GameObject childPickup_2 = Instantiate(gameObject, transform.position, Quaternion.identity);
            childPickup_2.transform.tag = littlePickupTag;
            childPickup_2.transform.localScale = new Vector3(0.5f,0.5f,1);
        }
        //Destroy the pickup
        Destroy(gameObject);
    }
}
