using UnityEngine;

public class LaserScript : MonoBehaviour
{
	private LineRenderer lineRenderer; //визуализатор траектории лазера

	//параметры сканирования
	[SerializeField] private int rayCount = 5; //максимальное количество лучей
	[SerializeField] private int maxDistance = 100; //максимальная длина луча

	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update()
	{
		DrawLaser();
	}

	// Нарисовать траекторию лазера
	public void DrawLaser()
	{
		Vector3[] scanOutline = new Vector3[rayCount + 1]; //массив точек траектории лазера

		//определить позицию и направление первого луча
		Vector2 currentPos = transform.position;
		float angle = (transform.rotation.eulerAngles.z + 90) / 180 * Mathf.PI;
		var x = Mathf.Cos(angle);
		var y = Mathf.Sin(angle);
		Vector2 currentDir = new Vector2(x, y);

		//для каждого луча
		for (int i = 0; i < rayCount; i++)
		{
			//занести текущую позицию в массив
			scanOutline[i] = currentPos;

			//получить позицию и направление следующего луча
			GetNextEndPoint(currentPos, currentDir, out Vector2 newPos, out Vector2 newDir);

			//обновить данные
			currentPos = newPos;
			currentDir = newDir;
		}

		//закончить траекторию лазера
		scanOutline[rayCount] = currentPos;

		//отобразить траекторию
		lineRenderer.positionCount = scanOutline.Length;
		lineRenderer.SetPositions(scanOutline);
	}

	// Получить позицию и направление следующего луча
	private void GetNextEndPoint(Vector2 pos, Vector2 dir, out Vector2 newPos, out Vector2 newDir)
	{
		RaycastHit2D hit = Physics2D.Raycast(pos, dir, maxDistance);
		if (hit.collider != null) //столковение с препятствием есть
		{
			newPos = hit.point - dir * 0.01f; //вычесть маленькое расстояние, чтобы лазер не "застревал" в препятствии
			Vector2 surface = Rotate90(hit.normal); //получить вектор вдоль поверхности препятствия
			newDir = Vector2.Reflect(-dir, surface); //получить направление отражённого луча
		}
		else //столкновения нет (лазер светит в бесконечность)
		{
			//продолжить луч по направлению
			newPos = pos + dir * maxDistance;
			newDir = dir;
		}
	}

	// Получить перпендикуляр вектора
	private Vector2 Rotate90(Vector2 v)
	{
		float temp = v.x;
		v.x = -v.y;
		v.y = temp;
		return v;
	}
}
