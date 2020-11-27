using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class DemonController : MonoBehaviour
{
    public GameObject mesh1, mesh2;
    public bool walk, crouch;

    public AudioClip screamClip;
    public string screamLine;
    public AudioSource audioSource;
    public Text text;

    public AudioClip[] growls;

    private Lines scream;
    private Transform target;
    NavMeshAgent agent;

    private AudioSource audioSelf;
    private bool play = true;


    // Start is called before the first frame update
    void Start()
    {
        audioSelf = GetComponent<AudioSource>();

        if (walk)
        {
            scream = new Lines(screamLine, screamClip);
            GetComponent<Animator>().SetTrigger("Walk");
            agent = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else if(crouch)
        {
            GetComponent<Animator>().SetTrigger("Crouch");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (walk)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            agent.SetDestination(target.position);

            if (distance < agent.stoppingDistance)
            {
                //attack
                scream.PlayDialogue(text, audioSource);
            }
        }

        if (!audioSelf.isPlaying && play)
        {
            audioSelf.clip = growls[Random.Range(0, growls.Length)];
            audioSelf.Play();
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player enter");
            GetComponent<Animator>().SetTrigger("Fade");
            audioSelf.Stop();
            play = false;
        }
    }
}
