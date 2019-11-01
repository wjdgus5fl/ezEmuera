using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Riey {
    public static class EzTranslate {
        private const string DLL_NAME = "ezemuera_trans";

        private static IntPtr Context;

        public static bool IsInitialized => EzTranslate.Context != IntPtr.Zero;

        public static bool Init(string ezPath, string ctxPath)
        {
            unsafe 
            {
                fixed(char* ezPathPtr = ezPath) 
                {
                    fixed(char* ctxPathPtr = ctxPath)
                    {
                        EzTranslate.Context = ez_init(ezPathPtr, (UIntPtr) ezPath.Length, ctxPathPtr, (UIntPtr) ctxPath.Length);

                        return EzTranslate.Context != IntPtr.Zero;
                    }
                }
            }
        }

        public static void Save(string ctxPath)
        {
            if (!EzTranslate.IsInitialized)
            {
                return;
            }

            unsafe
            {
                fixed(char* ctxPathPtr = ctxPath)
                {
                    ez_save(EzTranslate.Context, ctxPathPtr, (UIntPtr) ctxPath.Length);
                }
            }
        }

        public static void Delete()
        {
            if (!EzTranslate.IsInitialized)
            {
                return;
            }

            unsafe
            {
                ez_delete(EzTranslate.Context);
                EzTranslate.Context = IntPtr.Zero;
            }
        }

        public static string Translate(string text)
        {
            if (!EzTranslate.IsInitialized)
            {
                return text;
            }

            unsafe
            {
                fixed(char* textPtr = text)
                {
                    byte* outText;
                    UIntPtr outTextLen;
                    int ret = ez_translate(EzTranslate.Context, textPtr, (UIntPtr) text.Length, &outText, &outTextLen);

                    if (ret != 0) {
                        return text;
                    }

                    return Encoding.UTF8.GetString(outText, (int) outTextLen);
                }
            }
        }

        public static void AddBeforeDict(string key, string value)
        {
            if (!EzTranslate.IsInitialized)
            {
                return;
            }

            unsafe
            {
                fixed (char* keyPtr = key)
                {
                    fixed (char* valuePtr = value)
                    {
                        ez_add_before_dict(EzTranslate.Context, keyPtr, (UIntPtr) key.Length, valuePtr, (UIntPtr) value.Length);
                    }
                }
            }
        }

        public static void AddAfterDict(string key, string value)
        {
            if (!EzTranslate.IsInitialized)
            {
                return;
            }

            unsafe
            {
                fixed (char* keyPtr = key)
                {
                    fixed (char* valuePtr = value)
                    {
                        ez_add_after_dict(EzTranslate.Context, keyPtr, (UIntPtr) key.Length, valuePtr, (UIntPtr) value.Length);
                    }
                }
            }
        }

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static IntPtr ez_init(char* path, UIntPtr pathLen, char* ctxPath, UIntPtr ctxPathLen);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static void ez_save(IntPtr ctx, char* ctxPath, UIntPtr ctxPathLen);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static void ez_delete(IntPtr ctx);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static int ez_translate(IntPtr ctx, char* text, UIntPtr textLen, byte** outText, UIntPtr* outTextLen);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static void ez_add_before_dict(IntPtr ctx, char* key, UIntPtr keyLen, char* value, UIntPtr valueLen);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private extern unsafe static void ez_add_after_dict(IntPtr ctx, char* key, UIntPtr keyLen, char* value, UIntPtr valueLen);
    }
}
