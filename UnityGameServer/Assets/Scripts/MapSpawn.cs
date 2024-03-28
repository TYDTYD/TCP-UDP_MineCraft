using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct node
{
    public int x;
    public int y;
    public bool isZero;

    public node(int a, int b,bool zero)
    {
        x = a;
        y = b;
        isZero = zero;
    }
}

public class MapSpawn : MonoBehaviour
{
    public GameObject world;
    public GameObject Cube;
    public GameObject[] ObjectPooling = new GameObject[1000];
    const int n = 100;
    public int[,] graph = new int[n, n];
    Queue<node> q = new Queue<node>();
    int count = 7;

    private void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            ObjectPooling[i] = Cube;
        }
        CreateWidth(0, n, n, 0, n/2, count);
        Bfs();
        
    }
    
    public enum mapType
    {
        Cube=0,
        Plane,
        Sphere,
        Capsule
    }

    void CreateLength(int left, int right, int top, int down, int value, int c)
    {
        if (c == 0)
            return;
        for (int i = down; i < top; i++)
        {
            if (graph[value,i] == 0)
                graph[value,i] = c;
        }
        int d = down + 1;
        int t = top - 1;

        int r1 = Random.Range(Mathf.Min(d, t), Mathf.Max(d, t));
        int r2 = Random.Range(Mathf.Min(d, t), Mathf.Max(d, t));
        CreateWidth(left, value, top, down, r1, c - 1);
        CreateWidth(value, right, top, down, r2, c - 1);
        return;
    }

    void CreateWidth(int left, int right, int top, int down, int value, int c)
    {
        if (c == 0)
            return;
        for (int i = left; i < right; i++)
        {
            if (graph[i,value] == 0)
                graph[i,value] = c;
        }
        int d = left + 1;
        int t = right - 1;
        int r1 = Random.Range(Mathf.Min(d, t), Mathf.Max(d, t));
        int r2 = Random.Range(Mathf.Min(d, t), Mathf.Max(d, t));
        CreateLength(left, right, value, down, r1, c - 1);
        CreateLength(left, right, top, value, r2, c - 1);
        return;
    }

    public void MapInfo()
    {
        int seq = 0;
        for(int i=0; i<n; i++)
        {
            for(int j=0; j<n; j++)
            {
                Vector3 CubePos = new Vector3(i, graph[i, j], j);
                ObjectPooling[seq++].transform.Translate(CubePos);
                //Instantiate(Cube, CubePos, Quaternion.identity);
                if (graph[i, j] != 0)
                {
                    int idx = graph[i, j] - 1;
                    while (idx != 0)
                    {
                        Vector3 CubeDown = new Vector3(i, idx--, j);
                        ServerSend.BlockPosition(CubeDown, (int)mapType.Cube);
                        ObjectPooling[seq++].transform.Translate(CubeDown);
                        //Instantiate(Cube, CubeDown, Quaternion.identity);
                    }
                }
                ServerSend.BlockPosition(CubePos, (int)mapType.Cube);
            }
        }
    }

    public void Bfs()
    {
        int[] dx = { 1, 0, -1, 0 };
        int[] dy = { 0, 1, 0, -1 };
        int start = 0;
        if (graph[start,start] == 1)
            q.Enqueue(new node(start, start, false));
        else
            q.Enqueue(new node(start, start, true));
        bool[,] visited = new bool[n + 1, n + 1];
        visited[start,start] = true;

        while (q.Count!=0)
        {
            node p = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int nx = dx[i] + p.x;
                int ny = dy[i] + p.y;

                if (nx < 0 || nx >= n || ny < 0 || ny >= n)
                    continue;
                if (visited[nx,ny])
                    continue;
                if (graph[nx,ny] != 0)
                    q.Enqueue(new node(nx, ny, false));
                else
                    q.Enqueue(new node(nx, ny, true));
                if (graph[nx,ny] == 0)
                {
                    if (p.isZero)
                        graph[nx,ny] = graph[p.x,p.y];
                    else
                        graph[nx,ny] = graph[p.x,p.y] + 1;
                }
                else
                {
                    if (!p.isZero)
                        graph[nx,ny] = graph[p.x,p.y];
                    else
                        graph[nx,ny] = graph[p.x,p.y] + 1;
                }
                visited[nx,ny] = true;
            }
        }
    }
}