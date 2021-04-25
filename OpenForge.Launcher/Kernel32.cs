// Licensed to OpenForge under one or more agreements.
// OpenForge licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenForge.Launcher
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static IntPtr OpenProcess(Process process, ProcessAccessFlags flags)
        {
            var handle = OpenProcess(flags, false, process.Id);
            return handle == IntPtr.Zero ? throw new Exception(string.Format("Failed to open process ({0}).", GetLastError())) : handle;
        }

        public static int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer)
        {
            var result = ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, lpBuffer.Length, out _);
            return result == 0 ? throw new Exception("Failed to read from process.") : result;
        }

        public static uint VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int size, uint newProtect)
        {
            var result = VirtualProtectEx(hProcess, lpAddress, new IntPtr(size), newProtect, out var oldProtect);
            return result == 0 ? throw new Exception(string.Format("Failed to change access ({0}).", GetLastError())) : oldProtect;
        }

        public static int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer)
        {
            var result = WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, lpBuffer.Length, out var _);
            return result == 0 ? throw new Exception(string.Format("Failed to write to process ({0}).", GetLastError())) : result;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern int VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);
    }
}
