using Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameScene currentScene;
    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;
    public ChooseController chooseController;
    public VideoController videoController;
    public AudioController audioController;

    public HistoryController historyController;

    private State state = State.IDLE;


    private List<StoryScene> history = new List<StoryScene>();

    private enum State
    {
        IDLE, ANIMATE, CHOOSE, VIDEO
    }

    void Start()
    {
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            bottomBar.PlayScene(storyScene);
            backgroundController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[0]);
            PlayVoice(storyScene.sentences[0]);
        }
    }


    void Update()
    {
        if (state == State.ANIMATE && (Input.GetKeyDown(KeyCode.Space) || (Input.mousePosition.y > 45 && Input.GetMouseButtonDown(0))))
        {
            //todo skip animation
        }
       
        else if (state == State.IDLE
            && !historyController.isShow()
            && (Input.GetKeyDown(KeyCode.Space) || (Input.mousePosition.y>45
            && Input.GetMouseButtonDown(0))))
            {
                if (bottomBar.IsCompleted())
                {

                AudioStop();
                bottomBar.StopTyping();
                    if (bottomBar.IsLastSentence())
                    {
                        currentScene = (currentScene as StoryScene).nextScene;
                        PlayScene(currentScene);//currentScene as StoryScene



                }
                    else
                    {
                        bottomBar.PlayNextSentence();
                        int index = bottomBar.GetSentenceIndex();
                        StoryScene.Sentence s =
                            (currentScene as StoryScene)
                            .sentences[index];


                    PlayAudio(s);
                    PlayVoice(s);
                    historyController.addSentence(s);

                    }
                }
                else
                {
                    bottomBar.SpeedUp();
                }
        }   
    }


    public void goBack()
    {
        AudioStop();
        if (bottomBar.IsFirstSentence())
        {
            //historyController.removeLastSentence();
            if (history.Count > 1)
            {
                bottomBar.StopTyping();
                bottomBar.HideSprites();
                history.RemoveAt(history.Count - 1);
                StoryScene scene = history[history.Count - 1];
                currentScene = scene;
                print((scene.nextScene as StoryScene).sentences[0].text);
                history.RemoveAt(history.Count - 1);
                PlayScene(scene, scene.sentences.Count - 2, false);
            }
        }
        else
        {
            print("-----");
            bottomBar.GoBack();
        }
    }
    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        //print((scene as StoryScene).sentences[0].text);

        Coroutine c = StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated));
    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        state = State.ANIMATE;
        if (isAnimated)
        {
            bottomBar.Hide();
            foreach (StoryScene.Action a in bottomBar.getCurrentScene().actions)
            {
                bottomBar.ActSpeaker(a, true);
            }

            yield return new WaitForSeconds(0.35f);
        }
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;

            history.Add(storyScene);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]);
            PlayVoice(storyScene.sentences[sentenceIndex + 1]);

            historyController.addSentence(storyScene.sentences[sentenceIndex + 1]);

            if (isAnimated)
            {
                backgroundController.SwitchImage(storyScene.background);
                yield return new WaitForSeconds(0.35f);
                bottomBar.ClearText();
                bottomBar.Show();
                yield return new WaitForSeconds(0.35f);
            }
            else
            {
                backgroundController.SetImage(storyScene.background);
                bottomBar.ClearText();
            }
            bottomBar.PlayScene(storyScene, sentenceIndex, isAnimated);

            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
        else if (scene is videoScene)
        {
            state = State.VIDEO;
            videoController.playVideo(scene as videoScene);
        }
    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }

    private void PlayVoice(StoryScene.Sentence sentence)
    {
        audioController.PlayVoice(sentence.voice);
    }

    private void AudioStop()
    {
        audioController.StopSounds();
    }
}
