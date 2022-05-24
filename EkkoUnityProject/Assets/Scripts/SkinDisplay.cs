using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDisplay : MonoBehaviour
{
    private Animator anim;
    private SkinnedMeshRenderer bodyMesh;
    
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        bodyMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetTrigger("ArcaneSkin");
    }
}
