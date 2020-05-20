This repro reproduced an error where the SNI.dll is locked, preventing AppDomain unload.

Overview:
Program consists of a host application (HostApp), which loads a plugin (Plugin.Core) into a new appdomain and does a SQL query.
Both projects are net472.

What the program does:
1. Creates a new appdomain with Shadow Copy on
2. Loads plugin into appdomain
3. Does a SELECT agains the SQL database
4. Unloads the Appdomain
5. Tries to delete the SNI.dll file, which causes an exception.

How to run:

1. Build
2. Edit config file to point to a valid SQL database
3. Run program
4. Observe exception when SNI is locked.

What should have happened:

Since the appdomain is loaded with Shadow copy, all files are copied to a temporary folder and loaded from there. This means that the original files can be deleted. But this does not apply to SNI.dll, which is still loaded from the original location.

Why is this a problem:

The program is unable to unload the appdomain, and replace the plugin files without re-starting the entire application.