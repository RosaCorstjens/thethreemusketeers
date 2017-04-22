using UnityEngine;
using System.Collections;

public class MinimapOnOffC : MonoBehaviour {
	
	public GameObject minimapCam;
	//private bool  state = true;
	
	void  Update (){
		if(Input.GetKeyDown("m") && minimapCam){
			OnOffCamera();
		}
		
	}

	void  OnOffCamera (){
		if(minimapCam.active == true){
			minimapCam.active = false;
		}else{
			minimapCam.active = true;
		}
	}
}