using UnityEngine;

using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.IO;
using System.Collections.Generic;

using System.Threading;
//using System.Net;

//using System.Diagnostics;  

using System.Runtime.InteropServices;

//#define		SJS_PT_L_ROOM_USERLIST	13;


public class SJStreamBuffer
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)] 
	public	class _SJS_PT_HEAD
	{
		public	byte 	h;
		public	byte 	l; 
		public	int 	r; 
		public	UInt32 	uid;	
	}
	
	public static void WirteHead_ST_2( BinaryWriter bw, byte h , byte l , int r , UInt32 uid )
	{
		bw.Write( h  );
		bw.Write( l  );
		bw.Write( IPAddress.HostToNetworkOrder( r ) );	
		bw.Write( IPAddress.HostToNetworkOrder( uid ) );
	}
	
	
	public static void WirteHead_ST( BinaryWriter bw, byte h , byte l , int r , UInt32 uid )
	{

		_SJS_PT_HEAD pt = new _SJS_PT_HEAD();
		
		pt.h = h;
		pt.l = l;
		pt.r = r;
		pt.uid = uid;
		
        int Size = Marshal.SizeOf(pt);
		byte[] DataByte = new byte[Size];
		IntPtr buffer = Marshal.AllocHGlobal(Size);
		Marshal.StructureToPtr(pt, buffer, false);
		Marshal.Copy(buffer, DataByte, 0, Size);
		Marshal.FreeHGlobal(buffer); 
		
		
		bw.Write( DataByte , 0 , Size );
	}
	
	
	
	public static void WirteHead( BinaryWriter bw, byte h , byte l , int r , UInt32 uid )
	{
		bw.Write( h );
		bw.Write( l );
		bw.Write( (Int32)r );
		bw.Write( uid );
	}
	
	
	public	static	void	WriteHeadKW( BinaryWriter bw, byte h , byte l , UInt32	nSize )
	{
		bw.Write( h );
		bw.Write( l );
		bw.Write( nSize );
	}
	
	
	public static void ReadString( BinaryReader br, ref string str)
	{
		UInt16	nSize = br.ReadUInt16();
		
		if( nSize < 1 )
			return;
		
		str =	ExtendedTrim( Encoding.Default.GetString(br.ReadBytes(nSize)) );
	}
	
	public static void WriteString( BinaryWriter bw, string str )
	{            
		UInt16	nSize = (UInt16)str.Length;
		
		bw.Write( nSize );
		if( str.Length < 1 )
			return;
		byte[] btData = new byte[ nSize ];
		Encoding.Default.GetBytes( str , 0 , str.Length , btData , 0);
		bw.Write( btData );
	}
	
	
	public	static	uint GetHeaderSizeString( string str )
	{
		return (uint)str.Length;
	}
	
	
	public static void ReadString_uni( BinaryReader br, ref string str )
	{
		UInt16	nSize = 0;
		//nSize = (UInt16)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
		nSize = br.ReadUInt16();
		
		if( nSize > 0 )
		{
			str =	ExtendedTrim( Encoding.Unicode.GetString(br.ReadBytes(nSize*2)) );
		}else{
			str = "";
		}

		//Debug.Log( "ReadString_uni : [" + nSize.ToString() + "] : " + str );
	}
	
	public static string  ReadString_uni( BinaryReader br )
	{
		string str="";
		ReadString_uni(br ,ref str);
		return str;
	}
	
	public	static	int GetHeaderSizeUniString( string str )
	{
		return str.Length * 2 + 2;
	}
	/*
string strSendText = "전송할 메시지";
byte[] pbSource = Encoding.UTF8.GetBytes(strSendText);
byte[] pbDest = Encoding.Convert(
   Encoding.UTF8, Encoding.GetEncoding("euc-kr"), pbSource);
pbSource = Encoding.Convert(Encoding.GetEncoding("euc-kr"), Encoding.UTF8, pbDest);
char[] psUnicode = UTF8Encoding.UTF8.GetChars(pbSource);
string strReceiveText = new string(psUnicode); 
	*/
	
	public static void WriteString_uni( BinaryWriter bw, string str )
	{
		UInt16	nSize = (UInt16)str.Length;
		//bw.Write( (UInt16)IPAddress.HostToNetworkOrder( (short)nSize ) );
		bw.Write( (short)nSize );
		
		byte[] btData = new byte[ nSize * 2 ];
		Encoding.Unicode.GetBytes( str , 0 , str.Length , btData , 0);
		if( nSize > 0 )bw.Write( btData );


		//Debug.Log( "WriteString_uni : [" + nSize.ToString() + "] : " + str );

		//byte[] pbSource = Encoding.UTF8.GetBytes(str);
		//byte[] pbDest = Encoding.Convert(
		//  	Encoding.UTF8, Encoding.GetEncoding("euc-kr"), pbSource);
		//	pbSource = Encoding.Convert(Encoding.GetEncoding("euc-kr"), Encoding.UTF8, pbDest); 
		//if( nSize > 0 )bw.Write( pbDest );
	}
	
	
	public	static	void	ReadKWLONGLONG( BinaryReader br , ref string str )
	{
		byte[] btData = new byte[24];
		btData = br.ReadBytes(24);
		
		byte[] pbDest = Encoding.Convert( Encoding.UTF7, Encoding.Unicode , btData );
		char[] psUnicode = UnicodeEncoding.Unicode.GetChars( pbDest );
		string strReceiveText = new string(psUnicode); 
		
		str = strReceiveText;
		
		//Debug.Log( "ReadKWLONGLONG : " + strReceiveText );
	}
	
	
	public	static	void	WriteKWLONGLONG( BinaryWriter bw , string str )
	{
		byte[] btData = UnicodeEncoding.Unicode.GetBytes( str );
		byte[] pbDest = Encoding.Convert( Encoding.Unicode , Encoding.UTF7, btData );
		
		bw.Write( pbDest , 0 , 24 );
	}
	
	public	static	int 	GetSizeKWLONGLONG()
	{
		return 24;
	}
	
	public static string ExtendedTrim(string source)
	{
		string dest = source;
		
		int index = dest.IndexOf('\0');
		if( index > -1 )
		{
		dest = source.Substring(0,index+1);
		}
		return dest.TrimEnd('\0').Trim();
	}
	
	public	static	void 	Write_HostToNet_int( BinaryWriter bw , int val ) 	{bw.Write( (int)IPAddress.HostToNetworkOrder( (int)val ) );}
	public	static	int 	Read_NetToHost_int( BinaryReader br )				{return (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());}
	
	public	static	void 	Write_HostToNet_short( BinaryWriter bw , short val ){bw.Write( (short)IPAddress.HostToNetworkOrder( (short)val ) );}
	public	static	short 	Read_NetToHost_short( BinaryReader br )				{return (short)IPAddress.NetworkToHostOrder( (short)br.ReadInt16());}
}


