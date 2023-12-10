using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public int start = 1;
    public int end = 1;
    public int weight = 1;

    public TextMeshProUGUI startValue;
    public TextMeshProUGUI endValue;
    public TextMeshProUGUI weightValue;

    public bool isActive = true;
    public Color colorActive;
    public Color colorInactive;
    public Image[] images;

    void Start()
    {
        Button button = transform.AddComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        startValue.text = start.ToString();
        endValue.text = end.ToString();
        weightValue.text = weight.ToString();
    }

    public void More(int status)
    {
        // 0 -> Start
        // 1 -> End
        // 2 -> Weight
        if (status == 0) 
        {
            if (start != end)
            {
                start++;
            }
        }
        else if (status == 1) 
        {
            if (end != 99)
            {
                end++;
            }
        }
        else
        { 
            if (weight != 99)
            {
                weight++;
            }
        }
    }

    public void Less(int status)
    {
        // 0 -> Start
        // 1 -> End
        // 2 -> Weight
        if (status == 0)
        {
            if (start != 0)
            {
                start--;
            }
        }
        else if (status == 1)
        {
            if (end != start)
            {
                end--;
            }
        }
        else
        {
            if (weight != 0)
            {
                weight--;
            }
        }
    }

    public void OnButtonClick()
    {
        if (isActive)
        {
            DesactiveTask();
        }
        else
        {
            ActiveTask();
        }
    }

    public void ActiveTask()
    {
        isActive = true;
        foreach (Image image in images)
        {
            image.color = colorActive;
        }
    }

    public void DesactiveTask()
    {
        isActive = false;
        foreach (Image image in images)
        {
            image.color = colorInactive;
        }
    }
}
