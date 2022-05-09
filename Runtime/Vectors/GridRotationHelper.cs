using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WizardUtils.Vectors
{
    public enum GridRotationType { r0, r90, r180, r270 }

    public struct GridRotation
    {
        public GridRotationType rotation;

        public int ToTurns()
        {
            return (rotation == GridRotationType.r90 ? 1
                : rotation == GridRotationType.r180 ? 2
                : rotation == GridRotationType.r270 ? 3
                : 0);
        }

        public static GridRotation FromTurns(int turns)
        {
            return (turns == 1 ? GridRotation.r90
                : turns == 2 ? GridRotation.r180
                : turns == 3 ? GridRotation.r270
                : GridRotation.r0);
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
        #endregion
    }

    public static class GridRotationHelper
    {
        public static int rotationToTurns(GridRotationType rotation)
        {
            return (rotation == GridRotationType.r90 ? 1
                : rotation == GridRotationType.r180 ? 2
                : rotation == GridRotationType.r270 ? 3
                : 0);
        }
        public static GridRotationType turnsToRotation(int turns)
        {
            return (turns == 1 ? GridRotationType.r90
                : turns == 2 ? GridRotationType.r180
                : turns == 3 ? GridRotationType.r270
                : GridRotationType.r0);
        }

        public static int ToAngle(this GridRotationType self)
        {
            switch (self)
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

        public static Vector3Int ToLevelSpace(this Vector3Int localPosition, Vector3Int origin, GridRotationType rotation)
        {
            return origin + localPosition.Rotate(rotation);
        }

        public static Vector3Int Rotate(this Vector3Int position, GridRotationType rotation)
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
