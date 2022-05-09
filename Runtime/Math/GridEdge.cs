using System;
using UnityEngine;
using WizardUtils.Vectors;

namespace WizardUtils
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
        /// Returns a new GridEdge local to Origin
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public GridEdge ToLocal(GridEdge origin)
        {
            GridEdge newEdge = this;

            // find newEdge's absolute position relative to origin
            newEdge.Position -= origin.Position;
            GridEdge translatedEdge = newEdge;

            // rotate
            GridRotationType rotation = Direction.forward.GetRotationFrom(origin.Direction);
            GridEdge rotatedEdge = newEdge.Rotated(rotation);
            newEdge = newEdge.Rotated(rotation);
            
            return newEdge;
        }
        public GridEdge ToLevelSpace(Vector3Int origin, GridRotationType rotation)
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
                newEdge.Direction = this.Direction.Rotate(GridRotationType.r180);

                return newEdge;
            }
        }
        
        /// <summary>
        /// Rotate this GridEdge around the origin counter clockwise by specified degree
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GridEdge Rotated(GridRotationType rotation)
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

        #endregion
    }
}
