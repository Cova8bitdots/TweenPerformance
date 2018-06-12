using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
	public int totTweens = 500;
	public Transform target;
	public GameObject go;
	public float floatValue;


	void Awake()
	{
		LeanTween.init( totTweens * 2 );
	}

	IEnumerator CO_TestDOCompleteBy(bool byTarget, bool tweenFloatValue)
	{
		Debug.Log(string.Format("{0} :: Create {1} tweens on {2}", Time.realtimeSinceStartup, totTweens,  (tweenFloatValue ? "float" : "transform") ));
		for (int i = 0; i < totTweens; ++i) {
			Tween t = tweenFloatValue
				? DOTween.To(()=> floatValue, x=> floatValue = x, 2, 10)
				: target.DOMoveX(2, 10);
			if (!byTarget) t.SetId("myId");
			else if (tweenFloatValue) t.SetTarget(target);
		}
		yield return new WaitForSeconds(2f);

		Debug.Log(string.Format("{0} :: Complete {1} tweens by {2}", Time.realtimeSinceStartup, totTweens, (byTarget ? "target" : "id") ));

		float time = Time.realtimeSinceStartup;
		if (byTarget) target.DOComplete();
		else DOTween.Complete("myId");
		float elapsed = Time.realtimeSinceStartup - time;

		Debug.Log(string.Format("{0} :: Completed {1} tweens in {2} [ms]", Time.realtimeSinceStartup, totTweens, elapsed*1000.0f ));
	}

	IEnumerator CO_TestLTCompleteBy(bool byTarget, bool tweenFloatValue)
	{
		Debug.Log(string.Format("{0} :: Create {1} tweens on {2}", Time.realtimeSinceStartup, totTweens,  (tweenFloatValue ? "float" : "transform") ));
		for (int i = 0; i < totTweens; ++i)
		{
			LTDescr lt = tweenFloatValue ? LeanTween.value( floatValue, 2.0f, 10.0f) : LeanTween.moveLocalX( go , 2.0f, 10.0f );

			if (!byTarget) lt.setId((uint)i, 0);
		}
		yield return new WaitForSeconds(2f);

		Debug.Log(string.Format("{0} :: Complete {1} tweens by {2}", Time.realtimeSinceStartup, totTweens, (byTarget ? "target" : "id") ));

		float time = Time.realtimeSinceStartup;
		LeanTween.cancelAll();

		float elapsed = Time.realtimeSinceStartup - time;
		Debug.Log(string.Format("{0} :: Completed {1} tweens in {2} [ms]", Time.realtimeSinceStartup, totTweens, elapsed*1000.0f ));
	}

	public void TestDOCompleteBy_Transform(bool byTarget)
	{
		this.StartCoroutine(CO_TestDOCompleteBy(byTarget, false));
	}

	public void TestDOCompleteBy_Float(bool byTarget)
	{
		this.StartCoroutine(CO_TestDOCompleteBy(byTarget, true));
	}

	public void TestLTCompleteBy_Transform(bool byTarget)
	{
		this.StartCoroutine(CO_TestLTCompleteBy(byTarget, false));
	}

	public void TestLTCompleteBy_Float(bool byTarget)
	{
		this.StartCoroutine(CO_TestLTCompleteBy(byTarget, true));
	}
}


[CustomEditor(typeof(TestScript))]
public class TestScriptEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var test = target as TestScript ;

		if(	GUILayout.Button("DOTween Trans") )
		{
			test.TestDOCompleteBy_Transform( true);
		}
		if(	GUILayout.Button("DOTween float") )
		{
			test.TestDOCompleteBy_Transform( true);
		}

		if(	GUILayout.Button("LeanTween Trans") )
		{
			test.TestLTCompleteBy_Transform( true);
		}

		if(	GUILayout.Button("LeanTween float") )
		{
			test.TestLTCompleteBy_Float( true);
		}


	}
}