public	class KWConnect
{
	public	byte	nConnectType;
	public	byte	nServerVersion;
	//public	byte	nMainServerNum;
	public	byte	nConnectResult;
	//public	int 	nMoveNum;
	//public	string	strGameID;
	public	string 	strUserID;
	public	string 	strPW;
	public	string	strFindID;
	
	//
	//public	UInt32	ConfirmNum1;
	//public	UInt32	ConfirmNum2;
	//public	int		ConfirmNum3;
	//public	int		ConfirmNum4;
	
	public	void	ReadData( BinaryReader br )
	{
		nConnectType 	= br.ReadByte();
		nServerVersion 	= br.ReadByte();
		//nMainServerNum	= br.ReadByte();
		nConnectResult 	= br.ReadByte();
		//nMoveNum 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		//SJStreamBuffer.ReadString_uni(br, ref strGameID );
		SJStreamBuffer.ReadString_uni(br, ref strUserID );
		SJStreamBuffer.ReadString_uni(br, ref strPW );
		SJStreamBuffer.ReadString_uni(br, ref strFindID );
		//ConfirmNum1 = (UInt32)IPAddress.NetworkToHostOrder( (int)br.ReadUInt32());
		//ConfirmNum2 = (UInt32)IPAddress.NetworkToHostOrder( (int)br.ReadUInt32());
		//ConfirmNum3 = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		//ConfirmNum4 = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		
		
		//Debug.Log( "KWConnect  nConnectType : " + nConnectType );
		//Debug.Log( "KWConnect  nServerVersion : " + nServerVersion );
		//Debug.Log( "KWConnect  nConnectResult : " + nConnectResult );
		//Debug.Log( "KWConnect  nMoveNum : " + nMoveNum );
		
		//Debug.Log( "KWConnect  strUserID : " + strUserID );
		
		//Debug.Log( "KWConnect  ConfirmNum1 : " + ConfirmNum1 );
		//Debug.Log( "KWConnect  ConfirmNum2 : " + ConfirmNum2 );
		//Debug.Log( "KWConnect  ConfirmNum3 : " + ConfirmNum3 );
		//Debug.Log( "KWConnect  ConfirmNum4 : " + ConfirmNum4 );
	}
	
	public	void	WriteData( BinaryWriter bw )
	{
		bw.Write( nConnectType );
		bw.Write( nServerVersion );
		//bw.Write( nMainServerNum );
		bw.Write( nConnectResult );
		
		//bw.Write( (Int32)IPAddress.HostToNetworkOrder( (int)nMoveNum ) );// move num			//	4
		
		//SJStreamBuffer.WriteString_uni( bw , strGameID	); // game id				2
		SJStreamBuffer.WriteString_uni( bw , strUserID	); // user id		18	
		SJStreamBuffer.WriteString_uni( bw , strPW		); // pw				12	
		SJStreamBuffer.WriteString_uni( bw , strFindID	); // find id				2	
		
		//bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)ConfirmNum1 ) );				//	4
		//bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)ConfirmNum2 ) );			//	4
		//bw.Write( (Int32)IPAddress.HostToNetworkOrder( (int)ConfirmNum3 ) );			//	4
		//bw.Write( (Int32)IPAddress.HostToNetworkOrder( (int)ConfirmNum4 ) );			//	4
		
	}
	
	public	int 	GetWriteSize()
	{
		int size = 0;
		size += 3;
		//size += 4;
		//size += SJStreamBuffer.GetHeaderSizeUniString( strGameID );
		size += SJStreamBuffer.GetHeaderSizeUniString( strUserID );
		size += SJStreamBuffer.GetHeaderSizeUniString( strPW );
		size += SJStreamBuffer.GetHeaderSizeUniString( strFindID );
		//size += 16;
		return size;
	}
}


public	class 	KWItemDB
{
	public	int 	nCode;
	public	int 	nPriceBuy;
	public	int 	nPriceSell;
	public	int[] 	nStatAr = new int[10];
	
	public	void	ReadData( BinaryReader br )
	{
		nCode 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nPriceBuy 	= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nPriceSell	= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		for(int i = 0 ; i < 10 ; i++ )
		{
			nStatAr[i] = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		}
	}
	
}

public	class 	KWPartDB
{
	public	int 	nCode;
	public	byte	nPos;
	public	int 	nPriceBuy;
	public	int 	nPriceSell;
	public	int[] 	nStatAr = new int[8];
	
	public	void	ReadData( BinaryReader br )
	{
		nCode 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nPos		= br.ReadByte();
		nPriceBuy 	= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nPriceSell	= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		for(int i = 0 ; i < 8 ; i++ )
		{
			nStatAr[i] = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		}
	}
}


public	class 	KWUserParts
{
	//
	// idx : DB 의 프라이머리 키 인덱스 , 유니크 아이디
	//
	public	int 	nIdx;
	public	int 	nCode;
	public	byte	nState;
	public	void	ReadData( BinaryReader br )
	{
		nIdx 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nCode 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nState		= br.ReadByte();
	}
	public	void	WriteData( BinaryWriter bw )
	{
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nIdx ) );
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nCode ) );
		bw.Write( nState );
	}
	public	int 	GetWriteSize()
	{
		return 9;
	}
}

public	class 	KWUserItem
{
	public	int 	nIdx;
	public	int 	nCode;
	public	byte	nState;
	public	void	ReadData( BinaryReader br )
	{
		nIdx 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nCode 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nState		= br.ReadByte();
	}
	public	void	WriteData( BinaryWriter bw )
	{
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nIdx ) );
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nCode ) );
		bw.Write( nState );
	}
	public	int 	GetWriteSize()
	{
		return 9;
	}
}



public	class 	KWNetUser
{
	public	byte	nMainServerNum;
	public	int 	nUserNum;
	public	String	strUserID = "testID";
	public	String	strGameID = "";
	public	byte	nGrade;
	public	int 	nExp;
	public	byte	nTeam;
	public	string	strGold;
	public	Int64	nGold;
	
	public	List<KWUserParts> 	sPartList = new List<KWUserParts>();
	public	List<KWUserItem>	sItemList = new List<KWUserItem>();
	
	
	//
	public	byte		nUserState = 0;
	public	GameObject	game_obj;
	public	int 		nRank;
	public	byte		nRankType;
	
