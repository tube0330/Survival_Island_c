using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] Texture[] textures;
    [SerializeField] MeshRenderer meshes;
    [SerializeField] Transform tr;
    [SerializeField] GameObject ExplosionEff;
    [SerializeField] int hitCnt = 0;

    void Start()
    {
        meshes = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshes.material.mainTexture = textures[Random.Range(0, textures.Length)];
        tr = transform;
        ExplosionEff = Resources.Load<GameObject>("Eff/BigExplosionEffect");
    }
}
