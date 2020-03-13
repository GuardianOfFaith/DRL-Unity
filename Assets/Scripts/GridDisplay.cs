using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Policy_Evaluation;
using Policy_Evaluation_Grid;
public class GridDisplay : MonoBehaviour
{
    [Header("Size")]
    public int dimX = 1;
    public int dimY = 1;

    [Header("Prefabs")]
    [SerializeField, HideInInspector]
    GameObject lineObject;
    [SerializeField, HideInInspector]
    GameObject terminalObject;
    [SerializeField, HideInInspector]
    GameObject probaObject;

    [HideInInspector]
    public Transform lines;
    [HideInInspector]
    public Transform terminals;
    [HideInInspector]
    public Transform probas;

    [Space]
    List<GameObject> lineList;
    List<GameObject> probaList;
    List<GameObject> terminalList;

    Transform panel;

    // Start is called before the first frame update
    void Start()
    {
        lineList = new List<GameObject>();
        terminalList = new List<GameObject>();
        probaList = new List<GameObject>();
        panel = transform.GetChild(0);
        GenGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenGrid();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearGrid();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < Policy_Evaluation_line.S.Count; i++)
                for (int j = 0; j < Policy_Evaluation_line.A.Count; j++)
                    SetProbabilities(i, j, Model.instance.itpe[i]);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 1; i <= Policy_Evaluation_Grid_class.width; i++)
                for (int j = 1; j <= Policy_Evaluation_Grid_class.height; j++)
                    SetProbabilities(i, j, Model.instance.itpe[i - 1 + (j-1)* Policy_Evaluation_Grid_class.width]);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTerminal(UnityEngine.Random.Range(1,dimX + 1), UnityEngine.Random.Range(1, dimY + 1));
        }
    }

#region GridHandler
    void GenGrid()
    {
        GameObject go;
        RectTransform rt;
        if (dimX == 1)
        {
            for (int j = 1; j < dimY; j++)
            {
                go = Instantiate(lineObject, lines);
                lineList.Add(go);
                rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 1 - ((float)j / (float)dimY));
                rt.anchorMax = new Vector2(1, 1 - ((float)j / (float)dimY));
            }
        }
        else
        {
            for (int i = 1; i < dimX; i++)
            {
                if (dimY == 1)
                {
                    go = Instantiate(lineObject, lines);
                    lineList.Add(go);
                    rt = go.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(((float)i / (float)dimX), 0);
                    rt.anchorMax = new Vector2(((float)i / (float)dimX), 1);
                }

                for (int j = 1; j < dimY; j++)
                {
                    go = Instantiate(lineObject, lines);
                    lineList.Add(go);
                    rt = go.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(((float)i / (float)dimX), 1 - ((float)j / (float)dimY));
                    rt.anchorMax = new Vector2(((float)i / (float)dimX), 1 - ((float)(j - 1) / (float)dimY));

                    go = Instantiate(lineObject, lines);
                    lineList.Add(go);
                    rt = go.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(((float)(i - 1) / (float)dimX), 1 - ((float)j / (float)dimY));
                    rt.anchorMax = new Vector2(((float)i / (float)dimX), 1 - ((float)j / (float)dimY));

                    if (j == dimY - 1)
                    {
                        go = Instantiate(lineObject, lines);
                        lineList.Add(go);
                        rt = go.GetComponent<RectTransform>();
                        rt.anchorMin = new Vector2(((float)i / (float)dimX), 1 - ((float)dimY / (float)dimY));
                        rt.anchorMax = new Vector2(((float)i / (float)dimX), 1 - ((float)(dimY - 1) / (float)dimY));
                    }
                }
                if (i == dimX - 1)
                {
                    for (int j = 1; j < dimY; j++)
                    {
                        go = Instantiate(lineObject, lines);
                        lineList.Add(go);
                        rt = go.GetComponent<RectTransform>();
                        rt.anchorMin = new Vector2(((float)(dimX - 1) / (float)dimX), 1 - ((float)j / (float)dimY));
                        rt.anchorMax = new Vector2(((float)dimX / (float)dimX), 1 - ((float)j / (float)dimY));
                    }
                }
            }
        }
    }

    void ClearGrid()
    {
        foreach(GameObject line in lineList)
        {
            Destroy(line);
        }
        foreach (GameObject term in terminalList)
        {
            Destroy(term);
        }
        foreach (GameObject pro in probaList)
        {
            Destroy(pro);
        }
        probaList.Clear();
        terminalList.Clear();
        lineList.Clear();
    }

    public void SetProbabilities()
    {
        GameObject go;
        RectTransform rt;
        for (int j = 1; j < dimY + 1; j++)
        {
            for(int i = 1; i < dimX + 1; i++)
            {
                go = Instantiate(probaObject, probas);
                probaList.Add(go);
                rt = go.GetComponent<RectTransform>();
                go.GetComponent<Text>().text = ((float)Math.Round(UnityEngine.Random.Range(0f, 1f),2)).ToString();
                rt.anchorMin = new Vector2(((float)(i - 0.5f) / (float)dimX), 1 - ((float)(j - 0.5f) / (float)dimY));
                rt.anchorMax = new Vector2(((float)(i - 0.5f) / (float)dimX), 1 - ((float)(j - 0.5f) / (float)dimY));
            }
        }
    }

    void SetProbabilities(int posX, int posY, float val)
    {
        GameObject go;
        RectTransform rt;
        go = Instantiate(probaObject, probas);
        probaList.Add(go);
        rt = go.GetComponent<RectTransform>();
        
        go.GetComponent<Text>().text = ((float)Math.Round(val, 2)).ToString();
        rt.anchorMin = new Vector2(((float)(posX - 0.5f) / (float)dimX), 1 - ((float)(posY - 0.5f) / (float)dimY));
        rt.anchorMax = new Vector2(((float)(posX - 0.5f) / (float)dimX), 1 - ((float)(posY - 0.5f) / (float)dimY));
    }

    void SetTerminal(int posX, int posY)
    {
        GameObject go;
        RectTransform rt;
        go = Instantiate(terminalObject, terminals);
        terminalList.Add(go);
        rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(((float)(posX - 0.5f) / (float)dimX), 1 - ((float)(posY - 0.5f) / (float)dimY));
        rt.anchorMax = new Vector2(((float)(posX - 0.5f) / (float)dimX), 1 - ((float)(posY - 0.5f) / (float)dimY));
    }
#endregion
}
