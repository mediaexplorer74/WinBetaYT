# WinBetaYT v.1.0.7-alpha
![Logo](/Images/logo.png)

My fork of Dominic Maas' Winbeta_Video project for my own micro-RnD. :)

Main goal is to repair Youtube work (ideally: witnout any VPN) & adapt this app to W10M (UI)

## Screenshots
![W11Tiny](/Images/sshot1.png)
![W11Tiny](/Images/sshot2.png)

## About
The original WinBeta_Video is a very simple Windows 10 app that displays and plays YouTube videos 
from the WinBeta YouTube channel.
My fork still required the YouTube .NET API now, but not requred both MyToolkit and MyToolkit Extended (For playing YouTube Videos) at now.
I switched the project from mytoolkit to (on) LibVideo and VLCSharp video processing system.


## Status
- Youtube api v3 used. Result: WinBeta channel video list seems to be ok.
- VLCSharp library still here/there. 
- my experimental libvideo used for YT video stream url resolving (at now -- no success!)  


## Tech moments
- UWP platform (targets: x86, x64, ARM)
- Win. SDK build: 19041 
- Min. Win. SDK build: 14393
- You need to setup your own youtube apikey (see Constants.cs).


## To Do
* Create Settings page for "APIkey tuning"  
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