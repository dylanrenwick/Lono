﻿using System;
using System.Collections.Generic;
using System.Text;

using Lono.Data;

namespace Lono
{
    public interface IInputWrapper
    {
        Vector2 MousePos { get; }
        Vector2 MouseDelta { get; }

        bool GetKeyDown(VirtualKey key);
        bool GetKeyPressed(VirtualKey key);

        bool GetButtonDown(int button);
        bool GetButtonPressed(int button);

        void Update();
    }

    /// <summary>
    /// Enumeration for virtual keys.
    /// </summary>
    public enum VirtualKey
        : ushort
    {
        Back = 0x08,
        Tab = 0x09,
        Return = 0x0D,
        Shift = 0x10,
        Control = 0x11,
        Menu = 0x12,
        Pause = 0x13,
        CapsLock = 0x14,
        Escape = 0x1B,
        Space = 0x20,
        End = 0x23,
        Home = 0x24,
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        Select = 0x29,
        Print = 0x2A,
        Execute = 0x2B,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LeftWindows = 0x5B,
        RightWindows = 0x5C,
        Sleep = 0x5F,
        Numpad0 = 0x60,
        Numpad1 = 0x61,
        Numpad2 = 0x62,
        Numpad3 = 0x63,
        Numpad4 = 0x64,
        Numpad5 = 0x65,
        Numpad6 = 0x66,
        Numpad7 = 0x67,
        Numpad8 = 0x68,
        Numpad9 = 0x69,
        Multiply = 0x6A,
        Add = 0x6B,
        Separator = 0x6C,
        Subtract = 0x6D,
        Decimal = 0x6E,
        Divide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NumLock = 0x90,
        ScrollLock = 0x91,
        LeftShift = 0xA0,
        RightShift = 0xA1,
        LeftControl = 0xA2,
        RightControl = 0xA3,
        LeftMenu = 0xA4,
        RightMenu = 0xA5,
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,
        MediaNextTrack = 0xB0,
        MediaPrevTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,
        LaunchMail = 0xB4,
        LaunchMediaSelect = 0xB5,
        LaunchApplication1 = 0xB6,
        LaunchApplication2 = 0xB7,
        ProcessKey = 0xE5,
        ATTN = 0xF6,
        CRSel = 0xF7,
        EXSel = 0xF8,
        Play = 0xFA,
        Zoom = 0xFB,
        PA1 = 0xFD
    }
}
