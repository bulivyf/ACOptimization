using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACOProjectConsole
{

    public class AWorld
    {
        public static int MAX_PSTATIONS = 15;
        public static int MAX_DISTANCE = 100;
        public static int MAX_TOUR = (MAX_PSTATIONS * MAX_DISTANCE);

        public static int MAX_ANTS = 20;
        public static double ALPHA = 1.0;
        public static double BETA = 5.0;
        public static double RHO = 0.5; /* Intensity / Evaporation */
        public static double QVAL = 100;
        public static int MAX_TOURS = 500;
        public static double RAND_MAX = 1d;
        public static int MAX_TIME = (MAX_TOURS * MAX_PSTATIONS);
        public static double INIT_PHEROMONE = (1.0 / MAX_PSTATIONS);

        public static double best = (double)MAX_TOUR;
        public static int bestIndex;
        public Random rnd = new Random();
        public PowerStation[] pstations = new PowerStation[AWorld.MAX_PSTATIONS];
        public Ant[] ants = new Ant[AWorld.MAX_ANTS];
        public double[,] distance = new double[AWorld.MAX_PSTATIONS, AWorld.MAX_PSTATIONS];
        public double[,] pheromone = new double[AWorld.MAX_PSTATIONS, AWorld.MAX_PSTATIONS];

        public AWorld()
        {
            for(int i=0; i<AWorld.MAX_PSTATIONS; ++i)
            {
                pstations[i] = new PowerStation();
            }

            for (int i = 0; i < AWorld.MAX_ANTS; ++i)
            {
                ants[i] = new Ant(i);
            }
    }



    }
}
