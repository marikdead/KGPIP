using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    Animator test;
    // Start is called before the first frame update
    void Start()
    {
        test = GetComponent<Animator>();
    }
    public void OnPreRender()
    {
        test.Play("Scan Zone start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
