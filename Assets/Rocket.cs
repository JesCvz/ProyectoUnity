using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float MainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip WinSound;
    [SerializeField] AudioClip Deadsound;

    [SerializeField] ParticleSystem mainEnginePart;
    [SerializeField] ParticleSystem WinPart;
    [SerializeField] ParticleSystem DeadPart;

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
        mainEnginePart.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(WinSound);
        WinPart.Play();
        Invoke("LoadNextLevel", levelLoadDelay); 
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        mainEnginePart.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(Deadsound);
        DeadPart.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
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
            mainEnginePart.Stop();
            
        }
    }

    private void ApplyThrust(float ThrustthisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * ThrustthisFrame);
        if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
                mainEnginePart.Play();
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
