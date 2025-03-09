using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StudentHistory.Scripts.Editor
{
    public static class MaterialOptimizerTool
    {
        [MenuItem("Tools/MaterialOptimizer/Optimization")]

        public static void OptimizationMaterials()
        {
            var counter = 0;
            var materials = FindAssetsByType<Material>();

            foreach (var material in materials.Where(material => !material.enableInstancing))
            {
                material.enableInstancing = true;
                ++counter;
            }

            Debug.Log($"{counter} has been optimized");
        }

        private static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets("t:Material");
            
            for(int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
                
                if( asset != null ) 
                    assets.Add(asset);
            }
            return assets;
        }
    }
}

