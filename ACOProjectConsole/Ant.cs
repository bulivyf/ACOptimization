
using System.Collections;
using System.Collections.Generic;


namespace ACOProjectConsole
{
    public class Ant 
    {
        int id = -1;
        public int currPStation;
        public int nextPStation;
        public int[] tabu;
        public int pathIndex;
        public int[] path;
        public double tourLength;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Ant(int i)
        {
            id = i;
            currPStation = 0;
            nextPStation = 0;
            pathIndex = 0;
            tourLength = 0;
            tabu = new int[AWorld.MAX_PSTATIONS];
            path = new int[AWorld.MAX_PSTATIONS];
        }


    }
}
