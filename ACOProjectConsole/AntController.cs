/*
 *  Ant Algorithm for TSP
 *
 */

using System;

namespace ACOProjectConsole
{
	public class AntController
	{
		AWorld aworld;
		Random rnd = new Random();

		public AntController()
		{
			aworld = new AWorld();
		}

		double getSRand()
		{
			return (rnd.NextDouble() / (double)AWorld.RAND_MAX);
		}


		int getRand(double x)
		{
			return (int)((double)x * rnd.NextDouble() / (double)(AWorld.RAND_MAX + 1.0));
		}


		/*
		 *  Init()
		 *
		 *  Initialize the aworld.pstations, their aworld.distances and the Ant population.
		 *
		 */
		public void Init()
		{
			int from, to, ant;

			/* Create the aworld.pstations and their locations */
			for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
			{
				/* Randomly place aworld.pstations around the grid */
				aworld.pstations[from].x = getRand(AWorld.MAX_DISTANCE);
				aworld.pstations[from].y = getRand(AWorld.MAX_DISTANCE);

				for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				{
					aworld.distance[from, to] = 0.0;
					aworld.pheromone[from, to] = AWorld.INIT_PHEROMONE;
				}
			}

			/* Compute the aworld.distances for each of the aworld.pstations on the map */
			for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
			{
				for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				{
					if ((to != from) && (aworld.distance[from, to] == 0.0))
					{
						int xd = Math.Abs(aworld.pstations[from].x - aworld.pstations[to].x);
						int yd = Math.Abs(aworld.pstations[from].y - aworld.pstations[to].y);

						aworld.distance[from, to] = Math.Sqrt((xd * xd) + (yd * yd));
						aworld.distance[to, from] = aworld.distance[from, to];
					}
				}
			}

			/* Initialize the aworld.ants */
			to = 0;
			for (ant = 0; ant < AWorld.MAX_ANTS; ++ant)
			{
				/* Distribute the aworld.ants to each of the aworld.pstations uniformly */
				if (to == AWorld.MAX_PSTATIONS) to = 0;
				aworld.ants[ant].currPStation = to++;

				for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
				{
					aworld.ants[ant].tabu[from] = 0;
					aworld.ants[ant].path[from] = -1;
				}
				aworld.ants[ant].pathIndex = 1;
				aworld.ants[ant].path[0] = aworld.ants[ant].currPStation;
				aworld.ants[ant].nextPStation = -1;
				aworld.ants[ant].tourLength = 0.0;

				/* Load the ant's current station into tabu */
				aworld.ants[ant].tabu[aworld.ants[ant].currPStation] = 1;
			}
		}


		/*
		 *  RestartAnts()
		 *
		 *  ReInitialize the ant population to start another tour around the
		 *  graph.
		 *
		 */

		public void RestartAnts()
		{
			int ant, i, to = 0;

			for (ant = 0; ant < AWorld.MAX_ANTS; ++ant)
			{

				if (aworld.ants[ant].tourLength < AWorld.best)
				{
					AWorld.best = aworld.ants[ant].tourLength;
					AWorld.bestIndex = ant;
				}

				aworld.ants[ant].nextPStation = -1;
				aworld.ants[ant].tourLength = 0.0;

				for (i = 0; i < AWorld.MAX_PSTATIONS; ++i)
				{
					aworld.ants[ant].tabu[i] = 0;
					aworld.ants[ant].path[i] = -1;
				}

				if (to == AWorld.MAX_PSTATIONS) to = 0;
				aworld.ants[ant].currPStation = to++;

				aworld.ants[ant].pathIndex = 1;
				aworld.ants[ant].path[0] = aworld.ants[ant].currPStation;

				aworld.ants[ant].tabu[aworld.ants[ant].currPStation] = 1;
			}
		}


		/*
		 *  AntProduct()
		 *
		 *  Compute the denominator for the path probability equation (concentration
		 *  of aworld.pheromone of the current path over the sum of all concentrations of
		 *  available paths).
		 *
		 */

		public double AntProduct(int from, int to)
		{
			return ((Math.Pow(aworld.pheromone[from, to], AWorld.ALPHA) *
				Math.Pow((1.0 / aworld.distance[from, to]), AWorld.BETA)));
		}


		/*
		 *  selectPStation()
		 *
		 *  Using the path probability selection algorithm and the current aworld.pheromone
		 *  levels of the graph, select the next station the ant will travel to.
		 *
		 */
		public int selectPStation(int ant)
		{
			int from, to;
			double denom = 0.0;

			/* Choose the next station to visit */
			from = aworld.ants[ant].currPStation;

			/* Compute denom */
			for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
			{
				if (aworld.ants[ant].tabu[to] == 0)
				{
					denom += AntProduct(from, to);
				}
			}

			//assert(denom != 0.0);

			do
			{
				double p;
				to++;
				if (to >= AWorld.MAX_PSTATIONS) to = 0;
				if (aworld.ants[ant].tabu[to] == 0)
				{
					p = AntProduct(from, to) / denom;
					if (getSRand() < p) break;
				}
			} while (true);
			return to;
		}


