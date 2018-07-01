using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using MTUnity.Actions;

namespace MTUnity.Utils
{
	static class ThreadSafeRandom
	{
		[ThreadStatic]
		private static System.Random Local;
	
		public static System.Random ThisThreadsRandom {
			get { return Local ?? (Local = new System.Random (unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
		}
	}

	public static class MTExtension
	{
		public static void RandomShuffle<T> (this IList<T> list)
		{
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = ThreadSafeRandom.ThisThreadsRandom.Next (n + 1);
				T value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}

		static int _flag = 0;
		public static int GetRandomInt()
		{
			_flag++;
			if(_flag == int.MaxValue)
			{
				_flag = int.MinValue;
			}

			return ThreadSafeRandom.ThisThreadsRandom.Next (_flag);
		}

		public static void SetSortingLayer(this MonoBehaviour target, string sName)
		{
			if(target!= null && target.gameObject != null)
			{
				target.gameObject.SetSortingLayer(sName);
			}
		}

		public static void  SetSortingLayer(this GameObject target, string sName)
		{
			if(target == null)
			{
				return;
			}
			

			SpriteRenderer spRender = target.gameObject.GetComponentInChildren<SpriteRenderer>();
			if(spRender!= null)
			{
				spRender.sortingLayerName = sName;
			}
		}

	

		

		


		public static void PlayParticle (this GameObject target)
		{
			if(target == null)
			{
				return;
			}
			var anim = target.GetComponent<ParticleSystem> ();
			if (anim != null) {
//				anim.Stop();
//				anim.Clear();
//				anim.Simulate(0.02f);
				anim.Play();
				var em = anim.emission;
				em.enabled = true;
			} 

			int childCount = target.transform.childCount;
			for (int i = 0; i < childCount; ++i) {
				target.transform.GetChild (i).gameObject.PlayParticle();
			}

		}

		public static void StopParticle(this GameObject target)
		{
			if(target == null)
			{
				return;
			}
			var anim = target.GetComponent<ParticleSystem> ();
			if (anim != null) {
				var em = anim.emission;
				em.enabled = false;
				anim.Stop();
			} 
			
			int childCount = target.transform.childCount;
			for (int i = 0; i < childCount; ++i) {
				target.transform.GetChild (i).gameObject.StopParticle();
			}
		}

		/// <summary>
		/// 设置 GameObject 的 Alpha 值，取值 0 ～ 1。
		/// </summary>
		public static void SetAlpha (this GameObject go, float alpha)
		{
			SetGameObjectAlpha (go, alpha);
		}

		static void SetGameObjectAlpha (GameObject go, float alpha, bool ignoreCanvasGroup = false)
		{
			if (go == null) {
				return;
			}
			
			do {
				
				SpriteRenderer sprRenderer = go.GetComponent<SpriteRenderer> ();
				if (sprRenderer != null) {
					Color color = sprRenderer.color;
					color.a = alpha;
					sprRenderer.color = color;
					break;
				}

				TextMesh textMesh = go.GetComponent<TextMesh> ();
				if (textMesh != null) {
					Color color = textMesh.color;
					color.a = alpha;
					textMesh.color = color;
					break;
				}
			} while (false);
			
			if (!ignoreCanvasGroup) {
				CanvasGroup canvasGroup = go.GetComponent<CanvasGroup> ();
				if (canvasGroup != null) {
					canvasGroup.alpha = alpha;
					// 设置一次就可以了
					ignoreCanvasGroup = true;
				}
			}
			
			for (int i = 0, n = go.transform.childCount; i < n; i++) {
				SetGameObjectAlpha (go.transform.GetChild (i).gameObject, alpha, ignoreCanvasGroup);
			}
		}

	}
}