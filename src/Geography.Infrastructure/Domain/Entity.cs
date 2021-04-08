using System;

namespace Geography.Infrastructure.Domain
{
    public abstract class Entity { }

    public abstract class Entity<TId> : Entity
    {

        public TId Id { get; private set; }
        protected Entity() { }
        protected Entity(TId id)
        {
            this.Id = id;
        }
        public override bool Equals(object obj)
        {
            return obj is Entity<TId> other && other.GetType() == this.GetType() && Object.Equals(other.Id, this.Id);
        }

        public override int GetHashCode()
        {
            return this.Id != null ? this.Id.GetHashCode() : 0;
        }

    }
}
