using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NodeType {Start, End, Middle}
public class Node : MonoBehaviour
{
    Vector2 position;

    Vector2 parentPos;

    NodeType nodeType;
    GameUI gameui;
    MouseController mousecontroller;
    Node parentNode;
    List<Node> neighbors;
    
    public bool selected = false;
    public bool waypoint = false;
    void Start()
    {
        gameui = FindObjectOfType<GameUI>();
        if(gameui.getMode() == GameUI.Mode.PlaceStart)
        {
            nodeType = NodeType.Start;
        }
        else if (gameui.getMode() == GameUI.Mode.PlaceEnd)
        {
            nodeType = NodeType.End;
        }
        else if (gameui.getMode() == GameUI.Mode.PlaceNode)
        {
            nodeType = NodeType.Middle;
        }
        
        position = transform.position;
    }
    void Awake()
    {
        
    }
    void Update()
    {
        if (waypoint)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.magenta);
        }
        else if (selected)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.yellow);
        }
        else if(nodeType == NodeType.Middle)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.cyan);
        }
        else if (nodeType == NodeType.End)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.red);
        }
        else if (nodeType == NodeType.Start)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.gray);
        }
    }
    void OnTriggerEnter2D(Collider2D triggerCollider)
    {
        if (triggerCollider.tag == "MouseObject")
        {
            print("Collide!");
            

            mousecontroller = FindObjectOfType<MouseController>();
            /*mousecontroller.getMousePos();*/
            if (mousecontroller.MouseIsDown()) //mouse position is down and valid
            {
                if(gameui.getMode() == GameUI.Mode.DefaultMode)
                { 
                    
                }
                else if(gameui.getMode() == GameUI.Mode.AddEdges)
                {
                    mousecontroller.canPlaceEdge = true;
                }
                else if (gameui.getMode() != GameUI.Mode.AddEdges)
                {
                    mousecontroller.canPlaceEdge = false;
                }
            }
            
        }
    }

    public Vector2 getNodePos()
    {
        return (position);
    }


    public void addNeighbor(Node neighbor)
    {
        if(neighbor == null)
        {
            print("NULL!");
        }
        neighbors.Add(neighbor);
    }
    
    public List<Node> GetNeighbors()
    {
        return neighbors;
    }
    public void RemoveNode()
    {
        Destroy(gameObject);
    }

    public void SetParent(Node parentNode)
    {
        this.parentNode = parentNode;
        parentPos = this.parentNode.getNodePos();
    }

    public Node getParent()
    {
        return (parentNode);
    }
}
