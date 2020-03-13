using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Policy_Evaluation;
using Policy_Evaluation_Grid;
using Policy_Iteration_Grid;

public class Model : MonoBehaviour
{
    public static Model instance;
    Policy_Evaluation_line wazarudo;
    Policy_Evaluation_Grid_class pGrid;
    Policy_Iteration_Grid_class pItGrid;
    Value_Iteration vIt;

    float[,] rndup;
    public float[] itpe;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public void loadPolicyLine()
    {
        if (wazarudo == null)
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

    public void loadPolicyGrid()
    {
        if (pGrid == null)
            pGrid = new Policy_Evaluation_Grid_class();
        rndup = pGrid.create_random_uniform_policy(Policy_Evaluation_Grid_class.S.Count, Policy_Evaluation_Grid_class.A.Count);
        itpe = pGrid.iterative_policy_evaluation(Policy_Evaluation_Grid_class.S, Policy_Evaluation_Grid_class.A, Policy_Evaluation_Grid_class.T, Policy_Evaluation_Grid_class.P, Policy_Evaluation_Grid_class.R, rndup);
    }

    public void loadPolicyItGrid()
    {
        if(pItGrid == null)
            pItGrid = new Policy_Iteration_Grid_class();
        rndup = pItGrid.create_random_uniform_policy(Policy_Iteration_Grid_class.S.Count, Policy_Iteration_Grid_class.A.Count);
        itpe = pItGrid.policy_iteration(Policy_Iteration_Grid_class.S, Policy_Iteration_Grid_class.A, Policy_Iteration_Grid_class.T, Policy_Iteration_Grid_class.P, Policy_Iteration_Grid_class.R).V;
    }

    public void loadValue()
    {
        if (vIt == null)
            vIt = new Value_Iteration();
        rndup = vIt.create_random_uniform_policy(Value_Iteration.S.Count, Value_Iteration.A.Count);
        itpe = vIt.Value_evaluation(Value_Iteration.S, Value_Iteration.A, Value_Iteration.T, Value_Iteration.P, Value_Iteration.R);
    }
}