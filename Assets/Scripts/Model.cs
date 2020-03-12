using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Policy_Evaluation
{
    public class Model : MonoBehaviour
    {
        public static Model instance;
        Policy_Evaluation_line wazarudo;

        float[,] rndup;
        public float[] itpe;

        // Start is called before the first frame update
        void Start()
        {
            if (instance != null)
                return;

            instance = this;
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

        // Update is called once per frame
        void Update()
        {

        }

    }
}