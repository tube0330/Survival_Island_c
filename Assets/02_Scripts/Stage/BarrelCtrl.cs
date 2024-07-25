using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] Texture[] textures;
    [SerializeField] MeshRenderer meshes;
    [SerializeField] Transform tr;
    [SerializeField] GameObject ExplosionEff;
    [SerializeField] Rigidbody rb;
    [SerializeField] int hitCnt = 0;

    void Start()
    {
        meshes = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshes.material.mainTexture = textures[Random.Range(0, textures.Length)];
        tr = transform;
        rb = GetComponent<Rigidbody>();
        ExplosionEff = Resources.Load<GameObject>("Eff/BigExplosionEffect");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("BULLET"))
        {
            Debug.Log("닿음");

            ContactPoint contact = col.contacts[0]; // 첫 번째 충돌 지점 정보 가져오기
            Damage(contact.point, contact.normal); // 충돌 지점과 법선 전달
        }
    }

    void Damage(Vector3 hitPos, Vector3 hitNormal)
    {
        Vector3 firePos = transform.position; // 총알이 충돌한 오브젝트의 위치를 가져와서 사용할 수도 있습니다.
        Vector3 incomeVector = firePos - hitPos; // 방향 벡터 계산
        rb.AddForceAtPosition(incomeVector * 1500f, hitPos);

        if (++hitCnt == 5)
            ExplosionBarrel();
    }

    void ExplosionBarrel()
    {
        GameObject eff = Instantiate(ExplosionEff, tr.position, Quaternion.identity);
        Destroy(eff, 2.0f);

        Collider[] cols = Physics.OverlapSphere(tr.position, 20f, 1 << 9);
        foreach (Collider col in cols)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.mass = 1.0f;
                rb.AddExplosionForce(1000, tr.position, 20f, 1200f);
            }
        }
    }
}
