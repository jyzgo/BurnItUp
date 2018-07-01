using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MTUnity.Actions;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class ScrollRectWrapper : ScrollRect
	{
        private bool _disabled = false;
		private int touchNum = 0;
		private int touchIdx = 0;
		private int[] touchData = new int[10];
		private PointerEventData[] lastEventData = new PointerEventData[10];
        public bool disabled
        {
            set{ _disabled = value; }
        }
		ScrollRectWrapper()
		{
			for (int i = 0; i < 10; i++)
			{
				touchData[0] = 0;
			}
		}
		public override void OnBeginDrag (PointerEventData eventData)  
		{  
			#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
				base.OnBeginDrag(eventData);
				return;
			#endif
			if (touchNum == 0)
			{
				touchIdx = 0;
				base.OnBeginDrag(eventData);
			}
			touchNum++; 
			if (eventData.pointerId >= 0 && eventData.pointerId < 10)
			{
				touchData[eventData.pointerId] = 1;
			}
		} 

		public override void OnDrag (PointerEventData eventData)  
		{  
			#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
			base.OnDrag(eventData);
			return;
			#endif
			//Debug.Log("@@@@: " + eventData.pointerId.ToString());
			if (eventData.pointerId >= 0 && eventData.pointerId < 10)
				lastEventData[eventData.pointerId] = eventData;
			if(eventData.pointerId != touchIdx)
				return;

//			string logstr = "@@@@: ";
//			for (int i = 0; i < 10; i++)
//			{
//				logstr += touchData[i].ToString();
//			}
//			logstr += "  ";
//			logstr += touchIdx.ToString();
//			Debug.Log(logstr);

			base.OnDrag(eventData);  
		} 

        public override void OnEndDrag (PointerEventData eventData)  
		{  
			#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
			base.OnEndDrag(eventData);
			return;
			#endif
			if (eventData.pointerId >= 0 && eventData.pointerId < 10)
			{
				touchData[eventData.pointerId] = 0;
			}
			bool stillDrag = false;
			int lastIdx = touchIdx;
			if (touchIdx == eventData.pointerId)
			{
				for (int i = 0; i < 10; i++)
				{
					if (touchData[i] == 1)
					{
						stillDrag = true;
						lastIdx = touchIdx;
						touchIdx = i;
						break;
					}
				}
			}
			if (stillDrag)
			{
				if (lastEventData[lastIdx] != null)
					base.OnEndDrag(lastEventData[lastIdx]);
				if (lastEventData[touchIdx] != null)
					base.OnBeginDrag(lastEventData[touchIdx]);
			}
			else
			{
				base.OnEndDrag(eventData);
			}
			lastEventData[lastIdx] = null;

			touchNum--;  

//			string logstr = "@@@@: ";
//			for (int i = 0; i < 10; i++)
//			{
//				logstr += touchData[i].ToString();
//			}
//			Debug.Log("@@@@ OnEndDrag: " + eventData.pointerId.ToString() + " " + touchIdx.ToString() + " "
//				+ touchNum.ToString() + " " + logstr);
        } 
	}
}