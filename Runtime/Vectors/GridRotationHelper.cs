using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardUtils.Vectors
{
    internal enum GridRotationType { r0, r90, r180, r270 }

    public struct GridRotation
    {
        internal GridRotationType rotation;

        public int ToTurns()
        {
            switch (rotation)
            {
                case GridRotationType.r90:
                    return 1;
                case GridRotationType.r180:
                    return 2;
                case GridRotationType.r270:
                    return 3;
                default:
                    return 0;
            }
        }

        public static GridRotation FromTurns(int turns)
        {
            switch (turns % 4)
            {
                case 0:
                    return r0;
                case 1:
                    return r90;
                case 2:
                    return r180;
                default:
                    return r270;
            }
        }
        public static GridRotation FromDegrees(float degrees)
        {
            float rangedDegrees = (degrees + 360) % 360;
            if (rangedDegrees < 45 || rangedDegrees > 315)
            {
                return r0;
            }
            else if (rangedDegrees < 135)
            {
                return r90;
            }
            else if (rangedDegrees < 225)
            {
                return r180;
            }
            else
            {
                return r270;
            }
        }

        public GridRotation Invert()
        {
            switch (rotation)
            {
                case GridRotationType.r90:
                    return r270;
                case GridRotationType.r270:
                    return r90;
                default:
                    return this;
            }
        }
        public int ToAngle()
        {
            switch (rotation)
            {
                case GridRotationType.r90:
                    return 90;
                case GridRotationType.r180:
                    return 180;
                case GridRotationType.r270:
                    return 270;
                default:
                    return 0;
            }
        }

        #region Constants
        public static GridRotation r90 = new GridRotation() { rotation = GridRotationType.r90 };
        public static GridRotation r180 = new GridRotation() { rotation = GridRotationType.r180 };
        public static GridRotation r270 = new GridRotation() { rotation = GridRotationType.r270 };
        public static GridRotation r0 = new GridRotation() { rotation = GridRotationType.r0 };

        internal GridRotation(GridRotationType rotation)
        {
            this.rotation = rotation;
        }
        #endregion
    }

    public static class GridRotationHelper
    {
        public static int rotationToTurns(GridRotation rotation)
        {
            return (rotation.rotation == GridRotationType.r90 ? 1
                : rotation.rotation == GridRotationType.r180 ? 2
                : rotation.rotation == GridRotationType.r270 ? 3
                : 0);
        }
        public static GridRotation turnsToRotation(int turns)
        {
            return (turns == 1 ? GridRotation.r90
                : turns == 2 ? GridRotation.r180
                : turns == 3 ? GridRotation.r270
                : GridRotation.r0);
        }

        public static int ToAngle(this GridRotation self)
        {
            switch (self.rotation)
            {
                case GridRotationType.r90:
                    return 90;
                case GridRotationType.r180:
                    return 180;
                case GridRotationType.r270:
                    return 270;
                default:
                    return 0;
            }
        }

        public static Vector3Int ToLevelSpace(this Vector3Int localPosition, Vector3Int origin, GridRotation rotation)
        {
            return origin + localPosition.Rotate(rotation);
        }

        public static Vector3Int Rotate(this Vector3Int position, GridRotation rotation)
        {
            Vector3Int newPosition = position;
            
            //TODO: Matrix Math optimization
            int turns = GridRotationHelper.rotationToTurns(rotation);
            if (turns % 2 == 1)
            {
                int mirror = turns == 3 ? -1 : 1;
                newPosition = new Vector3Int
                {
                    x = newPosition.z * mirror,
                    y = newPosition.y,
                    z = newPosition.x * -1 * mirror
                };
            }
            else
            {
                int mirror = turns == 2 ? -1 : 1;
                newPosition = new Vector3Int
                {
                    x = newPosition.x * mirror,
                    y = newPosition.y,
                    z = newPosition.z * mirror
                };
            }

            return newPosition;
        }
    }
}
