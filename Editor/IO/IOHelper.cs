using System;
using System.IO;
using UnityEditor;

namespace WizardUtils.IO
{
    public static class IOHelper
    {
        /// <summary>
        /// Returns the Project Window's currently selected folder, or the folder the currently selected file is in
        /// </summary>
        /// <param name="defaultPath">What to return if we didn't select anything</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetActiveDirectory(string defaultPath = "Assets")
        {
            // Get the selected object in the Project window
            UnityEngine.Object obj = Selection.activeObject;
            if (obj == null) return "Assets"; // Default to root if nothing is selected

            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) throw new InvalidOperationException();

            // Ensure it's a folder
            if (AssetDatabase.IsValidFolder(path))
            {
                return path;
            }

            // If it's a file, return its parent directory
            return Path.GetDirectoryName(path);
        }
    }
}
