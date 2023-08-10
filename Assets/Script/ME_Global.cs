using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_Global : SJ_Singleton_Mono
{
    static  public  ME_Global g;
	override		public	SJ_Singleton_Mono	OnGetStatic() {return g; }
	override		public	void				OnSetStatic(SJ_Singleton_Mono s) { g = (ME_Global)s; }

    public  string  url;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
