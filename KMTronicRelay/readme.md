This library was designet to be used with the [KMTronic U1CRB USB Relay](https://www.kmtronic.com/usb-relays.html?product_id=52) but can also be used with other [KMTronic USB Relays](https://www.kmtronic.com/usb-relays.html) if they use the same Protocoll.  
```sh
KmtronicU1CrbRelay.Use("COM3", relay => {
    relay.Switch(SwitchNumber.One, SwitchAction.On);
    Thread.Sleep(500);
    relay.Switch(SwitchNumber.One, SwitchAction.Off);
    Thread.Sleep(500);
});
```

Supported Runtimes:
- dotnet core 2.0 (windows only)
- .net framework 4.5
- mono 5.10 (older versions may work, only requires `mono-runtime`)

for now only mono works cross platform but it looks like [dotnet core support is on its way](https://github.com/dotnet/corefx/pull/29033).