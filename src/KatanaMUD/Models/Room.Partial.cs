using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Room
    {
        public Exit GetExit(Direction direction)
        {
            var exit = new Exit() { Direction = direction };
            exit.ExitRoom = _exits[(int)direction](this);
            //TODO:
            // if(exit.ExitRoom == null) {
            //    exit.Portal = this.Portals.SingleOrDefault(x => x.Direction == (int)direction);
            //}
            return exit;
        }

        public IEnumerable<Exit> GetExits()
        {
            return Directions.Enumerate().Select(x => GetExit(x)).Where(y => y.IsValid()).ToList();
        }

        private static Func<Room, int?>[] _exits;

        static Room()
        {
            _exits = new Func<Room, int?>[10];
            _exits[(int)Direction.North] = r => r.NorthExit;
            _exits[(int)Direction.South] = r => r.SouthExit;
            _exits[(int)Direction.East] = r => r.EastExit;
            _exits[(int)Direction.West] = r => r.WestExit;
            _exits[(int)Direction.Northeast] = r => r.NorthEastExit;
            _exits[(int)Direction.Northwest] = r => r.NorthWestExit;
            _exits[(int)Direction.Southeast] = r => r.SouthEastExit;
            _exits[(int)Direction.Southwest] = r => r.SouthWestExit;
            _exits[(int)Direction.Up] = r => r.UpExit;
            _exits[(int)Direction.Down] = r => r.DownExit;
        }

        public IEnumerable<Actor> VisibleActors(Actor actor)
        {
            //TODO: Visibility
            return Actors.Where(x => !x.InPurgatory).ToList();
        }

        public IEnumerable<Actor> ActiveActors => Actors.Where(x => !x.InPurgatory).ToList();
    }

    /// <summary>
    /// Defines a single exit for a room. It is a transitory class, not persisted in the database, 
    /// meant simply for easier and abstract processing of exit data. 
    /// ExitRoom refers to the other room in this direction, if there is a regular exit.
    /// Portal refers to the portal if the exit is a portal exit. 
    /// These two values are exclusive; if ExitRoom is specified then Portal should not be.
    /// If Portal is specified then ExitRoom should not be. The portal will have its own method of
    /// Calculating the exit room.
    /// </summary>
    public class Exit
    {
        public Direction Direction { get; set; }
        public int? ExitRoom { get; set; }
        public Portal Portal { get; set; }

        internal bool IsValid()
        {
            return ExitRoom != null || Portal != null;
        }
    }

    public class Portal
    {
        // TODO: Remove. Simply a placeholder for now.
        internal string GetName(Room room)
        {
            return "PORTAL";
        }
    }

    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
        Northeast = 4,
        Northwest = 5,
        Southeast = 6,
        Southwest = 7,
        Up = 8,
        Down = 9
    }

    public static class Directions
    {
        public static IEnumerable<Direction> Enumerate()
        {
            for (var i = (int)Direction.North; i <= (int)Direction.Down; i++)
            {
                yield return (Direction)i;
            }
        }

        public static Direction Opposite(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Direction.South;
                case Direction.South: return Direction.North;
                case Direction.East: return Direction.West;
                case Direction.West: return Direction.East;
                case Direction.Northeast: return Direction.Southwest;
                case Direction.Southwest: return Direction.Northeast;
                case Direction.Northwest: return Direction.Southeast;
                case Direction.Southeast: return Direction.Northwest;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
            }

            throw new InvalidOperationException("Invalid Direction");
        }
    }
}