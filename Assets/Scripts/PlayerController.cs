using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{
    public int evidenceSpirit, evidencePhoto, evidenceEcto;

    public Animator[] hands;
    public Animator journal;
    public Text text, spiritBoxText, evidenceText;
    public ParticleSystem breath;
    public Text photoText, ectoText, spiritText;

    public AudioSource audioSource, spiritbox;
    public AudioClip introClip1, introClip2;
    public string introLine1, introLine2;

    public AudioClip staticClip, demonSightingClip, finishedTasksClip, escapedClip, screamClip;
    public string demonSightingLine, finishedTasksLine, escapedLine;
    public AudioClip[] ectoClips, coldClips, boxClips, spiritBoxClips;
    public string[] ectoLines, coldLines, boxLines, spiritBoxLines;
    public Renderer[] demonMeshes;

    public GameObject demonSpawners;
    public GameObject[] demonAttackers;

    public GameObject exit, fadePanel;
    public Image blackScreen;

    [HideInInspector]
    public Lines dialogueIntro1, dialogueIntro2, dialogueDemonSighting, dialogueFinishedTasks, dialogueEscaped, dialogueDeath;
    [HideInInspector]
    public Lines[] dialogueEcto, dialogueCold, dialogueBox, dialogueSpirit;

    private GameObject ecto, cold;

    private int index = 0;
    private bool intro1Played, intro2Played;
    private bool isPlaying, canCollectSample, askForSign, canPlayBox;

    private int ectoIndex=0, coldIndex=0, boxIndex=0;
    private int demonIndex;

    private bool escaped = false, death = false;

    private bool raise = false, seenDemon = false, spawn = false;

    private GameObject currentColdSpot;

    // Start is called before the first frame update
    void Start()
    {
        hands[index].SetTrigger("Raise");
        breath.Stop();

        //instantiate dialogue & subtitles
        dialogueEcto = new Lines[ectoClips.Length];
        for (int i = 0; i < dialogueEcto.Length; i++)
        {
            dialogueEcto[i] = new Lines(ectoLines[i], ectoClips[i]);
        }

        dialogueCold = new Lines[coldClips.Length];
        for (int i = 0; i < dialogueCold.Length; i++)
        {
            dialogueCold[i] = new Lines(coldLines[i], coldClips[i]);
        }

        dialogueBox = new Lines[boxClips.Length];
        for (int i = 0; i < dialogueBox.Length; i++)
        {
            dialogueBox[i] = new Lines(boxLines[i], boxClips[i]);
        }

        dialogueSpirit = new Lines[spiritBoxClips.Length];
        for (int i = 0; i < dialogueSpirit.Length; i++)
        {
            dialogueSpirit[i] = new Lines(spiritBoxLines[i], spiritBoxClips[i]);
        }

        dialogueIntro1 = new Lines(introLine1, introClip1);
        dialogueIntro2 = new Lines(introLine2, introClip2);
        dialogueDemonSighting = new Lines(demonSightingLine, demonSightingClip);
        dialogueFinishedTasks = new Lines(finishedTasksLine, finishedTasksClip);
        dialogueEscaped = new Lines(escapedLine, escapedClip);
        dialogueDeath = new Lines("*scream*", screamClip);
    }

    // Update is called once per frame
    void Update()
    {
        if (evidenceSpirit >= 3 && evidenceEcto >= 3 && evidencePhoto >= 3)
        {
            //activate demon
            foreach (Renderer rend in demonMeshes)
                rend.gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
            StartCoroutine(StartChase());
        }

        Debug.Log("isPlaying " + isPlaying);

        //swap hands
        if (isPlaying && !hands[index].gameObject.GetComponent<SwitchHands>().active)
        {
            isPlaying = false;
            index++;
            if (index >= hands.Length)
                index = 0;

            hands[index].SetTrigger("Raise");

        }
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space) && hands[index].gameObject.GetComponent<SwitchHands>().active)
        {
            hands[index].SetTrigger("Lower");
            isPlaying = true;

        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            //show/hide journal
            raise = !raise;
            journal.SetBool("Raise", raise);

            if (raise)
                hands[index].SetTrigger("Lower");
            else
                hands[index].SetTrigger("Raise");

        }

        //collect ectoplasm sample
        if (canCollectSample && Input.GetMouseButtonDown(0))
        {
            if (hands[index].name == "HandBeaker")
            {
                Destroy(ecto);
                hands[index].gameObject.GetComponent<AudioSource>().Play();
                ectoText.text += "I";
                text.enabled = false;
                //audioSource.clip = ectoClips[ectoIndex];
                StartCoroutine(dialogueEcto[ectoIndex].PlayDialogue(text, audioSource));
                audioSource.Play();
                ectoIndex++;
                canCollectSample = false;
                StartCoroutine(EvidenceRecorded());
                evidenceEcto++;
            }
        }

        if (askForSign && !audioSource.isPlaying && hands[index].name == "HandRadio")
        {
            askForSign = false;
            canPlayBox = true;
            StartCoroutine(dialogueBox[boxIndex].PlayDialogue(text, audioSource));
            boxIndex++;
            audioSource.Play();
        }

        //play spiritbox
        //if (canPlayBox && !audioSource.isPlaying)
        //{
        //    //play spirit box audio
        //    text.text = "Use spirit box";
        //    text.enabled = true;
        //}
        if (hands[index].name == "HandRadio")
        {
            if (canPlayBox && spiritbox.isPlaying == false)
            {
                PlaySpiritBox(true);
            }
            else if (!canPlayBox && spiritbox.isPlaying == false)
            {
                PlaySpiritBox(false);
            }
        }

        //take picture with camera
        if(Input.GetMouseButtonDown(0) && hands[index].name == "HandCamera")
        {
            foreach(Renderer rend in demonMeshes)
            {
                if (rend.isVisible && Vector3.Distance(transform.position, rend.gameObject.transform.position) < 20)
                {
                    rend.gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                    //demonMeshes[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                    rend.gameObject.transform.parent.gameObject.GetComponent<DemonController>().pictureTaken = true;
                    //demonMeshes[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                    photoText.text += "I";
                    StartCoroutine(EvidenceRecorded());
                    evidencePhoto++;
                }
            }
            
        }
        
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ectoplasm")
        {
            text.text = "[LMB] to collect sample with flask";
            text.enabled = true;
            canCollectSample = true;
            ecto = other.gameObject;
        }
        else if (other.gameObject.tag == "ColdSpot")
        {
            if (!other.gameObject.GetComponent<ColdSpotManager>().hasChecked)
            {
                currentColdSpot = other.gameObject;
                other.gameObject.GetComponent<ColdSpotManager>().hasChecked = true;
                //audioSource.clip = coldClips[coldIndex];
                StartCoroutine(dialogueCold[coldIndex].PlayDialogue(text, audioSource));
                coldIndex++;
                audioSource.Play();
            }
            askForSign = true;
            breath.Play();
        }
        else if (other.gameObject.name == "DemonAudioTrigger" && !seenDemon)
        {
            StartCoroutine(dialogueDemonSighting.PlayDialogue(text, audioSource));
            audioSource.Play();
            seenDemon = true;
        }
        else if (other.gameObject.name == "Intro1AudioTrigger" && !intro1Played)
        {
            intro1Played = true;
            StartCoroutine(dialogueIntro1.PlayDialogue(text, audioSource));
            audioSource.Play();
        }
        else if (other.gameObject.name == "Intro2AudioTrigger" && !intro2Played)
        {
            intro2Played = true;
            StartCoroutine(dialogueIntro2.PlayDialogue(text, audioSource));
            audioSource.Play();
            StartCoroutine(ShowInstructions());
        }
        else if (other.gameObject.tag == "Spawner" && !spawn && demonIndex < demonAttackers.Length-1)
        {
            demonAttackers[demonIndex].SetActive(false);
            demonIndex++;
            demonAttackers[demonIndex].SetActive(true);
            spawn = true;
        }
        else if (other.gameObject.tag == "ExitTrigger" && !escaped && !death)
        {
            StartCoroutine(dialogueEscaped.PlayDialogue(text, audioSource));
            audioSource.Play();
            //GetComponent<FirstPersonController>().enabled = false;
            fadePanel.GetComponent<Animator>().SetTrigger("FadeOut");
            escaped = true;
            foreach (GameObject demon in demonAttackers)
            {
                demon.SetActive(false);
            }
            StartCoroutine(EndGame("Escaped"));
        }
        else if (other.gameObject.tag == "Demon" && !escaped && !death)
        {
            blackScreen.enabled = true;
            StartCoroutine(dialogueDeath.PlayDialogue(text, audioSource));
            audioSource.Play();
            StartCoroutine(EndGame("Death"));
            death = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ectoplasm")
        {
            text.enabled = false;
            canCollectSample = false;
            ecto = null;
        }
        else if (other.gameObject.tag == "ColdSpot")
        {
            text.enabled = false;
            canPlayBox = false;
            askForSign = false;
            breath.Stop();

            if (other.gameObject == currentColdSpot && currentColdSpot.GetComponent<ColdSpotManager>().heardSpirit)
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "Spawner" && spawn)
            spawn = false;
    }




    private void PlaySpiritBox(bool coldSpot)
    {
        int chance = Random.Range(0, 101);
        Debug.Log(chance);
        if (coldSpot && chance <= 3)
        {
            if (!currentColdSpot.GetComponent<ColdSpotManager>().heardSpirit)
            {
                currentColdSpot.GetComponent<ColdSpotManager>().heardSpirit = true;
                spiritText.text += "I";
                StartCoroutine(EvidenceRecorded());
                evidenceSpirit++;
            }
            int randomIndex = Random.Range(0, spiritBoxClips.Length);
            //spiritbox.clip = spiritBoxClips[randomIndex];
            StartCoroutine(dialogueSpirit[randomIndex].PlayDialogue(spiritBoxText, spiritbox));
            spiritbox.Play();
        }
        else
        {
            spiritbox.clip = staticClip;
            spiritbox.Play();
        }
    }

    private IEnumerator ShowInstructions()
    {
        spiritBoxText.text = "Press [tab] to open journal\nPress [space] to change tool";
        spiritBoxText.enabled = true;
        yield return new WaitForSeconds(6);
        spiritBoxText.enabled = false;
    }

    private IEnumerator StartChase()
    {
        yield return new WaitForSeconds(4);
        demonAttackers[0].SetActive(true);
        demonSpawners.SetActive(true);
        dialogueFinishedTasks.PlayDialogue(text, audioSource);
    }

    private IEnumerator EvidenceRecorded()
    {
        evidenceText.enabled = true;
        //journal.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3);
        evidenceText.enabled = false;
    }

    private IEnumerator EndGame(string scene)
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(scene);
    }
}
