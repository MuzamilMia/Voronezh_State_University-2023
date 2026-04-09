using System;

namespace HousingManagementSystem
{
    public abstract class ObservableEntity
    {
        public event EventHandler Changed;

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}