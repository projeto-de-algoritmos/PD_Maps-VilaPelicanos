using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameTree gameTree;
    public CanvasGroup fakeLoad;
    public bool playerInFarm = false;

    public TextMeshProUGUI debug;

    [SerializeField]
    public Image edgePrefab;

    [SerializeField]
    public Transform parentEdge;

    [SerializeField]
    private GameObject rectanglePrefab;

    public AlgorithmBFS algorithBFS;
    public Game game;
    public PierreShop pierreShop;

    public Slider speedSlider;
    public float speed = 5;
    public TextMeshProUGUI speedValue;

    public List<Node> graph;
    public Node startCharacter01;
    public Node endCharacter01;
    public Node startCharacter02;
    public Node endCharacter02;
    public float friendship = 200;
    public Slider slider;
    public TextMeshProUGUI valueFriendship;
    public GameObject noPath;
    public int finishChars = 0;
    public TextMeshProUGUI totalDistance01;
    public TextMeshProUGUI totalDistance02;

    [SerializeField]
    public int[][] matrixAdj;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(UpdateSliderValue);
        speedSlider.onValueChanged.AddListener(UpdateSpeedValue);

        graph ??= new List<Node>();

        SetEdges(this);
    }

    public void StartFarmPlayer(Characters player)
    {
        playerInFarm = true;
        gameTree.player = player;        
        StartCoroutine(FarmPlayer());
    }

    IEnumerator FarmPlayer()
    {
        fakeLoad.LeanAlpha(1, .5f);
        yield return new WaitForSeconds(.5f);
        gameTree.ArrivedFarm();
        game.transform.localPosition = new Vector3(1920, game.transform.localPosition.y);
        gameTree.transform.localPosition = new Vector3(0, gameTree.transform.localPosition.y);
        fakeLoad.LeanAlpha(0, .5f);
    }

    public void ExitFarm()
    {
        gameTree.stopMove = true;
        StartCoroutine(FarmExit());
    }

    IEnumerator FarmExit()
    {
        fakeLoad.LeanAlpha(1, .5f);
        yield return new WaitForSeconds(.5f);
        game.transform.localPosition = new Vector3(0, game.transform.localPosition.y);
        gameTree.transform.localPosition = new Vector3(1920, gameTree.transform.localPosition.y);
        gameTree.ResetPlayer();
        fakeLoad.LeanAlpha(0, .5f);
        playerInFarm = false;
    }

    void UpdateSpeedValue(float newValue)
    {
        speed = newValue;
        speedValue.text = Math.Round(speed, 1).ToString();
    }

    public void StartGame()
    {
        if (finishChars != 0)
            return;

        Tuple<Node, Node> startNode = Tuple.Create(startCharacter01, startCharacter02);
        Tuple<Node, Node> endNode = Tuple.Create(endCharacter01, endCharacter02);
        List<AlgorithmBFS.NewNode> nodes = algorithBFS.MST(startNode, endNode, friendship);
        List<Node> path1 = new();
        List<Node> path2 = new();
        float distance01 = 0;
        float distance02 = 0;
        
        foreach (AlgorithmBFS.NewNode node in nodes)
        {
            if (path1.Count != 0 || path2.Count != 0)
            {
                distance01 += DistanceNode.Distance(node.node.Item1, path1[^1]);
                distance02 += DistanceNode.Distance(node.node.Item2, path2[^1]);
            }

            path1.Add(node.node.Item1);
            path2.Add(node.node.Item2);
        }

        if (nodes.Count != 0)
        {
            game.StartMoveChars(path1, 1);
            game.StartMoveChars(path2, 2);

            distance01 /= 2f;
            distance02 /= 2f;

            totalDistance01.text = "Distancia total: " + Math.Round(distance01, 1) + "m";
            totalDistance02.text = "Distancia total: " + Math.Round(distance02, 1) + "m";
        }
        else
            StartCoroutine(OpenErrorMessage());
    }

    IEnumerator OpenErrorMessage()
    {
        noPath.SetActive(true);
        yield return new WaitForSeconds(5f);
        noPath.SetActive(false);
    }

    // Função chamada quando o valor do slider é alterado.
    void UpdateSliderValue(float newValue)
    {
        friendship = newValue;
        valueFriendship.text = Math.Round(friendship/2,1).ToString() + "m";
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DeleteEdges(Manager manager)
    {
        int secury = 0;
        while (manager.parentEdge.childCount > 0 && secury < 500)
        {
            secury++;
            DestroyImmediate(manager.parentEdge.GetChild(0).gameObject);
        }
    }
    public void SetEdges(Manager manager)
    {
        DeleteEdges(manager);

        foreach (Node node in manager.graph)
        {
            if (node != null && node.nodesAdj.Count > 0)
            {
                List<Node> nodesNull = new List<Node>();
                node.ResetEdges();
                foreach (Node adj in node.nodesAdj)
                {
                    if (adj == null)
                    {
                        nodesNull.Add(adj);
                        continue;
                    }

                    float distance = DistanceNode.Distance(adj.transform.localPosition, node.transform.localPosition);
                    node.AddEdge(adj, distance);
                    if (!adj.nodesAdj.Contains(node))
                        adj.nodesAdj.Add(node);
                }

                foreach (Node nodeNull in nodesNull)
                    node.nodesAdj.Remove(nodeNull);
            }
        }

        int numNodes = manager.graph.Count;

        manager.matrixAdj = new int[numNodes][];

        for (int i = 0; i < numNodes; i++)
        {
            manager.matrixAdj[i] = new int[numNodes];
            manager.graph[i].Id = i;

            for (int j = 0; j < numNodes; j++)
            {
                manager.matrixAdj[i][j] = 0;
            }
        }

        foreach (Node node in manager.graph)
        {
            int currentNodeId = node.Id;

            if (node.Edges == null)
                continue;

            foreach (Node edge in node.Edges.Keys)
            {
                Node adjNode = edge;
                float peso = node.Edges[edge];

                int adjNodeId = adjNode.Id;

                manager.matrixAdj[currentNodeId][adjNodeId] = 1;

                if (!adjNode.ContainsNode(node))
                {
                    adjNode.AddEdge(node, peso);
                }

                if (manager.matrixAdj[adjNodeId][currentNodeId] != 1)
                {
                    
                    Vector3 position = (node.transform.localPosition + adjNode.transform.localPosition) / 2f;

                    float distance = DistanceNode.Distance(node.transform.localPosition, adjNode.transform.localPosition);

                    Image edgeImage = Instantiate(manager.edgePrefab, position, Quaternion.identity);

                    edgeImage.rectTransform.sizeDelta = new Vector2(distance, 4f);

                   

                    edgeImage.transform.SetParent(manager.parentEdge);
                    edgeImage.transform.localPosition = position;

                    Vector3 direction = (adjNode.transform.localPosition - node.transform.localPosition).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    edgeImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
                    edgeImage.rectTransform.localScale = Vector3.one;
                }
            }
        }
    }
}
