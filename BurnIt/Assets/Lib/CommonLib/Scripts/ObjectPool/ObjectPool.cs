using System;
using System.Collections.Generic;
using UnityEngine;
using MTUnity;
using System.Collections;
using MTUnity.Utils;
using System.IO;


public class ObjectPool : Singleton<ObjectPool> {

    /// <summary>
    /// <预设的Resources路径, Resource通过key创建的对象（用于复制新的对象）>，由CheckAndAddToPrefabDict方法实现
    /// </summary>
	Dictionary<string,GameObject> _prefabDict;
    /// <summary>
    /// <预设的Resources路径, 通过_prefabDict复制的缓存对象集合>，来源：IPreLoad预加载
    /// </summary>
	Dictionary<string,HashSet<GameObject>> _poolDict;
    /// <summary>
    /// The path dict.
    /// </summary>
	Dictionary<int,string> _pathDict;
    /// <summary>
    /// 记录的当前加载的对象的数量
    /// </summary>
	Dictionary<string,int> _prefabNum;
	

	void Awake()
	{
		_prefabDict = new Dictionary<string, GameObject>(); // path vs prefab
		_pathDict   = new Dictionary<int, string>();        //instanceid vs path
		_poolDict   = new Dictionary<string, HashSet<GameObject>>(); //path vs unused gameObject
		_prefabNum = new Dictionary<string, int>();
	}

    /// <summary>
    /// 预加载Resource下的Path路径对象count个，
    /// 缓存在_poolDict中，由DoLoad完成
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="count">Count.</param>
	public static void Preload(string path,int count)
	{
		var curInstance = ObjectPool.Instance;
		if(curInstance != null)
		{
			curInstance.IPreload(path,count);
		}
	}

    /// <summary>
    /// 预加载Resource下的Path路径对象count个，
    /// 换存在_poolDict中，由DoLoad完成
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="count">Count.</param>
	void IPreload(string path,int count)
	{
		GameObject curPrefab = CheckAndAddToPrefabDict(path);

		HashSet<GameObject> curSet = null;
		if(!_poolDict.TryGetValue(path,out curSet))
		{
			curSet = new HashSet<GameObject>();
			_poolDict.Add(path,curSet);
			DoLoad(curPrefab,count,path);
		}else
		{
#if UNITY_EDITOR
			Debug.LogWarningFormat ("ObjectPool::IPreload: <{0}> has been preloaded.", path);
#endif
		}


	}

    /// <summary>
    /// 遍历当前缓存对象数组，如果对象数量小于_prefabNum记录的数量，用DoLoad补上
    /// </summary>
	public static void RefillPool()
	{
		if(ObjectPool.Instance != null)
		{
			ObjectPool.Instance.InstanceRefill();
		}
	}

    /// <summary>
    /// 遍历当前缓存对象数组，如果对象数量小于_prefabNum记录的数量，用DoLoad补上
    /// </summary>
	public void InstanceRefill()
	{
		var poolEn = _poolDict.GetEnumerator();
		while(poolEn.MoveNext())
		{
			var keyStr = poolEn.Current.Key;
			var curHash = poolEn.Current.Value;
			int count = 0 ;
			if(_prefabNum.TryGetValue(keyStr,out count))
			{
				if(count > curHash.Count)
				{
					int addNum = count - curHash.Count;
					GameObject curPrefab = CheckAndAddToPrefabDict(keyStr);
					Debug.LogWarning("path is " + keyStr + " addNum " + addNum);
					DoLoad(curPrefab,addNum,keyStr,false);


				}
			}

		}
	}



//	public void ITestOut()
//	{
//		foreach(var curP in _pathDict)
//		{
#if UNITY_EDITOR
//			Debug.Log("path is " + curP.ToString());
#endif
//		}
//
//		foreach(var curP in _prefabDict)
//		{
#if UNITY_EDITOR
//			Debug.Log ("prefab key is " + curP.Key);
#endif
//		}
//
//		foreach(var curP in _poolDict)
//		{
//			var curCount = curP.Value.Count;
#if UNITY_EDITOR
//			Debug.Log("pool is " + curP.Key + " count is " + curCount);
#endif
//		}
//	}
	
    /// <summary>
    /// 加载指定路径下的指定对象count个
    /// </summary>
    /// <param name="preLoad">If set to <c>true</c> pre load.</param>
	void DoLoad(GameObject curPrefab,int count,string path,bool preLoad = true)
	{
		if(preLoad){
			_prefabNum.Add(path,count);
		}
		for(int i = 0 ; i < count; i ++)
		{
			var obj = Instantiate<GameObject>(curPrefab);

			//DontDestroyOnLoad(obj);
			
			obj.transform.SetParent(this.transform,false);
			obj.transform.position = new Vector3(-100f,-100f,0);
			StartCoroutine(DoActiveFalse(obj));


			
			int curInstanceId = obj.GetInstanceID();
			
			if(!_pathDict.ContainsKey(curInstanceId))
			{
				_pathDict.Add(curInstanceId,path);
			}else
			{
#if UNITY_EDITOR
				Debug.LogWarning ("ObjectPool::DoLoad: Current Instance has exist.");
#endif
			}

			if(!_poolDict.ContainsKey(path))
			{
#if UNITY_EDITOR
				Debug.LogWarningFormat ("ObjectPool::DoLoad: <{0}> is not exist.", path);
#endif
			}else
			{
				var curSet = _poolDict[path];
				curSet.Add(obj);
			}

//			yield return null;
		}
	}

