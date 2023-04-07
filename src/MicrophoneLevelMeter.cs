using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src {
  public static class MicrophoneLevelMeter {
    public static Microphone? CurrentMicrophone;
    public static event Action<double>? MicrophoneVolumeLevelUpdate;
    public static event Action? MicrophoneTriggerLevelTriggered;
    public static event Action? CharacterStartSpeaking;
    public static event Action? CharacterStopSpeaking;
    public static float TriggerLevel { get; set; } = 0.75f;
    public static float MaximumLevel { get; set; } = 80;
    public static double ActivationDelay { get; set; }
    public static float ActivationDelayTrigger { get; set; } = 0.65f;
    static byte[]? buffer;
    static bool isCharacterSpeaking = false;


    public static void Initialize() {
      SetUpDevice(Microphone.Default, 100);
    }

    public static void SetUpDevice(Microphone device, double bufferSizeMS) {
      CurrentMicrophone = device;

      CurrentMicrophone.BufferDuration = TimeSpan.FromMilliseconds(bufferSizeMS);
      buffer = new byte[CurrentMicrophone.GetSampleSizeInBytes(CurrentMicrophone.BufferDuration)];
      CurrentMicrophone.BufferReady += CurrentMicrophone_BufferReady;
      CurrentMicrophone.Start();
    }

    public static void Update(double deltaTime) {
      if (ActivationDelay > 0) {
        ActivationDelay -= 1 * deltaTime;
      }

      if (ActivationDelay >= ActivationDelayTrigger) {
        if (!isCharacterSpeaking) {
          isCharacterSpeaking = true;

          CharacterStartSpeaking?.Invoke();
        }

      }else if (isCharacterSpeaking) {
        isCharacterSpeaking = false;

        CharacterStopSpeaking?.Invoke();
      }
    }

    private static void CurrentMicrophone_BufferReady(object? sender, EventArgs e) {
      CurrentMicrophone.GetData(buffer);

      using (MemoryStream memoryStream = new MemoryStream(buffer!)) {
        using (BinaryReader binaryReader = new BinaryReader(memoryStream)) {

          double rmsSum = 0;
          while (memoryStream.Position < memoryStream.Length) {
            Int16 samplePoint = binaryReader.ReadInt16();

            rmsSum += samplePoint * samplePoint;
          }

          double rms = Math.Sqrt(rmsSum / buffer.Length);
          //CurrentValue = (float)rms / Int16.MaxValue;

          // Decibels!
          double levelDB = 20.0 * Math.Log10(rms);
          MicrophoneVolumeLevelUpdate?.Invoke(levelDB);

          if ((levelDB / MaximumLevel) >= TriggerLevel) {
            MicrophoneTriggerLevelTriggered?.Invoke();
            ActivationDelay = 1;
          }

        }
      }
    }
  }
}
