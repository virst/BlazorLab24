namespace LabGenerator
{
    public record CellData(int Row, int Col);

    public class LabGen
    {
        private readonly int w;
        private readonly int h;

        public readonly CellOptions[][] Map;

        private readonly int[] dr = [-1, 0, 1, 0];
        private readonly int[] dc = [0, 1, 0, -1];
        private readonly Random rnd;
        private readonly List<CellData> cellList;

        public LabGen(int h, int w)
        {
            this.w = w; this.h = h;
            Map = new CellOptions[this.h][];
            for (int i = 0; i < this.h; i++)
            {
                Map[i] = new CellOptions[this.w];
                for (int j = 0; j < this.w; j++)
                    Map[i][j] = CellOptions.None;
            }
            rnd = new Random();
            cellList = [];
        }

        public int MazeHeight => h;

        public int MazeWidth => w;

        public static bool InMazeBorders(int r, int c, int Height, int Width) //– возвращает значение true, если ячейка заданная своими координатами находится в границах лабиринта.
        {
            return r >= 0 && r < Height && c >= 0 && c < Width;
        }

        private void RemoveFrontierCell(int r, int c)  //– удаляет из списка обрабатываемых вершин элемент, заданный номером строки и столбца
        {
            EmumMedods.RemoveFlag(ref Map[r][c], CellOptions.CellFfrontier);

            for (int i = 0; i < cellList.Count; i++)
            {
                if (cellList[i].Col == c && cellList[i].Row == r)
                    cellList.RemoveAt(i);
            }
        }

        private void RemoveWall(int r, int c, int d) //  – удаляет стенку в карте лабиринта в указанной ячейке и указанном направлении, а также в смежной ячейке в противоположном направлени
        {
            switch (d)
            {
                case 0:
                    EmumMedods.AddFlag(ref Map[r][c], CellOptions.ExitNorth);
                    if (InMazeBorders(r - 1, c, h, w))
                        EmumMedods.AddFlag(ref Map[r - 1][c], CellOptions.ExitSouth);
                    break;
                case 1:
                    EmumMedods.AddFlag(ref Map[r][c], CellOptions.ExitEast);
                    if (InMazeBorders(r, c + 1, h, w))
                        EmumMedods.AddFlag(ref Map[r][c + 1], CellOptions.ExitWest);
                    break;
                case 2:
                    EmumMedods.AddFlag(ref Map[r][c], CellOptions.ExitSouth);
                    if (InMazeBorders(r + 1, c, h, w))
                        EmumMedods.AddFlag(ref Map[r + 1][c], CellOptions.ExitNorth);
                    break;
                case 3:
                    EmumMedods.AddFlag(ref Map[r][c], CellOptions.ExitWest);
                    if (InMazeBorders(r, c - 1, h, w))
                        EmumMedods.AddFlag(ref Map[r][c - 1], CellOptions.ExitEast);
                    break;

            }
        }
        private void MarkFrontierCells(int row, int col)
        {
            int i, r, c;
            //перебираем все 4 направления
            for (i = 0; i < 4; i++)
            {
                r = row + dr[i];
                c = col + dc[i];
                /* if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                    ((map[r][c] & CellOptions.CELL_VISITED) == 0x00) &&
                    ((map[r][c] & CellOptions.CELL_FRONTIER) == 0x00))*/
                if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                   (!EmumMedods.HasFlag(Map[r][c], CellOptions.CellVisited)) &&
                   (!EmumMedods.HasFlag(Map[r][c], CellOptions.CellFfrontier)))
                { //если ячейка в границах лабиринта и
                    //не является посещенной или граничной

                    //то помечаем ее как граничную
                    Map[r][c] |= CellOptions.CellFfrontier;

                    //добавляем ее в список граничных
                    cellList.Add(new CellData(r, c));
                    /*var tmp:Cell=new Cell(r, c);
                    CellList.push(tmp);*/
                }
            }
        }

        //добавление ячейки к пути, в напрравлении dir
        private void AttachCellToTree(int row, int col, int dir)
        {
            int i, r, c, dirOffset, direct;
            RemoveFrontierCell(row, col); //удаление из списка граничных
            Map[row][col] |= CellOptions.CellVisited; //пометка ячейки как посещенной

            if (dir == -1)
            { //если направление не выбрано, то

                //ищем случаное возможное направление
                dirOffset = (int)(rnd.NextDouble() * 4);

                //перебираем возможные направления
                for (i = 0; i < 4; i++)
                {
                    direct = (dirOffset + i) % 4;
                    r = row + dr[direct];
                    c = col + dc[direct];
                    if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                       ((Map[r][c] & CellOptions.CellVisited) != 0x00))
                    { //если не посещено и в пределах лабиринта, то

                        // удаляем стенку между двумя ячейками
                        RemoveWall(row, col, direct);
                        break;
                    }
                }
            }
            else
            {
                //если направление задано, то удаляем стенку в
                //заданном направлении
                RemoveWall(row, col, dir);
            }
        }

        private void CreateBranch(int row, int col)
        {
            int curRow, curCol;
            bool bMoved;
            int i, r, c, dirOffset, dir;

            //начинаем путь в случайном направоении
            AttachCellToTree(row, col, -1);

            //[row, col] перестает быть граничной
            MarkFrontierCells(row, col);
            curRow = row;
            curCol = col;
            do
            {
                bMoved = false; //пометка - движения нового не было

                //выбираем случайное направление
                dirOffset = (int)(rnd.NextDouble() * 4);
                for (i = 0; i < 4; i++)
                {
                    dir = (dirOffset + i) % 4;
                    r = curRow + dr[dir];
                    c = curCol + dc[dir];
                    if (InMazeBorders(r, c, MazeHeight, MazeWidth) &&
                       ((Map[r][c] & CellOptions.CellFfrontier) != 0x00))
                    { //если ячейка в лабиринте и не граничная

                        //добавляем ее в путь и связываем с предыдущей
                        AttachCellToTree(r, c, (dir + 2) % 4);

                        //новую ячеку делаем граничной
                        MarkFrontierCells(r, c);
                        bMoved = true; //пометка, что было движение
                        curRow = r;
                        curCol = c;
                        break; //работа с новой точкой
                    }
                }
            } while (bMoved); //двигаемся пока можем
        }

        //запуск генерации лабиринта
        public void Generate()
        {
            int n, r, c;

            //помечаем весь лабиринт, как полностью без проходов
            for (int i = 0; i < MazeHeight; i++)
                for (int j = 0; j < MazeWidth; j++)
                    Map[i][j] = 0;

            //выбираем случайную ячейку в лабиринте
            r = (int)(rnd.NextDouble() * MazeHeight);
            c = (int)(rnd.NextDouble() * MazeWidth);

            //помечаем данную ячейку как посещенную
            Map[r][c] |= CellOptions.CellVisited;

            //добавляем в список все граничные ячейки
            MarkFrontierCells(r, c);

            while (cellList.Count != 0)
            { //пока есть граничные ячейки выполняем цикл

                //выбираем случаную граничную ячейку из доступного списка
                n = (int)(rnd.NextDouble() * cellList.Count);

                //"прорубаем" проход в данном направлении
                CreateBranch(cellList[n].Row, cellList[n].Col);
            }
        }
    }
}
