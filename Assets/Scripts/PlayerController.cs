using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator[] hands;
    public Animator journal;
    public Text text, spiritBoxText;
    public ParticleSystem breath;
    public Text photoText, ectoText, spiritText;

    public AudioSource audioSource, spiritbox;
    public AudioClip introClip1, introClip2;
    public string introLine1, introLine2;

    public AudioClip staticClip, demonSightingClip;
    public string demonSightingLine;
    public AudioClip[] ectoClips, coldClips, boxClips, spiritBoxClips;
    public string[] ectoLines, coldLines, boxLines, spiritBoxLines;
    public Renderer[] demonMeshes;

    [HideInInspector]
    public Lines dialogueIntro1, dialogueIntro2, dialogueDemonSighting;
    [HideInInspector]
    public Lines[] dialogueEcto, dialogueCold, dialogueBox, dialogueSpirit;

    private GameObject ecto, cold;

    private int index = 0;
    private bool intro1Played, intro2Played;
    private bool isPlaying, canCollectSample, canPlayBox;

    private int ectoIndex=0, coldIndex=0, boxIndex=0;

    private bool raise = false, seenDemon = false;

    // Start is called before the first frame update
    void Start()
    {
        hands[index].SetTrigger("Raise");
        breath.Pause();

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
    }

    // Update is called once per frame
    void Update()
    {
        //swap hands
        if (isPlaying && !hands[index].GetCurrentAnimatorStateInfo(0).IsName("Raise"))
        {
            isPlaying = false;
            index++;
            if (index >= hands.Length)
                index = 0;
            
            hands[index].SetTrigger("Raise");
            
        }
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
        {
            hands[index].SetTrigger("Lower");
            isPlaying = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            //show/hide journal
            raise = !raise;
            journal.SetBool("Raise", raise);

        }

        //collect ectoplasm sample
        if (canCollectSample && Input.GetKeyDown(KeyCode.E))
        {
            if (hands[index].name == "HandBeaker")
            {
                Destroy(ecto);
                ectoText.text += "I";
                text.enabled = false;
                //audioSource.clip = ectoClips[ectoIndex];
                StartCoroutine(dialogueEcto[ectoIndex].PlayDialogue(text, audioSource));
                audioSource.Play();
                ectoIndex++;
            }
        }
         
        //play spiritbox
        if (canPlayBox && !audioSource.isPlaying)
        {
            //play spirit box audio
            text.text = "Use spirit box";
            text.enabled = true;
        }
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
            if (demonMeshes[0].isVisible)
            {
                demonMeshes[0].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                //demonMeshes[1].gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                photoText.text += "I";
                
            }
            
        }
        
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ectoplasm")
        {
            text.text = "Press [e] to collect sample with flask";
            text.enabled = true;
            canCollectSample = true;
            ecto = other.gameObject;
        }
        else if (other.gameObject.tag == "ColdSpot")
        {
            if (!other.gameObject.GetComponent<ColdSpotManager>().hasChecked)
            {
                other.gameObject.GetComponent<ColdSpotManager>().hasChecked = true;
                //audioSource.clip = coldClips[coldIndex];
                StartCoroutine(dialogueCold[coldIndex].PlayDialogue(text, audioSource));
                coldIndex++;
                audioSource.Play();
            }
            canPlayBox = true;
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
            breath.Pause();
        }
    }




    private void PlaySpiritBox(bool coldSpot)
    {
        int chance = Random.Range(0, 101);
        Debug.Log(chance);
        if (coldSpot && chance <= 5)
        {
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
}
