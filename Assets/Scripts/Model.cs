using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Policy_Evaluation;
using Policy_Evaluation_Grid;

public class Model : MonoBehaviour
{
    public static Model instance;
    Policy_Evaluation_line wazarudo;
    Policy_Evaluation_Grid_class pGrid;

    float[,] rndup;
    public float[] itpe;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            return;
        instance = this;

        pGrid = new Policy_Evaluation_Grid_class();
        rndup = pGrid.create_random_uniform_policy(Policy_Evaluation_Grid_class.S.Count, Policy_Evaluation_Grid_class.A.Count);
        itpe = pGrid.iterative_policy_evaluation(Policy_Evaluation_Grid_class.S, Policy_Evaluation_Grid_class.A, Policy_Evaluation_Grid_class.T, Policy_Evaluation_Grid_class.P, Policy_Evaluation_Grid_class.R, rndup);

        //rndup = new float[Policy_Evaluation_Grid_class.S.Count, Policy_Evaluation_Grid_class.A.Count];
        //itpe = pGrid.iterative_policy_evaluation(Policy_Evaluation_Grid_class.S, Policy_Evaluation_Grid_class.A, Policy_Evaluation_Grid_class.T, Policy_Evaluation_Grid_class.P, Policy_Evaluation_Grid_class.R, rndup);

    }

    void loadPolicyLine()
    {
        wazarudo = new Policy_Evaluation_line();
        rndup = wazarudo.create_random_uniform_policy(Policy_Evaluation_line.S.Count, Policy_Evaluation_line.A.Count);
        itpe = wazarudo.iterative_policy_evaluation(Policy_Evaluation_line.S, Policy_Evaluation_line.A, Policy_Evaluation_line.T, Policy_Evaluation_line.P, Policy_Evaluation_line.R, rndup);
        rndup = new float[Policy_Evaluation_line.S.Count, Policy_Evaluation_line.A.Count];
        for (int i = 0; i < Policy_Evaluation_line.S.Count; i++)
            for (int j = 0; j < Policy_Evaluation_line.A.Count; j++)
            {
                if (j == 1)
                {
                    rndup[i, j] = 1.0f;
                }
                else
                {
                    rndup[i, j] = 0f;
                }

            }
        itpe = wazarudo.iterative_policy_evaluation(Policy_Evaluation_line.S, Policy_Evaluation_line.A, Policy_Evaluation_line.T, Policy_Evaluation_line.P, Policy_Evaluation_line.R, rndup);
    }

    void loadPolicyGrid()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}