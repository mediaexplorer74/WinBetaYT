# WinBetaYT v.1.0.6-alpha
![Logo](/Images/logo.png)

My fork of Dominic Maas' Winbeta_Video project for my own micro-RnD. :)

Main goal is to adapt this app to W10M (builds 14393+, or, maybe, even 10240...)

## Screenshots
![x64](/Images/shot1.png)
![ARM](/Images/shot2.png)

## About
The original WinBeta_Video is a very simple Windows 10 app that displays and plays YouTube videos 
from the WinBeta YouTube channel.
My fork still required the YouTube .NET API now, but not requred both MyToolkit and MyToolkit Extended (For playing YouTube Videos) at now.
I switched the project from mytoolkit to (on) LibVideo and VLCSharp video processing system.


## Status
- Youtube api v3 used. Result: WinBeta channel video list seems to be ok.
- VLCSharp library used. Result: success on youtube video playing. 
- libvideo updated to fix youtube id - url resolving.  


## Tech moments
- UWP platform (targets: x86, x64, ARM)
- Win. SDK build: 19041 
- Min. Win. SDK build: 14393
- You need to setup your own youtube apikey (see Constants.cs).


## To Do
* Create Settings page for "APIkey tuning".  
* Try to improve/fix VideoPage's UI controls (Video quality, etc.)
* Continue some experiments with "Astoria compatibility" (hardcore!)

## License / Reference(s)
- MIT
- https://github.com/DominicMaas/winbeta-video Original "WinBeta Video" project
- https://github.com/videolan/libvlcsharp libvlcsharp multi-platform vlc "binding(s)"
- https://github.com/omansak/libvideo Osman Şakir (OMANSAK) Kapar's libvideo

## ..

As is. No support. RnD only

## .
[m][e] 2024