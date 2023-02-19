using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ToolBarController : MonoBehaviour
{
    public GameObject settings;
    public GameObject history;

    public void Start()
    {
        settings.SetActive(false);
        history.SetActive(false);
    }
    public void ShowSettings()
    {
        settings.SetActive(true);
    }
    public void ShowHistory()
    {
        history.SetActive(true);
    }
}