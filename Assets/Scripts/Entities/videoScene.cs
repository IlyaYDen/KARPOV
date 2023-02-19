using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewVideoScene", menuName = "Data/New Video Scene")]
[System.Serializable]
public class videoScene : GameScene
{
    //public RenderTexture RenderTexture;
    public VideoClip clip;
    public GameScene nextScene;
    //public VideoPlayer video;
    //public RawImage videoImage;
}
