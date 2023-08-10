﻿using UnityEngine;
using System.Text;
using SimpleJSON;

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;







public class SJ_JWT 
{

static public	
/* C:\Work\TileHeroLite_svn\Assets\Resources\key.pfx (2015-11-12 오후 4:09:02)
   StartOffset: 00000000, EndOffset: 000006BC, 길이: 000006BD */

// 모바일에서 바이너리 파일 읽을 방법이 없다..(라기 보단 www 로 받아서 하면 될꺼 같긴한데 번거롭다. 넷상에 파일 저장소도 있어야 하고.. ) 
// Resource.load 로 pfx 파일 바이너리 읽을수 없다. (검색하면 텍스트 에셋으로 하면 된다고 하는데 안댄다.)
// 그래서 그냥 파일 내용을 소스에 복사 했다. 
// 헥사 프로그램 에디터 사용해야 한다. (ex. HxD , 이거 안에 보면 메뉴로 c# 으로 복사 하기가 있다.)
// 
//// 2015-11-24 pfx




/* C:\Users\csj\Documents\네이트온 받은 파일\1.0.001.pfx (2015-11-27 오후 4:37:34)
   StartOffset: 00000000, EndOffset: 000006CC, 길이: 000006CD */

// 2015-11-27 psAd21dSqz1%
//byte[] rawData = {
//    0x30, 0x82, 0x06, 0xC9, 0x02, 0x01, 0x03, 0x30, 0x82, 0x06, 0x8F, 0x06,
//    0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82,
//    0x06, 0x80, 0x04, 0x82, 0x06, 0x7C, 0x30, 0x82, 0x06, 0x78, 0x30, 0x82,
//    0x03, 0x77, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07,
//    0x06, 0xA0, 0x82, 0x03, 0x68, 0x30, 0x82, 0x03, 0x64, 0x02, 0x01, 0x00,
//    0x30, 0x82, 0x03, 0x5D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D,
//    0x01, 0x07, 0x01, 0x30, 0x1C, 0x06, 0x0A, 0x2A, 0x86, 0x48, 0x86, 0xF7,
//    0x0D, 0x01, 0x0C, 0x01, 0x06, 0x30, 0x0E, 0x04, 0x08, 0xF9, 0x6B, 0x3D,
//    0xFE, 0xC1, 0x1A, 0xC5, 0x73, 0x02, 0x02, 0x08, 0x00, 0x80, 0x82, 0x03,
//    0x30, 0xCD, 0x64, 0xF2, 0xF2, 0x72, 0xA7, 0xB8, 0x56, 0x94, 0x9A, 0xCF,
//    0x84, 0xA2, 0xDC, 0x21, 0xEF, 0x45, 0xC8, 0x3C, 0xCF, 0x49, 0x0B, 0xC6,
//    0xE3, 0x2B, 0xD8, 0x67, 0xCB, 0x6C, 0x81, 0x3C, 0x3B, 0x18, 0xBE, 0xF0,
//    0x02, 0x85, 0x4D, 0x4B, 0xDC, 0xD3, 0xCF, 0x75, 0x94, 0x47, 0x16, 0xF9,
//    0x74, 0xAE, 0x2C, 0x4F, 0x1C, 0x97, 0xE9, 0x58, 0xA0, 0x6F, 0x75, 0xE3,
//    0xD1, 0xD7, 0xD8, 0x11, 0x31, 0x58, 0x25, 0x08, 0x1A, 0x31, 0x49, 0xA5,
//    0x60, 0x2A, 0x41, 0x8A, 0x30, 0x74, 0x02, 0xE6, 0xA5, 0x25, 0xE9, 0x62,
//    0xBB, 0xFA, 0x69, 0x23, 0x47, 0x9A, 0x46, 0x2E, 0x71, 0x57, 0xC3, 0xD2,
//    0xB5, 0x6A, 0x25, 0xF0, 0x16, 0x06, 0xC1, 0xD5, 0x0C, 0xA0, 0x79, 0xF9,
//    0xCE, 0xB3, 0xDF, 0xC2, 0xF9, 0xAA, 0x8C, 0xFB, 0x8C, 0x25, 0x40, 0x74,
//    0x43, 0x23, 0x85, 0x34, 0xFD, 0x97, 0xB0, 0xE1, 0xCC, 0xD4, 0xC4, 0xDB,
//    0x25, 0xEF, 0x0D, 0x0E, 0xF9, 0x97, 0x40, 0x06, 0xAA, 0x67, 0xA7, 0xBD,
//    0xEF, 0x77, 0xFD, 0x80, 0xDF, 0xF9, 0x87, 0x9C, 0x3D, 0xE9, 0x77, 0xDE,
//    0x5E, 0x3F, 0x7F, 0xFE, 0x7B, 0x0E, 0x25, 0xB7, 0x16, 0xD6, 0x11, 0xB2,
//    0x09, 0x9D, 0x9F, 0x62, 0x8A, 0x15, 0x30, 0xF7, 0xDB, 0x6A, 0x88, 0xBD,
//    0x6B, 0x4E, 0xD3, 0x43, 0xC3, 0x4A, 0x5D, 0xB8, 0x51, 0x7C, 0x8A, 0xDE,
//    0xC0, 0x04, 0x44, 0x27, 0xBE, 0x2A, 0xE9, 0x51, 0xAA, 0xDE, 0xDC, 0x62,
//    0x84, 0x99, 0x0C, 0x3C, 0xC7, 0xCE, 0x1B, 0x3B, 0x09, 0x50, 0x10, 0xAD,
//    0xF0, 0xED, 0x1A, 0xAE, 0xA7, 0xF3, 0xB4, 0x0E, 0x5C, 0x9D, 0x42, 0xB3,
//    0xDE, 0x14, 0xC9, 0xB9, 0x95, 0x08, 0xD2, 0xD3, 0x0A, 0xDB, 0x7D, 0x82,
//    0x52, 0x1A, 0x81, 0x2B, 0x46, 0x7F, 0x51, 0x8D, 0x8B, 0x0D, 0xBF, 0xD9,
//    0x00, 0x97, 0x80, 0x7D, 0xB9, 0xCA, 0xF7, 0x04, 0xD3, 0x92, 0x86, 0xC0,
//    0xC7, 0xAC, 0x10, 0x2C, 0x43, 0xD8, 0x36, 0x9A, 0x29, 0xF7, 0x71, 0x95,
//    0x94, 0xF1, 0xAD, 0x09, 0x45, 0x7F, 0x70, 0xBB, 0x40, 0x60, 0xEF, 0x56,
//    0x27, 0x06, 0x4A, 0x80, 0xE6, 0x7F, 0xE6, 0x81, 0x63, 0x12, 0x75, 0x9D,
//    0x6B, 0x3E, 0x85, 0xB1, 0xC9, 0x34, 0x3B, 0xBC, 0xAD, 0xD3, 0x24, 0x68,
//    0x8E, 0xEE, 0xB1, 0x90, 0x60, 0x4B, 0xAA, 0x75, 0x27, 0x6E, 0xBD, 0x3A,
//    0x0D, 0xE3, 0xA0, 0x22, 0x9F, 0xAD, 0x67, 0xC9, 0xA1, 0x5D, 0x36, 0xDC,
//    0x26, 0xC3, 0x60, 0x88, 0x90, 0x26, 0x63, 0x92, 0x66, 0xFB, 0xAC, 0x0C,
//    0x79, 0x90, 0xAD, 0x0A, 0x6B, 0xEC, 0xA8, 0xF6, 0x4C, 0xF4, 0xAA, 0xE1,
//    0x9E, 0x26, 0xFD, 0xE2, 0x76, 0xFC, 0xED, 0x4B, 0x1B, 0x82, 0x73, 0x15,
//    0x23, 0x04, 0xC0, 0x90, 0x6C, 0xBD, 0xA7, 0xDD, 0x7F, 0xCC, 0x1B, 0x1B,
//    0x70, 0x14, 0x36, 0xEA, 0x24, 0xC2, 0x03, 0x55, 0xE1, 0x68, 0x30, 0xA9,
//    0x41, 0xD1, 0xED, 0xD7, 0x4E, 0xE2, 0xC6, 0x40, 0x2F, 0xEA, 0x66, 0x41,
//    0x69, 0x1C, 0x61, 0xCC, 0x66, 0xE3, 0x3C, 0x26, 0xB8, 0x1A, 0x03, 0x6B,
//    0xD0, 0x6A, 0xA4, 0xB1, 0xB9, 0xE5, 0x0F, 0x2E, 0x43, 0xD3, 0x9F, 0x4B,
//    0x38, 0x9F, 0x4B, 0x42, 0x09, 0xE7, 0xA7, 0x1B, 0x68, 0x35, 0xFF, 0x38,
//    0x3F, 0xDE, 0x21, 0x4D, 0x79, 0x57, 0xE7, 0x86, 0x7F, 0x97, 0xFC, 0x4F,
//    0xD1, 0x27, 0xB8, 0x24, 0x1C, 0x3F, 0x16, 0x7E, 0xA5, 0x65, 0x18, 0x98,
//    0x90, 0x20, 0xAF, 0x46, 0xA7, 0xF8, 0xB8, 0xC6, 0xEE, 0x0B, 0x20, 0x7A,
//    0x66, 0xA2, 0x0F, 0x98, 0x95, 0x41, 0xC8, 0xA0, 0x02, 0xB4, 0xD5, 0xDA,
//    0x90, 0x2D, 0x32, 0x7A, 0x79, 0x4B, 0xD0, 0xE5, 0xE9, 0x66, 0xB4, 0x9D,
//    0xFB, 0x7F, 0x77, 0xDE, 0xFD, 0x80, 0xE6, 0x30, 0x2F, 0x0A, 0xD5, 0x32,
//    0xFD, 0x7E, 0x01, 0xAD, 0x50, 0xD1, 0x94, 0x03, 0x5C, 0x22, 0xE1, 0xF5,
//    0x60, 0xC9, 0x88, 0x49, 0x01, 0x90, 0x33, 0x46, 0xAA, 0xD8, 0x98, 0x51,
//    0x4F, 0x24, 0x82, 0x67, 0xBA, 0xD8, 0x74, 0x09, 0xF8, 0x08, 0x80, 0x35,
//    0x3E, 0x59, 0x0B, 0x23, 0x46, 0x0F, 0xB4, 0x1A, 0x06, 0xD6, 0xEF, 0x80,
//    0xD8, 0x86, 0x18, 0xEE, 0x48, 0x78, 0x80, 0xD7, 0xA6, 0x27, 0x19, 0xC1,
//    0x83, 0xFB, 0xA8, 0xE3, 0x28, 0xA5, 0x5A, 0x4E, 0x39, 0x6B, 0xEA, 0xA1,
//    0x22, 0xCA, 0x19, 0xB8, 0xA2, 0x46, 0x12, 0x67, 0x14, 0xF1, 0x26, 0x5F,
//    0xBE, 0x45, 0x24, 0x07, 0xFA, 0xB7, 0x43, 0xF7, 0xBB, 0x16, 0xAB, 0x7D,
//    0x90, 0x92, 0xF3, 0xA0, 0x51, 0x52, 0x7A, 0xAB, 0x57, 0x01, 0xD7, 0xE3,
//    0x85, 0x03, 0xF0, 0x5A, 0xED, 0xB8, 0x67, 0xEA, 0x2C, 0xBA, 0x38, 0xB4,
//    0xF5, 0x12, 0xB7, 0xAA, 0x09, 0x08, 0xB3, 0x4A, 0xC2, 0x94, 0xF6, 0xC0,
//    0xF4, 0x80, 0xA7, 0x5B, 0xF4, 0xE9, 0xAD, 0x6A, 0x70, 0x2B, 0xEB, 0xF0,
//    0xC5, 0xF3, 0x3A, 0xA9, 0x2B, 0xC5, 0x89, 0x3E, 0x1E, 0x53, 0xAC, 0x1D,
//    0x94, 0x6B, 0x20, 0x09, 0xDD, 0x4A, 0x47, 0x59, 0x32, 0x01, 0x0D, 0xB9,
//    0x81, 0x79, 0x12, 0xEB, 0xA0, 0xBB, 0xA3, 0x8B, 0xBD, 0x9F, 0x26, 0x76,
//    0x2B, 0x98, 0xF6, 0x7E, 0xAA, 0xC3, 0xB7, 0x36, 0x1F, 0x6F, 0x4B, 0x75,
//    0xF2, 0x03, 0x69, 0x67, 0xD9, 0x58, 0x49, 0xE5, 0xF7, 0x07, 0x50, 0xB6,
//    0x1A, 0xF2, 0xA5, 0x95, 0x4C, 0x99, 0xE6, 0x51, 0xA1, 0x2C, 0x79, 0x0B,
//    0x0D, 0x13, 0xAF, 0x58, 0xB2, 0x28, 0xBC, 0x74, 0x84, 0x9B, 0x09, 0xD1,
//    0x7F, 0x15, 0x4D, 0x0F, 0x82, 0x4F, 0x9F, 0xA7, 0xAB, 0x2D, 0xC0, 0xB6,
//    0x0A, 0x3D, 0xA2, 0xA9, 0xE7, 0x08, 0x20, 0xF7, 0x47, 0x95, 0x15, 0x24,
//    0x6E, 0xDD, 0x6B, 0x76, 0xB1, 0xBB, 0x09, 0xEB, 0x3D, 0xB9, 0x04, 0xFD,
//    0x41, 0xC4, 0xD3, 0x12, 0x67, 0x8D, 0xAC, 0x52, 0x10, 0xF7, 0x49, 0x20,
//    0x8C, 0x31, 0x7C, 0x9F, 0x02, 0x48, 0x7D, 0x98, 0x02, 0x91, 0x7A, 0xB8,
//    0xD3, 0x5A, 0x18, 0x4B, 0x7D, 0xA2, 0x0B, 0x0B, 0xE1, 0x8E, 0xBD, 0x2C,
//    0x5E, 0x30, 0x82, 0x02, 0xF9, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7,
//    0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82, 0x02, 0xEA, 0x04, 0x82, 0x02, 0xE6,
//    0x30, 0x82, 0x02, 0xE2, 0x30, 0x82, 0x02, 0xDE, 0x06, 0x0B, 0x2A, 0x86,
//    0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x0A, 0x01, 0x02, 0xA0, 0x82, 0x02,
//    0xA6, 0x30, 0x82, 0x02, 0xA2, 0x30, 0x1C, 0x06, 0x0A, 0x2A, 0x86, 0x48,
//    0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x01, 0x03, 0x30, 0x0E, 0x04, 0x08, 0x4E,
//    0x94, 0xC2, 0x3B, 0x34, 0xE3, 0x31, 0xF8, 0x02, 0x02, 0x08, 0x00, 0x04,
//    0x82, 0x02, 0x80, 0xE4, 0x38, 0x40, 0xC2, 0x85, 0x7D, 0xC5, 0xFF, 0x81,
//    0x2A, 0x48, 0xF9, 0x33, 0x98, 0x07, 0xAB, 0xFD, 0x6A, 0xBA, 0xCF, 0x5C,
//    0xB7, 0xD2, 0xD5, 0x28, 0x22, 0xE7, 0x88, 0x46, 0xAE, 0x40, 0x73, 0x21,
//    0x33, 0x29, 0xA9, 0x49, 0xBD, 0x7D, 0xB4, 0x49, 0x6C, 0x42, 0xB2, 0xA4,
//    0x41, 0x89, 0x5E, 0x4A, 0x8F, 0x31, 0xC6, 0xDE, 0x97, 0x06, 0x0E, 0x40,
//    0x29, 0xCB, 0x94, 0xD0, 0x19, 0xE2, 0xE6, 0xF1, 0x2F, 0xCC, 0x37, 0x98,
//    0xD7, 0x3A, 0x21, 0x89, 0x55, 0xD6, 0x15, 0x1D, 0x55, 0x6A, 0xBF, 0xF6,
//    0xB9, 0xEE, 0x92, 0xFC, 0x3C, 0xA5, 0x5B, 0x74, 0x0A, 0x27, 0xC5, 0xB8,
//    0x81, 0xF6, 0x1C, 0xC7, 0xFA, 0x47, 0x26, 0xAB, 0xF0, 0x42, 0x1C, 0x39,
//    0x3A, 0x36, 0x72, 0xBB, 0xB9, 0xBB, 0x57, 0x70, 0x89, 0x3B, 0x64, 0x43,
//    0x1A, 0x79, 0x71, 0x1B, 0x92, 0x80, 0xCE, 0xF1, 0xC5, 0x89, 0x53, 0x0C,
//    0xD2, 0x1E, 0x95, 0xCA, 0x78, 0x4F, 0xD1, 0xFE, 0x5E, 0x3B, 0x37, 0x4E,
//    0x10, 0xB7, 0xA9, 0x09, 0x35, 0x75, 0x37, 0xEF, 0xDB, 0x0E, 0x5D, 0x44,
//    0x0D, 0xD5, 0x37, 0x71, 0x82, 0x47, 0x41, 0xD2, 0xBC, 0xB9, 0x1E, 0x79,
//    0xEE, 0xF9, 0x90, 0x5D, 0x7A, 0x4E, 0x4D, 0xD3, 0xD6, 0x2E, 0xDD, 0xC0,
//    0x65, 0x07, 0xF8, 0xE5, 0x2F, 0xDF, 0x57, 0x12, 0x5E, 0x31, 0x59, 0x22,
//    0xB9, 0xFA, 0xE0, 0xCF, 0x88, 0xC3, 0x96, 0xF0, 0xE0, 0xE2, 0xB8, 0xBA,
//    0x3D, 0x7D, 0x4F, 0xD0, 0xCC, 0x2C, 0xC3, 0x70, 0xF8, 0x6D, 0xAC, 0x7F,
//    0x67, 0xE1, 0x14, 0x39, 0x81, 0x7D, 0xA4, 0x0A, 0xF4, 0x5C, 0x0F, 0x65,
//    0x13, 0x65, 0x47, 0x5D, 0x17, 0x5F, 0x1D, 0x56, 0x6F, 0xC0, 0x09, 0x44,
//    0x19, 0xC9, 0xAB, 0x57, 0xCC, 0x4A, 0x1B, 0xF7, 0x1F, 0xCC, 0x50, 0x1F,
//    0xC9, 0xB4, 0x45, 0x3E, 0xEB, 0x53, 0x0E, 0x6F, 0xCC, 0x21, 0x95, 0x7E,
//    0x8A, 0xF6, 0x53, 0x4A, 0x2C, 0x4C, 0x7C, 0x34, 0x62, 0x27, 0x5C, 0xDF,
//    0x2C, 0x62, 0x78, 0x93, 0x9C, 0x74, 0xB1, 0x21, 0x82, 0x09, 0xBA, 0x8D,
//    0xD0, 0x72, 0x90, 0x8A, 0x5F, 0x7F, 0xD2, 0x27, 0x3C, 0x2A, 0x5B, 0x08,
//    0x8C, 0xE3, 0xF2, 0x25, 0xF1, 0x42, 0xDE, 0xAF, 0x32, 0x6A, 0x7F, 0x21,
//    0x09, 0x57, 0xAA, 0x38, 0xE8, 0x0A, 0x7F, 0x21, 0x20, 0x63, 0x6D, 0x59,
//    0xA8, 0xA3, 0xB5, 0xA8, 0xC0, 0xBF, 0x21, 0xFA, 0xC8, 0x59, 0xBB, 0x58,
//    0x9B, 0x28, 0x7D, 0xC9, 0xEB, 0x11, 0x37, 0x86, 0xB6, 0x40, 0x57, 0xA3,
//    0x21, 0xE4, 0x04, 0x60, 0xEB, 0x68, 0xCE, 0x5A, 0x9C, 0x3F, 0x5C, 0x41,
//    0xDD, 0x17, 0x7A, 0x1A, 0x28, 0x61, 0xA5, 0x7F, 0x2C, 0x3E, 0x08, 0xF8,
//    0xD9, 0xF5, 0x75, 0x77, 0x18, 0x5D, 0xFB, 0x15, 0x81, 0x43, 0x34, 0x5A,
//    0xB6, 0xDF, 0x8A, 0x35, 0x7C, 0x6D, 0xCC, 0xC1, 0x60, 0x2A, 0x38, 0xB5,
//    0xC4, 0xD0, 0x1E, 0xC2, 0x16, 0x58, 0xE1, 0x59, 0xFD, 0xC4, 0x56, 0x32,
//    0x56, 0xD8, 0x92, 0x79, 0xE2, 0xF7, 0x5E, 0x10, 0x51, 0x32, 0x36, 0x67,
//    0xEB, 0xCA, 0x9E, 0x51, 0x62, 0xBC, 0x03, 0x34, 0x9C, 0x88, 0x78, 0xB6,
//    0x6F, 0x50, 0x8B, 0x14, 0xCB, 0xCC, 0x0D, 0x2E, 0xA7, 0x4B, 0x37, 0xDD,
//    0xBB, 0xE2, 0x8F, 0xD3, 0xD7, 0xBE, 0xF3, 0x87, 0x28, 0xE0, 0x49, 0x29,
//    0x3B, 0x80, 0x8F, 0x5C, 0xA6, 0x88, 0xA8, 0x19, 0x36, 0x8A, 0xDB, 0x20,
//    0x0F, 0x92, 0x9F, 0x47, 0x04, 0x5D, 0x8D, 0xF3, 0x06, 0xA8, 0x64, 0xDB,
//    0x4B, 0x3C, 0x26, 0x9C, 0xFA, 0x08, 0x38, 0x8D, 0xE6, 0xDD, 0x94, 0x4B,
//    0xE9, 0xF3, 0x0A, 0x28, 0x2A, 0x56, 0x4C, 0xD9, 0x4D, 0x18, 0xC9, 0xCA,
//    0xC2, 0x00, 0x3C, 0x24, 0xB9, 0x98, 0x32, 0xD4, 0x6D, 0xB4, 0x7D, 0xA0,
//    0xAD, 0xF3, 0x03, 0xDA, 0xD0, 0xF1, 0xD8, 0x75, 0x42, 0xDC, 0x3F, 0xAB,
//    0x60, 0xDF, 0xC2, 0x8B, 0x5E, 0x4B, 0xD7, 0x1A, 0x45, 0x7D, 0x0E, 0x11,
//    0x36, 0x05, 0x21, 0x3B, 0x83, 0xA9, 0x79, 0xAC, 0x1B, 0x90, 0x24, 0x5D,
//    0xB4, 0x53, 0xBF, 0x2C, 0x6A, 0x7B, 0x1D, 0x9B, 0xF5, 0xA7, 0x2F, 0x73,
//    0xD6, 0x8F, 0x62, 0xA7, 0x2F, 0xA6, 0xD1, 0x5E, 0x7E, 0xAD, 0xBC, 0x08,
//    0xF2, 0x8F, 0xF6, 0xC9, 0xBB, 0x1D, 0xF9, 0x32, 0x81, 0xAD, 0x06, 0xF7,
//    0x08, 0xE5, 0x6D, 0x37, 0x2B, 0x82, 0x7F, 0xED, 0x2C, 0x3F, 0x7A, 0x2A,
//    0x2C, 0xC8, 0xAE, 0x02, 0x9B, 0xD5, 0xB9, 0xE1, 0xD9, 0x54, 0xA0, 0x47,
//    0x48, 0x0B, 0x36, 0x40, 0x79, 0x90, 0x96, 0x34, 0x78, 0xC0, 0x1E, 0x55,
//    0x08, 0x53, 0x73, 0xF8, 0x8E, 0xD8, 0xA0, 0x25, 0xC1, 0x09, 0xDE, 0x13,
//    0x3D, 0x49, 0xC1, 0x46, 0xE1, 0x7A, 0x3C, 0x31, 0x25, 0x30, 0x23, 0x06,
//    0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x09, 0x15, 0x31, 0x16,
//    0x04, 0x14, 0xE4, 0xA6, 0xEC, 0xFC, 0x6F, 0x65, 0xED, 0xE7, 0x02, 0xF0,
//    0x44, 0xA5, 0x7F, 0xA1, 0xD4, 0x34, 0x1D, 0xB7, 0x53, 0x8F, 0x30, 0x31,
//    0x30, 0x21, 0x30, 0x09, 0x06, 0x05, 0x2B, 0x0E, 0x03, 0x02, 0x1A, 0x05,
//    0x00, 0x04, 0x14, 0x5A, 0x38, 0x81, 0xA0, 0xA7, 0x0E, 0xE7, 0x9D, 0xF2,
//    0xA6, 0x57, 0x6A, 0x16, 0x95, 0x32, 0x49, 0xA5, 0xD1, 0x26, 0x5F, 0x04,
//    0x08, 0x64, 0x58, 0x39, 0xC5, 0x09, 0x07, 0x17, 0x1E, 0x02, 0x02, 0x08,
//    0x00
//};


/* C:\Users\csj\Documents\네이트온 받은 파일\1.015.pfx (2015-12-08 오전 11:35:44)
   StartOffset: 00000000, EndOffset: 0000067C, 길이: 0000067D */
// vO72Ind3jQ
byte[] rawData = {
    0x30, 0x82, 0x06, 0x79, 0x02, 0x01, 0x03, 0x30, 0x82, 0x06, 0x3F, 0x06,
    0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82,
    0x06, 0x30, 0x04, 0x82, 0x06, 0x2C, 0x30, 0x82, 0x06, 0x28, 0x30, 0x82,
    0x03, 0x27, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07,
    0x06, 0xA0, 0x82, 0x03, 0x18, 0x30, 0x82, 0x03, 0x14, 0x02, 0x01, 0x00,
    0x30, 0x82, 0x03, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D,
    0x01, 0x07, 0x01, 0x30, 0x1C, 0x06, 0x0A, 0x2A, 0x86, 0x48, 0x86, 0xF7,
    0x0D, 0x01, 0x0C, 0x01, 0x06, 0x30, 0x0E, 0x04, 0x08, 0x93, 0xDE, 0x58,
    0xA8, 0x0B, 0xC9, 0xD5, 0xA1, 0x02, 0x02, 0x08, 0x00, 0x80, 0x82, 0x02,
    0xE0, 0xC2, 0x3D, 0x9A, 0x5B, 0x53, 0xDA, 0xB7, 0x7C, 0xFE, 0x44, 0xBF,
    0x5B, 0xDE, 0xCA, 0x9A, 0x91, 0xC8, 0x7B, 0xDE, 0xC1, 0x9F, 0x71, 0x01,
    0xE7, 0x1D, 0x86, 0x29, 0xA6, 0x5A, 0xF7, 0xC6, 0xFE, 0x60, 0x4D, 0x3A,
    0xB2, 0xCA, 0x05, 0x5F, 0x0D, 0xA8, 0x21, 0x30, 0xBE, 0xBD, 0x3F, 0x19,
    0xCF, 0xBB, 0x90, 0xFC, 0x04, 0x1C, 0x91, 0xD8, 0x17, 0x4C, 0xCA, 0x6F,
    0x19, 0x40, 0xF8, 0x3B, 0x17, 0x41, 0xE2, 0x1F, 0xEF, 0x60, 0x5A, 0x07,
    0x24, 0xD6, 0xBC, 0xA7, 0xC9, 0xE0, 0x71, 0x35, 0xBC, 0xAF, 0xD6, 0x25,
    0x18, 0x24, 0x63, 0xFC, 0x3C, 0xB2, 0x7D, 0x1C, 0x34, 0x93, 0xBA, 0x5A,
    0xE6, 0x61, 0x70, 0xC9, 0x75, 0xFE, 0x4F, 0x9E, 0x9C, 0x94, 0x30, 0xBE,
    0x08, 0xC3, 0xEB, 0x92, 0x23, 0xBA, 0xFF, 0x8F, 0x10, 0x32, 0x50, 0xE9,
    0x84, 0x2F, 0x1E, 0x49, 0x58, 0x7E, 0xB8, 0xEF, 0xE9, 0x82, 0x12, 0x40,
    0x55, 0x56, 0xAE, 0xE3, 0x23, 0x17, 0x67, 0xFF, 0x0C, 0xEA, 0x8F, 0x96,
    0x0F, 0x34, 0x47, 0xAB, 0xF0, 0x15, 0x52, 0xE6, 0x93, 0xB9, 0xF4, 0x96,
    0xF6, 0xCF, 0xBE, 0x92, 0x7C, 0xC2, 0xDE, 0xB9, 0x8B, 0xDC, 0x5E, 0x34,
    0xB1, 0x03, 0xED, 0xF1, 0xAB, 0xB0, 0xD0, 0x65, 0x96, 0x95, 0x0E, 0x9D,
    0x77, 0x5E, 0x58, 0x08, 0xB2, 0x9D, 0x1F, 0x28, 0x02, 0x3F, 0x1C, 0x7A,
    0xEB, 0x78, 0xF7, 0xFA, 0x15, 0x74, 0x13, 0x5C, 0xB0, 0x78, 0x7C, 0x6B,
    0xB9, 0x18, 0xD0, 0xBF, 0x23, 0x9B, 0xBD, 0xC7, 0x2A, 0x09, 0x2B, 0xDB,
    0x92, 0xBC, 0xB2, 0xE9, 0x98, 0xD0, 0xBF, 0xDF, 0x9A, 0xD2, 0x43, 0x93,
    0xD1, 0x8B, 0xE2, 0xF2, 0x8D, 0x84, 0x47, 0x62, 0x13, 0x76, 0x31, 0xC1,
    0xE0, 0xEA, 0x20, 0x36, 0xBA, 0x70, 0xE6, 0x82, 0x98, 0xF8, 0x1C, 0x56,
    0xF4, 0xC7, 0x15, 0xF1, 0xEF, 0x20, 0xBD, 0xF1, 0x78, 0xE0, 0x37, 0xE4,
    0xA3, 0x5C, 0x66, 0x40, 0x0C, 0x09, 0x11, 0x38, 0xB0, 0x3D, 0xDA, 0x2F,
    0x65, 0xA3, 0x7B, 0xC4, 0x3B, 0x7C, 0xCF, 0xDF, 0x3A, 0x54, 0xFF, 0xD2,
    0x14, 0xEF, 0x33, 0xC6, 0x68, 0x57, 0x08, 0xE3, 0x45, 0xA9, 0x4A, 0x00,
    0x46, 0xB4, 0xC4, 0xA1, 0x5A, 0x24, 0x11, 0x3B, 0x9D, 0x5D, 0x9F, 0xD2,
    0xC2, 0x4A, 0xA1, 0x33, 0xE3, 0x3F, 0x37, 0x18, 0x62, 0x06, 0xDA, 0xE4,
    0x06, 0xA3, 0x77, 0xD5, 0xBF, 0x5D, 0x95, 0x27, 0x6C, 0xDF, 0x9D, 0xA2,
    0xBC, 0x85, 0xE6, 0x66, 0xA0, 0x5C, 0x77, 0x45, 0x5B, 0x5A, 0x98, 0xC4,
    0xB4, 0x76, 0x7C, 0xFD, 0x03, 0x66, 0xCC, 0x6E, 0xA1, 0x63, 0x27, 0x4C,
    0x63, 0x15, 0x10, 0x42, 0x1F, 0x86, 0x47, 0xCD, 0x92, 0x47, 0xCC, 0x0E,
    0x8D, 0xA0, 0x87, 0x24, 0x62, 0xB2, 0x75, 0x91, 0xCD, 0xEE, 0x65, 0x6E,
    0x4D, 0x38, 0x1A, 0xFA, 0x96, 0xBD, 0xE8, 0x6C, 0x65, 0x3B, 0x91, 0x01,
    0x7E, 0xF4, 0x63, 0x8F, 0x71, 0xC4, 0x6A, 0x6D, 0x54, 0x1F, 0x37, 0x37,
    0x1A, 0x76, 0xC8, 0x0C, 0x82, 0x5F, 0x03, 0x5F, 0x84, 0xCF, 0xBF, 0xD9,
    0x6E, 0x54, 0xB3, 0x20, 0x00, 0x1A, 0xF9, 0xBF, 0x20, 0x0A, 0x46, 0x0C,
    0x65, 0xEF, 0x79, 0x18, 0x1A, 0xBF, 0x6C, 0x53, 0x71, 0xAD, 0x95, 0xC1,
    0x4E, 0x74, 0xBF, 0x8B, 0x92, 0x5C, 0xD9, 0x72, 0x94, 0xEB, 0x89, 0xA9,
    0xCE, 0x3B, 0xA2, 0xBE, 0x55, 0xC7, 0x23, 0xD9, 0x78, 0xFE, 0x0D, 0xD6,
    0xE3, 0x76, 0xFE, 0xC6, 0x17, 0xCE, 0x63, 0xA5, 0xB9, 0xE2, 0x3B, 0x0F,
    0xB8, 0x70, 0x78, 0x9E, 0x2E, 0x5D, 0xA7, 0x8C, 0xA4, 0x8D, 0x13, 0xB3,
    0xA3, 0x91, 0x8C, 0x73, 0x2C, 0xE8, 0xA8, 0x23, 0x2D, 0xB0, 0x39, 0x34,
    0xB4, 0xF0, 0x4B, 0xDB, 0x19, 0xBA, 0x31, 0x70, 0x62, 0x4C, 0x74, 0xEA,
    0xCF, 0x99, 0x0F, 0x3D, 0xC2, 0xCB, 0x18, 0x8F, 0xDD, 0xF3, 0x79, 0xDF,
    0x86, 0x04, 0xA5, 0x04, 0xF4, 0x96, 0xC0, 0x1C, 0x67, 0x29, 0x93, 0x32,
    0x3C, 0xA4, 0x1A, 0x24, 0x53, 0xFF, 0x85, 0x91, 0x17, 0x47, 0xB8, 0x0B,
    0x7F, 0x6A, 0x25, 0x17, 0x91, 0x8C, 0x08, 0x58, 0x6D, 0x7A, 0xCE, 0xAC,
    0x2C, 0xB3, 0x61, 0x80, 0x7D, 0x7E, 0x81, 0xF2, 0xC4, 0xBE, 0x53, 0x59,
    0x0D, 0x87, 0x8A, 0x05, 0xC3, 0x49, 0x20, 0x77, 0x13, 0x1D, 0x60, 0x07,
    0x5C, 0x28, 0xA2, 0xF2, 0x5B, 0x02, 0x51, 0x46, 0x03, 0xC7, 0x08, 0x75,
    0x6B, 0x79, 0x83, 0xA2, 0xE8, 0x98, 0xCF, 0x52, 0xF1, 0xB8, 0xE4, 0xB7,
    0xD3, 0x8A, 0x74, 0x2C, 0xD4, 0xBE, 0xCF, 0x02, 0x60, 0x96, 0xF2, 0x7D,
    0x44, 0x27, 0xCC, 0x58, 0x21, 0x58, 0xE7, 0x1A, 0xAF, 0x1E, 0xAA, 0x4B,
    0x51, 0xBE, 0x98, 0x19, 0x25, 0xE6, 0x20, 0x51, 0xD1, 0x58, 0xBE, 0xC6,
    0xDF, 0x90, 0xFF, 0x6F, 0xF2, 0xE0, 0x03, 0x52, 0xB9, 0xE2, 0x9A, 0xBF,
    0x21, 0x7E, 0x71, 0x57, 0xA8, 0xB3, 0x2C, 0xC8, 0x2F, 0x56, 0xFE, 0x50,
    0x7D, 0x24, 0xD2, 0xDD, 0x77, 0x40, 0xCE, 0xA0, 0xCA, 0x42, 0xBD, 0xBD,
    0x44, 0x75, 0x2C, 0x54, 0xCD, 0x70, 0x28, 0x3E, 0x3A, 0xB3, 0xC2, 0x84,
    0xD8, 0x8A, 0x6E, 0xA9, 0x7A, 0xDF, 0x6B, 0x68, 0xC0, 0xC3, 0xA7, 0x36,
    0x39, 0x4A, 0xF3, 0x32, 0xAA, 0x7D, 0xE9, 0x15, 0xCA, 0xD2, 0x66, 0x0E,
    0x03, 0x13, 0x9E, 0x10, 0x38, 0x49, 0xDF, 0xBD, 0x0C, 0x29, 0x98, 0x8A,
    0x59, 0x25, 0xF8, 0x37, 0xCA, 0x30, 0x82, 0x02, 0xF9, 0x06, 0x09, 0x2A,
    0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x07, 0x01, 0xA0, 0x82, 0x02, 0xEA,
    0x04, 0x82, 0x02, 0xE6, 0x30, 0x82, 0x02, 0xE2, 0x30, 0x82, 0x02, 0xDE,
    0x06, 0x0B, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x0A, 0x01,
    0x02, 0xA0, 0x82, 0x02, 0xA6, 0x30, 0x82, 0x02, 0xA2, 0x30, 0x1C, 0x06,
    0x0A, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x0C, 0x01, 0x03, 0x30,
    0x0E, 0x04, 0x08, 0x8C, 0x16, 0x16, 0xC7, 0x34, 0x7B, 0x8D, 0x3A, 0x02,
    0x02, 0x08, 0x00, 0x04, 0x82, 0x02, 0x80, 0xAC, 0xB0, 0x2A, 0x81, 0xC6,
    0x2F, 0xDA, 0x72, 0xB5, 0xDD, 0x19, 0x8C, 0x51, 0xD9, 0x97, 0xAB, 0xF8,
    0xFE, 0x08, 0x88, 0xF6, 0x96, 0x1C, 0xDE, 0xAD, 0x42, 0x78, 0x5A, 0xDE,
    0xF8, 0x79, 0x0E, 0xCA, 0x66, 0xA7, 0x9E, 0x5F, 0x3D, 0x33, 0xA1, 0xA7,
    0x0B, 0xAD, 0x41, 0xB2, 0x09, 0x10, 0xAF, 0xCC, 0xD9, 0x71, 0x16, 0x2F,
    0x29, 0x99, 0x3C, 0x05, 0x97, 0x49, 0x57, 0xE5, 0x69, 0xB4, 0x7E, 0x7B,
    0x99, 0xD3, 0x8B, 0x7F, 0xB8, 0xD6, 0xCA, 0x6C, 0xA9, 0xBE, 0xA0, 0x99,
    0x45, 0x40, 0x1F, 0x72, 0x95, 0x3F, 0x55, 0x0C, 0xBD, 0x40, 0x61, 0x4E,
    0x8E, 0xEF, 0xDF, 0x6A, 0x9F, 0xCC, 0xD3, 0x72, 0x76, 0xFC, 0x06, 0x2C,
    0x3E, 0x65, 0xE9, 0x27, 0xE5, 0xDC, 0x64, 0x28, 0x9A, 0xEA, 0xCA, 0xDB,
    0x46, 0xA4, 0x76, 0xBD, 0x27, 0x8F, 0x96, 0x9F, 0x7E, 0x63, 0x50, 0x1F,
    0xCA, 0x56, 0xC4, 0x8F, 0xBA, 0xD6, 0xE0, 0x91, 0x51, 0xAB, 0x41, 0xEB,
    0xC6, 0xCB, 0x41, 0xDC, 0x95, 0xFB, 0xF6, 0xDD, 0xD4, 0x33, 0xAD, 0x34,
    0x2D, 0xA2, 0x30, 0x9D, 0x4A, 0x1C, 0x1C, 0x2F, 0x8A, 0x72, 0xB0, 0x63,
    0xB0, 0x9B, 0x0C, 0x3D, 0x1F, 0xFE, 0xC6, 0x7B, 0x55, 0x45, 0xE4, 0xBC,
    0x24, 0x01, 0x95, 0xC9, 0x63, 0xE2, 0xDF, 0xF8, 0x05, 0x90, 0xED, 0xB6,
    0x5D, 0x1C, 0xB3, 0xD1, 0xCC, 0x73, 0x9F, 0xB9, 0x37, 0x9C, 0xBD, 0xB8,
    0x0C, 0xD8, 0xFB, 0x90, 0x47, 0xA6, 0x03, 0xD0, 0xD9, 0x84, 0x67, 0x0B,
    0x89, 0xEB, 0xC3, 0xA5, 0xE5, 0xB5, 0x09, 0xA9, 0xFA, 0xCC, 0x64, 0xA2,
    0x39, 0x84, 0xA0, 0xD4, 0x4E, 0x9E, 0x63, 0xD1, 0xD5, 0xCF, 0x1B, 0x79,
    0xF0, 0xB8, 0x1C, 0x4C, 0xE5, 0x86, 0x84, 0x7F, 0xD1, 0xC6, 0x86, 0x11,
    0x85, 0x46, 0xAF, 0xD7, 0xBA, 0xEC, 0xC2, 0xDD, 0x26, 0x33, 0x78, 0x7D,
    0x9A, 0x63, 0xC9, 0x1E, 0xD0, 0x20, 0xA8, 0x03, 0xE9, 0x28, 0xE5, 0x16,
    0xB8, 0xD4, 0x8D, 0xB5, 0x68, 0x72, 0x61, 0xB5, 0x79, 0xDE, 0xDC, 0xA2,
    0x4F, 0x79, 0x2F, 0xFC, 0x07, 0xD0, 0x37, 0x9E, 0xE3, 0xA2, 0xF6, 0x2E,
    0xE3, 0x01, 0x87, 0x7D, 0xA4, 0x3E, 0x80, 0x6E, 0xCD, 0xED, 0x66, 0x52,
    0x3F, 0xAF, 0xBE, 0x7E, 0xE7, 0xA4, 0xDB, 0x64, 0xD4, 0x63, 0x4B, 0xC8,
    0xB6, 0x3F, 0x36, 0x94, 0xFC, 0x7B, 0x1F, 0x45, 0x09, 0x41, 0x0B, 0x6C,
    0xCB, 0x2B, 0x2D, 0xDC, 0x11, 0x6C, 0x69, 0x54, 0xD6, 0xE5, 0x61, 0x70,
    0x44, 0x6C, 0xA5, 0x65, 0x36, 0x41, 0x49, 0xB7, 0xA8, 0x92, 0x07, 0x02,
    0x82, 0xA1, 0xF6, 0x85, 0x13, 0x97, 0x23, 0x53, 0xE9, 0x6D, 0x7F, 0xD9,
    0x4F, 0x0B, 0xC8, 0x04, 0xDA, 0x54, 0x3D, 0xA1, 0x16, 0x0A, 0xDF, 0x4E,
    0xC1, 0xA7, 0xB7, 0xFE, 0xAC, 0x8E, 0xFE, 0x28, 0x5F, 0xF4, 0xA0, 0x98,
    0xE2, 0x60, 0x41, 0xE7, 0x62, 0x68, 0x60, 0x6B, 0xF2, 0x08, 0x60, 0x24,
    0x65, 0x9E, 0x76, 0x80, 0xA4, 0x2B, 0x3B, 0x42, 0xA9, 0xB6, 0x5A, 0xBC,
    0x56, 0x6F, 0xFC, 0x02, 0xB5, 0x67, 0x64, 0xF9, 0x9F, 0x60, 0xCA, 0x04,
    0xFA, 0x2B, 0xD8, 0xCE, 0xEF, 0x91, 0xC0, 0x38, 0x7F, 0x0E, 0x09, 0x03,
    0xF4, 0xE5, 0x61, 0x2E, 0x43, 0xE1, 0x5B, 0x4A, 0x87, 0x92, 0x96, 0xDF,
    0xC7, 0x3F, 0x84, 0x22, 0x92, 0x47, 0x1B, 0xFE, 0xBB, 0xF7, 0x48, 0x8A,
    0xB6, 0xCE, 0xFD, 0x75, 0xE8, 0x95, 0x08, 0x4B, 0xB8, 0x92, 0x69, 0x90,
    0x29, 0x69, 0xB3, 0x8C, 0xC0, 0x7B, 0xA9, 0x2A, 0xB8, 0x8A, 0x88, 0xD2,
    0x5A, 0x86, 0xC7, 0x53, 0xF0, 0xF0, 0xC6, 0xF1, 0x99, 0x2D, 0x33, 0x57,
    0x66, 0x55, 0x1F, 0x1F, 0xD2, 0x5C, 0x6A, 0x3B, 0x56, 0x48, 0x36, 0x19,
    0x50, 0x0D, 0xA5, 0x2B, 0x7C, 0x4B, 0xD4, 0xB8, 0x67, 0xA9, 0x59, 0x9D,
    0xB8, 0x68, 0xBC, 0xCA, 0x2B, 0xF9, 0x17, 0x38, 0x13, 0x7E, 0xC6, 0x8D,
    0x53, 0x76, 0x81, 0x98, 0x1A, 0x4E, 0x4E, 0x5C, 0xC9, 0xAC, 0xA9, 0x78,
    0x86, 0x85, 0x26, 0x4B, 0xA0, 0x90, 0x4F, 0xE3, 0xB5, 0x28, 0xBC, 0xD1,
    0x8D, 0x11, 0x46, 0xBD, 0xDF, 0xC5, 0xEA, 0x2C, 0x06, 0x14, 0x75, 0xED,
    0x4D, 0xEF, 0x71, 0xEB, 0x30, 0x8E, 0x22, 0x26, 0x17, 0xA0, 0xBC, 0xE4,
    0x06, 0xC3, 0x37, 0xAE, 0x3F, 0x5C, 0x52, 0xAC, 0x17, 0x47, 0x8E, 0x85,
    0xA9, 0xE9, 0xBE, 0x40, 0x04, 0x72, 0xC0, 0x10, 0x31, 0xF4, 0xBB, 0x9E,
    0xC6, 0x7C, 0xFE, 0xBB, 0x25, 0xFD, 0x2F, 0xC5, 0xA6, 0x91, 0x4F, 0xCD,
    0x91, 0xA0, 0x31, 0x0F, 0x14, 0x05, 0x25, 0x9F, 0x66, 0x1F, 0xF3, 0x44,
    0x5D, 0x3A, 0xEC, 0xC2, 0x96, 0xCA, 0x3A, 0x77, 0x16, 0x07, 0xA3, 0x31,
    0x25, 0x30, 0x23, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01,
    0x09, 0x15, 0x31, 0x16, 0x04, 0x14, 0x9A, 0x5A, 0x43, 0xF6, 0x57, 0x22,
    0xE0, 0xE8, 0x44, 0xFD, 0x98, 0x63, 0x2B, 0x28, 0x4E, 0x67, 0x04, 0xC4,
    0x87, 0x09, 0x30, 0x31, 0x30, 0x21, 0x30, 0x09, 0x06, 0x05, 0x2B, 0x0E,
    0x03, 0x02, 0x1A, 0x05, 0x00, 0x04, 0x14, 0xE1, 0xAD, 0xEA, 0x89, 0xAE,
    0xB2, 0xD3, 0x8D, 0xAB, 0x3F, 0x17, 0x06, 0xEB, 0x38, 0xF3, 0x70, 0x5F,
    0xFB, 0x7E, 0xFE, 0x04, 0x08, 0x81, 0x7D, 0x16, 0x1C, 0x9B, 0x67, 0x43,
    0x39, 0x02, 0x02, 0x08, 0x00
};





