# Synopsis

An exercise/food tracker, a desktop application written in C#.

The sources are hosted on [GitHub](https://github.com/rinusser/Bulkr).


# General

This application is intended for tracking physical exercise and eating habits, making it easier to achieve your personal
goals. The tools supplied aren't intended for any specific fitness goal, they just help you track what you're doing and
offer analyses of the data you put in.

The code is written and tested with Mono and uses Gtk# for the GUI, so it should run on Windows, Linux and macOS.

### Nutrition Data

The nutrition data handled in this application currently follows EU regulations. In particular:
* *Salt* is tracked (in grams), instead of sodium.
* Dietary *Fiber* values are not included in the amount of total carbohydrates.
* The arrangement of UI elements resembles EU nutrition facts labels.

Other values or label stylings may be implemented later, once the core functionality is done.


# Requirements

In theory you should be able to compile this application on Windows and Linux, with Mono, Microsoft's .NET SDK or Visual Studio.

In practice only Mono in Linux was easy to set up, so this the only tested combination for now.

### Mono

* Mono (tested with 5.16.1 on Ubuntu)
* Gtk# (tested with 2.14.45)

### .NET SDK Command-Line

Using `msbuild` mostly worked, except for a missing reference to `Mono.Posix`. This might be resolved by installing the
standalone Gtk# binding, or somehow getting msbuild to actually use the NuGet-installed package.

### Visual Studio

I don't use Visual Studio and probably won't either. Monodevelop generates VS2017-compatible .sln and .csproj files,
so in theory Visual Studio 2017+ should be able to handle everything out of the box.


# Installation

Building the application should be as easy as telling your IDE to compile/run it, or invoking `msbuild` in the CLI.

The build configuration is still a work in progress.


# Usage

In IDEs you just need to to run the application.

In the CLI you can invoke it with:

| Environment                        | Command Line                                     |
| ---------------------------------- | ------------------------------------------------ |
| Windows (.NET SDK / Visual Studio) | `artifacts\Bulkr.Gui\bin\Release\Bulkr.exe`      |
| Windows (Mono)                     | `mono artifacts\Bulkr.Gui\bin\Release\Bulkr.exe` |
| Linux (Mono)                       | `mono artifacts/Bulkr.Gui/bin/Release/Bulkr.exe` |

If you built the "Debug" configuration, use the "Debug" directory instead of "Release".


# Tests

Test projects are located in `tests/`.


# Legal

### Copyright

Copyright (C) 2019 Richard Nusser

### License

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.

