using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Monte_Carlo_ES
{
    using StepFunc = Func<int, int, (float, int)>;
    using StepUntilEndFunc = Func<int, float[,], (int[], int[], float[], int[])>;

    public class MonteCarloExploringStarts
    {
        public (float[,], float[,]) MCES(List<int> S, List<int> A, List<int> T, float [,] Pi, StepFunc step_func, StepUntilEndFunc step_until_end, uint nb_iter, float gamma = .99f)
        {
            Random randNum = new Random();
            float[,] Q = new float[S.Count, A.Count];

            for (int i = 0; i < S.Count; ++i)
            {
                for (int j = 0; j < A.Count; ++j)
                {
                    Q[i, j] = (float) randNum.NextDouble();
                }
            }

            for (int i = 0; i < T.Count; ++i)
            {
                for (int j = 0; j < A.Count; ++j)
                {
                    Q[T[i], j] = 0f;
                }
            }

            float [,] returnsSum = new float[S.Count, A.Count];
            float [,] returnsCount = new float[S.Count, A.Count];

            for (int i = 0; i < nb_iter; ++i)
            {
                var s0 = S[randNum.Next(S.Count)];

                if (S.Contains(s0))
                {
                    continue;
                }

                var a0 = A[randNum.Next(A.Count)];

                (float r, int s) = step_func(s0, a0);
                (int[] s_list, int[] a_list, float[] r_list, int[] _) = step_until_end(s, Pi);
                float G = 0;

                s_list = new List<int>(s0).Concat(s_list).ToList().ToArray();
                a_list = new List<int>(a0).Concat(a_list).ToList().ToArray();
                r_list = new List<float>() {r}.Concat(r_list).ToList().ToArray();

                for (int t = s_list.Count(); t >= 0; --t)
                {
                    G = r_list[t] + gamma * G;
                    var st = s_list[t];
                    var at = a_list[t];

                    if (s_list.ToList().GetRange(0, t).Contains(st) && a_list.ToList().GetRange(0, t).Contains(at))
                    {
                        continue;
                    }

                    returnsSum[st, at] += G;
                    returnsCount[st, at] += 1;
                    Q[st, at] = returnsSum[st, at] / returnsCount[st, at];
                    for (int j = 0; j < A.Count; ++j)
                    {
                        Pi[st, j] = 0;
                    }

                    for (int j = 0; j < S.Count; ++j)
                    {
                        float[] Q_st = Enumerable.Range(0, S.Count).Select(x => Q[st, x]).ToArray();
                        Pi[st, argmax(Q_st)] = 1f;
                    }
                }
            }

            return (Q, Pi);
        }

        /**
         * Returns the index of the maximum value in the passed array
         */
        private int argmax(float [] array)
        {
            float max = array[0];
            int max_index = 0;

            for (int i = 1; i < array.Count(); ++i)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    max_index = i;
                }
            }

            return max_index;
        }
    }
}
