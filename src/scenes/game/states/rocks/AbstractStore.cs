namespace Game.State
{
    public abstract class AbstractStore<T>
    {
        public T value;

        public delegate void UpdateEventHandler(T next);
        public delegate T StateUpdate(T next);
        public event UpdateEventHandler OnUpdate;

        public virtual void Update(StateUpdate handler)
        {
            value = handler.Invoke(value);
            OnUpdate(value);
        }
    }
}

