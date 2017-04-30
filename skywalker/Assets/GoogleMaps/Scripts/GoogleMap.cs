using UnityEngine;
using System.Collections;

public class GoogleMap : MonoBehaviour
{
	public float curlat;
	public float curlong;
	public ArrayList a;
	public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;

	void Start()
	{
		a = new ArrayList();
		//Example code
		curlat = 70;
		curlong = 30;
		if (loadOnStart)
			Refresh();
		isNear();
	}

	public void Refresh()
	{
		if (autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");
		}
		StartCoroutine(_Refresh());
	}

	IEnumerator _Refresh()
	{
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
			else {
				qs += "center=" + WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
			}
			qs += "&zoom=" + zoom.ToString();
		}
		qs += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString().ToLower();
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");

		foreach (var i in markers) {
			qs += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", i.size.ToString().ToLower(), i.color, i.label);
			foreach (var loc in i.locations) {
				//if (loc.address != "")
				//	qs += "|" + WWW.UnEscapeURL (loc.address);
				//else
				qs += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		foreach (var i in paths) {
			qs += "&path=" + string.Format("weight:{0}|color:{1}", i.weight, i.color);
			if (i.fill)
				qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + WWW.UnEscapeURL(loc.address);
				else
					qs += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
			}
		}
		var req = new WWW(url + "?" + qs);
		yield return req;
		GetComponent<Renderer>().material.mainTexture = req.texture;
	}

	private void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
		{
			zoom += (int)(Input.GetAxis("Mouse ScrollWheel") * 10);
			if (zoom < 1)
				zoom = 1;
			Refresh();
		}
		//if (Input.touchCount == 2) {
		//	Touch touchZero = Input.GetTouch(0);
		//	Touch touchOne = Input.GetTouch(1);

		//	Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		//	Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		//	float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		//	float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
		//	float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
		//	if (deltaMagnitudeDiff < 0) {
		//		zoom--;
		//	}
		//	else {
		//		zoom++;
		//	}
		//}
	}
	private bool isNear()
	{
		foreach (var i in markers) {
			if (i.label == "Sight")
				foreach (var loc in i.locations) {
					if (loc.latitude < curlat + 0.5 && loc.latitude > curlat - 0.5 && loc.longitude < curlong + 0.5 && loc.longitude > curlong - 0.5) {
						a.Add(loc.address);
					}
				}
		}
		return true;
	}
}


public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;
}
