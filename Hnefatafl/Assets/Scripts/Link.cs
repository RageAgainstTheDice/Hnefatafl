using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour 
{

	public void OpenRuleDoc()
	{
		#if !UNITY_EDITOR
		openWindow("https://docs.google.com/document/d/1iASyKp8qcz-8muJcD3IP0m-AohFgX-8-xQU3TRVB4_M/edit?usp=sharing");
		#endif
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);

}