	public	int 		m_nSelectchrModel = 0;
	public	string		strUserNick;
	
	
	public	void	ReadData( BinaryReader br )
	{
		nMainServerNum 	= br.ReadByte();
		nUserNum 		= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		SJStreamBuffer.ReadString_uni(br, ref strUserID );
		SJStreamBuffer.ReadString_uni(br, ref strGameID );
		
		Debug.Log( "strGameID <"+strGameID+">    : " + strGameID.Length  );
		
		nGrade			= br.ReadByte();
		nExp			= (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		nTeam			= br.ReadByte();
		SJStreamBuffer.ReadKWLONGLONG( br , ref strGold  );

		// 
		int nCount = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		Debug.Log("KWUserParts Count : "+nCount);
		sPartList.Clear();
		for(int i=0;i<nCount;i++)
		{
			KWUserParts sPart = new KWUserParts();
			sPart.ReadData(br);
			sPartList.Add(sPart);
			
//Debug.Log( " part : idx : " + sPart.nIdx + "    code : " + sPart.nCode + "   state : " + sPart.nState );
			
		}
		
		//
		nCount = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		Debug.Log("KWUserItem Count : "+nCount);
		sItemList.Clear();
		for(int i=0;i<nCount;i++)
		{
			KWUserItem sItem = new KWUserItem();
			sItem.ReadData(br);
			sItemList.Add(sItem);
		}
		
	}
	
	public	void	WriteData( BinaryWriter bw )
	{
		bw.Write( nMainServerNum );
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nUserNum ) );
		SJStreamBuffer.WriteString_uni( bw , strUserID );
		SJStreamBuffer.WriteString_uni( bw , strGameID );
		bw.Write( nGrade );
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nExp ) );
		bw.Write( nTeam );
		SJStreamBuffer.WriteKWLONGLONG( bw , strGold );
		
		//
		int nCount = sPartList.Count;
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nCount ) );
		foreach( KWUserParts sParts in sPartList )
		{
			sParts.WriteData(bw);
		}
		
		//
		nCount = sItemList.Count;
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nCount ) );
		foreach( KWUserItem sItem in sItemList )
		{
			sItem.WriteData(bw);
		}
		
	}
	
	public	int 	GetWriteSize()
	{
		int size = 0;
		size += 1;
		size += 4;
		size += SJStreamBuffer.GetHeaderSizeUniString( strUserID );
		size += SJStreamBuffer.GetHeaderSizeUniString( strGameID );
		size += 1;
		size += 4;
		size += 1;
		size += SJStreamBuffer.GetSizeKWLONGLONG();
		
		//
		size += 4;
		foreach( KWUserParts sParts in sPartList )
		{
			size += sParts.GetWriteSize();
		}
		
		//
		size += 4;
		foreach( KWUserItem sItem in sItemList )
		{
			size += sItem.GetWriteSize();
		}	
		return size;
	}
	
	
	public	bool FindPartHave( int nNetPartCode )
	{
		foreach( KWUserParts part in sPartList )
		{
			if( part.nCode == nNetPartCode )
				return true;
		}
		return false;
	}
	
}

public	class KWMine
{
	public	KWNetUser 	sUser = new KWNetUser();
	public	int 		nMoveNum;
	public	string		strCash;
	public	Int64		nCash;
	
	public	UInt32		ConfirmNum1;
	public	UInt32		ConfirmNum2;
	public	int			ConfirmNum3;
	public	int			ConfirmNum4;
	
	public	void	ReadData( BinaryReader br )
	{
		sUser.ReadData(br);
		nMoveNum	= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		SJStreamBuffer.ReadKWLONGLONG( br , ref strCash );
		
		ConfirmNum1 = (UInt32)IPAddress.NetworkToHostOrder( (int)br.ReadUInt32());
		ConfirmNum2 = (UInt32)IPAddress.NetworkToHostOrder( (int)br.ReadUInt32());
		ConfirmNum3 = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		ConfirmNum4 = (int)IPAddress.NetworkToHostOrder( (int)br.ReadInt32());
		
		//Debug.Log( "KWMine  nMoveNum : " + nMoveNum );
	}
	
	public	void	WriteData( BinaryWriter bw )
	{
		sUser.WriteData( bw );
		bw.Write( (int)IPAddress.HostToNetworkOrder( (int)nMoveNum ) );
		SJStreamBuffer.WriteKWLONGLONG( bw , strCash );
		
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)ConfirmNum1 ) );			//	4
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)ConfirmNum2 ) );			//	4
		bw.Write( (Int32)IPAddress.HostToNetworkOrder( (int)ConfirmNum3 ) );			//	4
		bw.Write( (Int32)IPAddress.HostToNetworkOrder( (int)ConfirmNum4 ) );			//	4
	}
	
	public	int	GetWriteSize()
	{
		int size = 0;
		
		size = sUser.GetWriteSize();
		size += 4;
		size += SJStreamBuffer.GetSizeKWLONGLONG();
		size += 16;
		
		return size;
	}
}

public	class KWServer
{
	public	byte		nType;
	public	UInt16		nNum;
	public	UInt16		nPort;
	public	UInt16		nMaxConnectUser;
	public	UInt16		nCurConnectUser;
	public	string		strIP;
	public	string		strName;
	
	public	void	ReadData( BinaryReader br )
	{
		nType 	= br.ReadByte();
		nNum 	= (UInt16)IPAddress.NetworkToHostOrder( (short) br.ReadUInt16() );
		nPort	= (UInt16)IPAddress.NetworkToHostOrder( (short) br.ReadUInt16() );
		nMaxConnectUser = (UInt16)IPAddress.NetworkToHostOrder( (short)br.ReadUInt16() );
		nCurConnectUser = (UInt16)IPAddress.NetworkToHostOrder( (short)br.ReadUInt16() );
		
		//Debug.Log( "KWServer  nType : " + nType );
		//Debug.Log( "KWServer  nNum 	: " + nNum  );
		//Debug.Log( "KWServer  nPort : " + nPort  );
		//Debug.Log( "KWServer  nMaxConnectUser : " + nMaxConnectUser  );
		//Debug.Log( "KWServer  nCurConnectUser : " + nCurConnectUser  );
		
		SJStreamBuffer.ReadString_uni(br, ref  strIP );
		
		//Debug.Log( "KWServer  strIP : " + strIP  );
		
		SJStreamBuffer.ReadString_uni(br,ref  strName );
		
		//Debug.Log( "KWServer  strName : " + strName  );
	}
}

