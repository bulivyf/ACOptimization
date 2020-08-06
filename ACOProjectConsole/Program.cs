using System;

namespace ACOProjectConsole
{
	class Program
	{


		/*
		 *  main()
		 *
		 *  Main function for the ant algorithm.  Performs the simulation given the
		 *  constraints defined in AWorld.cs
		 *
		 */
		public static void Main(string[] args)
		{
			AntController ctrl = new AntController();
			int curTime = 0;
			//srand( time(NULL) );

			ctrl.Init();
			while (++curTime < AWorld.MAX_TIME)
			{
				if (ctrl.SimulateAnts() == 0)
				{
					ctrl.UpdateTrails();
					if (curTime != AWorld.MAX_TIME)
						ctrl.RestartAnts();
					Console.WriteLine("Time is " + curTime + " with best " + AWorld.best);
				}
			}

			Console.WriteLine("Best tour " + AWorld.best.ToString("00.000") + " for ant "+AWorld.bestIndex);
			Console.Write("\n");
			ctrl.WriteDataFile(AWorld.bestIndex);
			ctrl.WriteTable();
		}

	} // end class AntAlg
}
