using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameTree : MonoBehaviour
{
    public Manager manager;

    public GameObject treePrefab;
    public GameObject parentTree;

    public int numberOfTrees = 10;
    public float minDistanceBetweenTrees = 5f;
    public int energy = 60;

    public Slider numberOfTreesSlider;
    public Slider minDistanceSlider;
    public Slider energySlider;

    public TextMeshProUGUI numberOfTreesText;
    public TextMeshProUGUI minDistanceText;
    public TextMeshProUGUI energyText;

    public TextMeshProUGUI totalTrees;

    public Image allowedAreaImage;
    public List<Image> restrictedAreaImages;
    public float sizeTree = 100f;


    private List<GameObject> spawnedTrees = new();
    private List<GameObject> sortedTrees = new();
    public List<GameObject> targetTrees = new();
    
    public Characters player;
    public Transform startParent;
    public Vector2 startPosition;
    public Image imagePlayer;

    private Transform initialParent;
    private RectTransform initialRectTransform;

    private bool inMove = false;
    public bool stopMove = false;

    private void Start()
    {
        GenerateTrees();
    }

    public void ArrivedFarm()
    {
        imagePlayer.sprite = player.GetComponent<Image>().sprite;

        initialParent = player.transform.parent.transform;
        initialRectTransform = player.transform.GetComponent<RectTransform>();

        player.transform.SetParent(startParent);
        player.transform.localPosition = startPosition;
    }

    public void StartMoveChar()
    {
        StartCoroutine(MoveChar());
    }

    IEnumerator MoveChar()
    {
        yield return new WaitForSeconds(targetTrees.Count / 2);

        while (targetTrees.Count > 0 && energy > 0)
        {
            if (stopMove)
            {
                stopMove = false;
                BrokenAllTargetsTrees();
                inMove = false;
                break;
            }

            float minDistance = float.MaxValue;
            int index = 0;

            for (int i = 0; i < targetTrees.Count; i++)
            {
                float distance = Vector2.Distance(targetTrees[i].transform.position, player.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }
            
                        player.transform.SetParent(targetTrees[index].transform);
            player.transform.localPosition = new Vector3(30, -40);
            targetTrees[index].GetComponent<Tree>().FallTree();
            energySlider.value--;
            float currentTime = 0;

            while (currentTime < 3)
            {
                if (stopMove) break;
                currentTime += Time.deltaTime;
                yield return null;
            }
            spawnedTrees.Remove(targetTrees[index]);
            targetTrees.RemoveAt(index);
        }

        inMove = false;
    }

    public void ResetPlayer()
    {
        player.transform.SetParent(initialParent);
        player.GetComponent<RectTransform>().localPosition = initialRectTransform.localPosition;
    }

    public void GenerateTrees()
    {
        if (inMove) return;

        ResetSpawnedTrees();
        SpawnTrees();
        OrderTrees();
    }

    public void BrokenAllTargetsTrees()
    {
        while (targetTrees.Count > 0)
        {
            targetTrees[0].GetComponent<Tree>().FallTree();
            spawnedTrees.Remove(targetTrees[0]);
            targetTrees.RemoveAt(0);
        }
    }

    public void GenerateTarget()
    {
        if (inMove)
        {
            stopMove = true;        
            return;
        }
        inMove = true;
        stopMove = false;

        targetTrees.Clear();

        while (energy >= 5)
        {
            FindClosestTrees();
            energy -= 10;
        }

        StartMoveChar();
    }

    void ResetSpawnedTrees()
    {
        int childCount = parentTree.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = parentTree.transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }

        spawnedTrees.Clear();
        sortedTrees.Clear();
        targetTrees.Clear();
    }

    void Update()
    {
        numberOfTrees = (int)numberOfTreesSlider.value;
        minDistanceBetweenTrees = minDistanceSlider.value;
        energy = (int)energySlider.value * 5;

        numberOfTreesText.text = numberOfTrees.ToString();
        minDistanceText.text = minDistanceBetweenTrees.ToString();
        energyText.text = energy.ToString();

        totalTrees.text = spawnedTrees.Count.ToString();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            int count = 0;
            while (!IsValidPosition(randomPosition))
            {
                count++;
                randomPosition = GetRandomPosition();

                if (count > 200)
                {
                    Debug.Log($"{i}: Não foi possível encontrar uma posição!");                    
                    return;
                }
            }

            GameObject newTree = Instantiate(treePrefab, randomPosition, Quaternion.identity);
            newTree.transform.SetParent(parentTree.transform);
            newTree.transform.localPosition = randomPosition;
            newTree.transform.localScale = Vector3.one;
            spawnedTrees.Add(newTree);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector2 localPosition = allowedAreaImage.rectTransform.localPosition;
        Vector2 sizeDelta = allowedAreaImage.rectTransform.sizeDelta;

        float minX = localPosition.x - sizeDelta.x / 2;
        float maxX = localPosition.x + sizeDelta.x / 2;
        float minY = localPosition.y - sizeDelta.y / 2;
        float maxY = localPosition.y + sizeDelta.y / 2;

        float x = UnityEngine.Random.Range(minX, maxX);
        float y = UnityEngine.Random.Range(minY + sizeTree / 2, maxY);

        return new Vector3(x, y, 0f);
    }

    bool IsValidPosition(Vector3 position)
    {
        foreach (GameObject tree in spawnedTrees)
        {
            if (Vector3.Distance(tree.transform.localPosition, position) < minDistanceBetweenTrees)
            {
                return false;
            }
        }

        foreach (Image restrictedAreaImage in restrictedAreaImages)
        {
            Vector2 localPosition = restrictedAreaImage.rectTransform.localPosition;
            Vector2 sizeDelta = restrictedAreaImage.rectTransform.sizeDelta;

            Vector2 bottomLeft = new(localPosition.x - sizeDelta.x / 2, localPosition.y - sizeDelta.y / 2);
            Vector2 topRight = new(localPosition.x + sizeDelta.x / 2, localPosition.y + sizeDelta.y / 2);

            if (position.x > bottomLeft.x && position.x < topRight.x &&
                position.y + sizeTree / 2 > bottomLeft.y && position.y - sizeTree / 2 < topRight.y)
            {
                return false;
            }
        }

        return true;
    }

    void OrderTrees()
    {
        // Ordenar os filhos com base na posição Y de maneira decrescente
        spawnedTrees.Sort((a, b) => b.transform.localPosition.y.CompareTo(a.transform.localPosition.y));
        
        // Ordena as arvores com base no X
        sortedTrees = new(spawnedTrees);
        sortedTrees.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        // Reposicionar os filhos na cena
        for (int i = 0; i < spawnedTrees.Count; i++)
        {
            spawnedTrees[i].transform.SetSiblingIndex(i);
        }
    }

    void FindClosestTrees()
    {
        if (sortedTrees.Count < 1)
        {
            Debug.LogWarning("Não há árvores suficientes para encontrar as mais próximas.");
            return;
        }

        Tuple<GameObject, GameObject> closestPair = FindClosestPair(sortedTrees, 0, sortedTrees.Count - 1);

        if (closestPair != null)
        {
            targetTrees.Add(closestPair.Item1);

            if (closestPair.Item2 != closestPair.Item1)
                targetTrees.Add(closestPair.Item2);

            closestPair.Item1.GetComponent<Tree>().SetTarget(targetTrees.Count / 2);

            if (closestPair.Item2 != closestPair.Item1)
                closestPair.Item2.GetComponent<Tree>().SetTarget(targetTrees.Count / 2);

            sortedTrees.Remove(closestPair.Item1);

            if (closestPair.Item2 != closestPair.Item1)
                sortedTrees.Remove(closestPair.Item2);
        }
        else
        {
            Debug.LogWarning("Não foi possível encontrar as duas árvores mais próximas.");
        }
    }

    Tuple<GameObject, GameObject> FindClosestPair(List<GameObject> sortedTrees, int left, int right)
    {
        if (right - left < 2)
        {
            return Merge(sortedTrees, left, right);
        }

        int mid = (left + right) / 2;

        Tuple<GameObject, GameObject> leftClosest = FindClosestPair(sortedTrees, left, mid);
        Tuple<GameObject, GameObject> rightClosest = FindClosestPair(sortedTrees, mid + 1, right);

        Tuple<GameObject, GameObject> closestPair = leftClosest;

        float minDistance = Vector3.Distance(closestPair.Item1.transform.position, closestPair.Item2.transform.position);

        if (closestPair.Item1 == closestPair.Item2)
            minDistance = float.MaxValue;

        List<GameObject> strip = new ();
        int medium = (Math.Abs(right - left) / 2) + left;
        for (int i = left; i <= right; i++)
        {
            if (Math.Abs(sortedTrees[i].transform.position.x - sortedTrees[medium].transform.position.x) < minDistance)
            {
                strip.Add(sortedTrees[i]);
            }
        }

        strip.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        for (int i = 0; i < strip.Count - 1; i++)
        {
            for (int j = i + 1; j < i + 7 && j < strip.Count; j++)
            {
                float distance = Vector3.Distance(strip[i].transform.position, strip[j].transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPair = new Tuple<GameObject, GameObject>(strip[i], strip[j]);
                }
            }
        }

        float minDistanceRight = Vector3.Distance(rightClosest.Item1.transform.position, rightClosest.Item2.transform.position);

        if (rightClosest.Item1 == rightClosest.Item2)
            minDistanceRight = float.MaxValue;

        if (minDistanceRight < minDistance)
        {
            minDistance = minDistanceRight;
            closestPair = rightClosest;
        }

        strip = new List<GameObject>();

        for (int i = left; i <= right; i++)
        {
            if (Math.Abs(sortedTrees[i].transform.position.x - sortedTrees[right].transform.position.x) < minDistance)
            {
                strip.Add(sortedTrees[i]);
            }
        }

        strip.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        for (int i = 0; i < strip.Count - 1; i++)
        {
            for (int j = i + 1; j < strip.Count && (strip[j].transform.position.y - strip[i].transform.position.y) < minDistance; j++)
            {
                float distance = Vector3.Distance(strip[i].transform.position, strip[j].transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPair = new Tuple<GameObject, GameObject>(strip[i], strip[j]);closestPair = new Tuple<GameObject, GameObject>(strip[i], strip[j]);
                }
            }
        }

        return closestPair;
    }

    Tuple<GameObject, GameObject> Merge(List<GameObject> trees, int left, int right)
    {
        Tuple<GameObject, GameObject> closestPair;
        if (trees.Count > 1)
            closestPair = new Tuple<GameObject, GameObject>(trees[left], trees[right]);
        else
            closestPair = new Tuple<GameObject, GameObject>(trees[0], trees[0]);

        return closestPair;
    }

    void TemplateClosestPair()
    {
        if (sortedTrees.Count < 2)
        {
            Debug.LogWarning("Não há árvores suficientes para encontrar as mais próximas.");
            return;
        }

        GameObject closestTree1 = null;
        GameObject closestTree2 = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < sortedTrees.Count - 1; i++)
        {
            for (int j = i + 1; j < sortedTrees.Count; j++)
            {
                float distance = Vector3.Distance(sortedTrees[i].transform.position, sortedTrees[j].transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTree1 = sortedTrees[i];
                    closestTree2 = sortedTrees[j];
                }
            }
        }

        if (closestTree1 != null && closestTree2 != null)
        {
            Debug.Log("TemplateClosestPair: " + Vector3.Distance(closestTree1.transform.localPosition, closestTree2.transform.localPosition));
        }
        else
        {
            Debug.LogWarning("Não foi possível encontrar as duas árvores mais próximas.");
        }
    }
}
