using ElectronCgi.DotNet;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace Core
{
    public class WindowSource
    {
        public IntPtr handle { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }
    class Program
    {
        public List<IntPtr> IgnoreWindows { get; set; }

        private string[] ignoreList = new string[] { "Progman", "Button" };
        private List<WindowInfo> windows;

        public Program()
        {
            IgnoreWindows = new List<IntPtr>();
        }
        
        public List<WindowInfo> GetWindowsList()
        {
            windows = new List<WindowInfo>();
            EnumWindowsProc ewp = EvalWindows;
            NativeMethods.EnumWindows(ewp, IntPtr.Zero);
            return windows;
        }

        public List<WindowInfo> GetVisibleWindowsList()
        {
            List<WindowInfo> windows = GetWindowsList();

            return windows.Where(IsValidWindow).ToList();
        }


        private bool IsValidWindow(WindowInfo window)
        {
            return window != null && window.IsVisible && !string.IsNullOrEmpty(window.Text) && IsClassNameAllowed(window) && window.IsRectangleValid();
        }

        private bool IsClassNameAllowed(WindowInfo window)
        {
            string className = window.ClassName;

            if (!string.IsNullOrEmpty(className))
            {
                return ignoreList.All(ignore => !className.Equals(ignore, StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

        private bool EvalWindows(IntPtr hWnd, IntPtr lParam)
        {
            if (IgnoreWindows.Any(window => hWnd == window))
            {
                return true;
            }

            windows.Add(new WindowInfo(hWnd));

            return true;
        }

        public List<WindowSource> GetSources()
        {
            List<WindowSource> windowSources = new List<WindowSource>();
            List<WindowInfo> lists = GetVisibleWindowsList();
            lists.ForEach(list =>
            {
                WindowSource ws = new WindowSource();
                ws.handle = list.Handle;
                ws.name = list.ToString();
                ws.id = "window:" + list.Handle.ToString() + ":0";
                if (ws.name == "Counter-Strike")
                    ws.id = "window:" + list.Handle.ToString() + ":0";
                windowSources.Add(ws);
            });
            return windowSources;
        }

        static void Main(string[] args)
        {
            Program app = new Program();
            
            var connection = new ConnectionBuilder()
                .WithLogging()
                .Build();

            connection.On("fetch-sources", (string name) =>
            {
                return app.GetSources();
            });

            connection.On("focus-source", (string source) =>
            {
                WindowSource ws = JsonConvert.DeserializeObject<WindowSource>(source);
                NativeMethods.SetForegroundWindow(ws.handle);
                NativeMethods.ShowWindow(ws.handle, 1);
                return NativeMethods.GetWindowTextLength(ws.handle);
            });

            connection.Listen();
        }
    }
}
