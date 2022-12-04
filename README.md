# LoveMachine.Experiments
Spin-off project of LoveMachine for unstable/experimental stuff, using Koikatsu/KKS as testing ground.

[Download (Koikatsu)](https://github.com/Sauceke/LoveMachine.Experiments/releases/latest/download/LoveMachine.Experiments.KK.zip)

[Download (Koikatsu Sunshine)](https://github.com/Sauceke/LoveMachine.Experiments/releases/latest/download/LoveMachine.Experiments.KKS.zip)


## Depth control
Adds experimental (read: barely usable) support for two depth sensing toys:
- [Lovense Calor]
- The [Hotdog], a DIY device for transparent sleeves

To use:
1. Make sure you have the corresponding setting (Enable Lovense Calor depth control/Enable Hotdog depth control) enabled.
1. If LoveMachine is installed, disable auto-starting Intiface in the plugin settings.
1. If using a Hotdog, start the [Hotdog Server]. (If using a Calor, you don't need to start anything.)
1. Turn your device on **before** starting the game.
1. Start the game.
1. Start an H scene and select a penetrative position.
1. The plugin should take care of the rest and hand over control to your device.

## Acknowledgements
The plugin uses [BLEConsole](https://github.com/sensboston/BLEConsole) to communicate with Lovense Calor devices.

<!-- links -->
[Lovense Calor]: https://www.lovense.com/r/vu65q6
[Hotdog]: https://sauceke.github.io/hotdog
[Hotdog Server]: https://github.com/Sauceke/hotdog/releases/latest/download/HotdogServer.exe
