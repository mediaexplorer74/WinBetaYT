# WinBetaYT v.1.0.5-alpha
![Logo](/Images/logo.png)

My fork of Dominic Maas' Winbeta_Video project for my own micro-RnD. :)

Main goal is to adapt this app to W10M (builds 14393+, or, maybe, even 10240...)

## Screenshots
![x64](/Images/shot01.png)
![ARM](/Images/shot02.png)

## About
The original WinBeta_Video is a very simple Windows 10 app that displays and plays YouTube videos 
from the WinBeta YouTube channel.
My fork still required the YouTube .NET API now, but not requred both MyToolkit and MyToolkit Extended (For playing YouTube Videos) at now.
I switched the project from mytoolkit to (on) LibVideo and VLCSharp video processing system.


## Status
- Youtube WinBeta channel video list seems to be ok.
- I tryied to "inject" libvideo library. I catch problems with standard MediaElement. 
- A day after, I get success on video playing when I tried to use VLCSharp :) 


## Tech moments
- UWP platform (targets: x86, x64, ARM)
- Win. SDK build: 19041 
- Min. Win. SDK build: 14393
- You need to setup your own youtube apikey (see Constants.cs).


## To Do
* Create Settings page for "APIkey tuning".  
* fix UI controls (Video quality, etc.)

## License / Reference(s)
- MIT
- https://github.com/DominicMaas/winbeta-video Original "WinBeta Video" project

## ..

As is. No support. RnD only

## .
[m][e] 2024