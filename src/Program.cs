﻿using MonoGame.Framework;

namespace LiVerse {
  public class Program {
    public static int Main(string[] arguments) {
      // Check if ApplicationData Directory Exists
      if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "ApplicationData"))) {
        Console.Write("Fatal Error! Could not find ApplicationData Directory in the current Working Directory.");
        return -1;
      }

      // Create LocalApplicationData Dir
      Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LiVerse"));

      #if DEBUG // Run without error handler in Debug
      using (LiVerseApp app = new LiVerseApp()) {
        app.Run();
      }

      #else
      try {
        using (LiVerseApp app = new LiVerseApp()) {
          app.Run();
        }

      } catch (Exception ex) {
          Console.WriteLine($"Unhandled Runtime Exception Detected! HResult: {ex.HResult}\n{ex.Message}");
          Console.WriteLine(ex.StackTrace);
          LogException(ex);

          return -1;
      }
      #endif

      return 0;
    }

    static void LogException(Exception ex) {
      // Check if LogDirectory Exists
      string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LiVerse", "CrashLogs");
      string logFilePath = Path.Join(logDirectory, $"crashlog-{DateTime.Now.ToShortDateString().Replace("/", "_")}-{DateTime.Now.ToShortTimeString().Replace(":", "-")}.txt");
      Directory.CreateDirectory(logDirectory);

      string logFileData = $"HResult: {ex.HResult}\nMessage: {ex.Message}\nSource: {ex.Source}\n";

      if (ex.Data.Count >= 1) {
        logFileData += "Additional Data:";
         
        foreach(string data in ex.Data) {
          logFileData += $"{data}\n";
        }
      }


      logFileData += "Stack Trace:\n" + ex.StackTrace;

      try {
        File.WriteAllText(logFilePath, logFileData, System.Text.Encoding.UTF8);
      
      } catch (Exception _ex) {
        Console.WriteLine($"Could not write log file. {_ex.Message}");
        return;
      }

      Console.WriteLine($"Log File created at {logFilePath}");
    }
  }
}

