using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Policy_Iteration_Grid
{
    class ReturnType
    {
        public float[] V { get; set; }
        public float[,] Pi { get; set; }

        public ReturnType(float[] V, float[,] Pi)
        {
            this.V = V;
            this.Pi = Pi;
        }
    }
    class Policy_Iteration_Grid
    {
        #region grid_world
        public static int width = 10;
        public static int height = 10;
        public static int numStates = width * height;
        public static List<int> S = new List<int>();
        public static List<int> A = new List<int>() { 0, 1, 2, 3 };
        public static List<int> T = new List<int>() { width - 1, height - 1 };
        public static int[,,] P = new int[100, 4, 100];
        public static int[,,] R = new int[100, 4, 100];
        Policy_Iteration_Grid()
        {
            for (int i = 0; i < numStates; i++)
            {
                S.Add(i);
                if (i % width == 0)
                {
                    P[i, 0, i] = 1;
                }
                else
                {
                    P[i, 0, i - 1] = 1;
                }
                if ((i + 1) % width == 0)
                {
                    P[i, 1, i] = 1;
                }
                else
                {
                    P[i, 1, i + 1] = 1;
                }
                if (i < width)
                {
                    P[i, 2, i] = 1;
                }
                else
                {
                    P[i, 2, i - width] = 1;
                }
                if (i >= (numStates - width))
                {
                    P[i, 3, i] = 1;
                }
                else
                {
                    P[i, 3, i + width] = 1;
                }
            }
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        if (i == width - 1)
                        {
                            P[i, j, k] = 0;
                        }
                        if (i == numStates - 1)
                        {
                            P[i, j, k] = 0;
                        }
                        if (k == width - 1)
                        {
                            R[i, j, k] = -5;
                        }
                        if (k == numStates - 1)
                        {
                            R[i, j, k] = -5;
                        }
                    }
                }
            }
        }
        #endregion

        public float[,] create_random_uniform_policy(int stateSize, int actionSize)
        {
            float[,] toReturn = new float[stateSize, actionSize];
            for (int i = 0; i < stateSize; i++)
            {
                for (int j = 0; j < actionSize; j++)
                {
                    toReturn[i, j] = 1 / actionSize;
                }
            }
            return toReturn;
        }

        public float[] iterative_policy_evaluation(List<int> s, List<int> a, List<int> t, int[,,] p, int[,,] r, float[,] pi, float gamma = 0.99f, float theta = 0.00001f, float[] V = null)
        {
            if (0 < gamma && gamma < 1 && theta > 0)
            {
                if (V == null)
                {
                    Random random = new Random();
                    V = Enumerable.Repeat((float)random.NextDouble(), 100).ToArray();
                    foreach (var term in t)
                    {
                        V[term] = 0f;
                    }
                }
                while (true)
                {
                    float delta = 0f;
                    foreach (var state in s)
                    {
                        float temp_v = V[state];
                        float temp_sum = 0;
                        foreach (var action in a)
                        {
                            foreach (var p_state in s)
                            {
                                temp_sum += pi[state, action] * P[state, action, p_state] * (R[state, action, p_state] + gamma * V[p_state]);
                            }
                        }
                        V[state] = temp_sum;
                        delta = Math.Max(delta, Math.Abs(V[state] - temp_v));
                    }
                    if (delta < theta)
                    {
                        break;
                    }
                }
            }
            return V;
        }

        public ReturnType policy_iteration(List<int> s, List<int> a, List<int> t, int[,,] p, int[,,] r, float gamma = 0.99f, float theta = 0.00001f)
        {
            Random random = new Random();
            var V = Enumerable.Repeat((float)random.NextDouble(), 100).ToArray();
            foreach (var term in t)
            {
                V[term] = 0f;
            }
            var Pi = create_random_uniform_policy(s.Count, a.Count);
            while (true)
            {
                V = iterative_policy_evaluation(s, a, t, p, r, Pi, gamma, theta, V);
                bool policy_stable = true;
                foreach (var state in s)
                {
                    int old_action = -50;
                    for (int i = 0; i < s.Count; i++)
                    {
                        for (int j = 0; j < a.Count; j++)
                        {
                            if (i == state)
                            {
                                if (Pi[i, j] > old_action)
                                {
                                    old_action = j;
                                }
                            }
                        }
                    }

                    int best_action = 0;
                    float best_action_score = -999999f;
                    foreach (var action in a)
                    {
                        float tmp_score = 0f;
                        foreach (var s_p in s)
                        {
                            tmp_score += P[state, action, s_p] * (R[state, action, s_p] + gamma * V[s_p]);
                        }
                        if (best_action_score <= tmp_score)
                        {
                            best_action = action;
                            best_action_score = tmp_score;
                        }
                    }
                    for (int i = 0; i < s.Count; i++)
                    {
                        for (int j = 0; j < a.Count; j++)
                        {
                            if (i == state)
                            {
                                Pi[i, j] = 0;
                            }
                        }
                    }
                    Pi[state, best_action] = 1f;
                    if (best_action != old_action)
                    {
                        policy_stable = false;
                    }
                }
                if (policy_stable)
                {
                    break;
                }
            }
            ReturnType toReturn = new ReturnType(V, Pi);
            return toReturn;
        }
    }
}
