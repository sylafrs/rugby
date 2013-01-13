using UnityEngine;
using System.Collections;

public interface Debugable
{
    void ForDebugWindow();
    bool getToogled();
    void setToogled(bool t);
}