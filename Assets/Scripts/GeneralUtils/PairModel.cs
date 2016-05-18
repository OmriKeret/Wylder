using UnityEngine;
using System.Collections;

public class PairModel<T1, T2>
{
	public T1 First { get; private set; }
	public T2 Second { get; private set; }

	internal PairModel(T1 first, T2 second)
	{
		First = first;
		Second = second;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is PairModel<T1,T2>)) return false;
	//	var a = (PairModel<T1,T2>)obj;
	//	bool s = (isEquals (((PairModel<T,object>)obj).First, this.First));
	//	bool d = (isEquals(((PairModel<object,object>)obj).Second, this.Second));
		return (isEquals(((PairModel<T1,T2>)obj).First, this.First)) && (isEquals(((PairModel<T1,T2>)obj).Second, this.Second));
	}

	public bool isEquals(object o, object c){
		return(o.ToString ().ToUpper() == (c.ToString ()).ToUpper());
	}
	
	public override int GetHashCode()
	{            
		return (this.First.ToString() + this.Second.ToString()).GetHashCode();
	}    
}

public static class PairModel
{
	public static PairModel<T1, T2> New<T1, T2>(T1 first, T2 second)
	{
		var tuple = new PairModel<T1, T2>(first, second);
		return tuple;
	}
}