public	class KWRoom
{
	public	int 	nNum;
	public	string	strName;
	public	string	strPW;
	public	int 	nRoomMasterNum;
	public	byte	nRoomState;
	public	byte	nGameMode;
	public	byte	nTeamGameMode;
	public	UInt16	nStageNum;
	public	byte	nEnterLimitLevel;
	public	byte	nPlayTime;
	public	byte	nItemMode;
	public	int[] 	nUserPlaceAr = new int[8];
	
	public	List< KWNetUser >	sUserDataList = new List<KWNetUser>();
	
	public	void	ReadData( BinaryReader br )
	{
		nNum 				= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		SJStreamBuffer.ReadString_uni(br, ref  strName );
		SJStreamBuffer.ReadString_uni(br, ref  strPW );
		nRoomMasterNum 		= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		nRoomState 			= br.ReadByte();
		nGameMode 			= br.ReadByte();
		nTeamGameMode 		= br.ReadByte();
		nStageNum 			= (UInt16)IPAddress.NetworkToHostOrder( (short) br.ReadUInt16() );
		nEnterLimitLevel 	= br.ReadByte();
		nPlayTime 			= br.ReadByte();
		nItemMode 			= br.ReadByte();
		
		//Debug.Log( "KWRoom  strName : " + strName  );
		
		for(int i = 0 ; i < 8 ; i++ )
		{
			nUserPlaceAr[i] = br.ReadInt32();
		}
		
		int nC = (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		
		//Debug.Log( "KWRoom  nC : " + nC  );
		
		sUserDataList.Clear();
		
		for(int i = 0 ; i < nC ; i++ )
		{
			KWNetUser sUser = new KWNetUser();
			sUser.ReadData(br);
			sUserDataList.Add(sUser);
		}
	}
	
	public	void	AddUser( KWNetUser sUser )
	{
		sUserDataList.Add(sUser);
	}
	
	public	void	DelUser( KWNetUser sUser )
	{
		foreach( KWNetUser s_user in sUserDataList )
		{
			if( s_user.nUserNum == sUser.nUserNum )
			{
				sUserDataList.Remove( s_user );
				break;
			}
		}
	}
	
	public	KWNetUser GetUser( int nUserNum )
	{
		foreach( KWNetUser s_user in sUserDataList )
		{
			if( s_user.nUserNum == nUserNum )
			{
				return s_user;
			}
		}
		
		return null;
	}
	
}


public	class KWGameResult
{
	public	byte	nMainNum;
	public	string	strServerNum;
	public	byte	nRanking;
	public	byte	nGoalRanking;
	public	int 	nTakePoint;
	public	int 	nTakeExp;
	public	int 	nTakeGold;
	
	public	void	ReadData( BinaryReader br )
	{
		nMainNum 			= br.ReadByte();
		SJStreamBuffer.ReadString_uni(br, ref  strServerNum );
		nRanking 			= br.ReadByte();
		nGoalRanking		= br.ReadByte();
		nTakePoint			= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		nTakeExp			= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
		nTakeGold			= (int)IPAddress.NetworkToHostOrder( (int) br.ReadInt32() );
	}
}




public class SJNetBuffPrc : ISJAllocObj
{
	public byte[] 	recvBuffer = new byte[2048];
	
	public MemoryStream 	m_ms;
	public BinaryReader 	m_br;
	
	public 	byte		m_tcp_udp;
	public 	byte 		m_high , m_low;
	public 	int 		m_result;
	public	uint		m_UID;
	
	public	UInt32		m_PacketSize;
	
	

	public	byte[]			p_recv_debug;

	override	public	ISJAllocObj NewObj()
	{
		return (ISJAllocObj)(new SJNetBuffPrc());
	}
	
    override	public void OnAlloc()
	{
		//Debug.Log( "SJNetBuffPrc" );
	}
	
	
	public	uint	CopyRecvBuffKW( BinaryReader br )
	{
		m_high 		=	br.ReadByte();
		m_low 		=	br.ReadByte();
		m_PacketSize= 	br.ReadUInt32();
		
//		Debug.Log( "CopyRecvBuffKW m_high + " + m_high );
//		Debug.Log( "CopyRecvBuffKW m_low + " + m_low );
//		Debug.Log( "CopyRecvBuffKW m_PacketSize + " + m_PacketSize );
		
		recvBuffer	= 	br.ReadBytes( (int)(m_PacketSize - 6) );
		
		m_ms = new MemoryStream(recvBuffer,false); 
		m_br = new BinaryReader(m_ms);
		
		return m_PacketSize;
		/*
		MemoryStream ms_s = new MemoryStream(recvBuffer,true); 
		BinaryWriter bw_s = new BinaryWriter(ms_s);
		bw_s.Write(srcBuff ,0, nRecvSize);
		bw_s.Close();
		ms_s.Close();
		
		m_ms = new MemoryStream(recvBuffer,false); 
		m_br = new BinaryReader(m_ms);
		
		m_high 		=	m_br.ReadByte();
		m_low 		=	m_br.ReadByte();
		m_PacketSize= 	m_br.ReadUInt32();*/
	}
	
	public	void	CloseStream()
	{
		m_br.Close();
		m_ms.Close();
	}
}

//=============================================================================
//
// 리시브 함수
//


public class _SJNetHead
{
	public	int high;
	public	int low;
}

public	class _SJNetHead_Equal : EqualityComparer< _SJNetHead >
{
	public override bool Equals(_SJNetHead s1 , _SJNetHead s2 )
	{
		if ( s1.high == s2.high && s1.low == s2.low )
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public override int GetHashCode(_SJNetHead sh)
	{
		int hCode = sh.high ^ sh.low;
		return hCode.GetHashCode();
	}
}


public class _SJRecvObjFunc
{
	public	GameObject	recv_obj;
	public	string 		func;
}


public	class _SJRecv_Ref
{
	public	_SJNetHead			head;
	public	BinaryReader		br;
}

public	enum _SJ_NET_EVENT
{
	NONE  = 0,
	SUCC  = 1,
	FAILE = 2
}


//=============================================================================
public class SJNetPrc 
{
	
	public	bool	m_bConnectNet = false;
	public	bool	m_bOnline = false;
	
	
	private 		Socket sock 	= null;
	//private 		Socket sock_udp = null;
	private byte[] 	recvBuffer = new byte[10240];
	private byte[] 	sendBuffer = new byte[10240];
	
	
	private static 	SJNetPrc instance;
	//EndPoint		udp_endPoint_Server , endpoint_Client;
	
	//=========================================================================
	public 	bool				isWait = false;
	public	int					LowEvent = -1;
	

