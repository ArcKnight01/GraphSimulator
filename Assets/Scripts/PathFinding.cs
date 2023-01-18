using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PathFinding : MonoBehaviour
{
    MouseController mouse;
    public List<Vector2> Nodes;
    List<Vector2> OpenNodes;
    List<Vector2> ClosedNodes;
    Vector2 startNode = Vector2.zero;
    Vector2 endNode = Vector2.zero;
    
    Node[] nodes;

    void Start()
    {
        print("PATHFINDER SCRIPT START");
        /*nodes = FindObjectsOfType<Node>();*/
        mouse = FindObjectOfType<MouseController>();
        /*startNode = mouse.getStartNodePos();
        endNode = mouse.getEndNodePos();*/
        //add start Node to 
    }

    public int NumNodes
    {
        get
        {
            return (Nodes.Count);
        }
    }

    public int NumOpen
    {
        get
        {
            return (OpenNodes.Count);
        }
    }
    public void AddNode(Vector2 nodePos)
    {
        Nodes.Add(nodePos);
    }

    double getDistance(Vector2 p0, Vector2 p1)
    {
        return Mathf.Sqrt((p1.x - p0.x) * (p1.x - p0.x) + (p1.y - p0.y) * (p1.y - p0.y));
    }
    double calculateFCost(Vector2 node)
    {
        print("F cost is "+ (calculateHCost(node) + calculateGCost(node)));
        return (calculateHCost(node) + calculateGCost(node));
    }

    double calculateHCost(Vector2 node)
    {   
        //how far from end node
        return (getDistance(node,endNode)); 
    }
    double calculateGCost(Vector2 node)
    {   
        //how far from start node
        return (getDistance(node, startNode));
    }
    
    void FindPath(Vector2 startNode, Vector2 endNode)
    {
        Vector2 currentPos;
        double fCostMin;

        if(startNode == null) {
            print("Null start node");
            startNode = mouse.getStartNodePos();
        }
        if(endNode == null)
        {
            print("Null end node");
            endNode = mouse.getEndNodePos();
        }

        //Start algorthm by adding the startNode to the openNodes list.
        print("Adding startnode to openNodes");
        OpenNodes.Add(startNode);
        //Loop through an infinite loop until endNode = currentNode
        while (NumOpen > 0)
        {
            //initialize these cause my code cant seem to handle not knowing initial values
            fCostMin = calculateFCost(OpenNodes[0]);
            currentPos = OpenNodes[0];
            
            //set current to the node in the OpenNodes list with lowest F-Cost

            //Find Lowest F-Cost & associated position of node (using positions because its hard to differentiate nodes without it as easily,
            //much easier to find a postion and convert pos to a node, and vice versa
            //iterate through every element the the OpenNodes list
            for (int i = 0; i < NumOpen; i++)
            {
                //find the node with the smallest F-Cost
                //if the F-Cost of node in openNodes is smaller than
                if(calculateFCost(OpenNodes[i]) <= fCostMin)
                {
                    //if calculated f-cost is less than the minimum, set new minimum to f-cost
                    fCostMin = calculateFCost(OpenNodes[i]);
                    //temporarily save current position to the position of the node at index i, if there are no smaller f-costs, this will be the node pos of current node
                    currentPos = OpenNodes[i];
                }
            }
            //remove current node from OpenNodes
            OpenNodes.Remove(currentPos);
            //add current node to closed nodes
            ClosedNodes.Add(currentPos);

            if(currentPos == endNode)
            {
                print("PATH FOUND USING A-STAR");
                //path found!
                return;
            }

            //iterate through neighbors of the current position, and check if the position of each neighbor is in closed or not traversable (by definition if not in neighbor not trav.
            for (int i = 0; i < getNeighborList(currentPos).Count; i++)
            {
                if (ClosedNodes.Contains(getNeighborList(currentPos)[i].getNodePos()))
                {
                    //skip
                }
                else
                {
                    //if the neighbor is in not in OpenNodes list, or has shorter path to the neighbor
                    if (!(OpenNodes.Contains(getNeighborList(currentPos)[i].getNodePos())))
                    {
                        //calculate f-cost of neighbor, and print it out. Idealy set it, but that would add another layer of complexity to programming this.
                        print(calculateFCost(getNeighborList(currentPos)[i].getNodePos()));
                        getNeighborList(currentPos)[i].SetParent(findNodeWithPos(currentPos)); //set parent of neigbor to current
                        print(getNeighborList(currentPos)[i].getParent()); //debugging
                                                                           //Check if neighbore is not in the openNodes list
                        if (!(OpenNodes.Contains(getNeighborList(currentPos)[i].getNodePos())))
                        {
                            //if not in OpenNodes list, add to openNodes List
                            OpenNodes.Add(getNeighborList(currentPos)[i].getNodePos());
                        }
                    }
                }
            }
        }
        
        
        
    }
    
    Node findNodeWithPos(Vector2 pos)
    {
        Node currentNode = null;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (pos == nodes[i].getNodePos())
            {
                currentNode = (nodes[i]);
            }
        }
        if(currentNode != null)
        {
            print("Found node @ " + pos);
        }
        return currentNode;
        

    }

    List<Node> getNeighborList(Vector2 pos)
    {
        print("Getting Neighbor List");
        Node currentNode = findNodeWithPos(pos);
        return currentNode.GetNeighbors();
    }
    
    public void DisplayPath()
    {
        print("Pathfinding...");
        nodes = FindObjectsOfType<Node>();
        print("Found mouse location!");
        mouse = FindObjectOfType<MouseController>();
        startNode = mouse.getStartNodePos();
        endNode = mouse.getEndNodePos();
        print("Found start & end @ " + startNode + ", " + endNode);
        print("Now running A*");
        FindPath(mouse.getStartNodePos(), mouse.getEndNodePos());
        Vector2 currentPos = endNode;
        findNodeWithPos(currentPos).waypoint = true;
        while (true)
        {
            findNodeWithPos(currentPos).getParent().waypoint = true;
            if (findNodeWithPos(currentPos).getParent().getNodePos() == startNode)
            {
                print("Returned to start!");
                return;
            }
            else
            {
                currentPos = findNodeWithPos(currentPos).getParent().getNodePos();
            }
        }

    }
}


