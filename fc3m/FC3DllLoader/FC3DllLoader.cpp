// FC3DllLoader.cpp : Defines the entry point for the console application.
//
#ifdef _WIN64
#define POINTER_TYPE ULONGLONG
#else
#define POINTER_TYPE DWORD
#endif


#include "stdafx.h"
#include "afxwin.h"
#include "windows.h"
#include "FC3DllLoader.h"
#include "MemoryModule.h"
using namespace cli;
using namespace System;

//typedef struct _IMAGE_NT_HEADERS {
//    DWORD Signature;
//    IMAGE_FILE_HEADER FileHeader;
//    IMAGE_OPTIONAL_HEADER32 OptionalHeader;
//} IMAGE_NT_HEADERS32, *PIMAGE_NT_HEADERS32;
////
//typedef struct _IMAGE_FILE_HEADER {
//    WORD    Machine;
//    WORD    NumberOfSections;
//    DWORD   TimeDateStamp;
//    DWORD   PointerToSymbolTable;
//    DWORD   NumberOfSymbols;
//    WORD    SizeOfOptionalHeader;
//    WORD    Characteristics;
//} IMAGE_FILE_HEADER, *PIMAGE_FILE_HEADER;
////
//typedef struct _IMAGE_OPTIONAL_HEADER {
//    //
//    // Standard fields.
//    //
//
//    WORD    Magic;
//    BYTE    MajorLinkerVersion;
//    BYTE    MinorLinkerVersion;
//    DWORD   SizeOfCode;
//    DWORD   SizeOfInitializedData;
//    DWORD   SizeOfUninitializedData;
//    DWORD   AddressOfEntryPoint;
//    DWORD   BaseOfCode;
//    DWORD   BaseOfData;
//
//    //
//    // NT additional fields.
//    //
//
//    DWORD   ImageBase;
//    DWORD   SectionAlignment;
//    DWORD   FileAlignment;
//    WORD    MajorOperatingSystemVersion;
//    WORD    MinorOperatingSystemVersion;
//    WORD    MajorImageVersion;
//    WORD    MinorImageVersion;
//    WORD    MajorSubsystemVersion;
//    WORD    MinorSubsystemVersion;
//    DWORD   Win32VersionValue;
//    DWORD   SizeOfImage;
//    DWORD   SizeOfHeaders;
//    DWORD   CheckSum;
//    WORD    Subsystem;
//    WORD    DllCharacteristics;
//    DWORD   SizeOfStackReserve;
//    DWORD   SizeOfStackCommit;
//    DWORD   SizeOfHeapReserve;
//    DWORD   SizeOfHeapCommit;
//    DWORD   LoaderFlags;
//    DWORD   NumberOfRvaAndSizes;
//    IMAGE_DATA_DIRECTORY DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES];
//} IMAGE_OPTIONAL_HEADER32, *PIMAGE_OPTIONAL_HEADER32;
//

//[System.Flags]
//		public enum LoadLibraryFlags : uint
//		{
//			DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
//			LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
//			LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
//			LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
//			LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
//			LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
//		}
//

int main(array<System::String ^> ^args)
{
	Console::WriteLine(L"FC3Dll Loader 0.01alpha \n");




	char* dlls = "I:\\Games\\dunia2\\git\\fc3editor\\fc3m\\Debug\\FC3.dll";
	
	

	/*HINSTANCE hDLL = LoadLibraryEx((LPCWSTR)dlls, (HANDLE)IntPtr::Zero,  0x00000002);
	if (hDLL == INVALID_HANDLE_VALUE)
	{
		Console::WriteLine("Invalid DLL");
		Console::ReadLine();
		return;
	}
*/

	PMEMORYMODULE memory = (PMEMORYMODULE)LoadFromMemory(dlls);


	//PMEMORYMODULE memory = (PMEMORYMODULE)MemoryLoadLibrary(hDLL, false);
	//memory->headers->OptionalHeader

	FARPROC proc = MemoryGetProcAddress(memory, "DllMain");

	Console::WriteLine("FARPROC {0}", (IntPtr)proc);

	if (memory->headers->OptionalHeader.AddressOfEntryPoint != 0) {
		DllEntryProc DllEntry = (DllEntryProc) (memory->codeBase + memory->headers->OptionalHeader.AddressOfEntryPoint);
		if (DllEntry == 0) {
			Console::WriteLine("Library has no entry point.");
		}

		bool successful = (*DllEntry)((HINSTANCE)memory->codeBase, DLL_PROCESS_ATTACH, 0);

	}

	MemoryFreeLibrary(memory);
	Console::WriteLine("fin");
	Console::ReadLine();
}
HMEMORYMODULE LoadFromMemory(char* file)
{
	FILE *fp;
	unsigned char *data=NULL;
	size_t size;
	HMEMORYMODULE module;


	fp = fopen(file, "rb");
	if (fp == NULL)
	{
		printf("Can't open DLL file \"%s\".", file);
		goto exit;
	}

	fseek(fp, 0, SEEK_END);
	size = ftell(fp);
	data = (unsigned char *)malloc(size);
	fseek(fp, 0, SEEK_SET);
	fread(data, 1, size, fp);
	fclose(fp);

	module = MemoryLoadLibrary(data, false);
	if (module == NULL)
	{
		printf("Can't load library from memory.\n");
		goto exit;
	}
	return module;
exit:
	if (data)
		free(data);
}
void log(System::String ^msg)
{
	Console::WriteLine(msg);
}