    /// <summary>
    /// 等待一帧Inactive Obj
    /// </summary>
    /// <returns>The active false.</returns>
    /// <param name="obj">Object.</param>
	IEnumerator DoActiveFalse(GameObject obj)
	{
		yield return null;//new WaitForSeconds(3f);
		obj.SetActive(false);
		obj.transform.position = new Vector3(0,0,0);
	}
	

	public static void Reset()
	{
		var curInstance = ObjectPool.Instance;
		if(curInstance != null)
		{
			curInstance.Ireset();
		}
	}

	void Ireset()
	{
        var poolPre = _prefabDict.GetEnumerator();
        while (poolPre.MoveNext())
        {
            Destroy(poolPre.Current.Value);
        }

		_prefabDict.Clear();
		_pathDict.Clear();

		var poolEn = _poolDict.GetEnumerator();
		while(poolEn.MoveNext()){
			var curPair = poolEn.Current;
			var curSet = curPair.Value;
			var setEn = curSet.GetEnumerator();
			while(setEn.MoveNext())
			{
				var curObj = setEn.Current;
				if(curObj != null)
				{
					Destroy(curObj);
				}else
				{
				
#if UNITY_EDITOR
					Debug.LogWarningFormat ("ObjectPool::Ireset: GameObject<{0}> shouldn't be null.", curPair.Key);
#endif
				}
			}

		}
		_poolDict.Clear();
        _prefabNum.Clear();
	}

	public static GameObject GetGameObject(string path,bool onlyPooled = false)
	{
		var curInstance = ObjectPool.Instance;
		if(curInstance == null)
		{
			return null;
		}

		GameObject obj = curInstance.IGetGameObject (path, onlyPooled);



		return obj;
	}

	GameObject GenGameObject(string path)
	{
		var curPrefab = Resources.Load<GameObject>(path);
		if(curPrefab != null)
		{
			return Instantiate<GameObject>(curPrefab);
		}else
		{
#if UNITY_EDITOR
			Debug.LogWarningFormat ("ObjectPool::GenGameObject: Prefab<{0}> is not exist.", path);
#endif
		}
		return null;
	}

	GameObject IGetGameObject(string path,bool onlyPooled = true)
	{

		GameObject curObject = null;
		HashSet<GameObject> curSet = null;
		if(_poolDict.TryGetValue(path,out curSet))
		{
			if(curSet.Count > 0 )
			{
				var curHash = curSet.GetEnumerator();
				GameObject curObj = null;
				int i = 0 ;
				while (curHash.MoveNext())
				{
					i++;
					curObj = curHash.Current;
					if(curObj != null && curObj.activeSelf == false)
					{
						break;
					}
				}

				if(i > 1){
#if UNITY_EDITOR
					Debug.LogWarning ("ObjectPool::IGetGameObject: Empty num is " + i + " path is " + path);
#endif
				}

				if(curObj == null)
				{

#if UNITY_EDITOR
					Debug.LogWarning ("ObjectPool::IGetGameObject: count is " + curSet.Count);
#endif
#if UNITY_EDITOR
					Debug.LogWarning ("ObjectPool::IGetGameObject: GameObject be null unexpected path is " + path );
#endif
					curObject = GenGameObject(path);
				}else
				{
					curSet.Remove(curObj);
					curObject = curObj;
				}


			}
		}else
		{
#if UNITY_EDITOR
			Debug.LogWarning ("ObjectPool::IGetGameObject: PATH NOT IN DICT " + path);
#endif
		}

		if (!onlyPooled && curObject == null)
		{
			GameObject curPrefab = CheckAndAddToPrefabDict(path);
			GameObject newObj = Instantiate(curPrefab ) as GameObject;
#if UNITY_EDITOR
			Debug.LogWarning ("ObjectPool::IGetGameObject: No Object in poll,create path " + path);
#endif

			SaveDebugData(path);

			curObject = newObj;
		}

		if(curObject != null)
		{
			int id = curObject.GetInstanceID();
			if(!_pathDict.ContainsKey(id))
			{
				_pathDict.Add(id,path);
			}
			curObject.transform.localPosition = new Vector3(0,0,0);
//			curObject.transform.parent = null;

			curObject.SetActive(true);
		}
		return curObject;
	}

	public string startTime = "";
	public string NoObjcetInPool = "";

