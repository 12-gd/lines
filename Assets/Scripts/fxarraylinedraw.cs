using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class fxarraylinedraw : MonoBehaviour
{
    //debug
    public Transform debugBall;
    public LineRenderer lr;
    public float lineDistX, lineDistY;

    //line array & drawing area 
    public Transform daUpperLeft;
    public float daWidthOffset;
    public float daHeigthOffset;
    public float daWidth;
    public float daHeight;
    public float daResolution = 0.1f;

    public float daAngle;

    //
    private float[,] la;
    private Transform[,] latr;
    private LineRenderer[] lrs;

    // Start is called before the first frame update
    void Start()
    {
        InitLineArray(daWidth, daHeight, daResolution);
    }

    void InitLineArray(float width,float height,float res)
    {
        if (res >= width || res >= height) return;
        //float resolution = width * res;
        int num_columns = Mathf.RoundToInt(width / res);
        int num_rows = Mathf.RoundToInt(height / res);

        //if (Application.isEditor) Debug.Log("colummns: " + num_columns + " rows: " + num_rows);

        la = new float[num_columns, num_rows];
        latr = new Transform[num_columns, num_rows];
        lrs = new LineRenderer[num_columns];

        lr.gameObject.SetActive(true);

        for (int c = 0; c < lrs.Length; c++)
        {
            lrs[c] = Instantiate(lr);
            lrs[c].positionCount = num_rows;
        }

        lr.gameObject.SetActive(false);

        float default_angle = Mathf.PI * 0.25f;

        for (int i = 0; i < num_columns; i++)
        {

            for (int j = 0; j < num_rows; j++)
            {
                //default_angle = ((float)j / (float)num_rows) ;
                default_angle = Mathf.Sin( (float)j * 0.001f) * ((float)i * Mathf.PI );
                //default_angle = Mathf.Sin((float)j * 0.001f);
                la[i, j] = default_angle;
            }
        }

        DrawLa(la);
    }

    void DrawLa(float[,] la)
    {
        int num_columns = la.GetLength(0);
        int num_rows = la.GetLength(1);

        float columnStep = daWidth / num_columns;
        float rowStep = daHeight / num_rows;

        for (int i = 0; i < num_columns; i++)
        {
            for (int j = 0; j < num_rows; j++)
            {
                //la[i, j] = default_angle;
                GameObject d = Instantiate(debugBall.gameObject);
                latr[i, j] = d.transform;
                d.transform.parent = daUpperLeft;
                d.name = i.ToString() + "_" + j.ToString();
                d.transform.localPosition = new Vector3( (columnStep * i) , 0f , (rowStep * j));
                //(row / float(num_rows)) * PI
                float deg = la[i,j] * Mathf.Rad2Deg;
                d.transform.RotateAroundLocal(Vector3.up, deg);
            }
        }

        //move lines to basepos
        for (int i = 0; i < num_columns; i++)
        {
            lrs[i].SetPosition(0, latr[i, 0].transform.localPosition);
            // Debug.Log(i + " "+latr[i, 0].transform.localPosition);
            for (int j = 0; j < num_rows; j++)
            {

                Vector3 newpos = new Vector3(lrs[i].GetPosition(0).x * lineDistX, 0f, j*lineDistY);//
                //lr.SetPosition(j, newpos);
                lrs[i].SetPosition(j, newpos);
            }
        }

       // Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLa();
    }

    void UpdatePos(float num_colummns, float num_rows)
    {

    }

    void UpdateLa()
    {
        int num_columns = la.GetLength(0);
        int num_rows = la.GetLength(1);

        //Vector3 startPos = latr[0, 0].;

        for (int i = 0; i < num_columns; i++)
        {
            lrs[i].SetPosition(0, latr[i, 0].transform.localPosition);
           // Debug.Log(i + " "+latr[i, 0].transform.localPosition);
            for (int j = 1; j < num_rows; j++)
            {
                float deg = (Mathf.Sin((float)j * daAngle) * Mathf.Rad2Deg) * Time.deltaTime;//first v

                //float deg = (daAngle * Mathf.Rad2Deg);// * Time.deltaTime;

                //modify 'angle'
                latr[i,j].RotateAroundLocal(Vector3.up, deg);

                //draw line using 'angle'
                
                Vector3 newpos =  (lrs[i].GetPosition(j-1)) + Vector3.ClampMagnitude( (latr[i, j].forward*lineDistY),lineDistY );
                //lr.SetPosition(j, newpos);
                lrs[i].SetPosition(j, newpos);
            }
        }
    }
}