	static public	X509Certificate2 certificate;

	public static string	GetAccessToken(string clientIdEMail, string keyFilePath, string szScope)
	{

		certificate = new X509Certificate2(rawData , "vO72Ind3jQ");

		JSONClass	j_header	= new JSONClass();
		JSONClass	j_claimset	= new JSONClass();

		j_header["alg"] = "RS256";

		var times = GetExpiryAndIssueDate();

		j_claimset["iss"]		= "client";
		j_claimset["exp"].AsInt	= times[1];

		byte[]	headerBytes	= Encoding.UTF8.GetBytes( j_header.ToString() );
		string	headerEncoded = Base64UrlEncode(headerBytes);



		byte[]	claimsetBytes = Encoding.UTF8.GetBytes( j_claimset.ToString() );
		string	claimsetEncoded = Base64UrlEncode(claimsetBytes);

		//// input
		string	input = headerEncoded + "." + claimsetEncoded;
		byte[]	inputBytes = Encoding.UTF8.GetBytes(input);


		RSACryptoServiceProvider rsa = certificate.PrivateKey as RSACryptoServiceProvider;
		byte[]	signatureBytes = rsa.SignData(inputBytes, "SHA256");
		string	signatureEncoded = Base64UrlEncode(signatureBytes);

		//// jwt
		string jwt = headerEncoded + "." + claimsetEncoded + "." + signatureEncoded;


		Debug.Log( jwt );
		return jwt;
	}

// eyJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJjbGllbnQiLCAiZXhwIjoxNDQ4OTM1MTU1fQ.xTo8EZGcJRO4sKVCV2uQUPc1AThxcxsju_L3SuRAu0R_WLdLFXz1GMfi8FZePo9Tq3ODZBVn4w2Zu5h434Et-_xXwiC8UOLmMN_neILub9OpgB-LacNjTQd6yZtYdGU2loiUKiju5zgS9QAB4DjQ9y6CZ1SG8lLQ_gviGHK8-es
// eyJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJjbGllbnQiLCAiZXhwIjoxNDQ4OTM1MTczfQ.oon4Uq5DuOR5TA2yP5LCmcZg9oQg0RNCZn9YHk21HwbnFpJWgZBR2cHAe5XmMnGEUHmD96S2FZPKS_lGkb11tf9WbhTQmPnRTTmvXXkj29VUVRzNxab43IbDyupp3DTj7-WDSqxGWA3qBD3GQsWY5q0BgkEEpndovW47IycvKtw
	private static string Base64UrlEncode(byte[] input)
	{
		var output = Convert.ToBase64String(input);
		output = output.Split('=')[0]; // Remove any trailing '='s
		output = output.Replace('+', '-'); // 62nd char of encoding
		output = output.Replace('/', '_'); // 63rd char of encoding
		return output;
	}