	void SaveDebugData (string path)
	{
		// removed as instructed by jyz - billtt
		//if(CheatPanel.isOutPutFile)
		//{
		//	NoObjcetInPool += (GameManager.Instance.currLevel + "," + path + "\n");
		//}
	}

    /// <summary>
    /// 延迟delay秒将obj放回池中
    /// </summary>
	static void PoolObject(GameObject obj,float delay = 0f)
	{
		var curInstance = ObjectPool.Instance;
		if(curInstance == null)
		{
			return;
		}
		curInstance.StartCoroutine(curInstance.DoPool(obj,delay));
	}

    /// <summary>
    /// 延迟delay秒将obj放回池中
    /// </summary>
    /// <returns>The pool.</returns>
    /// <param name="obj">Object.</param>
    /// <param name="delay">Delay.</param>
	IEnumerator  DoPool(GameObject obj,float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		var curInstance = ObjectPool.Instance;
		if(curInstance != null)
		{	
			curInstance.IPoolObject(obj);
		}
	

	}

    /// <summary>
    /// 如果对象是从池中取出的，隐藏并放回池中，
    /// 否则，销毁掉
    /// </summary>
    /// <param name="obj">Object.</param>
	void IPoolObject(GameObject obj)
	{
		if(obj == null)
		{
#if UNITY_EDITOR
			Debug.LogWarning ("ObjectPool::IPoolObject: obj shouldn't be null");
#endif
			return;
		}

		int id = obj.GetInstanceID();
		string path = string.Empty;
		if(!_pathDict.TryGetValue(id,out path))
		{
#if UNITY_EDITOR
			Debug.LogWarning ("ObjectPool::IPoolObject: Not exist in pathdict! Obj name is " + obj.name);
#endif
			Destroy(obj);
			return;
		}

		HashSet<GameObject> curSet;
		if(!_poolDict.TryGetValue(path,out curSet))
		{
#if UNITY_EDITOR
			Debug.LogWarning ("ObjectPool::IPoolObject: Current pool not exist");
#endif
			Destroy(obj);
			return;
		}
		obj.SetActive(false);
		obj.transform.SetParent(transform,false);
		obj.StopParticle();
		curSet.Add(obj);

	}

	GameObject CheckAndAddToPrefabDict(string path)
	{
		GameObject curPrefab;
		if(!_prefabDict.TryGetValue(path,out curPrefab))
		{
			var curObj= Resources.Load<GameObject>(path);
			if(curObj == null)
			{
				Debug.LogWarning("Path don't have prefab path is :" + path );
				return null;
			}

			curPrefab = Instantiate(curObj);
			curPrefab.transform.position = new Vector3(-100f,-100f,-100f);
			curPrefab.transform.SetParent(this.transform,true);
			curPrefab.SetActive(false);
			if(curPrefab == null)
			{
#if UNITY_EDITOR
				Debug.LogWarning ("ObjectPool::CheckAndAddToPrefabDict: Prefab not exist,path is " + path);
#endif
			}
			_prefabDict.Add(path,curPrefab);

		}

		return curPrefab;
	}


    /// <summary>
    /// 延迟delay秒将obj放回池中，如果是从池中取出才放回，否则销毁；
    /// nodeCount如果为0，同时先处理obj的所有孩子
    /// </summary>
	public static void PoolDestroy(GameObject obj,float delay = 0f,string sortingLayer = "")
	{
		var curInstance = ObjectPool.Instance;
		if(curInstance == null)
		{
			return;
		}
		curInstance.IPoolDestory(obj,delay,0,sortingLayer);
	}

    /// <summary>
    /// 延迟delay秒将obj放回池中，如果是从池中取出才放回，否则销毁；
    /// nodeCount如果为0，同时先处理obj的所有孩子
    /// </summary>
	void IPoolDestory(GameObject obj,float delay = 0f,int nodeCount = 0 ,string sortingLayer = "")
	{
		if(obj == null)
		{
			return;
		}
		int ID = obj.GetInstanceID();
		if(_pathDict.ContainsKey(ID))
		{
			obj.SetActive(false);
			obj.transform.SetParent(transform,false);
			if(!sortingLayer.Equals(""))
			{
				obj.SetSortingLayer(sortingLayer);
			}
			PoolObject(obj);
		} else
		{
			if(nodeCount == 0 )
			{
				int count = obj.transform.childCount;
				Transform[] curTransforms = new Transform[count];
				for(int i = 0 ; i  < count;++i)
				{
					curTransforms[i] = obj.transform.GetChild(i);
				}

				for(int i = 0 ; i < curTransforms.Length;i ++)
				{
					var curTrans = curTransforms[i];
					var curObj = curTrans.gameObject;
					var curCount = nodeCount+1;
					IPoolDestory(curObj,delay,curCount);
				}


			}
			Destroy(obj,delay);
			
		}
	}


	
	

}
