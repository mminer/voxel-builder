using UnityEngine;

public class Constants : MonoBehaviour
{
	public Block _block;
	public static Block block => instance._block;

	public Transform _placementGuide;
	public static Transform placementGuide => instance._placementGuide;

	static Constants instance;
	
	void Awake()
	{
		instance = this;
	}
}
