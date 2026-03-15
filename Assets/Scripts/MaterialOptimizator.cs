using UnityEngine;

public class MaterialOptimizator : MonoBehaviour
{
    private static readonly int ColorID = Shader.PropertyToID("_Color");

    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Material _simpleMaterial;
    
    private Color[] _colors;
    private Material[] _sharedMaterials;
    private int _materialsCount;

    private void Start()
    {
        _materialsCount = _skinnedMeshRenderer.materials.Length;
        _colors = new Color[_materialsCount];
        _sharedMaterials = new Material[_materialsCount];
        
        for (var i = 0; i < _materialsCount; i++)
        {
            _colors[i] = _skinnedMeshRenderer.materials[i].color;
            _sharedMaterials[i] = _simpleMaterial;
        }
        
        _skinnedMeshRenderer.sharedMaterials = _sharedMaterials;
        
        for (var i = 0; i < _materialsCount; i++)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor(ColorID, _colors[i]);
            _skinnedMeshRenderer.SetPropertyBlock(block, i);
        }
    }
}