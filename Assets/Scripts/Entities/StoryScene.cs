using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName ="Data/New Story Scene")]
[System.Serializable]
public class StoryScene : GameScene
{
    public List<Sentence> sentences;
    public List<Action> actions;
    public Sprite background;
    public GameScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
        public bool showName;
        public List<Action> actions;

        public List<Action> actionsAfter;

        public AudioClip music;
        public AudioClip sound;
        public AudioClip voice;

    }
    [System.Serializable]
    public struct Action
    {
        public Speaker speaker;
        public int spriteIndex;
        public Type actionType;
        public Vector2 coords;
        public float size;
        public float moveSpeed;

        [System.Serializable]
        public enum Type
        {
            NONE, APPEAR, MOVE, DISAPPEAR
        }
    }
}



public class GameScene : ScriptableObject { }
