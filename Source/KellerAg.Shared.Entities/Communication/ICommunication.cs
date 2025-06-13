using System;
using System.Collections.Generic;
using System.Threading;

namespace KellerAg.Shared.Entities.Communication
{
    /// <summary>
    /// Interface for a communication interface such as SerialPort, TCP/IP or Bluetooth
    /// </summary>
    public interface ICommunication
    {
        /// <summary>Name der Schnittstelle</summary>
        string Name { get; }

        /// <summary>= true, wenn die Schnittstelle ein Echo zurückgibt</summary>
        bool EchoOn { get; set; }

        /// <summary>= true, Echo automatisch ermitteln</summary>
        bool AutoEcho { get; set; }

        /// <summary>= true, wenn die Schnittstelle offen ist</summary>
        bool IsOpen { get; }

        /// <summary>= true, wenn die Schnittstelle eine Bluetoothschnittstelle ist</summary>
        bool IsBluetooth { get; set; }

        /// <summary>gibt die geschwindigkeit der Schnittstelle zurück</summary>
        int Speed { get; }

        /// <summary>Schnittstelle</summary>
        object Interface { get; set; }

        /// <summary>
        /// Notifies on new byte during active ReadContinuous
        /// </summary>
        event EventHandler<NewBytesArgument> ReadContinuousOnByte;

        /// <summary>
        /// Starts to read continuous and locks port until cancellationToken is cancelled
        /// </summary>
        /// <param name="cancellationToken"></param>

        void ReadContinuous(CancellationToken cancellationToken);

        /// <summary>
        /// Data send and receive over interface with expected receive byte count as "end sign"
        /// </summary>
        /// <param name="command">data send</param>
        /// <param name="readByteCount">expected byte to receive</param>
        /// <returns>data receive</returns>
        byte[] Send(byte[] dataSend, int readByteCount);

        /// <summary>
        /// Data send and receive over interface with an end sign
        /// </summary>
        /// <param name="dataSend">data send</param>
        /// <param name="endSign">receive data until the end sign</param>
        /// <returns>received bytes</returns>
        byte[] Send(byte[] dataSend, byte endSign);

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="readByteCount">erwartete anzahl Bytes</param>
        void send(byte[] command, out byte[] rcfBuffer, int readByteCount);

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="endSign">empfangen bis zu diesem Zeichen</param>
        void send(byte[] command, out byte[] rcfBuffer, byte endSign);

        /// <summary>
        /// Öffnet die Schnittstelle
        /// </summary>
        /// <param name="sender">ausführendes Objekt</param>
        void open(object sender);

        /// <summary>
        /// Schliesst die Schnittstelle
        /// </summary>
        /// <param name="sender">ausführendes Objekt</param>
        void close(object sender);

        /// <summary>
        /// Konfiguriert mehrere Parameter der Schnittstelle.
        /// </summary>
        /// <param name="newConfig">neue Konfiguration</param>
        void setConfig(Dictionary<string, object> newConfig);

        /// <summary>
        /// Konfiguriert einzelne Parameter der Schnittstelle.
        /// </summary>
        /// <param name="key">Konfigurations-Variable</param>
        /// <param name="value">neuer Konfigurationswert</param>
        /// <returns>=true, wenn sich etwas geändert hat</returns>
        bool setConfig(string key, object value);

        /// <summary>
        /// Einzelne Konfiguration auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Wert</returns>
        object getConfig(string key);

        /// <summary>
        /// Konfiguration auslesen
        /// </summary>
        /// <returns>Konfiguration der Schnittstelle</returns>
        Dictionary<string, object> getConfigCopy();
    }
}