using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SearchService;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.06f;

    [Header("DialogueUI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueNameText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Animator portraitAnimatorRight;
    [SerializeField] private Animator portraitAnimatorLeft;
    [SerializeField] private Image portraitReferenceRight;
    [SerializeField] private Image portraitReferenceLeft;
    [SerializeField] private Image dialogueIcon;
    [SerializeField] private GameObject speakerNamePanel;

    [Header("ChoicesUI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private string currentDialogueName;
    public bool isDialoguePlaying { get; private set; }

    private bool isCanContinueToNextLine = false;
    private bool isWaitingAfterDisplayingLine = false;

    private Coroutine displayLineCoroutine;

    public static DialogueManager instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";



    private bool isFirstSpeakerLeft = true;
    private bool isFirstSpeakerRight = true;
    private bool isPrevSpeakerLeft = true;
    private bool isPrevSpeakerRight = true;
    private string prevSpeaker = "";
    private string curSpeaker = "";
    private bool isInRightTransit = false;
    private bool isInLeftTransit = false;
    private bool isExitingDialogue = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more then one dialogue manager in the scene");
        }
        instance = this;

        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueIcon.gameObject.SetActive(false);
        portraitReferenceLeft.gameObject.SetActive(false);
        portraitReferenceRight.gameObject.SetActive(false);

        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        GetComponent<AudioSource>().enabled = false;
    }



    void Start()
    {
        
    }

    private void Update()
    {
        //return if isn't playing
        if (!isDialoguePlaying)
        {
            return;
        }
        
        if (isInRightTransit && portraitReferenceRight.rectTransform.anchoredPosition.x > -100)
        {
            portraitReferenceRight.rectTransform.anchoredPosition = new Vector2(portraitReferenceRight.rectTransform.anchoredPosition.x - 150 * Time.deltaTime, portraitReferenceRight.rectTransform.anchoredPosition.y);
        }
        else if (isInRightTransit)
        {
            isInRightTransit = false;
        }

        if (isInLeftTransit && portraitReferenceLeft.rectTransform.anchoredPosition.x < -700)
        {
            portraitReferenceLeft.rectTransform.anchoredPosition = new Vector2(portraitReferenceLeft.rectTransform.anchoredPosition.x + 150 * Time.deltaTime, portraitReferenceLeft.rectTransform.anchoredPosition.y);
        }
        else if (isInLeftTransit)
        {
            isInLeftTransit = false;
        }

        if (!isExitingDialogue)
        {
            return;
        }

        if (portraitReferenceRight.rectTransform.anchoredPosition.x < 100)
        {
            portraitReferenceRight.rectTransform.anchoredPosition = new Vector2(portraitReferenceRight.rectTransform.anchoredPosition.x + 200 * Time.deltaTime, portraitReferenceRight.rectTransform.anchoredPosition.y);
            
        }
        if (portraitReferenceLeft.rectTransform.anchoredPosition.x > -1100)
        {
            portraitReferenceLeft.rectTransform.anchoredPosition = new Vector2(portraitReferenceLeft.rectTransform.anchoredPosition.x - 200 * Time.deltaTime, portraitReferenceLeft.rectTransform.anchoredPosition.y);
        }
        if (portraitReferenceRight.rectTransform.anchoredPosition.x >= 100 && portraitReferenceLeft.rectTransform.anchoredPosition.x <= -1100)
        {
            isExitingDialogue = false;
            ExitingAnimationIsFinished();
        }
    }

    public void ProceedDialogue()
    {
        if (!isWaitingAfterDisplayingLine)
        {
            if (isCanContinueToNextLine)
            {
                ContinueStory();
            }
            else if (isDialoguePlaying)
            {
                isCanContinueToNextLine = true;
            }
        }
    }

    private void Enter(TextAsset inkJSON)
    {
        currentDialogueName = inkJSON.name;
        currentStory = new Story(inkJSON.text);
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        portraitReferenceLeft.gameObject.SetActive(true);
        portraitReferenceRight.gameObject.SetActive(true);
        //reset portrait, layout and name
        speakerNameText.text = "?";
        portraitAnimatorLeft.Play("Default");
        portraitAnimatorRight.Play("Default");
        ContinueStory();
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Enter(inkJSON);
    }

    public void EnterDialogueMode(TextAsset inkJSON, AudioClip audio)
    {
        GetComponent<AudioSource>().clip = audio;
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
        Enter(inkJSON);
    }

    private void ExitDialogueMode()
    {
        StartCoroutine(AudioFadeOut.FadeOut(GetComponent<AudioSource>(), 1.0f));
        isExitingDialogue = true;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ExitingAnimationIsFinished()
    {
        GetComponent<AudioSource>().enabled = false;
        isDialoguePlaying = false;
        portraitReferenceLeft.gameObject.SetActive(false);
        portraitReferenceRight.gameObject.SetActive(false);
        isFirstSpeakerLeft = true;
        isFirstSpeakerRight = true;
        if (currentDialogueName == "EducationDialogue")
        {
            SceneManager.instance.StartEducationBattle();
        }
        else if (currentDialogueName == "EducationDialogue1")
        {
            SceneManager.instance.StartBombardment();
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Text for current line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            //handle tags 
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("tag could not be appropriatly parsed! " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    dialogueNameText.text = tagValue;
                    curSpeaker = tagValue;
                    switch (tagValue)
                    {
                        case "???":
                            dialogueNameText.color = new Color(128f / 255f, 0f, 0f);
                            break;
                        case "Балтимор":
                            dialogueNameText.color = new Color(65f / 255f, 105f / 255f, 225f / 255f);
                            break;
                        case "Саня":
                            dialogueNameText.color = new Color(43f / 255f, 6f / 255f, 23f / 255f);
                            break;
                        case "Лёха":
                            dialogueNameText.color = new Color(243f / 255f, 229f / 255f, 171f / 255f);
                            break;
                        default:
                            dialogueNameText.color = Color.white;
                            break;
                    }
                    break;
                case LAYOUT_TAG:
                    if (tagValue == "right" && ((curSpeaker != prevSpeaker && isPrevSpeakerRight) || isFirstSpeakerRight))
                    {
                        isFirstSpeakerRight = false;
                        isInRightTransit = true;
                        portraitReferenceRight.rectTransform.anchoredPosition = new Vector2(140, 30);
                    }
                    else if (tagValue == "left" && ((curSpeaker != prevSpeaker && isPrevSpeakerLeft) || isFirstSpeakerLeft))
                    {
                        isFirstSpeakerLeft = false;
                        isInLeftTransit = true;
                        portraitReferenceLeft.rectTransform.anchoredPosition = new Vector2(-940, 30);
                    }
                    if (tagValue == "right")
                    {
                        isPrevSpeakerLeft = false;
                        isPrevSpeakerRight = true;
                        speakerNamePanel.transform.transform.localPosition = new Vector2(200, 150);
                    }
                    else if (tagValue == "left")
                    {
                        isPrevSpeakerLeft = true;
                        isPrevSpeakerRight = false;
                        speakerNamePanel.transform.transform.localPosition = new Vector2(-200, 150);
                    }
                    prevSpeaker = curSpeaker;
                    break;
                case PORTRAIT_TAG:
                    if (isPrevSpeakerRight)
                    {
                        portraitAnimatorRight.Play(tagValue);
                    }
                    else if (isPrevSpeakerLeft)
                    {
                        portraitAnimatorLeft.Play(tagValue);
                    }
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //defensive check to make sure that we can realise choice system
        if (currentChoices.Count > choices.Length) 
        {
            Debug.LogError("More Choices were given, then the UI supports. Number of Choices given: " + currentChoices.Count);
        }

        int index = 0;
        //enable and initialize the choices
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //hide choices, which we don't need now
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);

        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // empty the dialogue text
        dialogueText.text = "";

        isCanContinueToNextLine = false;
        dialogueIcon.gameObject.SetActive(false);

        HideChoices();

        bool isAddingRichTextTag = false;

        //display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            if (isCanContinueToNextLine)
            {
                dialogueText.text = line;
                break;
            }

            //check for Rich Text Tag
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        /*if (line.Length / typingSpeed < 1)
        {
            yield return new WaitForSeconds(1 - line.Length / typingSpeed);
        }*/

        //after line is displayed
        isWaitingAfterDisplayingLine = true;
        yield return new WaitForSeconds(0.5f);
        isWaitingAfterDisplayingLine = false;

        isCanContinueToNextLine = true;

        DisplayChoices();

        dialogueIcon.gameObject.SetActive(true);
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButtons in choices)
        {
            choiceButtons.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (isCanContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }
}
