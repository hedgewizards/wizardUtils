using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils
{
    public class DescriptorManifest<T> : DescriptorManifest, IDescriptorManifest<T>
        where T : ManifestedDescriptor
    {
        public List<T> Items;
#if UNITY_EDITOR
        private static DescriptorManifest<T> Editor_GlobalManifest; 
#endif

        void Reset()
        {
            Items ??= new List<T>();
        }

        public override void Add(object descriptor) => Add((T)descriptor);

        public override bool Contains(object descriptor) => Contains((T)descriptor);

        public override void Remove(object descriptor) => Remove((T)descriptor);

        public void Add(T descriptor)
        {
            Items.Add(descriptor);
        }

        public bool Contains(T descriptor)
        {
            return Items.Contains(descriptor);
        }

        public void Remove(T descriptor)
        {
            Items.Remove(descriptor);
        }

        public T FindByKey(string key)
        {
            return Items
                .Where(i => i.GetKey().Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
        }

        public bool TryFindByKey(string key, out T item)
        {
            item = FindByKey(key);

            return item != null;
        }

        public bool TryGet(ushort id, out T item)
        {
            if (id < 0 || id >= Items.Count)
            {
                item = default;
                return false;
            }
            item = Items[id];
            return true;
        }


        #region Editor Access
#if UNITY_EDITOR


        public static bool TryGetGlobalManifest<TManifest>(out TManifest result)
            where TManifest : DescriptorManifest<T>
        {
            bool success = TryGetGlobalManifest(out DescriptorManifest<T> _result);
            result = _result as TManifest;
            return success;
        }


        public static bool TryGetGlobalManifest(out DescriptorManifest<T> result)
        {
            if (Editor_GlobalManifest == null)
            {
                Editor_LoadGlobalManifest();
                if (Editor_GlobalManifest == null)
                {
                    result = default;
                    return false;
                }
            }

            result = Editor_GlobalManifest;
            return true;
        }

        public static bool Editor_TryGetByKeyInGlobalManifest(string key, out T result)
        {
            if (!TryGetGlobalManifest(out var manifest))
            {
                result = default;
                return false;
            }

            return manifest.TryFindByKey(key, out result);
        }

        private static void Editor_LoadGlobalManifest()
        {
            var assetGuids = AssetDatabase.FindAssets($"t:{nameof(DescriptorManifest)}");
            var assetPaths = assetGuids.Select(id => AssetDatabase.GUIDToAssetPath(id));
            var assets = assetPaths.Select(path => AssetDatabase.LoadAssetAtPath<ScriptableObject>(path));
            Editor_GlobalManifest = assets.OfType<DescriptorManifest<T>>()
                .Where(m => m.UseAsDefaultManifestInEditor)
                .FirstOrDefault();

            if (Editor_GlobalManifest == null)
            {
                Debug.LogWarning($"Missing GlobalManifest for type {nameof(T)}! something is probably about to explode.");
            }
        }
#endif
        #endregion
    }
}