	public	List< SJNetBuffPrc >			m_RecvPacket;	
	public 	SJAllocObjMng< SJNetBuffPrc >	m_RecvPacketMng;
	public  System.Object 					m_SyncPacket;
	
	public	List< _SJ_NET_EVENT >			m_SJNetEvent;
	
	string	server_tcp_ip;
	int 	server_tcp_port;
	

	//=========================================================================
	
	MemoryStream ms_tcp_send;
	BinaryWriter bw_tcp_send;
	
	MemoryStream ms_udp_send;
	BinaryWriter bw_udp_send;
	
	
	SJSerialBuffer				m_sjSerialBuff = new SJSerialBuffer();
	
	UInt32 						m_nCurPacketSize = 0;
	bool 						m_bRecvNextBuff = false;
	SJSerialBuffer				m_sjSB_NextRecvBuff = new SJSerialBuffer();
	
	// KW
	public	int  				m_ConnectedAfter_Type = 0;
	

	public	KWConnect			m_sKwConnectInf 		= new KWConnect();
	public 	KWMine				m_sKwMine				= new KWMine();
	public	List<KWServer>		m_sKwServerList 		= new List<KWServer>();
	public	List<KWNetUser> 	m_sKwLobbyWaitUserList 	= new List<KWNetUser>();
	public	List<KWRoom>		m_sKwRoomList			= new List<KWRoom>();
	public	KWRoom				m_sKwCurRoom 			= new KWRoom();
	public	List<KWGameResult>	m_sKwResultList			= new List<KWGameResult>();
	public	List<KWItemDB>		m_sKwItemDBList			= new List<KWItemDB>();
	public	List<KWPartDB>		m_sKwPartDBList			= new List<KWPartDB>();
	
	
	public	KWNetUser			m_psKwUser_Self;	
	
	public	string				m_strLogin_UserID;
	public	string				m_strLogin_UserPW;
	
	//
	public	byte				m_nEnterRoomResult = 0;
	public	byte				m_nOnRecv_OtherLeaveRoom_isLobbyEnter = 0;
	public	int					m_nOnRecv_GamePlayTime_Time = 0;
	public	byte				m_nOnRecv_GameStart = 0;
	public	KWNetUser			m_psUser_CurEvent;
	
	//
	public	byte				m_nResult_CurEvent = 0;
	
	//
	public	GameObject			go_Recv_obj_main = null;
	SJNetRecv_Base				sjRecv_obj_main =  null;
	public	GameObject			go_Recv_obj_sub = null;
	
	public	_SJNetHead_Equal	m_eqSJNetHead	= new _SJNetHead_Equal();
	public	Dictionary<_SJNetHead,_SJRecvObjFunc> 	m_dOnRecvObj;
	
	// 테스트 , 넷 회선 평가
	public	SJ_AverageScore		SJ_AverageScore = new SJ_AverageScore();

	public	float				time_LastRecv;
	
	public enum USER_ROOM_STATE
	{
		LOAD_START 	= 2,
		LOAD_END	= 3,
		GAME_START 	= 4,
		GAME_END	= 5
	}
	
	public  SJNetPrc()
	{
		if(instance != null)
		{
			Debug.LogError("Cannot have two instances of singleton. Self destruction in 3...");
			return;
		}
		instance 	= this;
		
		m_RecvPacket  	= new List<SJNetBuffPrc>();
		m_RecvPacketMng = new SJAllocObjMng<SJNetBuffPrc>();
		SJNetBuffPrc sjPacket = new SJNetBuffPrc();
		m_RecvPacketMng.AllocObj( sjPacket , 50 );
		
		m_SyncPacket = new System.Object();
		
		m_SJNetEvent = new List<_SJ_NET_EVENT>();
		
		m_sjSerialBuff.Create();
		m_sjSB_NextRecvBuff.Create();
		
		
		m_dOnRecvObj = new Dictionary<_SJNetHead, _SJRecvObjFunc>(m_eqSJNetHead);
		

		// 회선 평가 테스트...
		SJ_AverageScore.Clear();
		SJ_AverageScore.Add_AverageScore( -1 , 0 );
		SJ_AverageScore.Add_AverageScore( 0.5f , 1 );
		SJ_AverageScore.Add_AverageScore( 0.3f , 10 );
		SJ_AverageScore.Add_AverageScore( 0.2f , 100 );
		SJ_AverageScore.Ready();
	}

	public static SJNetPrc Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new SJNetPrc();
			}
			return instance;
		}
	}
	
	public	static	bool	Is_Online()
	{
		return Instance.m_bOnline;
	}

	public	static	bool	Is_OffLine()
	{
		return !Instance.m_bOnline;
	}
	
	public	void	SetTCP_Port_RecvObj( string ip , int port , GameObject	recv_obj_main , GameObject	recv_obj_sub )
	{
		server_tcp_ip 	= ip;
		server_tcp_port = port;
		go_Recv_obj_main= recv_obj_main;
		//go_Recv_obj_sub = recv_obj_sub;
		
		sjRecv_obj_main = go_Recv_obj_main.GetComponent<SJNetRecv_Base>();
	}
	