	private static int[] GetExpiryAndIssueDate()
	{
		var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		var issueTime = DateTime.UtcNow;
		var iat = (int)issueTime.Subtract(utc0).TotalSeconds;

		// 

		var exp = (int)issueTime.AddSeconds(3600).Subtract(utc0).TotalSeconds;

		//Debug.Log( "JWT : 시간 : iat : " + iat );
		//Debug.Log( "JWT : 시간 : exp : " + exp );

		return new[] { iat, exp };
	}
}



//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Json;
//using System.Security.Cryptography.X509Certificates; 

 

//public class GoogleJsonWebToken
//{
//private static ILogger Log = LogManager.GetCurrentClassLogger();
//public const string SCOPE_AUTH_ANDROIDPUBLISHER = "https://www.googleapis.com/auth/androidpublisher";

//public static dynamic GetAccessToken(string clientIdEMail, string keyFilePath, string szScope)
//{
//// certificate
//var certificate = new X509Certificate2(keyFilePath, "notasecret");

 

//// header
//var header = new { alg = "RS256", typ = "JWT" };

 

//// claimset
//var times = GetExpiryAndIssueDate();
//var claimset = new
//{
//iss = clientIdEMail,
//scope = szScope,
//aud = "https://accounts.google.com/o/oauth2/token",
//iat = times[0],
//exp = times[1],
//};

