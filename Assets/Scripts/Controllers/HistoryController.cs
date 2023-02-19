using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class HistoryController : MonoBehaviour
    {
        List<StoryScene.Sentence> sentences = new List<StoryScene.Sentence>();
        public GameObject historyTextSpeaker;
        public GameObject historyText;

        public GameObject content;
        public GameObject main;
        public GameObject exit;

        private TextMeshProUGUI textName;
        private TextMeshProUGUI textSentence;

        private bool show;


        public void Start()
        {
            main.SetActive(false);
            show = false;
        }
        public void addSentence(StoryScene.Sentence s)
        {
            if (!sentences.Contains(s))
            {

                sentences.Add(s);
            }
        }
        public void removeLastSentence()
        {
            sentences.RemoveAt(sentences.Count - 1);
        }

        public void hide()
        {
            show = false;
            main.SetActive(false);
        }
        public void Show() {
            show = true;
            for (var i = content.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            foreach (StoryScene.Sentence s in sentences)
            {

                print(s.text);
                if (s.showName) {
                    GameObject prefabInstance = Instantiate(historyTextSpeaker, content.transform);
                    textName = prefabInstance.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                    textSentence = prefabInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                    textName.text = s.speaker.name;
                    textSentence.text = s.text;
                }
                else
                {
                    GameObject prefabInstance = Instantiate(historyText, content.transform);
                    textSentence = prefabInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                    GameObject historyTextrr = prefabInstance.transform.Find("Text").gameObject;
                    print(prefabInstance.name);

                    if (textSentence == null)
                    {
                        Debug.LogError("textSentence not found on child object.");
                        return;
                    }

                    textSentence.text = s.text;
                }

            }

            main.SetActive(true);
        }

        internal bool isShow()
        {
            return show;
        }
    }
}