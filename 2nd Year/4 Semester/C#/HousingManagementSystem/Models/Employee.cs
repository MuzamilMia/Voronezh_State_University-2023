namespace HousingManagementSystem
{
    public abstract class Employee : ObservableEntity
    {
        public int Id { get; }
        public string Name { get; }
        public string Position { get; }
        // public bool IsAvailable { get; protected set; } = true;
        public bool IsAvailable { get; set; } = true;
        protected Employee(int id, string name, string position)
        {
            Id = id;
            Name = name;
            Position = position;
        }

        public abstract bool CanHandleRequest(Request request);
    }
}