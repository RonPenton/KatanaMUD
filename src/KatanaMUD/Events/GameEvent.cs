using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Events
{
    public abstract class GameEvent : IComparable<GameEvent>, IEquatable<GameEvent>
    {
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// The Priority Queue handle of the event, in case it needs to be removed 
        /// </summary>
        public C5.IPriorityQueueHandle<GameEvent> Handle { get; set; }

        public int CompareTo(GameEvent other)
        {
            return ExecutionTime.CompareTo(other.ExecutionTime);
        }

        public bool Equals(GameEvent other)
        {
            return ExecutionTime.Equals(other.ExecutionTime);
        }


        public abstract void Execute();
    }
}
