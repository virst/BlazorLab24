namespace LabGenerator
{
    [Flags]
    public enum CellOptions
    {
        ExitEast = 0x01, //есть проход на восток
        ExitNorth = 0x02, //есть проход на север
        ExitWest = 0x04, //есть проход на запад
        ExitSouth = 0x08, //есть проход на юг
        CellFfrontier = 0x20, //граничная
        CellVisited = 0x10,  // посещенная 
        None = 0x00  // пусто

    }

    public static class EmumMedods
    {
        public static void AddFlag(ref CellOptions e, CellOptions v)
        {
            e |= v;
        }

        public static void RemoveFlag(ref CellOptions e, CellOptions v)
        {
            e &= ~v;
        }

        public static bool HasFlag(CellOptions e, CellOptions v)
        {
            return (e & v) == v;
        }

    }
}