//	public	 void 	SetRecvObjSub( GameObject	recv_obj_sub )
//	{
//		go_Recv_obj_sub = recv_obj_sub;
//	}

	
	public	BinaryWriter GetTCP_Buff()
	{
		ms_tcp_send = new MemoryStream(sendBuffer,true); 
		bw_tcp_send = new BinaryWriter(ms_tcp_send);
		
		return bw_tcp_send;
	}
	
	public	void	SendTCPServer_Buff( int nSize )
	{
		if( sock == null )
			return;
		
		bw_tcp_send.Close();
		ms_tcp_send.Close();
		
		sock.Send( sendBuffer , nSize , SocketFlags.None );
	}
	
	public	void	Disconnect()
	{
		sock.Disconnect( false );
		//sock_udp.Disconnect( false );
		//sock_udp.Close();

		m_bConnectNet = false;
		m_bOnline = false;
	}
	
	
	internal bool 	ConnectServer( string ip = "" , int port = -1 )
	{
		sock = new Socket(
							AddressFamily.InterNetwork,
							SocketType.Stream,
							ProtocolType.Tcp );
		
		if( string.IsNullOrEmpty(ip) == false && port > 0 )
		{
			server_tcp_ip = ip;
			server_tcp_port = port;
		}

		Debug.Log( "Connect try  " + server_tcp_ip + "   " + server_tcp_port );
		
		try
		{
			sock.Connect( server_tcp_ip, server_tcp_port );
		}
		catch(SocketException e)
		{
			Debug.Log( " sock.Connect( server_tcp_ip, server_tcp_port )  SocketException :  " + e.ToString() );	
			
			//go_Recv_obj_main.SendMessage( "OnConnectServer_Error",null,SendMessageOptions.DontRequireReceiver );
			
			m_SJNetEvent.Add( _SJ_NET_EVENT.FAILE );

			return false;
		}
		
		if( sock.Connected )
		{
			Debug.Log( "Connected" );

			m_SJNetEvent.Add( _SJ_NET_EVENT.SUCC );
			
			m_bConnectNet = true;
			m_bOnline	  = true;
			
			//go_Recv_obj_main.SendMessage( "OnConnectServer",m_ConnectedAfter_Type,SendMessageOptions.DontRequireReceiver );
			
			sock.BeginReceive( 
								recvBuffer,
								0,
								recvBuffer.Length,
								SocketFlags.None,
								new AsyncCallback( ReceiveComplete ),
								null 
			                  );
		}
		else
		{
			Debug.Log( "Fail to connect" );
			return false;
		}  
		
		isWait = false;
		return true;
	}
	
	internal void 	ConnectServer_Async( GameObject go_recv = null )
	{
		Debug.Log( "Async Connect try  " + server_tcp_ip + "   " + server_tcp_port );

		if( go_recv != null ) go_Recv_obj_sub = go_recv;
		
		sock = new Socket(
							AddressFamily.InterNetwork,
							SocketType.Stream,
							ProtocolType.Tcp );
		
//		try
//		{
			sock.BeginConnect( server_tcp_ip, server_tcp_port , new AsyncCallback( OnCollbackBeginConnect ) , sock );
//		}
//		catch(SocketException e)
//		{
//			Debug.Log( "Async  sock.Connect( server_tcp_ip, server_tcp_port )  SocketException :  " + e.ToString() );	
//			
//			go_Recv_obj_main.SendMessage( "OnConnectServer_Error",null,SendMessageOptions.DontRequireReceiver );
//			
//			return;
//		}
		
		//AndroidManager.Instance.LogAndroid( " ConnectServer_Async~~~" );
		
	}
	
	
	public void OnCollbackBeginConnect(IAsyncResult ar)
	{
		
		//AndroidManager.Instance.LogAndroid( " OnCollbackBeginConnect~~~" );
		
        try 
		{
           	// Retrieve the socket from the state object.
            Socket client = (Socket) ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);
        } 
		catch (Exception e) 
		{
			Debug.Log( "OnCollbackBeginConnect Connected  error!!! : " + e.ToString() );	
			
			
			// 
			// 샌드 메세지는 메인 쓰레드만 가능..
			//
			// 이벤트큐로 처리.
			//
			//go_Recv_obj_main.SendMessage( "OnConnectServer_Error",null,SendMessageOptions.DontRequireReceiver );
			
			lock(m_SyncPacket)
			{
				m_SJNetEvent.Add( _SJ_NET_EVENT.FAILE );
			}
			
			
			return;
        }
		
		Debug.Log( "OnCollbackBeginConnect Connected" );
		
		lock(m_SyncPacket)
		{
			m_SJNetEvent.Add( _SJ_NET_EVENT.SUCC );
		}
		
		m_bConnectNet = true;
		m_bOnline	  = true;

		sock.BeginReceive( 
							recvBuffer,
							0,
							recvBuffer.Length,
							SocketFlags.None,
							new AsyncCallback( ReceiveComplete ),
							null 
		                  );
		
		
		isWait = false;
		
	}
		
	void ReceiveComplete( IAsyncResult ar )
	{

		try
		{
			if( null == sock )
			    return;
			int len = sock.EndReceive( ar );
			if( len == 0 )
			{

			    //Shutdown();
			}
			else
			{
				//Debug.Log( "ReceiveComplete : " +len.ToString() );

				if( len < 6 )
				{
					Debug.Log( "ReceiveComplete len < 6 !!!!!!" );
				}
				
				
				MemoryStream	ms = new MemoryStream(recvBuffer,false); 
				BinaryReader	br = new BinaryReader(ms);
				
				int nTotalRead = 0;
				
				int len_Remain = len;
				
				while(true)
				{
					if( len < 6 )
					{
						break;
					}
					
					
					if( m_bRecvNextBuff == false )
					{
						byte 	high 		=	br.ReadByte();
						byte 	low 		=	br.ReadByte();
						UInt32 	PacketSize	= 	br.ReadUInt32();
						
						//Debug.Log( "Recv : high : " + high + ":    low : "+ low + " :      PacketSize : " + PacketSize );

						//
						// 모자라면 임시 버퍼에 넣는다.
						//
						if( len_Remain < PacketSize )
						{
							Debug.Log( "ReceiveComplete len < packet.m_PacketSize!!!! : " + len + " : " + PacketSize );
							m_bRecvNextBuff = true;
							m_nCurPacketSize = PacketSize;
							m_sjSB_NextRecvBuff.Start();
							m_sjSB_NextRecvBuff.GetBW().Write(recvBuffer,0,len);
							break;
						}else{
							ms.Seek(-6 , SeekOrigin.Current);
						}
							
					}else{
						
						//
						// 받은걸 계속 넣는다.
						//
						m_sjSB_NextRecvBuff.GetBW().Write(recvBuffer,0,len);
						
						//Debug.Log( "ReceiveComplete   countinue~~~~~~   " );
						
						//
						// 다 받았는지..
						//
						
						if( m_sjSB_NextRecvBuff.GetTotalWriteSize() < m_nCurPacketSize )
						{
							//
							// 아직 부족.
							//
							
							Debug.Log( "ReceiveComplete   countinue~~~~~~  m_sjSB_NextRecvBuff.GetTotalWriteSize() < m_nCurPacketSize  : " + m_sjSB_NextRecvBuff.GetTotalWriteSize().ToString() + " : " + m_nCurPacketSize.ToString() );
							
							break;
						}else{
							//
							// ok....
							//
							
							Debug.Log( "ReceiveComplete   countinue~~~~~~  complete " );
							m_bRecvNextBuff = false;
							ms = new MemoryStream(m_sjSB_NextRecvBuff.GetBuff(),false); 
							br = new BinaryReader(ms);
						}
					}
					
					SJNetBuffPrc packet = null;
					lock(m_SyncPacket)
					{
						packet = m_RecvPacketMng.GetNewObj();
					}
					
					if( packet == null )
					{
						Debug.Log( "error SJNetBuffPrc packet = m_RecvPacketMng.GetNewObj()" );
						break;
					}
					
					int nCurRead = (int)packet.CopyRecvBuffKW( br );
					nTotalRead += nCurRead;
					
					lock(m_SyncPacket)
					{
						packet.p_recv_debug = recvBuffer;
						m_RecvPacket.Add( packet );

						// 기타 처리
						SJ_AverageScore.Add_Score( (int)packet.m_PacketSize );
						//time_LastRecv =	Time.realtimeSinceStartup;	
					}
					
					if( nTotalRead >= len )
					{
						break;
					}
					
					len_Remain -= nCurRead;
					
					//Debug.Log( "ReceiveComplete Remian Packet : " + len + " : " + nTotalRead );
				}
				
				
				
				br.Close();
				ms.Close();
				
			    sock.BeginReceive(
						recvBuffer,
						0,
						recvBuffer.Length,
						SocketFlags.None,
						new AsyncCallback( ReceiveComplete ),
						null );
			}
		}
		catch( Exception e )
		{
			Debug.Log(e.Message);
		}
	}


	public	SJNetBuffPrc	GetPop_RecvPacket()
	{
		SJNetBuffPrc p = null;
		lock(m_SyncPacket)
		{
			if( m_RecvPacket.Count > 0 )
			{
				p = m_RecvPacket[0];
				m_RecvPacket.RemoveAt(0);
			}			
		}
		return p;
	}


	void SendComplete( IAsyncResult ar )
	{
		try
		{
			if( null == sock )
			    return;
			int len = sock.EndSend( ar );
			if( len == 1 )
			{
			    //ConsoleMessage( "Send success" );
				//Debug.Log("Send success");
			}
		}
		catch( Exception e )
		{
			//ConsoleMessage( "Exception: " + e.Message );
			//Shutdown();
			Debug.Log( e.Message );
		}
	}
	
	
	//
	public	BinaryWriter 	GetSJBuff_BW()
	{
		return m_sjSerialBuff.GetBW_Begin();
	}
	
	
	//public	BinaryWriter 	GetSJBuff_BW_RoomBroad()
	//{
	//	return m_sjSerialBuff.GetBW_Begin_KW_RoomBroad();
	//}
	
	//public	BinaryWriter 	GetSJBuff_BW_RoomBroad_NetID()
	//{
	//	return m_sjSerialBuff.GetBW_Begin_KW_RoomBroad_NetID();
	//}
	
	
	
	public	int  	SendSJBuff( byte high , byte low , GameObject	recv_obj = null , string recv_func = "" , 
		bool recv_other = false , byte recv_high = 0 , byte recv_low = 0 )
	{
		if( sock == null )
		{
			return 0;
		}
		m_sjSerialBuff.BuffPacket_End( high , low );

		try 
		{
			int r =	sock.Send( m_sjSerialBuff.GetBuff() , (int)m_sjSerialBuff.GetPacket_End() , SocketFlags.None );
		}catch (SocketException e)
		{
			//Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
			Debug.Log( "sending socket error : " + e.Message + " : " + e.ErrorCode );
		}

		if( recv_obj != null )
		{
			_SJNetHead sjhead = new _SJNetHead();

			if( recv_other == false )
			{
				sjhead.high = high;
				sjhead.low 	= low;
			}else{
				sjhead.high = recv_high;
				sjhead.low 	= recv_low;
			}

			_SJRecvObjFunc 	recv_obj_func = new _SJRecvObjFunc();
			recv_obj_func.recv_obj = recv_obj;
			recv_obj_func.func = recv_func;

			m_dOnRecvObj[sjhead] = recv_obj_func;
		}

		return 1;
	}
	
	//public	void 	SendSJBuff_KWRoom( byte nEvent )
	//{
	//	if( sock == null )
	//		return;
	//	m_sjSerialBuff.BuffPacket_End_KW_RoomBroad( nEvent );
	//	sock.Send( m_sjSerialBuff.GetBuff() , (int)m_sjSerialBuff.GetPacket_End() , SocketFlags.None );
	//}
	
	//public	void 	SendSJBuff_KWRoom_NetID( byte nEvent )
	//{
	//	if( sock == null )
	//		return;
	//	m_sjSerialBuff.BuffPacket_End_KW_RoomBroad_NetID( nEvent );
	//	sock.Send( m_sjSerialBuff.GetBuff() , (int)m_sjSerialBuff.GetPacket_End() , SocketFlags.None );
	//}
	
	// KW func
	public	KWRoom 	GetKwRoom(int nNum)
	{
		foreach( KWRoom s_room in m_sKwRoomList )
		{
			if( s_room.nNum == nNum )
			{
				return s_room;
			}
		}
		return null;
	}




	//
	/// <summary>
	/// Old---
	/// </summary>
	/// 

	public	void	SetLoginIDPW( string	strUserID , string	strUserPW )
	{
		m_strLogin_UserID = strUserID;
		m_strLogin_UserPW = strUserPW;
	}
	
	// KW protocol
	// 172.30.1.10/11100
	public	void	SendLogin()
	{
		isWait = true;
		Debug.Log("Send_Login 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		
		string	str_id , str_pw;
		str_id = m_strLogin_UserID + "\0";
		str_pw = m_strLogin_UserPW + "\0";
		
		nSendSize += 6; // head
		
		nSendSize += 3; // 4

		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString(str_id);
		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString(str_pw);
		nSendSize += 2; 
		
		Debug.Log("Send_Login nSendSize : " + nSendSize );
		
		bw.Write( (byte)2 );	//	1
		bw.Write( (byte)2 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		// 6
		
		bw.Write( (byte)1 );// connect type		//	1
		bw.Write( (byte)2 );// server version	//	1
		bw.Write( (byte)0 );// connect result	//	1
		// 3
		
		// userid1~99/1111
		SJStreamBuffer.WriteString_uni( bw , str_id); // user id		18	
		SJStreamBuffer.WriteString_uni( bw , str_pw); // pw				12	
		SJStreamBuffer.WriteString_uni( bw , ""); // find id				2	
		
		SendTCPServer_Buff( (int)nSendSize );
		
		Debug.Log("Send_Login 22");
	}
	
	public	void	SendLogin_SJBuff()
	{
		BinaryWriter bw = GetSJBuff_BW();
		
		string	str_id , str_pw;
		str_id = m_strLogin_UserID + "\0";
		str_pw = m_strLogin_UserPW + "\0";
		
		bw.Write( (byte)1 );// connect type		//	1
		bw.Write( (byte)2 );// server version	//	1
		bw.Write( (byte)0 );// connect result	//	1
		
		SJStreamBuffer.WriteString_uni( bw , str_id); 	// user id		18	
		SJStreamBuffer.WriteString_uni( bw , str_pw); 	// pw			12	
		SJStreamBuffer.WriteString_uni( bw , ""); 		// find id		2
		
		SendSJBuff( 2 , 2 );
	}
	
	
	public	void SendLoginGame()
	{
		isWait = true;
		Debug.Log("SendLoginGame 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		nSendSize += (uint)m_sKwConnectInf.GetWriteSize();
		nSendSize += (uint)m_sKwMine.GetWriteSize();
		
		
		bw.Write( (byte)3 );	//	1
		bw.Write( (byte)1 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		m_sKwConnectInf.WriteData( bw );
		m_sKwMine.WriteData( bw );
		
		SendTCPServer_Buff( (int)nSendSize );
		
		Debug.Log("SendLoginGame 22");
	}
	

	public	void SendMakeRoom()
	{
		isWait = true;
		Debug.Log("SendMakeRoom 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString( "다 같이 쇼핑 한번^^!!! \0" );
		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString( "" );
		nSendSize += 2;
		nSendSize += 2;
		nSendSize += 4;
		
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)1 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SJStreamBuffer.WriteString_uni( bw , "다 같이 쇼핑 한번^^!!! \0");
		SJStreamBuffer.WriteString_uni( bw , "");
		
		bw.Write( (byte)0 );	// game mode
		bw.Write( (byte)0 );	// team game mode
		bw.Write( (UInt16)0 );	// stage num
		bw.Write( (byte)0 );	// enter limit level
		bw.Write( (byte)1 );	// play time
		bw.Write( (byte)0 );	// item mode
		bw.Write( (byte)8 );	// max user count
		
		SendTCPServer_Buff( (int)nSendSize );
		
		Debug.Log("SendMakeRoom 22");
	}
	
	
	public	void	SendMakeRoom_SJBuff()
	{
		BinaryWriter bw = GetSJBuff_BW();
		
		SJStreamBuffer.WriteString_uni( bw , "다 같이 쇼핑 한번^^!!! \0");
		SJStreamBuffer.WriteString_uni( bw , "");
		
		bw.Write( (byte)0 );	// game mode
		bw.Write( (byte)0 );	// team game mode
		bw.Write( (UInt16)0 );	// stage num
		bw.Write( (byte)0 );	// enter limit level
		bw.Write( (byte)1 );	// play time
		bw.Write( (byte)0 );	// item mode
		bw.Write( (byte)8 );	// max user count
		
		SendSJBuff( 4 , 1 );
		
		
		MemoryStream 	ms = new MemoryStream( m_sjSerialBuff.GetBuff() , false );
		BinaryReader	br = new BinaryReader( ms );
		
		byte 	high = br.ReadByte();
		byte 	low = br.ReadByte();
		int 	size = (int)IPAddress.NetworkToHostOrder( br.ReadInt32() );
		
		
		string str = "";
		SJStreamBuffer.ReadString_uni( br , ref str );
		
		
		//Debug.Log( " aaaa :  " + high.ToString() + "   " + low.ToString() + "    " + size.ToString() + "   " + str );
	}
	
	
	
	
	public	void	SendEnterRoom( KWRoom sRoom , string	strPW )
	{
		isWait = true;
		Debug.Log("SendEnterRoom 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		nSendSize += 4;
		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString( strPW );
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)5 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		bw.Write( (int) IPAddress.HostToNetworkOrder( (int) sRoom.nNum ) );
		SJStreamBuffer.WriteString_uni( bw , strPW);
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendEnterRoom 22");
	}
	
	public	void	SendLeaveRoom()
	{
		isWait = true;
		Debug.Log("SendLeaveRoom 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)7 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendLeaveRoom 22");
	}
	
	public	void	SendGameRoomUserReadyState()
	{
		isWait = true;
		Debug.Log("SendGameRoomUserReadyState 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)11 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendGameRoomUserReadyState 22");
	}
	
	public	void	SendReqGameStart()
	{
		isWait = true;
		Debug.Log("SendReqGameStart 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)15 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendReqGameStart 22");
	}
	
	
	public	void	SendGameLoadComplete()
	{
		isWait = true;
		Debug.Log("SendReqGameStart 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)101 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendReqGameStart 22");
	}
	
	
	public	void	SendUserGameEnd()
	{
		isWait = true;
		Debug.Log("SendUserGameEnd 11");
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		
		bw.Write( (byte)4 );	//	1
		bw.Write( (byte)104 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
		
		SendTCPServer_Buff( (int)nSendSize );
		Debug.Log("SendUserGameEnd 22");
	}
	
	
	public	void	SendCreateNickName( string strNick )
	{
		Debug.Log("SendCreateNickName 11");
		
		isWait = true;
		UInt32 nSendSize = 0;
		BinaryWriter bw = GetTCP_Buff();
		
		nSendSize = 6;
		nSendSize += (uint)SJStreamBuffer.GetHeaderSizeUniString( strNick );
		
		bw.Write( (byte)2 );	//	1
		bw.Write( (byte)41 );	//	1
		bw.Write( (UInt32)IPAddress.HostToNetworkOrder( (int)nSendSize ) );	//	4 
	
		SJStreamBuffer.WriteString_uni( bw , strNick );
		
		SendTCPServer_Buff( (int)nSendSize );
		
		Debug.Log("SendCreateNickName 22");
	}
	
	
	
	//-------------------------------------------------------------------------
	
	public	bool	IsRoomMaster()
	{
		if( m_sKwCurRoom.nRoomMasterNum == m_sKwMine.sUser.nUserNum )
			return true;
		
		return false;
	}
	
	public	bool	IsRoomMaster( KWNetUser s_user )
	{
		if( m_sKwCurRoom.nRoomMasterNum == s_user.nUserNum )
			return true;
		
		return false;
	}
	
	public	bool	IsSelf( KWNetUser	s_user )
	{
		if( m_sKwMine.sUser.nUserNum == s_user.nUserNum )
		{
			return true;
		}
		return false;
	}

	//-------------------------------------------------------------------------
	
	static	public	void	Write_Vector3_Serialize( BinaryWriter bw , Vector3 v3 )
	{
		bw.Write( v3.x );
		bw.Write( v3.y );
		bw.Write( v3.z );
	}

	static	public	Vector3	Read_Vector3_Serialize( BinaryReader br )
	{
		Vector3 v3 = Vector3.zero;
		v3.x = br.ReadSingle();
		v3.y = br.ReadSingle();
		v3.z = br.ReadSingle();
		return v3;
	}

	static	public	void	Read_Vector3_Serialize_Position( BinaryReader br , Transform tr )
	{
		tr.position = Read_Vector3_Serialize(br);
	}

}
