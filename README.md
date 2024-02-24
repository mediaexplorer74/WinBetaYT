# WinBetaYT v.1.0.0 pre-alpha

My fork of Dominic Maas' Winbeta_Video project for my own micro-RnD...


## Screenshots
![](/Images/shot01.png)
![](/Images/shot02.png)


## About
The original WinBeta_Video is a very simple Windows 10 app that displays and plays YouTube videos 
from the WinBeta YouTube channel.
WinBeta Video requires the YouTube .NET API, and both MyToolkit and MyToolkit Extended (For playing YouTube Videos).
So, I'll try to fix all "Video not playing" issues.


## Status
- Youtube WinBeta channel video list seems to be ok.
- I tryied to "inject" libvideo library (to save damaged myToolkit.Extended's YouTube feature) 
- No success. No video playing, sadly. Standard Media Player begins to do infinite re-cycling :(
....however, my internet tooo slow? idk... ;)


## Tech moments
- UWP platform (targets: x86, x64, ARM)
- Win. SDK build: 19041 
- Min. Win. SDK build: 14393
- You need to setup your own youtube apikey (see Constants.cs).


## To Do
* fix myToolkit.Extended bugs after "min. w10m sdk downshifting"

## ..

As is. No support. RnD only

## .
[m][e] 2024