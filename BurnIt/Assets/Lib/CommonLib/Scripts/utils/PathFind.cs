using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Vector2i
{
	public int x;
	public int y;
}
public class AstarListNode
{
	//节点x坐标
	public int x;
	//节点y坐标
	public int y;
	//当前节点到起始点的代价
	public int f;
	//当前节点到目标节点的代价
	public int g;
	//总代价
	public int h;

	//当前节点父节点
	public AstarListNode father = null;
	//当前节点子节点
	public AstarListNode next = null;
}

public class AstarList
{
	public AstarListNode openHead;
	public AstarListNode closedHead;
}

public class PathFind
{
	public int astarPathCount;
	public List<Vector2i> astarPathList = new List<Vector2i>();
	List<List<int>> _map;
	int _startX;
	int _startY;
	int _endX;
	int _endY;

	public bool AStarSearch(List<List<int>> map, int startX, int startY, int endX, int endY)
	{
		astarPathCount = 0;
		astarPathList.Clear();
		_map = map;
		_startX = startX;
		_startY = startY;
		_endX = endX;
		_endY = endY;

		if (AstarIsBlock(endX, endY))
			return false;
		int[] offsetX = new int[]{0, 0, -1, 1};
		int[] offsetY = new int[]{1, -1, 0, 0};
//		int[] offsetX = new int[]{0, 0, -1, 1, -1, -1, 1, 1};
//		int[] offsetY = new int[]{1, -1, 0, 0, 1, -1, 1, -1};
		AstarList astar = new AstarList();
		astar.openHead = new AstarListNode();
		astar.closedHead = new AstarListNode();
		AstarListNode currentNode = null;
		AstarListNode startNode = new AstarListNode();
		AstarNodeInit(startNode, null, _startX, _startY, _endX, _endY);
		AstarAddNode(astar.openHead, startNode);

		while (astar.openHead.next != null)
		{
			currentNode = AstarGetMinCostList(astar);
			if (currentNode.x == endX && currentNode.y == endY)
			{
				break;
			}
			else
			{
				AstarAddNode(astar.closedHead, currentNode);
				AstarRemoveNode(astar.openHead, currentNode);
				for (int i = 0; i < 4; i++)
				{
					int x = currentNode.x + offsetX[i];
					int y = currentNode.y + offsetY[i];
					if (x < 0 || x >= _map.Count || y < 0 || y >= _map[0].Count)
					{
						continue;
					}
					else
					{
						if (!AstarCheckNodeInList(x, y, astar.openHead)
							&& !AstarCheckNodeInList(x, y, astar.closedHead)
							&& !AstarIsBlock(x, y))
						{
							AstarListNode endNode = new AstarListNode();
							AstarNodeInit(endNode, currentNode, x, y, endX, endY);
							AstarAddNode(astar.openHead, endNode);
						}
					}
				}
			}
		}
		if (astar.openHead.next == null && (currentNode.x != endX || currentNode.y != endY))
		{
			astarPathCount = 0;
		}
		else
		{
			while (currentNode != null)
			{
				Vector2i point = new Vector2i();
				point.x = currentNode.x;
				point.y = currentNode.y;
				//astarPathList.Add(point);
				astarPathList.Insert(0, point);
				currentNode = currentNode.father;
				astarPathCount++;
			}
			return true;
		}
		return false;
	}

	public bool AstarIsBlock(int x, int y)
	{
		if (x >= 0 && x < _map.Count && y >= 0 && y < _map[0].Count)
		{
			int v = _map[x][y];
			if (v == 0)
				return true;
		}
		return false;
	}

	void AstarNodeInit(AstarListNode current, AstarListNode father, int x, int y, int endX, int endY)
	{
		current.x = x;
		current.y = y;
		current.father = father;
		current.next = null;
		if (father != null)
			current.f = father.f + 1;
		else
			current.f = 0;
		current.g = Math.Abs(endX - x) + Math.Abs(endY - y);
		current.h = current.f + current.g;
	}
	//添加节点
	void AstarAddNode(AstarListNode head, AstarListNode node)
	{
		while (head.next != null)
			head = head.next;
		head.next = node;
	}
	//删除节点
	void AstarRemoveNode(AstarListNode head, AstarListNode node)
	{
		AstarListNode current = head;
		head = head.next;
		while (head != null)
		{
			if (head == node)
			{
				current.next = head.next;
				head.next = null;
				break;
			}
			else
			{
				current = head;
				head = head.next;
			}
		}
	}
	//检查（x,y）是否在列表中
	bool AstarCheckNodeInList(int x, int y, AstarListNode head)
	{
		bool result = false;
		head = head.next;
		while (head != null)
		{
			if (head.x == x && head.y == y)
			{
				result = true;
				break;
			}
			else
			{
				head = head.next;
			}
		}
		return result;
	}
	//获得成本最小的节点
	AstarListNode AstarGetMinCostList(AstarList astar)
	{
		AstarListNode min = astar.openHead.next;
		AstarListNode current = min.next;
		while (current != null)
		{
			if (current.h < min.h)
				min = current;
			current = current.next;
		}
		return min;
	}
}