		/*
		 *  SimulateAnts()
		 *
		 *  Simulate a Math.Single step for each ant in the population.  This function
		 *  will return zero once all aworld.ants have completed their tours.
		 *
		 */

		public int SimulateAnts()
		{
			int k;
			int moving = 0;

			for (k = 0; k < AWorld.MAX_ANTS; ++k)
			{
				/* Ensure this ant still has aworld.pstations to visit */
				if (aworld.ants[k].pathIndex < AWorld.MAX_PSTATIONS)
				{
					aworld.ants[k].nextPStation = selectPStation(k);
					aworld.ants[k].tabu[aworld.ants[k].nextPStation] = 1;
					aworld.ants[k].path[aworld.ants[k].pathIndex++] = aworld.ants[k].nextPStation;
					aworld.ants[k].tourLength += aworld.distance[aworld.ants[k].currPStation, aworld.ants[k].nextPStation];

					/* Handle the final case (last station to first) */
					if (aworld.ants[k].pathIndex == AWorld.MAX_PSTATIONS)
					{
						aworld.ants[k].tourLength +=
							aworld.distance[aworld.ants[k].path[AWorld.MAX_PSTATIONS - 1], aworld.ants[k].path[0]];
					}

					aworld.ants[k].currPStation = aworld.ants[k].nextPStation;
					++moving;
				}

			}

			return moving;
		}


		/*
		 *  UpdateTrails()
		 *
		 *  Update the aworld.pheromone levels on each arc based upon the number of aworld.ants
		 *  that have travelled over it, including evaporation of existing aworld.pheromone.
		 *
		 */

		public void UpdateTrails()
		{
			int from, to, i, ant;

			/* Pheromone Evaporation */
			for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
			{
				for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				{
					if (from != to)
					{
						aworld.pheromone[from, to] *= (1.0 - AWorld.RHO);
						if (aworld.pheromone[from, to] < 0.0) aworld.pheromone[from, to] = AWorld.INIT_PHEROMONE;
					}
				}
			}

			/* Add new aworld.pheromone to the trails */

			/* Look at the tours of each ant */
			for (ant = 0; ant < AWorld.MAX_ANTS; ++ant)
			{
				/* Update each leg of the tour given the tour length */
				for (i = 0; i < AWorld.MAX_PSTATIONS; ++i)
				{
					if (i < AWorld.MAX_PSTATIONS - 1)
					{
						from = aworld.ants[ant].path[i];
						to = aworld.ants[ant].path[i + 1];
					}
					else
					{
						from = aworld.ants[ant].path[i];
						to = aworld.ants[ant].path[0];
					}

					aworld.pheromone[from, to] += (AWorld.QVAL / aworld.ants[ant].tourLength);
					aworld.pheromone[to, from] = aworld.pheromone[from, to];
				}

			}

			for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
			{
				for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				{
					aworld.pheromone[from, to] *= AWorld.RHO;
				}
			}

		}


		/*
		 *  WriteDataFile()
		 *
		 *  For the ant with the best tour (shortest tour through the graph), emit
		 *  the path in two data files (plotted together).
		 *
		 */
		public void WriteDataFile(int ant)
		{
			int station;
			//FILE *fp;

			Console.WriteLine("Ant = " + ant + " MAX_PSTATIONS = " + AWorld.MAX_PSTATIONS);

			Console.WriteLine("PowerStation #: (x,y) locations:");

			//fp = fopen("aworld.pstations.dat", "w");
			for (station = 0; station < AWorld.MAX_PSTATIONS; ++station)
			{
				Console.WriteLine(station.ToString("00") + ": ("+aworld.pstations[station].x + "," + aworld.pstations[station].y+")");
			}
			//fclose(fp);

			Console.WriteLine("Ant solution path:");

			//fp = fopen("solution.dat", "w");
			for (station = 0; station < AWorld.MAX_PSTATIONS; ++station)
			{
				Console.Write("("+
					aworld.pstations[aworld.ants[ant].path[station]].x + "," +
					aworld.pstations[aworld.ants[ant].path[station]].y +")->");
			}
			Console.Write("("+
				aworld.pstations[aworld.ants[ant].path[0]].x + "," +
				aworld.pstations[aworld.ants[ant].path[0]].y +")");

			//fclose(fp);
		}


		public void WriteTable()
		{
			int from, to;
			Console.WriteLine("\nPheromone ratings for [(from;by row), (to;per column)] powerstations.");
			Console.Write("--: ");
			for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				Console.Write(to.ToString("  00  "));
			Console.WriteLine();
			for (from = 0; from < AWorld.MAX_PSTATIONS; ++from)
			{
				for (to = 0; to < AWorld.MAX_PSTATIONS; ++to)
				{
					if(to==0) Console.Write(from.ToString("00") + ": ");
					if (from == to) 
						Console.Write("----- ");
					else
						Console.Write(aworld.pheromone[from, to].ToString("00.00")+" ");
				}
				Console.Write("\n");
			}
			Console.Write("\n");
		}
	}

} // end namespace

// EOF
