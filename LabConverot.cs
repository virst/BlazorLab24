using LabGenerator;

namespace BlazorLab24
{
    public static class LabConverot
    {
        public static int[][] Converct(LabGen lg)
        {
            int[][] mass;
            int n = lg.MazeHeight * lg.MazeWidth;
            Rcv rcv = new(lg.MazeWidth);
            Rcv rcv2 = new(lg.MazeWidth);

            mass = new int[n][];
            for (int i = 0; i < n; i++)
            {
                mass[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        mass[i][j] = int.MaxValue;
                    else
                        mass[i][j] = 0; // каждый связан с самим собой 
                }
            }

            for (int i = 0; i < n; i++)
            {
                rcv.Val = i;

                for (int j = 0; j < n; j++)
                {
                    if (EmumMedods.HasFlag(lg.Map[rcv.Row][rcv.Col], CellOptions.ExitNorth) &&
                        LabGen.InMazeBorders(rcv.Row - 1, rcv.Col, lg.MazeHeight, lg.MazeWidth))
                    {
                        rcv2.SetRC(rcv.Row - 1, rcv.Col);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.Map[rcv.Row][rcv.Col], CellOptions.ExitSouth) &&
                            LabGen.InMazeBorders(rcv.Row + 1, rcv.Col, lg.MazeHeight, lg.MazeWidth))
                    {
                        rcv2.SetRC(rcv.Row + 1, rcv.Col);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.Map[rcv.Row][rcv.Col], CellOptions.ExitEast) &&
                         LabGen.InMazeBorders(rcv.Row, rcv.Col + 1, lg.MazeHeight, lg.MazeWidth))
                    {
                        rcv2.SetRC(rcv.Row, rcv.Col + 1);
                        mass[i][rcv2.Val] = 1;
                    }

                    if (EmumMedods.HasFlag(lg.Map[rcv.Row][rcv.Col], CellOptions.ExitWest) &&
                         LabGen.InMazeBorders(rcv.Row, rcv.Col - 1, lg.MazeHeight, lg.MazeWidth))
                    {
                        rcv2.SetRC(rcv.Row, rcv.Col - 1);
                        mass[i][rcv2.Val] = 1;
                    }
                }
            }

            return mass;
        }
    }
}
