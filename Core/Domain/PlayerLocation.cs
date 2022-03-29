namespace Core.Domain
{
    public class PlayerLocation : EntityBase
    {
        public int PlayerId;
        public Player? Player;
        public int LocationId;
        public Location? Location;

    }
}
