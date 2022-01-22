using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
	public event Action OnBuffBegin;
	public event Action OnBuffUpdate;
	public event Action OnBuffEnd;

}
