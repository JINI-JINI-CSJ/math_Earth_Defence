using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SJ_TriggerSimple<T>
{
    public Dictionary<int,HashSet<T>> dic_hash_set = new Dictionary<int, HashSet<T>>();

    // 리스트 캐쉬
    public  HashSet<T>  hs_cash = new HashSet<T>();

    public  void    Clear()
    {
        dic_hash_set.Clear();
        hs_cash.Clear();
    }

    public  void    Add( int trigger , T obj )
    {
        HashSet<T> hs = null;
        if( dic_hash_set.TryGetValue( trigger , out hs ) == false )
        {
            hs = new HashSet<T>();
            dic_hash_set.Add( trigger , hs );
        }
        hs.Add( obj );
        hs_cash.Add(obj);
    }

    public  void    Remove( int trigger , T obj )
    {
        HashSet<T> hs = FindTrigger(trigger);
        if( hs == null ) return;
        hs.Remove(obj);
        hs_cash.Remove(obj);
    }

    public  void    Remove( T obj )
    {
        hs_cash.Remove(obj);

        foreach( KeyValuePair<int,HashSet<T>>  kv in dic_hash_set )
        {
            if( kv.Value.Contains( obj ) )
            {
                kv.Value.Remove(obj);
                return;
            }
        }
    }

    public  HashSet<T>  FindTrigger( int trigger )
    {
        HashSet<T> hs = null;
        if( dic_hash_set.TryGetValue( trigger , out hs ) == false )
        {
            return null;
        }
        return hs;
    }


    public  HashSet<T>  GetAllObj()
    {
        HashSet<T> hs = new HashSet<T>();
        foreach( KeyValuePair<int,HashSet<T>>  kv in dic_hash_set )
        {
            foreach( T ss in kv.Value )
            {
                hs.Add(ss);
            }
        }
        return hs;
    }



}
