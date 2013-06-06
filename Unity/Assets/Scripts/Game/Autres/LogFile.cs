using UnityEngine;
using System.Collections;
using System.IO;

public class LogFile {

	string nameFile;
	StreamWriter writer;
	long length;

	public LogFile()
	{
		nameFile = "";
		writer = null;
	}

	public void SetName(string sName)
	{
		if (sName != "")
			nameFile = sName;
	}

	public string GetName()
	{
		return nameFile;
	}
	
	public long GetLength()
	{
		return length;
	}

	public void WriteLine(string line, bool onlyCreate = false)
	{
		if (line == "")
			return;

		if (!File.Exists(nameFile + ".txt"))
			writer = File.CreateText(nameFile + ".txt");
		else
		{
			writer = File.AppendText(nameFile + ".txt");
		}
		writer.WriteLine(line);
		writer.Close();
		
		FileInfo info = new FileInfo(nameFile + ".txt");
		length = info.Length;
	}
	
	public void ClearFile()
	{
		if (!File.Exists(nameFile + ".txt"))
			return;
		
		File.WriteAllText(nameFile + ".txt", "");
	}
}
