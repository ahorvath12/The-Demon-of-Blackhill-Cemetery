using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator[] hands;
    public Animator journal;
    public Text text;
    public ParticleSystem breath;
    public Text photoText, ectoText, spiritText;

    public AudioSource audioSource, spiritbox;
    public AudioClip staticClip;
    public AudioClip[] ectoClips, coldClips, boxClips, spiritBoxClips;
    public Renderer[] demonMeshes;
    

    private GameObject ecto, cold;

    private int index = 0;
    private bool isPlaying, canCollectSample, canPlayBox;

    private int ectoIndex=0, coldIndex=0, boxIndex=0;

    private bool raise = false;

    // Start is called before the first frame update
    void Start()
    {
        hands[index].SetTrigger("Raise");
        breath.Pause();
    }

    // Update is called once per frame
    void Update()
    {
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
            raise = !raise;
            journal.SetBool("Raise", raise);

        }

        if (canCollectSample && Input.GetKeyDown(KeyCode.E))
        {
            if (hands[index].name == "HandBeaker")
            {
                Destroy(ecto);
                ectoText.text += "I";
                text.enabled = false;
                audioSource.clip = ectoClips[ectoIndex];
                audioSource.Play();
                ectoIndex++;
            }
        }
         
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

        if(Input.GetMouseButtonDown(0) && hands[index].name == "HandCamera")
        {
            foreach (Renderer rend in demonMeshes)
            {
                if (rend.isVisible)
                {
                    rend.gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Fade");
                    photoText.text += "I";
                }
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
                audioSource.clip = coldClips[coldIndex];
                coldIndex++;
                audioSource.Play();
            }
            canPlayBox = true;
            breath.Play();
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
        if (coldSpot && chance < 5)
        {
            int randomIndex = Random.Range(0, spiritBoxClips.Length);
            spiritbox.clip = spiritBoxClips[randomIndex];
            spiritbox.Play();
        }
        else
        {
            spiritbox.clip = staticClip;
            spiritbox.Play();
        }
    }
}
