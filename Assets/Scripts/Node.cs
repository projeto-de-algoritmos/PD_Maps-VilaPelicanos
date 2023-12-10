using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField]
    private int id;

    [SerializeField]
    private Dictionary<Node, float> edges;

    [SerializeField]
    public List<Node> nodesAdj = new();

    public Game game;

    private Button button;

    [SerializeField]
    private bool isPierreShop = false;

    public bool isFarmPlayer = false;
    public bool isSchedulaGame = false;

    public Dictionary<Node, float> Edges
    {
        get { return edges; }
        set { edges = value; }
    }

    public void ResetEdges()
    {
        edges = new Dictionary<Node, float>();
    }

    public void AddEdge(Node node, float distance)
    {
        edges ??= new Dictionary<Node, float>();

        edges.Add(node, distance);
    }

    public bool ContainsNode(Node node)
    {
        edges ??= new Dictionary<Node, float>();
        return edges.ContainsKey(node);
    }


    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(ClickNode);
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public void ClickNode()
    {
        game.SelectionNode(this);
    }

    public bool IsPierreShop
    {
        get { return isPierreShop;}
        set { isPierreShop = value;}
    }
    
}
