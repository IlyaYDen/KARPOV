using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottomBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;
    //public GameObject name;

    private int sentenceIndex = -1;
    private StoryScene currentScene;
    private State state = State.COMPLETED;
    private Animator animator;
    private bool isHidden = false;
    private bool nameIsHidden = true;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;

    private Coroutine typingCoroutine;
    private float speedFactor = 1f;

    public StoryScene getCurrentScene()
    {
        return currentScene;
    } 
    private enum State
    {
        PLAYING, SPEEDED_UP, COMPLETED
    }

    private void Start()
    {
        sprites = new Dictionary<Speaker, SpriteController>();
        animator = GetComponent<Animator>();
    }

    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    public void Hide()
    {
        if (!isHidden)
        {
            nameIsHidden = true;
            animator.SetTrigger("Hide");
            //animator.SetTrigger("NameHide");
            isHidden = true;
        }
    }


    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        barText.text = "";
        personNameText.text = "";
        //name.SetActive(false);
    }

    public void PlayScene(StoryScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        currentScene = scene;
        this.sentenceIndex = sentenceIndex;



        PlayNextSentence(isAnimated);
    }

    public void PlayNextSentence(bool isAnimated = true)
    {
        sentenceIndex++;


        //action
        PlaySentence(isAnimated);
    }

    public void GoBack()
    {
        sentenceIndex--;
        StopTyping();
        HideSprites();
        PlaySentence(false);
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED || state == State.SPEEDED_UP;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    public bool IsFirstSentence()
    {
        return sentenceIndex == 0;
    }

    public void SpeedUp()
    {
        state = State.SPEEDED_UP;
        speedFactor = 0.001f;
    }

    public void StopTyping()
    {
        state = State.COMPLETED;
        StopCoroutine(typingCoroutine);
    }



    private void PlaySentence(bool isAnimated = true)
    {
        speedFactor = 0.5f;
        typingCoroutine = StartCoroutine(TypeText(currentScene.sentences[sentenceIndex].text));

        personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;

        if (currentScene.sentences[sentenceIndex].showName)
        {

            if (nameIsHidden)
            {
                nameIsHidden = false;
                animator.SetTrigger("NameShow");
            }
        }
        else
        {


            if (!nameIsHidden)
            {
                nameIsHidden = true;
                animator.SetTrigger("NameHide");
            }
        }

        //name.SetActive(true);
        ActSpeakers(isAnimated);
    }

    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(speedFactor * 0.05f);
            if(++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }
    public void HideSprites()
    {/*
        List<StoryScene.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            StoryScene.Action action = actions[i];
            print(action.actionType + "");
            if (action.actionType.Equals(StoryScene.Action.Type.APPEAR))
            {
                SpriteController controller;
                controller = sprites[action.speaker];
                controller.Hide(false);
            }
        }*/
        while (spritesPrefab.transform.childCount > 0)
        {
            DestroyImmediate(spritesPrefab.transform.GetChild(0).gameObject);
        }
        sprites.Clear();

        List<StoryScene.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            if (!actions[i].Equals(StoryScene.Action.Type.APPEAR))
            {
                StoryScene.Action act = actions[i];

                SpriteController controller;
                if (!sprites.ContainsKey(act.speaker))
                {
                    controller = Instantiate(act.speaker.prefab.gameObject, spritesPrefab.transform)
                        .GetComponent<SpriteController>();
                    sprites.Add(act.speaker, controller);
                }
                else
                {
                    controller = sprites[act.speaker];
                }
                controller.Setup(act.speaker.sprites[act.spriteIndex]);
                controller.Move(act.coords, 10000, false);
                controller.Show(act.coords, false);
            }

            //yield return new WaitForSeconds(speedFactor * 0.05f);
        }

    }
    private void ActSpeakers(bool isAnimated = true)
    {
        List<StoryScene.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i], isAnimated);

            //yield return new WaitForSeconds(speedFactor * 0.05f);
        }
    }

    public void ActSpeaker(StoryScene.Action action, bool isAnimated = true)
    {
        SpriteController controller;
        if (!sprites.ContainsKey(action.speaker))
        {
            controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform)
                .GetComponent<SpriteController>();
            sprites.Add(action.speaker, controller);
        }
        else
        {
            controller = sprites[action.speaker];
        }
        switch (action.actionType)
        {
            case StoryScene.Action.Type.APPEAR:
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Move(action.coords, 10000, false);
                controller.Show(action.coords, isAnimated);
                controller.ChangeSize(action.size);
                return;
            case StoryScene.Action.Type.MOVE:
                controller.Move(action.coords, action.moveSpeed, isAnimated);
                break;
            case StoryScene.Action.Type.DISAPPEAR:
                controller.Hide(isAnimated);
                break;
            case StoryScene.Action.Type.NONE:
                controller.ChangeLocation(action.coords);
                isAnimated = false;
                break;
        }

        controller.ChangeSize(action.size);
        controller.SwitchSprite(action.speaker.sprites[action.spriteIndex], isAnimated);
    }
}
