using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Monte_Carlo_ES
{
    public class MCESModel : MonoBehaviour
    {
        public static MCESModel instance;

        public float[,] Pi;
        public float[,] Q;

        private static int IntPow(int x, int pow)
        {
            int ret = 1;
            while ( pow != 0 )
            {
                if ( (pow & 1) == 1 )
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        public static int[] GenerateStates()
        {
            int s = 0;
            int[] states = new int[19683];

            for(int i = 0; i < 19683 / 3; i += 3)
            {
                s += (i % 3) * IntPow(10, i / 3);
                states[i] = (i % 3);
            }

            return states;
        }

        public static int[] GenerateTerminalStates(int[] S)
        {
            TicTacToe game = new TicTacToe();
            List<int> T = new List<int>();

            foreach (int s in S)
            {
                game.SetStateFromInt(s);
                game.checkWin();
                if (game.won)
                {
                    T.Add(s);
                }
            }

            return T.ToArray();
        }

        void Start()
        {
            if (instance != null)
            {
                return;
            }

            instance = this;

            int[] S = GenerateStates();
            int[] A = Enumerable.Range(0, 9).ToArray();
            int[] T = GenerateTerminalStates(S);

            (Q, Pi) = new MonteCarloExploringStarts().MCES(S, A, T, Pi, step_func, step_until_end, 1000);
        }
    }
}
