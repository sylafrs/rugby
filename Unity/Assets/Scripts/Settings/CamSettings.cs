[System.Serializable]
public class CamSettings
{
	public float zoom;
	public CameraTargetList	target;
	public float nearClip;
}

public enum CameraTargetList
{
	BALL,
	OWNER,
	NULL
}