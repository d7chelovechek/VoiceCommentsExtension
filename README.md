# Voice Comments extension
An extension for Visual Studio 2022 that allows to record and listen voice comments on code editor.

## Installation

* You can install the extension by downloading it from the marketplace, following the [link](https://marketplace.visualstudio.com/items?itemName=d7chelovechek.vcext2022); 
* You can downloading it directly from Visual Studio 2022, searching extension for the name "Voice Comments Extension for Visual Studio 2022".

## Instructions

* First, you need to install the caret in the place where you need to add a voice comment;
* Next you need to open Recorder. This can be done from:
	- the Edit menu;
	- the Context menu of the code editor;
	- using the keyboard shortcuts "Ctrl + K, Shift + R".

![EditMenu](https://github.com/d7chelovechek/VoiceCommentsExtension/assets/73136877/0d30b9c8-544b-4b92-912c-1d9d087dac6f) ![ContextMenu](https://github.com/d7chelovechek/VoiceCommentsExtension/assets/73136877/bfb0ce7e-87c4-4e45-8b15-485ec52072eb)

* By opening Recorder, you can record a voice comment. The Recorder window allows you to:
	- Start recording a voice comment;
	- Pause recording of a voice comment;
	- Cancel recording of a voice comment;
	- Save the voice comment recording (at the moment, all voice comments are saved in the current project in the ".voice-comments" folder).

![RecorderView](https://github.com/d7chelovechek/VoiceCommentsExtension/assets/73136877/bf3473f1-c995-48a0-8fcc-be352d7fd378)

* After saving the voice comment recording, a voice comment player will be added to the current position of the caret in code editor. The voice comment player allows you to:
	- Start listening a voice comment;
	- Pause listening of a voice comment;
	- Rewind listening of a voice comment by clicking on the voice visualizer.

![VoiceCommentView](https://github.com/d7chelovechek/VoiceCommentsExtension/assets/73136877/1288811e-07d8-4832-aba7-41122160deee)

## Features

* Currently, the voice comment view supports default themes (dark, light, light (blue));
* Currently, the following content types are supported:
	- C/C++;
	- C#;
	- Ts/Js;
	- F#;
	- SQL/T-SQL;
	- VB.
* Currently, the following audio formats are supported:
	- wav.

## Issues

If you have any problems using it or have suggestions for development, then do not hesitate to open "Issue".
