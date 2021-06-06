namespace Player
{
    public interface ICanJump
    {
        public void Jump();
        public void Landing();
        public string ID { get; }
    }
}
