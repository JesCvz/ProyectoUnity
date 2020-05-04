using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float MainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;


    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //todo somewhere stop sound when dead
        if(state==State.Alive)
        {
            Thrust();
            Rotate();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; } //ignore anymore collisions

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("OK");
                break;

            case "Landing":
                state = State.Transcending;
                Invoke("LoadNextLevel",1f); // paramatrerise time
                break;

            default:
                state = State.Dying;
                Invoke("LoadFirstLevel",2f);
                break;
        }
    }

    private void LoadFirstLevel()
    {

        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        float ThrustthisFrame = MainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up*ThrustthisFrame);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual rotation

        
        float rotationthisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationthisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationthisFrame);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }


}
