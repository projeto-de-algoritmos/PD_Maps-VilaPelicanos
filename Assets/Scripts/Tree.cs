using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    public List<Sprite> trees = new();
    public List<Sprite> stumps = new();
    public Image treeImage;
    public Image stumpImage;
    public Animator animator;
    public CanvasGroup target;
    public TextMeshProUGUI indexText;

    void Start()
    {
        if (treeImage != null && trees.Count > 0)
            SetRandomTreeImage();
        else
            Debug.LogError("Certifique-se de atribuir uma lista de sprites e um componente Image no Inspector.");
    }

    void SetRandomTreeImage()
    {
        int randomIndex = Random.Range(0, trees.Count);
        treeImage.sprite = trees[randomIndex];
        stumpImage.sprite = stumps[randomIndex];
    }

    public void FallTree()
    {
        animator.Play("BrokenTree");
        StartCoroutine(HideTarget());
    }

    public void SetTarget(int index)
    {
        target.gameObject.SetActive(true);
        indexText.text = index.ToString();

        StartCoroutine(ShowTarget(index));
    }

    IEnumerator ShowTarget(int index)
    {
        yield return new WaitForSeconds(index);

        target.LeanAlpha(1, .5f);
    }

    public void CallHideTarget()
    {
        StartCoroutine(HideTarget());
    }

    IEnumerator HideTarget()
    {
        yield return new WaitForSeconds(2f);

        target.LeanAlpha(0, .5f);

        yield return new WaitForSeconds(.5f);

        target.gameObject.SetActive(false);
    }

}
