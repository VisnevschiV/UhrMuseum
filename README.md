# UhrMuseum

**A hands-on XR museum experience for discovering how a mechanical watch works.**

UhrMuseum turns watch assembly into an interactive, guided experience. Explore the parts of a mechanical watch, manipulate its components in 3D, and learn through direct interaction rather than a screen full of instructions. The project is built in Unity with custom XR interactions and a data-driven tutorial flow.

> **Portfolio focus:** XR interaction design · Unity / C# · two-hand interactions · interactive 3D mechanics · guided learning

## The experience

The experience guides the visitor through a sequence of watch-assembly stages. Instructions combine on-screen text, an optional reference image, and audio narration. The next step can be triggered by a timed transition, picking up or dropping a specific object, or completing a snap interaction.

At the center is a mechanical-watch assembly activity:

- **Disassemble and inspect:** use a two-hand pull-apart interaction to separate watch components; they ease back into place when released.
- **Scale components:** use both hands to resize a grabbed object.
- **Assemble with snap feedback:** align parts with their target and let the snap interaction position and orient them.
- **Bring the mechanism to life:** interlocking gears drive one another with tooth-count-based speed relationships and opposite rotation.
- **Learn by doing:** configurable stages connect object interactions to contextual instruction text, images, and voice clips.

## XR development highlights

This project is a practical example of building interactions around the user's hands and the objects they manipulate:

- Custom `XRGrabInteractable` implementations for two-interactor scaling and disassembly.
- Interaction-driven progression, with listeners attached and cleaned up as tutorial stages change.
- Reusable snap behavior for aligning and animating mechanical components.
- A gear behavior model that discovers touching gears and propagates rotation through the mechanism.
- ScriptableObject-authored instructions, keeping tutorial content and progression conditions editable in the Unity Inspector.
- Audio and visual feedback integrated into the assembly flow.

## Built with

- **Unity 6** — Editor version `6000.0.23f1`
- **C#**
- **XR Interaction Toolkit** `3.1.1`
- **OpenXR Plugin** `1.14.1`
- **XR Hands** `1.5.0`
- **Input System** `1.13.1`
- **Universal Render Pipeline** `17.0.3`

The project includes OpenXR and Oculus XR settings. The active XR runtime and device support depend on the local Unity/OpenXR configuration. Android's configured minimum SDK is API 32.

## Run the project

1. Install **Unity Hub** and Unity Editor `6000.0.23f1`.
2. In Unity Hub, choose **Add project from disk** and select this repository.
3. Open the project and let Unity resolve packages and import assets.
4. Open `Assets/Scenes/SampleScene.unity`.
5. For headset testing, connect and configure a compatible XR device and OpenXR runtime, then press **Play**. Without an XR runtime/device configured, the headset experience may not start as intended.

`SampleScene` is the enabled scene in the project's build settings. The project settings currently specify Android minimum SDK 32; select and configure the appropriate build target and XR provider in Unity before creating a device build.

## Project structure

| Path | Contents |
| --- | --- |
| `Assets/Scenes/` | Main and basic Unity scenes |
| `Assets/_ProjectAssets/Scripts/` | Watch assembly, gear, snap, tutorial, and audio behavior |
| `Assets/_ProjectAssets/Data/` | ScriptableObject tutorial instructions and related content |
| `Assets/_ProjectAssets/3D/` | Watch component models |
| `Assets/_ProjectAssets/Audio/` | Narration and experience audio |
| `Assets/Samples/` | Unity XR Hands and XR Interaction Toolkit samples |

## Extending the guided experience

Tutorial content is authored in the `Instructions` ScriptableObject. Each instruction can include text, an image, and an audio clip, plus a progression condition: a delay, a named object's grab or drop, or a snap completion. This keeps the learning sequence editable without hard-coding each instruction into the UI.

## About

UhrMuseum is an XR development project exploring how spatial interaction can make a complex physical mechanism approachable, tactile, and memorable.

---

**Developed by [VisnevschiV](https://github.com/VisnevschiV).**
