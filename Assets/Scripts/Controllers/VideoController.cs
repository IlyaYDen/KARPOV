using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoImage;
    public GameController gameController;

    private bool show;

    public void Start()
    {
        videoImage.SetActive(false);
        show = false;
    }
    public void playVideo(videoScene videoScene)
    {
        videoPlayer.playOnAwake = false;
        videoImage.SetActive(true);
        // Set video source
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = (gameController.currentScene as videoScene).clip;
        videoPlayer.loopPointReached += EndReached;

        videoPlayer.Play();
        //videoPlayer. = "YOUR_VIDEO_URL_HERE";
    }
    void EndReached(VideoPlayer vp)
    {
        // Activate object or perform other action here
        videoImage.SetActive(false);
        gameController.currentScene = (gameController.currentScene as videoScene).nextScene;
        gameController.PlayScene(gameController.currentScene);
    }
}