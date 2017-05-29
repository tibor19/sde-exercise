namespace AspNetCoreOAuth2Sample.Model
{
    public class Rule
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; }
        public string Script { get; set; }
        public string Stage { get; set; }
    }
}