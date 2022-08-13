namespace UtopiaBackendChallenge.Lib
{
    public class RMCharacter
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? status { get; set; }
        public string? species { get; set; }
        public string? type { get; set; }
        public string? gender { get; set; }
        public RMOrigin origin { get; set; }
        public RMLocation location { get; set; }
        public string? image { get; set; }
        public List<string> episode { get; set; }
        public string? url { get; set; }
        public string? created { get; set; }

        public RMCharacter()
        {
            origin = new RMOrigin();
            location = new RMLocation();
            episode = new List<string>();
        }
    }

    public class RMOrigin
    {
        public string? name { get; set; }
        public string? url { get; set; }
    }

    public class RMLocation
    {
        public string? name { get; set; }
        public string? url { get; set; }
    }

    public class RMInfo
    {
        public int count { get; set; }
        public int pages { get; set; }
        public string? next { get; set; }
        public string? prev { get; set; }
    }

    public class RMResults<T>
    {
        public RMInfo info { get; set; }
        public List<T> results { get; set; }

        public RMResults()
        {
            results = new List<T>();
            info = new RMInfo();
        }
    }
}