//JavaScriptSerializer ser = new JavaScriptSerializer();

 

//// encoded header
//var headerSerialized = ser.Serialize(header);
//var headerBytes = Encoding.UTF8.GetBytes(headerSerialized);
//var headerEncoded = Base64UrlEncode(headerBytes);

 

//// encoded claimset
//var claimsetSerialized = ser.Serialize(claimset);
//Log.DebugFormat("claimset[{0}]", claimsetSerialized);
//var claimsetBytes = Encoding.UTF8.GetBytes(claimsetSerialized);
//var claimsetEncoded = Base64UrlEncode(claimsetBytes);

//// input
//var input = headerEncoded + "." + claimsetEncoded;
//var inputBytes = Encoding.UTF8.GetBytes(input);

//// signiture
//var rsa = certificate.PrivateKey as RSACryptoServiceProvider;
//var cspParam = new CspParameters
//{
//KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
//KeyNumber = rsa.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
//};
//var aescsp = new RSACryptoServiceProvider(cspParam) { PersistKeyInCsp = false };
//var signatureBytes = aescsp.SignData(inputBytes, "SHA256");
//var signatureEncoded = Base64UrlEncode(signatureBytes);

 

//// jwt
//var jwt = headerEncoded + "." + claimsetEncoded + "." + signatureEncoded;
//Log.DebugFormat("jwt[{0}]", jwt);

//var client = new WebClient();
//client.Encoding = Encoding.UTF8;
//var uri = "https://accounts.google.com/o/oauth2/token";
//var content = new NameValueCollection();

//content["assertion"] = jwt;
//content["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer";

//string response = Encoding.UTF8.GetString(client.UploadValues(uri, "POST", content));

//Log.DebugFormat("response[{0}]", response);
//var result = ser.Deserialize<dynamic>(response);

//return result;
//}

 

//private static string Base64UrlEncode(byte[] input)
//{
//var output = Convert.ToBase64String(input);
//output = output.Split('=')[0]; // Remove any trailing '='s
//output = output.Replace('+', '-'); // 62nd char of encoding
//output = output.Replace('/', '_'); // 63rd char of encoding
//return output;
//}

 

//private static int[] GetExpiryAndIssueDate()
//{
//var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
//var issueTime = DateTime.UtcNow;

//var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
//var exp = (int)issueTime.AddMinutes(55).Subtract(utc0).TotalSeconds;

//return new[] { iat, exp };
//}
//}

//[출처] [ServerSide] C# Google Inapp 영수증 확인까지..(Google Inapp Billing Version 3)|작성자 쿠울
