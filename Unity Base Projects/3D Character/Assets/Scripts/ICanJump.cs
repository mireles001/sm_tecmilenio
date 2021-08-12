namespace Player
{
    public interface ICanJump
    {
        public void Jump();
        public void Landing(float velocity);
        public string ID { get; }
    }
}
