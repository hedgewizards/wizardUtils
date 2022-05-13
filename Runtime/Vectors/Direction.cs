﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Vectors
{
    [Serializable]
    public struct Direction : IEquatable<Direction>
    {
        public enum Directions
        {
            zero,
            up,
            down,
            left,
            right,
            forward,
            back
        }

        public Directions DirectionType;

        public Vector3Int Vector
        {
            get => DirectionType switch
            {
                Directions.zero => Vector3Int.zero,
                Directions.up => Vector3Int.up,
                Directions.down => Vector3Int.down,
                Directions.left => Vector3Int.left,
                Directions.right => Vector3Int.right,
                Directions.forward => Vector3Int.forward,
                Directions.back => Vector3Int.back,
            };
            set
            {
                if (value == Vector3Int.up)
                    DirectionType = Directions.up;
                else if (value == Vector3Int.down)
                    DirectionType = Directions.down;
                else if (value == Vector3Int.left)
                    DirectionType = Directions.left;
                else if (value == Vector3Int.right)
                    DirectionType = Directions.right;
                else if (value == Vector3Int.forward)
                    DirectionType = Directions.forward;
                else if (value == Vector3Int.back)
                    DirectionType = Directions.back;
                else if (value == Vector3Int.zero)
                    DirectionType = Directions.zero;
                else
                    throw new ArgumentOutOfRangeException($"{value} is not a direction");
            }
        }
        public int x => Vector.x;
        public int y => Vector.y;
        public int z => Vector.z;

        /// <summary>
        /// The direction 90 degrees left of our current direction
        /// </summary>
        public Direction RotateLeft()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return Direction.left;
                case Directions.down:
                    return Direction.left;
                case Directions.left:
                    return Direction.back;
                case Directions.right:
                    return Direction.forward;
                case Directions.forward:
                    return Direction.left;
                case Directions.back:
                    return Direction.right;
                default:
                    return this;
            }
        }

        /// <summary>
        /// The direction 90 degrees left of our current direction
        /// </summary>
        public Direction RotateRight()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return Direction.right;
                case Directions.down:
                    return Direction.right;
                case Directions.left:
                    return Direction.forward;
                case Directions.right:
                    return Direction.back;
                case Directions.forward:
                    return Direction.right;
                case Directions.back:
                    return Direction.left;
                default:
                    return this;
            }
        }
        /// <summary>
        /// The direction 90 degrees left of our current direction
        /// </summary>
        public Direction RotateUp()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return Direction.back;
                case Directions.down:
                    return Direction.forward;
                case Directions.left:
                    return Direction.up;
                case Directions.right:
                    return Direction.up;
                case Directions.forward:
                    return Direction.up;
                case Directions.back:
                    return Direction.up;
                default:
                    return this;
            }
        }

        /// <summary>
        /// The direction 90 degrees left of our current direction
        /// </summary>
        public Direction RotateDown()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return Direction.forward;
                case Directions.down:
                    return Direction.back;
                case Directions.left:
                    return Direction.down;
                case Directions.right:
                    return Direction.down;
                case Directions.forward:
                    return Direction.down;
                case Directions.back:
                    return Direction.down;
                default:
                    return this;
            }
        }

        public Direction(Vector3Int vector)
        {
            DirectionType = Directions.zero;
            Vector = vector;
        }

        public Direction(Vector3 fuzzyDirection)
        {
            DirectionType = Directions.zero;
            Vector = new Vector3Int
            {
                x = Mathf.RoundToInt(fuzzyDirection.x),
                y = Mathf.RoundToInt(fuzzyDirection.y),
                z = Mathf.RoundToInt(fuzzyDirection.z),
            };
        }

        public Direction (Directions direction)
        {
            DirectionType = direction;
        }

        public static bool IsValidVector(Vector3Int vector)
        {
            return vector.magnitude == 1;
        }

        /// <summary>
        /// Rotate this GridEdge around the origin counter clockwise by specified degree
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Direction Rotate(GridRotationType rotation)
        {
            if (this == Direction.up || this == Direction.down)
            {
                return this;
            }

            // Convert our initial direction to 0-4 turns from forward
            // add to that 0-4 turns
            int turns = (toTurns() + GridRotationHelper.rotationToTurns(rotation)) % 4;

            // convert back to direction
            return fromTurns(turns);
        }

        public GridRotationType GetRotationFrom(Direction origin)
        {
            return GridRotationHelper.turnsToRotation(this.countTurns(origin));
        }
        int countTurns(Direction origin)
        {
            if (this == Direction.up || this == Direction.down
                || origin == Direction.up || origin == Direction.down)
            {
                throw new ArgumentOutOfRangeException();
            }

            int turns = this.toTurns() - origin.toTurns();

            return (turns + 8) % 4;
        }

        int toTurns()
        {
            return (this == Direction.right) ? 1
                : (this == Direction.back) ? 2
                : (this == Direction.left) ? 3
                : 0;
        }
        Direction fromTurns(int turns)
        {
            return (turns == 1 ? Direction.right
                : turns == 2 ? Direction.back
                : turns == 3 ? Direction.left
                : Direction.forward);
        }

        const float oneOverRoot2 = 0.707107f;
        public Quaternion QuaternionFromForward()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return new Quaternion(-oneOverRoot2, 0, 0, oneOverRoot2);
                case Directions.down:
                    return new Quaternion(oneOverRoot2, 0, 0, oneOverRoot2);
                case Directions.left:
                    return new Quaternion(0, -oneOverRoot2, 0, oneOverRoot2);
                case Directions.right:
                    return new Quaternion(0, oneOverRoot2, 0, oneOverRoot2);
                case Directions.back:
                    return new Quaternion(0, -1, 0, 0);
                default:
                    return Quaternion.identity;
            }
        }
        #region Constants
        public static Direction up { get => new Direction(Directions.up); }
        public static Direction down { get => new Direction(Directions.down); }
        public static Direction left { get => new Direction(Directions.left); }
        public static Direction right { get => new Direction(Directions.right); }
        public static Direction forward { get => new Direction(Directions.forward); }
        public static Direction back { get => new Direction(Directions.back); }

        public static string[] names => new string[]
        {
            nameof(up),
            nameof(down),
            nameof(left),
            nameof(right),
            nameof(forward),
            nameof(back)
        };
        public static Direction[] all => new Direction[] { up, down, left, right, forward, back };
        public static Vector3Int[] allVectors => new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };
        #endregion

        #region C# Functions

        public override int GetHashCode()
        {
            return Vector.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Direction) return Equals((Direction)obj);
            return false;
        }
        public bool Equals(Direction other)
        {
            return Vector.Equals(other.Vector);
        }
        public static bool operator ==(Direction a, Direction b)
        {
            return a.Vector.Equals(b.Vector);
        }
        public static bool operator !=(Direction a, Direction b)
        {
            return !a.Vector.Equals(b.Vector);
        }
        public override string ToString()
        {
            switch (DirectionType)
            {
                case Directions.up:
                    return "Up";
                case Directions.down:
                    return "Down";
                case Directions.left:
                    return "Left";
                case Directions.right:
                    return "Right";
                case Directions.back:
                    return "Back";
                case Directions.forward:
                    return "Forward";
                default:
                    return "Invalid";
            }
        }

        #endregion
    }
}
