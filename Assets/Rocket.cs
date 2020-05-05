using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float MainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip WinSound;
    [SerializeField] AudioClip Deadsound;
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
            RespondToThrustImput();
            RespondToRotate();
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
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(WinSound);
        Invoke("LoadNextLevel", 2f); // paramatrerise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(Deadsound);
        Invoke("LoadFirstLevel", 2.5f);
    }

    private void LoadFirstLevel()
    {

        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustImput()
    {
        float ThrustthisFrame = MainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(ThrustthisFrame);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust(float ThrustthisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * ThrustthisFrame);
        if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
    }

    private void RespondToRotate()
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
