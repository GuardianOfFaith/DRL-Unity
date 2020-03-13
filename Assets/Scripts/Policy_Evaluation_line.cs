using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policy_Evaluation
{
    public class Policy_Evaluation_line
    {
        public static List<int> S = new List<int>() { 0, 1, 2, 3 , 4};
        public static List<int> A = new List<int>() { 0, 1 };
        public static List<int> T = new List<int>() { 0, 4 };
        public static int[,,] P = new int[S.Count, A.Count, S.Count];
        public static int[,,] R = new int[S.Count, A.Count, S.Count];
        public Policy_Evaluation_line()
        {
            foreach (var s in S)
            {
                if (s != 0)
                    P[s, 0, s - 1] = 1;
                if (s < 4)
                    P[s, 1, s + 1] = 1;
            }
            R[1, 0, 0] = -1;
            R[3, 1, 4] = 1;
        }

        public float[,] create_random_uniform_policy(int stateSize, int actionSize)
        {
            float[,] Pi = new float[stateSize, actionSize];
            for(int i = 0; i < stateSize; i++)
            {
                for (int j = 0; j < actionSize; j++)
                {
                    Pi[i, j] = 1/actionSize;
                }
            }
            return Pi;
        }

        public float[] iterative_policy_evaluation(List<int> S, List<int> A, List<int> T, int[,,] P, int[,,] R, float[,] Pi, float gamma = 0.99f, float theta = 0.000001f)
        {
            Random randNum = new Random();
            float[] V = Enumerable.Repeat(0, S.Count).Select(i => (float)randNum.NextDouble()).ToArray();
            for (int i = 0; i < T.Count; ++i)
            {
                V[T[i]] = 0f;
            }

            while (true)
            {
                float delta = 0;
                foreach (int s in S)
                {
                    float temp_v = V[s];
                    float temp_sum = 0f;

                    foreach (int a in A)
                    {
                        foreach (int s_p in S)
                        {
                            temp_sum += Pi[s, a] * P[s, a, s_p] * (
                                    R[s, a, s_p] + gamma * V[s_p]
                                    );
                        }
                    }
                    V[s] = temp_sum;
                    delta = Math.Max(delta, Math.Abs(V[s] - temp_v));
                }

                if (delta < theta)
                {
                    break;
                }
            }

            return V;
        }

    }
}
