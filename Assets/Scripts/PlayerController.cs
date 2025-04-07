using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioClip pickUpSound;
    public AudioClip obstacleSound;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private Vector3 initialPosition;
    private GameObject[] pickups;
    private bool hasLost = false;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        count = 0;
        SetCountText();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        initialPosition = transform.position;
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
    }

    void OnMove(InputValue movementValue)
    {
        if (hasLost == false)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();

            movementX = movementVector.x;
            movementY = movementVector.y;
        }

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if(count >= 12 )
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            audioSource.PlayOneShot(pickUpSound);
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }

        if(other.gameObject.CompareTag("Obstacle"))
        {
            audioSource.PlayOneShot(obstacleSound);
            loseTextObject.SetActive(true);
            winTextObject.SetActive(false);
            hasLost = true;

            Invoke("ResetGame", 5f);     // Començarà nova partida després 5 seconds
        }
    }

    private void ResetGame()
    {
        loseTextObject.SetActive(false);
        winTextObject.SetActive(false);

        transform.position = initialPosition;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;      // Per aturar de fer voltes
        movementX = 0;
        movementY = 0;

        hasLost = false;

        count = 0;
        SetCountText();

        foreach (GameObject pickup in pickups)
        {
            pickup.SetActive(true);
        }
    }
}
