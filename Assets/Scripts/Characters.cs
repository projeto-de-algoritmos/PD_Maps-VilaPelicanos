using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Characters : MonoBehaviour
{
    public Sprite imageChar;
    public Game game;
    public Manager manager;
    public int numChar;

    public void StartChar(List<Node> nodes, Manager _manager, Game _game)
    {
        manager = _manager;
        game = _game;
        StartCoroutine(Movendo(nodes));
    }

    IEnumerator Movendo(List<Node> nodes)
    {
        foreach (Node node in nodes) 
        {
            gameObject.LeanMoveLocal(node.transform.localPosition, 2f / manager.speed).setEaseInOutQuad();                                 

            yield return new WaitForSeconds(2f/manager.speed);
        }

        manager.finishChars++;

        while (manager.finishChars != 2)
            yield return null;

        if (nodes[^1].IsPierreShop)
        {
            if (!manager.pierreShop.gameObject.activeSelf) 
                manager.pierreShop.gameObject.SetActive(true);

            if (!manager.pierreShop.isOpen)
            {
                manager.pierreShop.isOpen = true;
                manager.pierreShop.OpenShop(this);

                while (manager.pierreShop.isOpen)
                    yield return null;
            }
        } else if (nodes[^1].isFarmPlayer)
        {
            if (!manager.playerInFarm)
                manager.StartFarmPlayer(this);

            while (manager.playerInFarm)
                yield return null;
        }
        else if (nodes[^1].isSchedulaGame)
        {
            if (!manager.inSchedulaGame)
                manager.StartSchedulaGame();

            while (manager.inSchedulaGame)
                yield return null;
        }

        yield return new WaitForSeconds(2);

        manager.finishChars--;
        game.isMoving--;

        if (manager.pierreShop.isOpen)
        {
            while (manager.pierreShop.isOpen)
                yield return null;

            yield return new WaitForSeconds(2);
        }

        gameObject.transform.localPosition = nodes[0].transform.localPosition;
    }

    public void SelectedChar()
    {
        game.SelectionCharacter(this);
    }
}
