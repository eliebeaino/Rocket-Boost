using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RocketController : MonoBehaviour
{

    Rigidbody myRigibody;
    [SerializeField] float boostPower =10f;
    [SerializeField] float rotationPower = 10f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip successSFX;

    [SerializeField] ParticleSystem mainEngineVFX;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] ParticleSystem successVFX;

    bool collisionDisabled = false;

    AudioSource audioSource;
    enum State { Alive,Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        myRigibody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if (state == State.Alive)
        {
            Thrusting();
            Rotation();
        }

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L)) LoadNextLvl();
            if (Input.GetKeyDown(KeyCode.C)) collisionDisabled = !collisionDisabled;
        }
    }
    
    private void Rotation()
    {
        myRigibody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationPower * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationPower * Time.deltaTime);
        }
        myRigibody.freezeRotation = false;

    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myRigibody.AddRelativeForce(Vector3.up * boostPower * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineVFX.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineVFX.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(successSFX);
                successVFX.Play();
                Invoke("LoadNextLvl", levelLoadDelay);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(deathSFX);
                Invoke("LoadFirstLvl", levelLoadDelay);
                deathVFX.Play();
                break;
        }
    }

    private void LoadFirstLvl()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLvl()
    {
        SceneManager.LoadScene(1);
    }
}
