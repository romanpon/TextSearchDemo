namespace TextSearchDemo
{
    public static class Weights
    {
        private readonly static Dictionary<string, int> weights = new Dictionary<string, int> {
            { "Building.ShortCut", 7 },
            { "Building.Name", 9},
            { "Building.Description", 5 },
            { "Lock.Building.Name", 8},
            { "Lock.Building.ShortCut", 5},
            { "Lock.Building.Description", 0},
            { "Lock.Type", 3},
            { "Lock.Name", 10},
            { "Lock.SerialNumber", 8 },
            { "Lock.Floor", 6},
            { "Lock.RoomNumber", 6 },
            { "Lock.Description", 6 },
            { "Group.Name", 9},
            { "Group.Description", 5 },
            { "Medium.Group.Name", 8 },
            { "Medium.Group.Description", 0 },
            { "Medium.Type", 3 },
            { "Medium.Owner", 10 },
            { "Medium.SerialNumber", 8 },
            { "Medium.Description", 6 }
        };

        public static int GetWeight(string propertyName)
        {
            if (weights.TryGetValue(propertyName, out int weight))
            {
                return weight;
            }
            else
            {
                return -1;
            }
        }
    }
}
