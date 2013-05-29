using UnityEngine;
using System.Collections;
using System.IO;

public class LogFile {

	string nameFile;
	StreamWriter writer;

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

	public void WriteLine(string line)
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
	}
}
