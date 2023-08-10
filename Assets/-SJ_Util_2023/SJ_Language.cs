using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public	class _SJ_LANG_TAG_ID
{
	public	string	part = "";
	public	int		ID = 0;
	public	string	word = "";

	[Multiline]
	public	string	_desc = "";


	public	string	Str( bool debug = false )
	{
		return SJ_Language.Str( part , ID , word , debug ); 
	}

	public	void	Log( string str )
	{
		Debug.Log( "_SJ_LANG_TAG_ID  [part:"+part+"]  [int:"+ID+"]   [word:"+word+"]"  + "    <"+str+">" );
	}

}


class _SJ_LANG_TAG_ID_EqualityComparer : IEqualityComparer<_SJ_LANG_TAG_ID>
{
    public bool Equals(_SJ_LANG_TAG_ID b1, _SJ_LANG_TAG_ID b2)
    {
        if (b2 == null && b1 == null)
           return true;
        else if (b1 == null | b2 == null)
           return false;
        else if( string.Equals( b1.part , b2.part) &&  b1.ID == b2.ID
                            && string.Equals( b1.word , b2.word) )
            return true;
        else
            return false;
    }

    public int GetHashCode(_SJ_LANG_TAG_ID bx)
    {
        int hCode = bx.part.GetHashCode() ^ bx.ID ^ bx.word.GetHashCode();
        return hCode.GetHashCode();
    }
}

public class SJ_Language
{
	static	public	int		Total_Lang = 0;
	static	public	int		select_Lang = 0;
	static	public	int		read_CSV_Line = 0;
	static	public	string	read_Recent_Tag;
	static	public	Dictionary<_SJ_LANG_TAG_ID , List<string>>  dic_Data = new Dictionary<_SJ_LANG_TAG_ID, List<string>>( new _SJ_LANG_TAG_ID_EqualityComparer() );


	static	public	void	Load_OneLine( _SJ_LANG_TAG_ID id , List<string> strs , int start_idx = 0 )
	{
		List<string>	list_str = new List<string>();
		list_str.AddRange( strs );

		if( start_idx > 0 )
		{
			list_str.RemoveRange(0,start_idx);
		}

		//id.Log( strs[start_idx] );
		dic_Data[id] = list_str;
	}

	static	public	void	Load_StringData_Text( string str_Data )
	{
																	
		SJ_CSV_Read.OpenCSV_Text( str_Data );

		read_Recent_Tag = "";
		Load_StringData_OpenedCSV();
	}

	static	public	int	Load_StringData_OpenedCSV( bool debug = false )
	{
		int load_count = 0;

		read_Recent_Tag = "";
		while(true)
		{
			if( read_CSV_Line < 1 )
			{
				// 4 + lang 
				// 기본 데이터 + 언어 갯수
				read_CSV_Line = 4 + Total_Lang;
			}

			string str_read_line = "";

			// `` 분류,	ID_Num,	ID_Str,	주석 ,언어들..		
			List<string> strs = SJ_CSV_Read.Read_Line(read_CSV_Line );

			//Debug.Log( str_read_line );

			if( strs == null ) break;

			_SJ_LANG_TAG_ID k = new _SJ_LANG_TAG_ID();

			string tag = strs[0];

			if( string.IsNullOrEmpty(tag) )
			{ 
				tag = read_Recent_Tag;
			}
			else
			{
				read_Recent_Tag = tag;
			}

			k.part = tag;
			SJ_CSV_Read.Exist_SetInt(ref k.ID , strs[1]);
			k.word = strs[2];

			List<string> list_word = new List<string>();
			for( int i = 4 ; i < strs.Count ; i++ )list_word.Add( strs[i] );

			// 키 아이디가 둘다 없으면 메인언어 단어가 키
			if( string.IsNullOrEmpty(strs[1]) && string.IsNullOrEmpty(strs[2]) )
			{
				k.word = list_word[0];
			}

			//Debug.Log( list_word[0] );

			if( debug )
			{
				k.Log( list_word[0] );
			}

			dic_Data[k] = list_word;
			load_count++;
		}
		SJ_CSV_Read.CloseCSV();
		return load_count;
	}


	static	public	_SJ_LANG_TAG_ID	temp_find = new _SJ_LANG_TAG_ID();
	static	public	string	Str( string part , int id , string word , bool debug = false )
	{
		List<string> list_find = null;

		temp_find.part = part;
		temp_find.ID = id;
		temp_find.word = word;

		if( dic_Data.TryGetValue( temp_find , out list_find ) )
		{
			return list_find[select_Lang];
		}

		if( debug )
		{
			Debug.LogError("error!!!! SJ_Language : " + part + " : " + id + " : " + word );
			temp_find.Log("");
		}

		return "";
	}

	static	public	void	Str_Exist( ref string ref_str , string part , int id , string word )
	{
		string find_str = Str( part , id, word);
		if( string.IsNullOrEmpty(find_str) == false )
		{
			ref_str = find_str;
		}
	}

	static public	string	Str( string part  , int id )
	{
		return Str(part , id , "");
	}

	static public	string	Str( string part  , string word )
	{
		return Str(part , 0 , word);
	}

	//static public	string	StrTag( string part  , string tag )
	//{
	//	return"";
	//}

	static public	string	Str( int id )
	{
		return Str("" , id , "");
	}

	static public	string	Str( string word )
	{
		return Str("" , 0 , word);
	}



}
