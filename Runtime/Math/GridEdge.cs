using System;
using UnityEngine;
using WizardUtils.Vectors;

namespace WizardUtils.Vectors
{
    [Serializable]
    public struct GridEdge
    {
        public Vector3Int Position;
        public Direction Direction;

        public GridEdge(Vector3Int position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        /// <summary>
        /// Returns a new GridEdge from origin towards self<br/><br/>
        /// If a vertical edge, only returns the first of 4 possible solutions (the one with no rotation)
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public GridEdge ToLocal(GridEdge origin)
        {
            GridEdge newEdge = this;

            // find newEdge's absolute position relative to origin
            newEdge.Position -= origin.Position;

            // rotate
            if (origin.Direction.IsVertical)
            {
                return newEdge;
            }
            else
            {
                GridRotation rotation = Direction.forward.GetRotationFrom(origin.Direction);
                newEdge = newEdge.Rotated(rotation);

                return newEdge;
            }
        }
        public GridEdge ToLevelSpace(Vector3Int origin, GridRotation rotation)
        {
            GridEdge newEdge = this;

            newEdge.Position = this.Position.ToLevelSpace(origin, rotation);
            newEdge.Direction = this.Direction.Rotate(rotation);

            return newEdge;
        }

        /// <summary>
        /// the GridEdge that touches this one
        /// </summary>
        /// <returns></returns>
        public GridEdge Sibling
        {
            get
            {
                GridEdge newEdge = this;

                newEdge.Position = this.Position + Direction.Vector;
                newEdge.Direction = this.Direction.Rotate(GridRotation.r180);

                return newEdge;
            }
        }
        public Vector3Int FacingPosition => this.Position + Direction.Vector;

        /// <summary>
        /// Rotate this GridEdge around the origin counter clockwise by specified degree
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GridEdge Rotated(GridRotation rotation)
        {
            GridEdge newEdge = this;

            // Rotate Position
            newEdge.Position = newEdge.Position.Rotate(rotation);

            // Rotate Direction
            newEdge.Direction = newEdge.Direction.Rotate(rotation);

            return newEdge;
        }

        public static GridEdge Zero => new GridEdge(Vector3Int.zero, Direction.forward);

        #region C# functions

        public override string ToString()
        {
            return $"({Position.x}, {Position.y}, {Position.z}) {Direction}";
        }

        public override bool Equals(object obj)
        {
            if (obj is GridEdge) return Equals((GridEdge)obj);
            return false;
        }
        public bool Equals(GridEdge other)
        {
            return Direction.Equals(other.Direction)
                && Position.Equals(other.Position);
        }

        public static bool operator ==(GridEdge a, GridEdge b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(GridEdge a, GridEdge b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return Direction.GetHashCode() * Position.GetHashCode();
        }

        public static GridEdge operator +(GridEdge a, Vector3Int offset)
        {
            return new GridEdge()
            {
                Position = a.Position + offset,
                Direction = a.Direction
            };
        }

        public static GridEdge operator -(GridEdge a, Vector3Int offset)
        {
            return new GridEdge()
            {
                Position = a.Position - offset,
                Direction = a.Direction
            };
        }

        #endregion
    }
}
