using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nodePrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public LineRenderer linePrefab;

    int numEndNodes = 0;
    int numStartNodes = 0;

    GameUI gameui;
    Node[] nodes;

    public List<Vector2> vertices;
    Vector2 start;
    Vector2 end;
    Vector2 mousePos;
    Vector2 worldPosition;

    float screenHalfWidthWorldUnits;
    float screenHalfHeightWorldUnits;
    
    public float speed = 7;
    public double margin = 0.75;
    double placeableTop, placeableBottom;
    bool isCollidingWithNode = false;
    Node closestNode;
    Node edgeVertex0;
    Node edgeVertex1;
    public bool canPlaceEdge = false;
    int numEdgePoints = 0;

    void Start()
    {
        
        gameui = FindObjectOfType<GameUI>();
        
        float halfPlayerWidth = transform.localScale.x / 2f;
        float halfPlayerHeight = transform.localScale.y / 2f;
        screenHalfWidthWorldUnits = Camera.main.aspect * Camera.main.orthographicSize - halfPlayerWidth;
        screenHalfHeightWorldUnits = Camera.main.aspect * Camera.main.orthographicSize - halfPlayerHeight;
        placeableTop = screenHalfHeightWorldUnits - margin;
        placeableBottom = -screenHalfHeightWorldUnits + margin;
    }

    // Update is called once per frame
    void Update()
    {
        nodes = FindObjectsOfType<Node>();
        
        
        mousePos = Input.mousePosition; //get mouse pos in pixels from left corner
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos); 
        transform.position = worldPosition; //convert to "world" position 

        ConstrainMousePosition(); //constrain it to be bounded by the window, and not overlap with buttons 
        
        
        
        if (MouseIsDown() && Input.GetMouseButtonDown(0)) //Mouse held & clicked!
        {
            if(gameui.getMode() == GameUI.Mode.PlaceNode)
            {   
                
                GameObject newNode = (GameObject)Instantiate(nodePrefab, worldPosition, Quaternion.identity);
                newNode.transform.localScale = Vector2.one * 1f;
                vertices.Add(worldPosition);
            }
            else if(gameui.getMode() == GameUI.Mode.PlaceStart)
            {
                if(numStartNodes < 1)
                {
                    GameObject newNode = (GameObject)Instantiate(startPrefab, worldPosition, Quaternion.identity);
                    newNode.transform.localScale = Vector2.one * 1f;
                    //set world position to nodes list
                    vertices.Add(worldPosition);
                    numStartNodes += 1;
                    start = worldPosition;
                }
                else
                {
                    print("can only have one end node!");
                    print(start);
                }
                
            }
            else if (gameui.getMode() == GameUI.Mode.PlaceEnd)
            {
                if (numEndNodes < 1)
                {
                    GameObject newNode = (GameObject)Instantiate(endPrefab, worldPosition, Quaternion.identity);
                    newNode.transform.localScale = Vector2.one * 1f;
                    vertices.Add(worldPosition);
                    end = worldPosition;
                    numEndNodes += 1;
                }
                else
                {
                    print("Can only have one end node!");
                    print(end);
                }
                    
            }
        }
        if (gameui.getMode() == GameUI.Mode.AddEdges && canPlaceEdge)
        {
            if (numEdgePoints == 0)
            {
                setEdgeVertex0();
                edgeVertex0.selected = true;
                numEdgePoints++;

            }
            else if (numEdgePoints == 1)
            {
                if (closestNode != edgeVertex0)
                {
                    if(edgeVertex0 == null || edgeVertex1 == null)
                    {
                        print(edgeVertex1 + " " + edgeVertex0 + " NULL!");
                    }
                    setEdgeVertex1();
                    numEdgePoints++;
                    edgeVertex0.selected = false;
                    drawEdge(edgeVertex0.getNodePos(), edgeVertex1.getNodePos());
                    edgeVertex0.addNeighbor(edgeVertex1);
                    edgeVertex1.addNeighbor(edgeVertex0);
                    /*edgeVertex0 = null;
                    edgeVertex1 = null;*/
                    gameui.setMode(GameUI.Mode.DefaultMode);
                    /*closestNode = null;*/

                }
            }

        }
    }
    
    void ConstrainMousePosition()
    {
        if (transform.position.x < -screenHalfWidthWorldUnits)
        {
            transform.position = new Vector2(-screenHalfWidthWorldUnits, transform.position.y);
        }
        if (transform.position.x > screenHalfWidthWorldUnits)
        {
            transform.position = new Vector2(screenHalfWidthWorldUnits, transform.position.y);
        }
        if (transform.position.y < -screenHalfHeightWorldUnits)
        {
            transform.position = new Vector2(transform.position.x, -screenHalfHeightWorldUnits);
        }
        if (transform.position.y > screenHalfHeightWorldUnits)
        {
            transform.position = new Vector2(transform.position.x, screenHalfHeightWorldUnits);
        }
    }

    void OnTriggerEnter2D(Collider2D triggerCollider)
    {
        
        if (triggerCollider.tag == "Node")
        {
            isCollidingWithNode = true;
           
            double minDist = Mathf.Sqrt((transform.position.x) * (transform.position.x) + (transform.position.y) * (transform.position.y));

            for (int i = 0; i < nodes.Length; i++)
            {
                double distanceToNode = Mathf.Sqrt((nodes[i].getNodePos().x - transform.position.x) * (nodes[i].getNodePos().x - transform.position.x) + (nodes[i].getNodePos().y - transform.position.y) * (nodes[i].getNodePos().y - transform.position.y));
                if(distanceToNode < minDist)
                {
                    minDist = distanceToNode;
                    closestNode = nodes[i];
                }
            }
            
        }
    }

    public Vector2 getMousePos()
    {
        return (worldPosition);
    }

    public bool MouseIsDown()
    {
        if (Input.GetMouseButton(0) && worldPosition.y > placeableBottom && worldPosition.y < placeableTop)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setEdgeVertex0()
    {
        edgeVertex0 = closestNode;
    }

    public void setEdgeVertex1()
    {
        edgeVertex1 = closestNode;
    }
    void drawEdge(Vector2 p0, Vector2 p1)
    {
        LineRenderer line = (LineRenderer)Instantiate(linePrefab);
        Vector3[] points = { new Vector3(p0.x, p0.y), new Vector3(p1.x, p1.y) };
        line.SetPositions(points);
        numEdgePoints = 0;
    }

    public Vector2 getStartNodePos()
    {
        return (start);
    }
    public Vector2 getEndNodePos()
    {
        return (end);
    }
}


