using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Mode { DefaultMode, AddEdges, PlaceNode, PlaceStart, PlaceEnd };
    PathFinding pathfinder = new PathFinding();
    Mode mode;
    public Button addVertex, addStartVertex, addEndVertex, addEdge, pathFind;
    void Start()
    {

        /*GameObject edgeButtonObject = GameObject.Find("Add Edge");*/
        pathFind.onClick.AddListener(delegate {
            print("Pathfinder button pressed!");
            pathfinder.DisplayPath();
        });
        addStartVertex.onClick.AddListener(delegate { OnClick(Mode.PlaceStart); });
        addVertex.onClick.AddListener(delegate { OnClick(Mode.PlaceNode); });
        addEndVertex.onClick.AddListener(delegate { OnClick(Mode.PlaceEnd); });
        addEdge.onClick.AddListener(delegate { OnClick(Mode.AddEdges); });
    }

    // Update is called once per frame
    void Update()
    {
        print(mode);
    }

    void OnClick(Mode buttonMode)
    {
        print("Pressed!");
        if(mode != buttonMode)
        {
            mode = buttonMode;
        }
        else
        {
            mode = Mode.DefaultMode;
        }
        
    }
   public Mode getMode()
    {
        return (mode);
    }

    public void setMode(Mode m)
    {
        mode = m;
    }
}
