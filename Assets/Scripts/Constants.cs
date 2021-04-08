using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
	public Block _block;
	public static Block block {
		get { return instance._block; }	
	}
	
	public Transform _placementGuide;
	public static Transform placementGuide {
		get { return instance._placementGuide; }	
	}
	
	static Constants instance;
	
	void Awake ()
	{
		instance = this;
	}
}
