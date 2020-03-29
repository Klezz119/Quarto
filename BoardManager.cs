using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public QuartoManager[,] QuartoManagers { set; get; }
    private QuartoManager selectedQuartoManager;
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private int selectionX = -1;
    private int selectionY = -1;
    public List<GameObject> QuartomanPrefabs;
    private List<GameObject> activeQuartoman = new List<GameObject>();
    public bool isYourTurn = true;
    // Start is called before the first frame update
    private void Start()
    {
        SpawnAllQuartoman();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
        DrawQuartoBoard();
        
        if (Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY>=0)
            {
                if(selectedQuartoManager == null)
                {
                    //Select the Quartomanager
                    SelectQuartomanager(selectionX,selectionY);
                }
                else
                {
                    //Move the Quartomanager
                    MoveQuartomanager(selectionX, selectionY);
                }
            }
        }
    }
    private void SelectQuartomanager(int x,int y)
    {
        if (QuartoManagers[x, y] == null)
            return;
        // if(QuartoManagers[x,y].)
        selectedQuartoManager = QuartoManagers[x, y];

    }
    private void MoveQuartomanager (int x, int y)
    {
        if(selectedQuartoManager.PossibleMove(x,y))
        {
            QuartoManagers[selectedQuartoManager.CurrentX, selectedQuartoManager.CurrentY] = null;
            selectedQuartoManager.transform.position = GetTileCenter(x, y);
            QuartoManagers[x, y] = selectedQuartoManager;
            isYourTurn = !isYourTurn;
        }
        selectedQuartoManager = null;

    }
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 40.0f, LayerMask.GetMask("QuartoPlane"))) 
        {
            //Debug.Log(hit.point);
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
       else
        {
            selectionX = -1;
            selectionY = -1;
        }


    }
    private void SpawnQuartoman(int index, int x, int y)
    {
        Vector3 temp = new Vector3(0.0f,0.75f, 0);
        GameObject go = Instantiate(QuartomanPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
        
        go.transform.SetParent(transform);
        
        go.transform.Rotate(270, 0, 0);
        go.transform.position += temp;

        QuartoManagers[x, y] = go.GetComponent<QuartoManager>();
        QuartoManagers[x, y].SetPosition(x, y);
        activeQuartoman.Add(go);

    }
    private void SpawnAllQuartoman()
    {
        activeQuartoman = new List<GameObject>();
        QuartoManagers = new QuartoManager[15, 7];
        //Spawn all
        SpawnQuartoman(0, 4, 0);
        SpawnQuartoman(1, 4, 1);
        SpawnQuartoman(2, 4, 2);
        SpawnQuartoman(3, 4, 3);
        SpawnQuartoman(4, 5, 0);
        SpawnQuartoman(5, 5, 1);
        SpawnQuartoman(6, 5, 2);
        SpawnQuartoman(7, 5, 3);
        SpawnQuartoman(8, 6, 0);
        SpawnQuartoman(9, 6, 1);
        SpawnQuartoman(10, 6, 2);
        SpawnQuartoman(11, 6, 3);
        SpawnQuartoman(12, 7, 0);
        SpawnQuartoman(13, 7, 1);
        SpawnQuartoman(14, 7, 2);
        SpawnQuartoman(15, 7, 3);


    }
    private Vector3 GetTileCenter( int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    private void DrawQuartoBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 4;
        for (int i = 0;i<=4;i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start,start+widthLine);
            //Poziome
            for (int j =0;j<=8;j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        // Draw the selection
        if(selectionX >=0 && selectionY >=0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(
                Vector3.forward * (selectionY +1)+ Vector3.right * selectionX,
                Vector3.forward * selectionY  + Vector3.right * (selectionX + 1));
        }
    }
}
