namespace DoubleDCore.Community
{
    public class LeaderboardMember
    {
        public int ID { get; }
        public string Name { get; }
        public string AvatarLink { get; }
        public int Position { get; }
        public string Value { get; }

        public LeaderboardMember(int id, string name, string avatarLink, int position, string value)
        {
            ID = id;
            Name = name;
            AvatarLink = avatarLink;
            Position = position;
            Value = value;
        }
    }
}